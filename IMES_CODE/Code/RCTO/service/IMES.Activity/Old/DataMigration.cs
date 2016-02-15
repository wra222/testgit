using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
namespace IMES.Activity
{
    /// <summary>
    /// 此Activity仅用于FA上线PAK未上线的阶段，此阶段FA产生的数据需要移转到旧的DB，
    /// 这通过调用数据移转的存储过程来实现，根据PCode，从Session中获取存储过程在
    /// 该站需要使用的参数，并调用存储过程
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         仅用于FA上线PAK未上线的阶段
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据PCode，从Session中获取存储过程在该站需要使用的参数;
    ///         2.调用数据移转存储过程;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         PCode
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         写入旧格式的数据
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class DataMigration : BaseActivity
    {
        public DataMigration()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                string no1 = string.Empty;
                string no2 = string.Empty;
                string keyid = string.Empty;
                string pcode = CurrentSession.GetValue(Session.SessionKeys.PCode).ToString();
                IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();


                switch (pcode)
                {
                    case "OPFA002":  //Offline Print CT(TSB-Combine)
                        var ctList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.CTList);
                        foreach (var ct in ctList)
                        {
                            miscRepository.ImesToFis(ct, pcode, no1, no2);
                        }
                        break;
                    case "OPFA004":      //ProdID Print
                        var prodNoList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);
                        foreach (var proid in prodNoList)
                        {
                            miscRepository.ImesToFis(proid, pcode, no1, no2);
                        }
                        break;
                    case "OPFA005":  //Reprint ProdID
                        IProductRepository pRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                        string startProdId = CurrentSession.GetValue(Session.SessionKeys.PrintLogBegNo).ToString();
                        string endProdId = CurrentSession.GetValue(Session.SessionKeys.PrintLogEndNo).ToString();

                        var prodList = pRepository.GetProductIdsByRange(startProdId, endProdId);

                        foreach (var proid in prodList)
                        {
                            miscRepository.ImesToFis(proid, pcode, no1, no2);
                        }
                        break;
                    case "OPFA020":  //Combine KP CT
                        miscRepository.ImesToFis(Key, pcode, no1, no2);
                        break;
                    case "OPFA021":  //HDD Test
                        miscRepository.ImesToFis(Key, pcode, no1, no2);
                        break;
                    case "OPFA022":  //Combine LCM and BTDL/TPDL
                        miscRepository.ImesToFis(Key, pcode, no1, no2);
                        break;
                    case "OPFA025":  //Virtual MO
                        string virtualMO = ((IList<string>)CurrentSession.GetValue(Session.SessionKeys.VirtualMOList))[0];
                        miscRepository.ImesToFis(virtualMO, pcode, no1, no2);
                        break;
                    case "OPFA031":  //Dismantle
                        miscRepository.ImesToFis(Key, pcode, no1, no2);
                        break;
                    case "OPFA026":
                        keyid = CurrentSession.GetValue(Session.SessionKeys.OldMONO).ToString();
                        no1 = CurrentSession.GetValue(Session.SessionKeys.NewMONO).ToString();
                        no2 = CurrentSession.GetValue(Session.SessionKeys.Qty).ToString();
                        miscRepository.ImesToFis(keyid, pcode, no1, no2);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                var ex = new Exception("Data migration failed." + e.Message, e);
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}



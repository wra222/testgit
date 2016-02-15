// INVENTEC corporation (c)2011 all rights reserved. 
// Description:非BT转 BT 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-03   Kerwin                       create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Text.RegularExpressions;
namespace IMES.Activity
{
    /// <summary>
    ///        将符合条件的Product添加到ProductBT表中。
    ///        ProductBT.BT ='BT'+convert(nvarchar(20),getdate(),112)   
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-PAK-BT_CHANGE
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.ModelName
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       无
    /// </para> 
    ///<para>
    /// 数据更新:
    ///        将符合条件的Product添加到ProductBT表中。
    ///        ProductBT.BT ='BT'+convert(nvarchar(20),getdate(),112)          
    /// </para>
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class ChangeToBT : BaseActivity
    {
        ///<summary>
        /// constructor
        ///</summary>
        public ChangeToBT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 删除ProductBT表中符合条件的Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string ModelName = CurrentSession.GetValue(Session.SessionKeys.ModelName).ToString();

            var CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
           //Vincent add refer to ConstValue Check Model
            bool allowChangeBT=true;
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> notAllowModelREList = partRep.GetConstValueTypeList("NotAllowChangeBTModelRE");
            if (notAllowModelREList != null && notAllowModelREList.Count > 0)
            {
                foreach (ConstValueTypeInfo item in notAllowModelREList)
                {
                    Regex regex = new Regex(item.value.Trim());
                    if (regex.IsMatch(ModelName))
                    {
                        allowChangeBT = false;
                        break;
                    }
                }
            }

           // if (ModelName.Substring(9, 2) == "00" || ModelName.Substring(9, 2) == "01")
            if (allowChangeBT)
            {
                if (CurrentProductRepository.PreQueryForChangeToBTDeffered(ModelName) == true)
                {
                    string BT = "BT" + DateTime.Now.ToString("yyyyMMdd");
                    CurrentProductRepository.ChangeToBTDefferedDefered(CurrentSession.UnitOfWork, ModelName, Editor, BT);
                }
                else
                {
                    List<string> erpara = new List<string>();
                    erpara.Add(ModelName);
                    //2012-7-13, for Mantis, 修改报错信息，由“资料有误”变为“找不到匹配的Product!”     
                    //throw new FisException("PAK084", erpara);
                    throw new FisException("CHK202", erpara);                    
                }
            }
            else   //2012-7-13,for Mantis
            {
                List<string> erpara = new List<string>();
                erpara.Add(ModelName);
                throw new FisException("CHK909", erpara);
            }

            return base.DoExecute(executionContext);
        }
    }
}

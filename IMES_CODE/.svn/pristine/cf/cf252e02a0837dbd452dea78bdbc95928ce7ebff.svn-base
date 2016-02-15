// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块保存上传的Delivery、Pallet数据(for运筹User)
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
// Known issues:
using System;
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块保存上传的Delivery、Pallet数据(for运筹User)
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Upload Po Data for OB user(Normal/Domestic)
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         将dotDNList保存到IMES2012_HP_EDI..[PAK.PAKComn]表；
    ///         将dashDNList保存到IMES2012_HP_EDI..[PAK_PAKComn]表；
    ///         将PalletList保存到IMES2012_HP_EDI..[PAK.PAKPaltno]表
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         dotDNList, dashDNList, PizzaPltList
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPizzaRepository
    /// </para> 
    /// </remarks>
    public partial class UploadOBPoData : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadOBPoData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();

            IList<PakDotPakComnInfo> dotDNList = CurrentSession.GetValue("dotDNList") as IList<PakDotPakComnInfo>;
            IList<PakDashPakComnInfo> dashDNList = CurrentSession.GetValue("dashDNList") as IList<PakDashPakComnInfo>;
            IList<PakDotPakPaltnoInfo> palletList = CurrentSession.GetValue("PalletList") as IList<PakDotPakPaltnoInfo>;


            
            string textRecordCount= (string) CurrentSession.GetValue("TextRecordCount");
            string uploadOKRecordCount = (string)CurrentSession.GetValue("UploadOKRecordCount");
            textRecordCount = textRecordCount ?? "0";
            uploadOKRecordCount = uploadOKRecordCount ?? "0";
            EDIUploadPOLogInfo log = new EDIUploadPOLogInfo
            { editor = this.Editor,
                 cdt =DateTime.Now,
              textFileRecordCount = int.Parse(textRecordCount),
               uploadNGDeliveryNo="",
                uploadOKRecordCount = int.Parse(uploadOKRecordCount)
            };
            dnRep.InsertEDIUploadPOLog(log);

            /*
             * Answer to: ITC-1360-1360
             * Description: Pre-delete all exist records before insert.
            */
            foreach (PakDotPakComnInfo ele in dotDNList)
            {
                if (pizzaRepository.CheckExistPakDotPakComn(ele.internalID))
                {
                    pizzaRepository.DeletePakDotPakComnDefered(CurrentSession.UnitOfWork, ele.internalID);
                }
            }

            foreach (PakDotPakComnInfo ele in dotDNList)
            {
                ele.logID = log.id;
                pizzaRepository.AddPakDotPakComnDefered(CurrentSession.UnitOfWork, ele);
            }

            foreach (PakDashPakComnInfo ele in dashDNList)
            {
                if (pizzaRepository.CheckExistPakDashPakComn(ele.internalID))
                {
                    pizzaRepository.DeletePakDashPakComnDefered(CurrentSession.UnitOfWork, ele.internalID);
                }
            }

            foreach (PakDashPakComnInfo ele in dashDNList)
            {                
                pizzaRepository.AddPakDashPakComnDefered(CurrentSession.UnitOfWork, ele);
            }

            foreach (PakDotPakPaltnoInfo ele in palletList)
            {
                if (pizzaRepository.CheckExistPakDotPakPaltno(ele.internalID))
                {
                    pizzaRepository.DeletePakDotPakPaltnoDefered(CurrentSession.UnitOfWork, ele.internalID);
                }
            }

            foreach (PakDotPakPaltnoInfo ele in palletList)
            {
                pizzaRepository.AddPakDotPakPaltnoInfoDefered(CurrentSession.UnitOfWork, ele);
            }

            return base.DoExecute(executionContext);
        }
    }
}

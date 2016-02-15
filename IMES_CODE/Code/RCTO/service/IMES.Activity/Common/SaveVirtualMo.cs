/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2011-12-15   208014 Chen Xu    Modify Plant value
 * 
 * Known issues:Any restrictions about this file 
 * UC Revision: 6789
 */



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
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.DataModel;


namespace IMES.Activity
{
    /// <summary>
    /// 将生成的MO插入MO表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于102Virtual MO站
    ///         
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
    ///         Session.VirtualMOList
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         MO
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///          IMORepository
    /// </para> 
    /// </remarks>
    public partial class SaveVirtualMo : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SaveVirtualMo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save Virtual MO
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var moList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.VirtualMOList);
            string virtualMO = moList[0];
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            short Qty;
            short.TryParse(CurrentSession.GetValue(Session.SessionKeys.Qty).ToString(),out  Qty);

            string st = (string)CurrentSession.GetValue(Session.SessionKeys.StartDate);
            DateTime startDate = DateTime.Parse(st);

            string plant = "CP81";
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<SysSettingInfo> plantList = ipartRepository.GetSysSettingInfoes(new SysSettingInfo() { name = "PlantCode" });
            if (plantList != null && plantList.Count > 0)
            {
                plant = plantList[0].value;
            }
            
            //INSERT INTO [IMES_GetData].[dbo].[MO]([MO],[Plant],[Model],[CreateDate],[StartDate],[Qty],[SAPStatus],[SAPQty],
		    //[Print_Qty],[Status ],[Cdt],[Udt])
	        //VALUES(@VirtualMO, 'CP81', @Model, GETDATE(), GETDATE(), @Qty, '', 0, 0, 'H', GETDATE(), GETDATE())

            //UC Revision: 6789: 保存StartDate
            //INSERT INTO [IMES_GetData].[dbo].[MO]([MO],[Plant],[Model],[CreateDate],[StartDate],[Qty],[SAPStatus],[SAPQty],
		    //[Print_Qty],[Status ],[Cdt],[Udt])
	        //VALUES(@VirtualMO, 'CP81', @Model, GETDATE(), @StartDate, @Qty, '', 0, 0, 'H', GETDATE(), GETDATE())

            // UC Revision: 8613 :
            // INSERT INTO [IMES_GetData].[dbo].[MO]([MO],[Plant],[Model],[CreateDate],[StartDate],[Qty],[SAPStatus],[SAPQty],
            // [Print_Qty],[Transfer_Qty],[Status ],[Cdt],[Udt])
	        // VALUES(@VirtualMO, 'CP81', @Model, GETDATE(), @StartDate, @Qty, '', 0 @Qty, 0, 0, 'H', GETDATE(), GETDATE())

            // Revision: 8815: 
            // INSERT INTO [IMES_GetData].[dbo].[MO]([MO],[Plant],[Model],[CreateDate],[StartDate],[Qty],[SAPStatus],[SAPQty],
            // [Print_Qty],[Transfer_Qty], [CustomerSN_Qty], [Status ],[Cdt],[Udt])
	        // VALUES(@VirtualMO, 'CP81', @Model, GETDATE(), @StartDate, @Qty, '', 0 @Qty, 0, 0, 0, 'H', GETDATE(), GETDATE())


            MO currentMO = new MO();
            currentMO.MONO = virtualMO;
            //currentMO.Plant = "CP81";
            currentMO.Plant = plant;
            currentMO.Model = model;
            currentMO.StartDate = startDate;
            currentMO.Qty = Qty;
            currentMO.SAPStatus = "";
            currentMO.SAPQty = Qty;
            currentMO.PrtQty = (short)0;
            currentMO.TransferQty = (short)0;
            currentMO.CustomerSN_Qty = (short)0;
            currentMO.Status = "H";

            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            moRepository.Add(currentMO,CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}

/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: 获取Lot No List
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-12  Kaisheng               Create
 * Known issues:
 * TODO：
 * UC 具体业务： 获取Lot No List
 * 
 */

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Process;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Linq;

namespace IMES.Activity
{
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于SA： Combine Pch in lot
    /// </para>
    /// <para>
    /// 实现逻辑：获取Lot No List
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
    ///         
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
    /// IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GetLotListForCombinPCB : BaseActivity
	{
        /// <summary>
        /// GetLotListForCombinPCB
        /// </summary>
        public GetLotListForCombinPCB()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 获取LotSetting表中PassQty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            int intDay=0;
            int intLotqty = (int)CurrentSession.GetValue("PassQtyinlotSetting");
            
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> oqcTimeSpanValueLst = partRepository.GetValueFromSysSettingByName("OQCTimeSpan");
            if (oqcTimeSpanValueLst != null && oqcTimeSpanValueLst.Count > 0)
            {
                intDay = int.Parse(oqcTimeSpanValueLst[0]);  //(OQCTimeSpanValueLst[0]);
            }
            else
            {
                intDay = 3;   //默认为3；
            }
            CurrentSession.AddValue("OQCTimeSpan", intDay);
            string[] statusparam = { "1", "2" };
            IList<LotInfo> currentLotlst = itemRepository.GetLotList(statusparam, intDay, currentMB.MBStatus.Line, currentMB.MBCode, intLotqty);
            string mbType = (string)CurrentSession.GetValue("GetMBType");
            IList<LotInfo> retorderbylot = new List<LotInfo>();
            if (mbType.ToUpper() == "PC")
            {
                retorderbylot = (from item in currentLotlst where item.type == "PC" || item.type == "FRU" select item).ToList();//orderby item.cdt
            }
            else
            {
                retorderbylot = (from item in currentLotlst where item.type == mbType select item).ToList();//orderby item.cdt
            }
            if (retorderbylot.Count==0)
            {
                //报错：“没有符合条件的Lot”
                List<string> errpara = new List<string>();
                errpara.Add(currentMB.Sn);
                FisException ex = new FisException("CHK284", errpara);//error number
                throw ex;
            }
            //retorderbylot[0].lotNo
            //retorderbylot[0].type
            //retorderbylot[0].qty
            //retorderbylot[0].status
            //retorderbylot[0].cdt
            for (int i = 0; i <= retorderbylot.Count - 1; i++)
            {
                retorderbylot[i].editor = retorderbylot[i].cdt.ToString();
            }
            CurrentSession.AddValue("LotListforCombinePcb", retorderbylot);
            
            return base.DoExecute(executionContext);
        }
	
	}
}

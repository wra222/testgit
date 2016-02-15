/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 用于QC Repair时获取Return Station信息。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2012-8-1     Jessica Liu                  For QC Repair
 * Known issues:
 */

using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 获取Return Station
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于维修站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB或者Session.Product
    ///         Session.ReapirDefectInfo
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
    ///         PCARepair_DefectInfo 或者ProductRepair_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         RepairDefect
    ///         DefectCodeStationInfo
    /// </para> 
    /// </remarks>
    public partial class GetReturnStationForQCRepair : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetReturnStationForQCRepair()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 用于Repair时修改Defect信息。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {           
            RepairDefect defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);

            IProduct productObj = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);                        
           
            var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
            #region mark by Vincent
            //DefectCodeStationInfo cond = new DefectCodeStationInfo();
            //IList<DefectCodeStationInfo> info = null;
            //cond.defect = defect.DefectCodeID;
            //cond.pre_stn = productObj.Status.StationId;
            //cond.crt_stn = Station;
            //cond.cause = defect.Cause;
            //info = stationRepository.GetDefectCodeStationList(cond);
            //if (info != null && info.Count > 0)
            //{
            //    defect.ReturnStation = info[0].nxt_stn;
            //}
            //else
            //{
            //    DefectCodeStationInfo cond1 = new DefectCodeStationInfo();
            //    IList<DefectCodeStationInfo> info1 = null;
            //    cond1.defect = defect.DefectCodeID;
            //    cond1.pre_stn = productObj.Status.StationId;
            //    cond1.crt_stn = Station;
            //    cond1.cause = "";
            //    info1 = stationRepository.GetDefectCodeStationList(cond1);

            //    if (info1 != null && info1.Count > 0)
            //    {
            //        defect.ReturnStation = info1[0].nxt_stn;
            //    }
            //    else
            //    {
            //        List<string> errpara = new List<string>();
            //        FisException e = new FisException("CHK950", errpara);
            //        throw e;    
            //    }
            //}
            #endregion
            //IList<DefectCodeNextStationInfo> nextStationList= stationRepository.GetNextStationFromDefectStation(productObj.Status.StationId,
            //                                                                                      this.Station,
            //                                                                                      string.IsNullOrEmpty(defect.MajorPart) ? string.Empty : defect.MajorPart.Trim(),
            //                                                                                      string.IsNullOrEmpty(defect.Cause) ? string.Empty : defect.Cause.Trim(),
            //                                                                                      string.IsNullOrEmpty(defect.DefectCodeID) ? string.Empty : defect.DefectCodeID.Trim());
            IList<DefectCodeNextStationInfo> nextStationList = stationRepository.GetNextStationFromDefectStation(productObj.Status.StationId,
                                                                                                  this.Station,
                                                                                                  string.IsNullOrEmpty(defect.MajorPart) ? string.Empty : defect.MajorPart.Trim(),
                                                                                                  string.IsNullOrEmpty(defect.Cause) ? string.Empty : defect.Cause.Trim(),
                                                                                                  string.IsNullOrEmpty(defect.DefectCodeID) ? string.Empty : defect.DefectCodeID.Trim(),
                                                                                                  productObj.Family,
                                                                                                  productObj.Model);
           
            if (nextStationList == null || nextStationList.Count == 0)
            {
                List<string> errpara = new List<string>();
                FisException e = new FisException("CHK950", errpara);
                throw e;
            }

            defect.ReturnStation = nextStationList[0].NXT_STN;
            
            CurrentSession.AddValue(Session.SessionKeys.CurrentRepairdefect, defect);

            return base.DoExecute(executionContext);
        }
	}
}

// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据输入的ProductID,OQC Repair Save 时，需要额外复制QOC Repair 记录给FA Repair 使用
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-15   Dean Man                     create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的ProductID,OQC Repair Save 时，需要额外复制QOC Repair 记录给FA Repair 使用
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
    ///         根据输入的ProductID,OQC Repair Save 时，需要额外复制QOC Repair 记录给FA Repair 使用
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///         Repair
    ///         TestLog
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class WriteFARepairList : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public WriteFARepairList()
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
            /*IRepairTarget repairTarget=null;

            repairTarget =(IRepairTarget)CurrentSession.GetValue(ExtendSession.SessionKeys.ProductRepair);

            Repair repair=new Repair();*/
            
            //IRepairTarget repairTarget = GetRepairTarget();
            //TestLog testLog = (TestLog)CurrentSession.GetValue(ExtendSession.SessionKeys.TestLog);
            //IList<RepairDefect> repairDefects=(IList<RepairDefect>)CurrentSession.GetValue(ExtendSession.SessionKeys.RepairDefect);
            //Repair repair = new Repair(0,testLog.Sn, repairTarget.RepairTargetModel, testLog.Type, testLog.Line, Station, IMES.FisObject.Common.Repair.Repair.RepairStatus.NotFinished, repairDefects, testLog.Editor, testLog.ID.ToString(), DateTime.Now, DateTime.Now);
              
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //Repair currentRepair =currentProduct.GetCurrentRepair();
            //int ProductRepairID = currentRepair.ID;
            //IList<RepairDefect> currentRepairDefects =currentRepair.Defects;
            
            //TestLog OQCTestLog = new TestLog(0, testLog.Sn, testLog.Line, testLog.FixtureId, Station, testLog.Status, testLog.JoinID, Editor, testLog.Type, DateTime.Now);
            //currentProduct.SetExtendedProperty(ExtendSession.SessionKeys.ProductRepairID, ProductRepairID.ToString(), Editor);
            
            //Project.ProductInfo insert/update
            currentProduct.SetExtendedProperty(ExtendSession.SessionKeys.OQCRepairStation, Station, Editor);
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);

            
            Repair OQCRepair = currentProduct.GetCurrentRepair();
            /*SELECT t1.ID,t1.ProductID,t1.Model,t1.Type,t1.Line,t1.Station,t1.TestLogID,t1.Status,t1.Editor,t1.Cdt,t1.Udt,
                t2.ID,t2.ProductRepairID,t2.Type,t2.DefectCode,t2.Cause,t2.Obligation,t2.Component,t2.Site,t2.Location,t2.MajorPart,t2.Remark,t2.VendorCT,t2.PartType,t2.OldPart,t2.OldPartSno,t2.NewPart,t2.NewPartSno,t2.Manufacture,t2.VersionA,t2.VersionB,t2.ReturnSign,t2.Mark,t2.SubDefect,t2.PIAStation,t2.Distribution,t2.[4M],t2.Responsibility,t2.Action,t2.Cover,t2.Uncover,t2.TrackingStatus,t2.IsManual,t2.MTAID,t2.Editor,t2.Cdt,t2.Udt 
                FROM ProductRepair t1,ProductRepair_DefectInfo t2 
                WHERE t1.ID=t2.ProductRepairID AND t1.ProductID='T01237475' AND t1.Status= 0 
                order by t2.Cdt desc*/

            RepairDefect repairDefect = null;
            IList<RepairDefect> repairDefects = new List<RepairDefect>();

            if (OQCRepair == null)
            {                
                throw new FisException();
            }
            else
            {
                foreach (RepairDefect item in OQCRepair.Defects)
                {
                   /* item.ID = 0;
                    item.RepairID = 0;
                    item.Cdt = DateTime.Now;
                    item.Udt = DateTime.Now;*/

                    /*repairDefect = new RepairDefect(0, 0, item.Type,item.DefectCodeID, null, null, null, null, null,
                        null, null, null, null, null, null, null, null, null, false, null,
                        null, null, "0", null, "0", null, null, null, null, null, null, null,
                        null, null, null, item.Editor, DateTime.Now, DateTime.Now);*/

                    repairDefect = new RepairDefect(0, 0, item.Type, item.DefectCodeID, item.Cause, item.Obligation, item.Component, item.Site, item.Location,
                                                    item.MajorPart, item.Remark, item.VendorCT, item.PartType, item.OldPart, item.OldPartSno,item.NewPart, item.NewPartSno,
                                                    item.NewPartDateCode,item.IsManual,item.Manufacture,item.VersionA, item.VersionB, item.ReturnSign, item.Side, item.Mark,
                                                    item.SubDefect, item.PIAStation, item.Distribution, item._4M, item.Responsibility, item.Action, item.Cover, item.Uncover,
                                                    item.TrackingStatus, item.MTAID, null, CurrentSession.Editor, DateTime.Now, DateTime.Now);
                    repairDefects.Add(repairDefect);
                
                }
                Repair repair = new Repair(0,
                                           OQCRepair.Sn,
                                           OQCRepair.Model,
                                           OQCRepair.Type,
                                           OQCRepair.LineID,
                                           Station,
                                           IMES.FisObject.Common.Repair.Repair.RepairStatus.NotFinished,
                                           //OQCRepair.Defects,
                                           repairDefects,
                                           //OQCRepair.Editor,
                                           CurrentSession.Editor,
                                           OQCRepair.TestLogID,
                                           OQCRepair.LogId,
                                            DateTime.Now,
                                            DateTime.Now);
                //currentProduct.Repairs.Add(repair);
                currentProduct.AddRepair(repair);
                productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
                //this.UpdateMainObject(currentProduct);
            }


            return base.DoExecute(executionContext);

            //CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);        
        }
	}
}

// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.Update MBStatus
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   Kerwin                       create
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;
using IMES.FisObject.FA.Product;
using System.Data.SqlClient;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 更新MB状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB为主线对象的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Property设定更新MB对象状态;
    ///         2.保存MB对象;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         update PCBStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class UpdateMBStatus : BaseActivity
    {
        /// <summary>
        /// 过站结果Pass or Fail
        /// </summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(MBStatusEnum), typeof(UpdateMBStatus));
        
        /// <summary>
        /// 更新为指定站 SessionKey
        /// </summary>
        public static DependencyProperty ManualStationProperty = DependencyProperty.Register("ManualStation", typeof(string), typeof(UpdateMBStatus));


        ///<summary>
        /// 过站结果Pass or Fail
        ///</summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public MBStatusEnum IsPass
        {
            get
            {
                return ((MBStatusEnum)(base.GetValue(UpdateMBStatus.IsPassProperty)));
            }
            set
            {
                base.SetValue(UpdateMBStatus.IsPassProperty, value);
            }
        }

        ///<summary>
        /// 更新为指定站 SessionKey
        ///</summary>
        [DescriptionAttribute("ManualStation")]
        [CategoryAttribute("ManualStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ManualStation
        {
            get
            {
                return ((string)(base.GetValue(UpdateMBStatus.ManualStationProperty)));
            }
            set
            {
                base.SetValue(UpdateMBStatus.ManualStationProperty, value);
            }
        }

        ///<summary>
        /// 构造函数
        ///</summary>
        public UpdateMBStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// update mb status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            var station = this.Station;
            if (this.IsPass == MBStatusEnum.CL)
            {
                station = "CL";
            }

            if (!string.IsNullOrEmpty(ManualStation))
            {
                string mStation = CurrentSession.GetValue(ManualStation) as string;
                if (mStation != null)
                {
                    station = mStation;
                }
            }

            string line = default(string);
            if (string.IsNullOrEmpty(this.Line))    
            {
                line = mb.MBStatus.Line;
            }
            else
            {
                line = this.Line;
            }
            string AllowPass = "";
           
            if (CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass) != null)
            {
                AllowPass = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass);
            }
            MBStatus status = null;
            if (AllowPass == "N" &&  this.IsPass == MBStatusEnum.Fail)
            {
                status = new MBStatus(mb.Key.ToString(), station, this.IsPass, 999, this.Editor, line, DateTime.Now, DateTime.Now);
            }
            else
            {
                status = new MBStatus(mb.Key.ToString(), station, this.IsPass, this.Editor, line, DateTime.Now, DateTime.Now);
            }

            #region recode PCBStatusEx for table structure
            
              IList<TbProductStatus> preStatusList = mbRepository.GetMBStatus(new List<string>() { mb.Sn });
              mbRepository.UpdatePCBPreStationDefered(CurrentSession.UnitOfWork,
                                                                                  preStatusList);

            #endregion
            #region record previous PCB Status
            //IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //System.Data.DataTable preStatus = CreateDataTable.CreateProductStatusTb();
            //preStatus.Rows.Add(mb.Sn,
            //                                   mb.MBStatus.Station,
            //                                    mb.MBStatus.Status== MBStatusEnum.Pass ? 1 : 0,
            //                                    "",
            //                                    mb.MBStatus.Line,
            //                                    mb.MBStatus.TestFailCount,
            //                                    mb.MBStatus.Editor,
            //                                    //mb.MBStatus.Udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
            //                                    mb.MBStatus.Udt
            //                                    );

            //System.Data.DataTable curStatus = CreateDataTable.CreateProductStatusTb();
            //curStatus.Rows.Add(mb.Sn,
            //                                   status.Station,
            //                                   status.Status == MBStatusEnum.Pass ? 1 : 0,
            //                                   "",
            //                                   status.Line,
            //                                   status.TestFailCount,
            //                                   status.Editor,
            //                                   //status.Udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
            //                                   status.Udt
            //                                   );

            //SqlParameter para1 = new SqlParameter("PreStatus", System.Data.SqlDbType.Structured);
            //para1.Direction = System.Data.ParameterDirection.Input;
            //para1.Value = preStatus;

            //SqlParameter para2 = new SqlParameter("Status", System.Data.SqlDbType.Structured);
            //para2.Direction = System.Data.ParameterDirection.Input;
            //para2.Value = curStatus;


            //productRepository.ExecSpForNonQueryDefered(CurrentSession.UnitOfWork,
            //                                                                                 IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_PCA,
            //                                                                                 "IMES_UpdatePCBStatus",
            //                                                                                para1,
            //                                                                                para2);
            #endregion
            mb.MBStatus = status;
            
            mbRepository.Update(mb, CurrentSession.UnitOfWork);
            
            return base.DoExecute(executionContext);
        }
    }
}

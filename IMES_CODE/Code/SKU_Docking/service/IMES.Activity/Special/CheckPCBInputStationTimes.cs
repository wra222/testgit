
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.PCA.MB;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CheckPCBInputStationTimes
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// 1.GetMBLog
    /// 2.Check MBLog 筆數是否大於設定Qty
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    /// 1.沒有找到MBLog
    /// 2.未設定OverInputTimes
    /// 3.MBLog超過OverInputTimes
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class CheckPCBInputStationTimes : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckPCBInputStationTimes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check MBLog 是否小於OverInputTimes 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (string.IsNullOrEmpty(CheckLogStation))
            {
                CheckLogStation = this.Station;
            }
            string key = this.Key;
            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            IList<MBLog> MBLogList = iMBRepository.GetMBLog(key, CheckLogStation);
            if (OverInputTimes < 0)
            {
                throw new FisException("CHK1051", new string[] {});
            }
            if (MBLogList.Count >= OverInputTimes)
            {
                List<string> errpara = new List<string>();
                errpara.Add(CheckLogStation);
                errpara.Add(MBLogList.Count.ToString());
                throw new FisException("CHK1066",errpara);
                //%1站點已做過%2次數，請報廢
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckLogStationProperty = DependencyProperty.Register("CheckLogStation", typeof(string), typeof(CheckPCBInputStationTimes));

        /// <summary>
        /// CheckLogStation
        /// </summary>
        [DescriptionAttribute("CheckLogStation")]
        [CategoryAttribute("InArguments Of CheckPCBInputStationTimes")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckLogStation
        {
            get
            {
                return ((string)(base.GetValue(CheckPCBInputStationTimes.CheckLogStationProperty)));
            }
            set
            {
                base.SetValue(CheckPCBInputStationTimes.CheckLogStationProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OverInputTimesProperty = DependencyProperty.Register("OverInputTimes", typeof(int), typeof(CheckPCBInputStationTimes));

        /// <summary>
        /// OverInputTimes
        /// </summary>
        [DescriptionAttribute("OverInputTimes")]
        [CategoryAttribute("InArguments Of CheckPCBInputStationTimes")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int OverInputTimes
        {
            get
            {
                return ((int)(base.GetValue(CheckPCBInputStationTimes.OverInputTimesProperty)));
            }
            set
            {
                base.SetValue(CheckPCBInputStationTimes.OverInputTimesProperty, value);
            }
        }
        
    }
}

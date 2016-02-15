using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Station;
using IMES.DataModel;
using System.Collections.Generic;
using System.Linq;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel;
using System;
using IMES.Common;
namespace IMES.Activity
{
    public partial class CheckAutoICT : BaseActivity
    {
        /// <summary>
        /// InitializeComponent
        /// </summary>
        public CheckAutoICT()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Set AOI Station
        /// </summary>
        public static DependencyProperty AOIStationProperty = DependencyProperty.Register("AOIStation", typeof(string), typeof(CheckAutoICT), new PropertyMetadata("0B"));
        /// <summary>
        /// Set AOI Station
        /// </summary>
        [DescriptionAttribute("AOIStation")]
        [CategoryAttribute("AOIStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AOIStation
        {
            get
            {
                return ((string)(base.GetValue(CheckAutoICT.AOIStationProperty)));
            }
            set
            {
                base.SetValue(CheckAutoICT.AOIStationProperty, value);
            }
        }

        /// <summary>
        /// DoExecute
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //MB
            IMB mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
             if (mb == null )
            {
                FisException e = new FisException("CHK002", new string[] { });
                e.stopWF = true;
                throw e;
            }
             string fixtureId = (string)CurrentSession.GetValue(Session.SessionKeys.FixtureID);
            if (!IsAllowDoTest(mb.Sn.Substring(0,2), fixtureId) )
            {
                FisException e = new FisException("CHK1098", new string[] { fixtureId, mb.MBCode });
                e.stopWF = true;
                throw e;
            }
           if(!CheckAOI(mb))
           {
               FisException e = new FisException("CHK1099", new string[] {});
                e.stopWF = true;
                throw e;
           }
           return base.DoExecute(executionContext);
        }

        private bool IsAllowDoTest(string mbCode, string fixtureId)
        {
            IList<ConstValueInfo> constValueList = ActivityCommonImpl.Instance.GetConstValueListByType("MBCodeToICTFixtureId", "");
            return (constValueList.Count > 0 &&
                        constValueList.Any(x => x.name == mbCode && x.value.Split(new Char[] { ',', '~' }).Contains(fixtureId)));
        }
        private bool CheckAOI(IMB mb)
        {
            IList<ConstValueInfo> constValueList = ActivityCommonImpl.Instance.GetConstValueListByType("CheckAOILine", "");
            bool b=true;
            if (constValueList.Count > 0 && constValueList.Any(x => x.value == this.Line))
            {
                IList<MBLog> MBLogList = mb.MBLogs.Where(x => x.StationID ==AOIStation).ToList();
                b = MBLogList.Count > 0 && MBLogList.OrderByDescending(x => x.Cdt).First().Status == 1;
               
            }
            return b;       
        }
    }

}

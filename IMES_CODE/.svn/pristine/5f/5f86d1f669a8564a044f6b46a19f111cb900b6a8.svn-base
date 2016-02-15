/*
 * INVENTEC corporation (c)2015 all rights reserved. 
 * Description: Generate Small MB
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2015-12-09   Vincent            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.Extend;
using IMES.Common;
using IMES.Infrastructure.Extend.Dictionary;
using IMES.Infrastructure.Util;
using System.ComponentModel;

namespace IMES.Activity
{   
    /// <summary>
    /// GenSmallMB
    /// </summary>
    public partial class GenSmallMB : BaseActivity
    {
        /// <summary>
        /// GenSmallMB
        /// </summary>
        public GenSmallMB()
        {
            InitializeComponent();
        }

        private const string PrintLogName = "SmallBoardMB";
        private const string PrintLogDescr = "ParentMB:{0} MB:{1} SB:{2}";


        /// <summary>
        ///  多連板設置
        /// </summary>
        public static DependencyProperty IsSplitMBProperty = DependencyProperty.Register("IsSplitMB", typeof(bool), typeof(GenSmallMB), new PropertyMetadata(true));

        /// <summary>
        ///  多連板設置
        /// </summary>
        [DescriptionAttribute("IsSplitMB")]
        [CategoryAttribute(" IsSplitMB Arguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSplitMB
        {
            get
            {
                return ((bool)(base.GetValue(IsSplitMBProperty)));
            }
            set
            {
                base.SetValue(IsSplitMBProperty, value);
            }
        }     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IMB parentMB = utl.IsNull<IMB>(session, Session.SessionKeys.MB);
          
            IList<String> sbSNList = session.GetValue(ExtendSession.SessionKeys.SmallBoardSNList) as IList<string>;

            if (sbSNList == null || sbSNList.Count == 0)
            {
                return base.DoExecute(executionContext);
            }


            IList<String> smallBoardPartNoList = utl.IsNull <IList<String>>(session, ExtendSession.SessionKeys.SmallBoardPartNoList) as IList<string>;
            IList<String> smallBoardECRList = utl.IsNull <IList<String>>(session, ExtendSession.SessionKeys.SmallBoardECRList) as IList<string>;

            string moNo = parentMB.SMTMO;            
            string custSn = string.Empty;
            string custVer = string.Empty;
            string cvsn = parentMB.Sn;
            string uuid = string.Empty;
             string mac = string.Empty;
            string dateCode = session.GetValue(Session.SessionKeys.DCode) as string;
            //string ecr = session.GetValue(Session.SessionKeys.ECR) as string;
            //string iecVer = session.GetValue(Session.SessionKeys.IECVersion) as string;

            var sbMBList = new List<IMB>();
            session.AddValue(ExtendSession.SessionKeys.SmallBoardMBList, sbMBList);

            IMBRepository mbRep = utl.mbRep;

            IPrintLogRepository LogRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            int index=0;    
            foreach (string sn in sbSNList)
            {
                string model = smallBoardPartNoList[index];
                string[] ecrList = smallBoardECRList[index].Split(GlobalConstName.SlashChar);
                string ecr = ecrList[0];
                string iecVer = ecrList[1];
                string[] sbSN = sn.Split(GlobalConstName.SlashChar);
                MB mb = new MB(sbSN[0], moNo, custSn, model, dateCode, mac, uuid, ecr, iecVer, custVer, cvsn, DateTime.Now, DateTime.Now);
                MBStatus mbStatus = new MBStatus(sbSN[0], this.Station, MBStatusEnum.Pass, this.Editor, this.Line, DateTime.Now, DateTime.Now);
                mb.MBStatus = mbStatus;   

                MBLog newLog = new MBLog(0, mb.Sn, mb.Model, Station, 1, Line, Editor, DateTime.Now);
                mb.AddLog(newLog);
                if (this.IsSplitMB)
                {
                    mb.SetExtendedProperty(ConstName.PCBInfo.ChildMBSN, sbSN[1], this.Editor);
                }
                else
                {
                    mb.SetExtendedProperty(ConstName.PCBInfo.ChildMBSN, parentMB.Sn, this.Editor);
                }
                mb.SetExtendedProperty(ConstName.PCBInfo.ParentMBSN, parentMB.Sn, this.Editor);
                mb.SetExtendedProperty(ConstName.PCBInfo.IsSmallBoard, ConstName.Letter.Y, this.Editor);
                mbRep.Add(mb, session.UnitOfWork);
                sbMBList.Add(mb);

                var item = new PrintLog
                {
                    Name = PrintLogName,
                    BeginNo = sn,
                    EndNo = sn,
                    Station = this.Station,
                    Cdt = DateTime.Now,
                    LabelTemplate = string.Empty,
                    Descr =string.Format(PrintLogDescr,mb.Sn, sbSN[1], sbSN[0])  ,
                    Editor = this.Editor
                };

                LogRep.Add(item, session.UnitOfWork);
                
                IMES.FisObject.PCA.MB.MBInfo items = new IMES.FisObject.PCA.MB.MBInfo();
                items.InfoType = ConstName.PCBInfo.ChildMBSN;
                items.InfoValue = mb.Sn;
                items.Editor = this.Editor;
                parentMB.AddMBInfo(items);
                index++;
            }

            return base.DoExecute(executionContext);
        }
    }
}

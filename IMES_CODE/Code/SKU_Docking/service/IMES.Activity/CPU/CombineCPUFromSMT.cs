// INVENTEC corporation (c)2016  all rights reserved. 
// Description: SMT AOI 站母板結合CPU
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2016-02-03  Vincent
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.PAK.DN;
using IMES.Common;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// CombineCPUFromSMT
    /// </summary>
    public partial class CombineCPUFromSMT : BaseActivity
    {
        const string CPU = "CPU";
        const string MB = "MB";
        ///<summary>
        ///CombineCPUFromSMT
        ///</summary>
        public CombineCPUFromSMT()
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
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IMB mb = utl.IsNull<IMB>(session, Session.SessionKeys.MB);
            IList<string> cpuSNList = utl.IsNull<IList<string>>(session, ExtendSession.SessionKeys.CPUSnList);
            if (cpuSNList.Count==0)
            {
                throw new FisException("CQCHK1127", new string[] { mb.Sn });
            }
            string fixtureId = session.GetValue<string>(Session.SessionKeys.FixtureID);

            IMBRepository mbRep = utl.mbRep;
            DateTime now =DateTime.Now;
            IList<PCBPartInfo> mbPartList = mbRep.GetPCBPartByPCBNos(new List<String> { mb.Sn });
            IList<PCBPartInfo> snPartList = mbRep.GetPCBPartByPartSnList(cpuSNList);

            IList<int> idList = mbPartList.Select(x => x.ID).Distinct().ToList();
             idList = idList.Union(snPartList.Select(x => x.ID).Distinct()).Distinct().ToList();

             //1.backup PCB_Part
            if (idList.Count>0)
            {
               IList<UnpackPCBPartInfo> bkPCBPartList =new List<UnpackPCBPartInfo>();
                foreach (int id in idList)
                {
                    var item = mbPartList.Where(x=>x.ID == id).FirstOrDefault();
                    if (item == null)
                    {
                        item = snPartList.Where(x => x.ID == id).FirstOrDefault();
                    }

                    if (item != null)
                    {
                        bkPCBPartList.Add(new UnpackPCBPartInfo
                        {
                            BomNodeType = item.BomNodeType,
                            Cdt = item.Cdt,
                            CheckItemType = item.CheckItemType,
                            CustmerPn = item.CustmerPn,
                            Editor = item.Editor,
                            IECPn = item.IECPn,
                            PartNo = item.PartNo,
                            PartSn = item.PartSn,
                            PartType = item.PartType,
                            PCB_PartID = id,
                            PCBNo = item.PCBNo,
                            Station = item.Station,
                            Udt = item.Udt,
                            UEditor = this.Editor,
                            UPdt = now
                        });
                    }
                }
                mbRep.BackupPCBPartDefered(session.UnitOfWork, bkPCBPartList);

                //2.delete PCB_Part
                mbRep.RemovePCBPartByIDsDefered(session.UnitOfWork, idList);

            }

          
            //3.Insert PCB_Part  
             IList<PCBPartInfo> pcbPartList = new List<PCBPartInfo>();
            foreach(string sn in cpuSNList)
            {
                var pcbPartInfo = snPartList.Where(x=>x.PartSn ==sn).FirstOrDefault();
                if (pcbPartInfo == null ) //不存在
               {
                   pcbPartList.Add(new PCBPartInfo
                   {
                       PartSn = sn,
                       PartType = CPU,
                       BomNodeType = MB,
                       CheckItemType = fixtureId?? string.Empty,
                       CustmerPn = string.Empty,
                       IECPn = string.Empty,
                       PartNo = mb.Model,
                       PCBNo = mb.Sn,
                       Station = this.Station,
                       Editor = this.Editor,
                       Cdt = now,
                       Udt = now
                   });
                 
               }
               else
               {
                   pcbPartList.Add(new PCBPartInfo
                  {
                      PartSn = sn,
                      PartType = CPU,
                      BomNodeType = MB,
                      CheckItemType = fixtureId ?? string.Empty,
                      CustmerPn = pcbPartInfo.IECPn,
                      IECPn = string.Empty,
                      PartNo = mb.Model,
                      PCBNo = mb.Sn,
                      Station = this.Station,
                      Editor = this.Editor,
                      Cdt = pcbPartInfo.Cdt,
                      Udt = now
                  });            
               }
            }
            mbRep.InsertPCBPartDefered(session.UnitOfWork, pcbPartList);            
            return base.DoExecute(executionContext);
        }
      
    }
}

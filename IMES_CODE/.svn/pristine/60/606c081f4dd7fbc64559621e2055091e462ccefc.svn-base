// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-02-28   210003                       ITC-1360-0460
// 2012-03-06   210003                       ITC-1360-1109
// 2012-03-06   210003                       ITC-1360-0455
// 2012-03-13   210003                       UC Checnged
// Known issues:
//

using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Process;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.MO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IMES.CheckItemModule.MB.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.MB.Filter.dll")]
    class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //            7.PCA卡站(WC=32)并且Part Check
            //Part Check参见[CI-MES12-SPEC-Common-IMES_HP_PartCheck.xlsx]
            //异常情况：
            //A.	若MB没有找到纪录，则提示” 該主板半制流程尚未完成”
            //B.	若MB已与ProdID绑定，则提示” 該主板已經結合機器！”

            //其他part检查异常不在此描述
            if (partUnit != null)
            {
                Session session = SessionManager.GetInstance.GetSession(((PartUnit)partUnit).ProductId, Session.SessionType.Product);
                string sn = ((PartUnit)partUnit).Sn;
                if (session == null)
                {
                    throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
                }
                if (string.IsNullOrEmpty(station))
                {
                    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.MB.Filter.CheckModule.Check" });
                }

                 IMB mb;
                if (!string.IsNullOrEmpty(sn))
                {
                    var mb_repository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    mb = mb_repository.Find(sn);
                    if (mb == null)
                    {
                        throw new FisException("CHK862", new string[] { });
                    }
                }
                else
                {
                    throw new FisException("CQCHK0019", new string[] { "MB",""});
                }

                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var product = (Product) product_repository.GetProductByMBSn(sn);
                if (product != null)
                {
                    throw new FisException("CHK863", new string[] { });
                }
                // Mantis 324
                if ((string)session.GetValue("IsCPUOnBoard")=="Y"&& string.IsNullOrEmpty(mb.CVSN))
                {
                    throw new FisException("CQCHK1058", new string[] { });
                }
                // Mantis 324

                string IsRCTOMB = (string)session.GetValue("IsRCTOMB");
                string pilotMo = "";
                if (!CheckMBType(sn, IsRCTOMB, mb, out pilotMo))
                {
                    FisException ex = new FisException("CHK1050", new string[] { sn });
                    ex.stopWF = false;
                    throw ex;
                }
                //Check Pilot MO
                if (!string.IsNullOrEmpty(pilotMo))
                {
                    IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
                    PilotMoInfo moInfo = moRep.GetPilotMo(pilotMo);
                    if (moInfo == null)
                    {
                        FisException ex = new FisException("CHK1095", new string[] { pilotMo });
                        ex.stopWF = false;
                        throw ex;
                    }
                    else if (moInfo.state != PilotMoStateEnum.Release.ToString())
                    {
                        FisException ex = new FisException("CHK1096", new string[] { pilotMo, moInfo.state });
                        ex.stopWF = false;
                        throw ex;
                    }
                }
                // check MB hasMBRepaired BY Model
                if (!CheckMBAllowCombine(session, mb))
                {
                    FisException ex = new FisException("CQCHK1125", new string[] { sn });
                    ex.stopWF = false;
                    throw ex;
                }

                IProcessRepository current_process_repository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
                current_process_repository.SFC(session.Line, session.Customer, station, ((PartUnit)partUnit).Sn, "MB");
//                current_process_repository.SFC(session.Line, session.Customer, "32", ((PartUnit)part_unit).Sn, "MB");
               
                
            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.MB.Filter.CheckModule.Check" });

            }
        }

        private bool CheckMBType(string sn, string IsRCTO,IMB mb, out string pilotMo)
        {
            IMBMORepository iMBMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository>();
            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            RctombmaintainInfo condition = new RctombmaintainInfo();
            IList<RctombmaintainInfo> list = new List<RctombmaintainInfo>();
            string MBCode = sn.Substring(0,2);
            string MBFamily = "";
            string MBType = sn.Substring(5,1);
            //IMB MB = (IMB)iMBRepository.Find(sn);
            //Vincent add Check PilotMB
            pilotMo = (string)mb.GetExtendedProperty("PilotMo");
            IPart Part = (IPart)iPartRepository.GetPartByPartNo(mb.PCBModelID);
            MBFamily = Part.Descr;
            if (IsRCTO == "Y")
            {
                if (MBType != "R")
                {
                    condition.code = MBCode;
                    condition.family = MBFamily;
                    //Vincent fixed bug 多連板 case
                    condition.type = "C";   //MBType;
                    list = iMBMORepository.GetRctombmaintainInfoList(condition);
                    if (list.Count == 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (MBType == "R")
                {
                    condition.code = MBCode;
                    condition.family = MBFamily;
                    condition.type = MBType;
                    list = iMBMORepository.GetRctombmaintainInfoList(condition);
                    if (list.Count == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckMBAllowCombine(Session session, IMB mb)
        {
            bool passmb = true;
             IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> matchmodel = partRep.GetConstValueTypeList("Model_MB_ALLPASS").Where(x => Regex.IsMatch(product.Model, x.value)).ToList();
            if (matchmodel != null && matchmodel.Count > 0) 
            {
                IList<MBLog> mblogs = mb.MBLogs.Where(x => x.StationID == "32").ToList();

                if (mblogs != null &&
                     mblogs.Count > 0)
                {
                    passmb = false;
                }
            
            }
            return passmb;
        
        }
    }
}

// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.MO;
using IMES.DataModel;

namespace IMES.CheckItemModule.DockingMB.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingMB.Filter.dll")]
    public class CheckModule: ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //Check: MBSFC[(31,1)—>(32,1)] 注：10码sn如果长11码，去除最后一位校验码
            if (partUnit != null)
            {
                string mbSn = ((PartUnit) partUnit).Sn;
                if (mbSn.Length == 11 && Is10CharSn(mbSn))
                {
                    mbSn = mbSn.Substring(0, 10);
                }
                Session session = SessionManager.GetInstance.GetSession(((PartUnit)partUnit).ProductId, Session.SessionType.Product);
                if (session == null)
                {
                    throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
                }
                if (string.IsNullOrEmpty(station))
                {
                    throw new FisException("CHK174", new[] { "IMES.CheckItemModule.DockingMB.Filter.CheckModule.Check" });
                }
                IProcessRepository currentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
                currentProcessRepository.SFC(session.Line, session.Customer, "32", mbSn, "MB");

                //Check Pilot MB
                IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                IMB MB = (IMB)mbRep.Find(mbSn);
                //Vincent add Check PilotMB
                string pilotMo = (string)MB.GetExtendedProperty("PilotMo");
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
            }
            else
            {
                throw new FisException("CHK174", new[] { "IMES.CheckItemModule.DockingMB.Filter.CheckModule.Check" });
            }
        }

        bool Is10CharSn(string subject)
        {
            if ((subject.Length == 10 || subject.Length == 11) && (string.Compare(subject.Substring(4, 1), "M") == 0 || string.Compare(subject.Substring(4, 1), "B") == 0))
            {
                return true;
            }
            return false;
        }

    }
}

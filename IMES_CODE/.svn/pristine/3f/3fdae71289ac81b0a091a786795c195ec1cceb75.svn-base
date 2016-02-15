using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class SATestCheckRuleManager : MarshalByRefObject,ISATestCheckRule
    {

        #region ISATestCheckRule 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        public int AddSATestCheckRuleItem(IMES.DataModel.SATestCheckRuleDef def)
        {
            int id=0;
            try 
            {
                IList<PcaTestCheckInfo> count = itemRepository.CheckSATestCheckRuleExist(def.code.Trim());
                if(count.Count>0)
                {
                    List<string> param = new List<string>();
                    FisException fe = new FisException("DMT147",param);
                    throw fe;
                }
                PcaTestCheckInfo info = new PcaTestCheckInfo();
                PO2VO(def, info);

                itemRepository.AddSATestCheckRuleItem(info);
                id = info.id;
            }
            catch(Exception)
            {
                throw;
            }
            return id;
        }

        private static void PO2VO(IMES.DataModel.SATestCheckRuleDef def, PcaTestCheckInfo info)
        {
            info.id = def.id;
            info.code = def.code;
            info.mac = (def.mac == "Y" ? "Y" : "N");
            info.mbct = (def.mbct == "Y" ? "Y" : "N");
            info.hddv = def.hddv;
            info.bios = def.bios;
            info.editor = def.editor;
            info.cdt = Convert.ToDateTime(def.cdt);
            info.udt = Convert.ToDateTime(def.udt);
        }

        public IList<IMES.DataModel.SATestCheckRuleDef> GetAllSATestItems()
        {
           IList<SATestCheckRuleDef> dataLst=new List<SATestCheckRuleDef>();
            try 
            {
               IList<PcaTestCheckInfo> infLst= itemRepository.GetAllSATestCheckRuleItems();
               VO2PO(dataLst, infLst);
            }
            catch(Exception)
            {
                throw;
            }
            return dataLst;
        }

        private static void VO2PO(IList<SATestCheckRuleDef> dataLst, IList<PcaTestCheckInfo> infLst)
        {
            foreach (PcaTestCheckInfo tmp in infLst)
            {
                SATestCheckRuleDef def = new SATestCheckRuleDef();
                def.id = tmp.id;
                def.code = tmp.code;
                def.mac = (tmp.mac.ToUpper() == "Y" ? "Y" : "N");
                def.mbct = (tmp.mbct.ToUpper() == "Y" ? "Y" : "N");
                def.hddv = tmp.hddv;
                def.bios = tmp.bios;
                def.editor = tmp.editor;
                def.cdt = tmp.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                def.udt = tmp.udt.ToString("yyyy-MM-dd hh:mm:ss");
                dataLst.Add(def);
            }
        }

        public void RemoveSATestCheckRuleItem(int id)
        {
            try 
            {
                itemRepository.RemoveSATestCheckRuleItem(id);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int UpdateSATestCheckRuleItem(IMES.DataModel.SATestCheckRuleDef def)
        {
            int id = 0;
            try 
            {
                IList<PcaTestCheckInfo> count = itemRepository.CheckSATestCheckRuleExist(def.code.Trim());
                if (count.Count > 0)
                {
                    if (def.id == count[0].id)
                    {
                        PcaTestCheckInfo info = new PcaTestCheckInfo();
                        PO2VO(def, info);
                        itemRepository.UpdateTestCheckRuleItem(info, def.id);
                        id = info.id;
                    }
                    else 
                    {
                        List<string> param = new List<string>();
                        FisException fe = new FisException("DMT147", param);
                        throw fe;
                    }
                   
                }
               
            }
            catch(Exception)
            {
                throw;
            }
            return id;
        }

        #endregion
    }
}

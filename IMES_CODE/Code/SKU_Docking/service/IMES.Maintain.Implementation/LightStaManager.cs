using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class LightStaManager : MarshalByRefObject,ILightSta
    {

        #region ILightSta 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
        public int AddLightStationItem(IMES.DataModel.LightStDef def)
        {
            int id=0;
            try 
            {
                IList<FaPaLightstInfo> infLst=itemRepository.CheckFaPaLightStationExist(def.iecpn, def.family, def.sation);
                if (infLst.Count>0)
                {
                    List<string> param = new List<string>();
                    FisException fe = new FisException("DMT133", param);
                    throw fe;
                }
                else 
                {
                    FaPaLightstInfo info = new FaPaLightstInfo();
                    PO2VO(def, info);
                    itemRepository.AddFaPaLightStationItem(info);
                    id = info.id;
                }
            }
            catch(Exception)
            {
                throw;
            }
            return id;
        }

        private static void PO2VO(IMES.DataModel.LightStDef def, FaPaLightstInfo info)
        {
            info.id = def.id;
            info.pno = def.iecpn;
            info.stn = def.sation;
            info.family = def.family;
            info.editor = def.editor;
            info.cdt = Convert.ToDateTime(def.cdt);
            info.udt = Convert.ToDateTime(def.udt);
        }

        public void DelelteLightStationItem(int id)
        {
            try 
            {
                itemRepository.DeleteFaPaLightStationItem(id);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.LightStDef> GetAllLightStation()
        {
            IList<LightStDef> datalst = new List<LightStDef>();
            try 
            {
                IList<FaPaLightstInfo> voList=itemRepository.GetAllFaPaLightStations();
                
                VO2PO(voList, datalst);
            }
            catch(Exception ee)
            {
                throw;
            }
            return datalst;
        }

        private static void VO2PO(IList<FaPaLightstInfo> voList, IList<LightStDef> datalst)
        {
            if (voList != null)
            {
                foreach (FaPaLightstInfo info in voList)
                {
                    LightStDef def = new LightStDef();
                    def.id = info.id;
                    def.iecpn = info.pno;
                    def.family = info.family;
                    def.sation = info.stn;
                    def.editor = info.editor;
                    def.cdt = info.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                    def.udt = info.udt.ToString("yyyy-MM-dd hh:mm:ss");
                    datalst.Add(def);
                }
            }
        }

        public void UpdateLightStationItem(IMES.DataModel.LightStDef def, int id)
        {
            try 
            {
                IList<FaPaLightstInfo> infLst = itemRepository.CheckFaPaLightStationExist(def.iecpn, def.family, def.sation);
                if (infLst!=null&&infLst.Count>0)
                {
                     FaPaLightstInfo info = infLst[0];
                    if (info.id != id)
                    {
                        List<string> param = new List<string>();
                        FisException fe = new FisException("DMT133", param);
                        throw fe;
                    }
                }
               
               
                    FaPaLightstInfo vo = new FaPaLightstInfo();
                    PO2VO(def,vo);
                    itemRepository.UpdateFaPaLightStationItem(vo);
                
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

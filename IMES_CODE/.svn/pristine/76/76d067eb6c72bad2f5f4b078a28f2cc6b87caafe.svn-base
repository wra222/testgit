using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.Infrastructure;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Maintain.Implementation
{
    public class MasterLabelManager : MarshalByRefObject, IMasterLabel
    {

        #region IMasterLabel 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
        public IList<IMES.DataModel.MasterLabelDef> GetAllMasterLabels()
        {
            IList<MasterLabelDef> masterLabelItems = new List<MasterLabelDef>();
            try
            {
                IList<MasterLabelInfo> masterLabelTemps = (IList<MasterLabelInfo>)itemRepository.GetAllMasterLabels();
                if(masterLabelTemps!=null)
                {
                    foreach(MasterLabelInfo info in masterLabelTemps)
                    {
                        MasterLabelDef def = VO2PO(info);
                        masterLabelItems.Add(def);
                    }
                }
            }
            catch (FisException fex)
            {
                logger.Error(fex.Message);
                throw fex;
            }
            catch (System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            return masterLabelItems;
        }

        private static MasterLabelDef VO2PO(MasterLabelInfo info)
        {
            MasterLabelDef def = new MasterLabelDef();
            def.id = info.id;
            def.vc = info.vc.ToUpper();
            def.family = info.family;
            def.code = info.code.ToUpper();
            def.editor = info.editor;
            def.udt = info.udt.ToString("yyyy-MM-dd HH:mm:ss");
            def.cdt = info.cdt.ToString("yyyy-MM-dd HH:mm:ss");
            return def;
        }
        private static MasterLabelInfo PO2VO(MasterLabelDef def)
        {
            MasterLabelInfo inf = new MasterLabelInfo();
            inf.id = def.id;
            inf.vc = def.vc;
            inf.family = def.family;
            inf.editor = def.editor;
            inf.code = def.code;
            inf.cdt = Convert.ToDateTime(def.cdt);
            inf.udt = Convert.ToDateTime(def.udt);
            return inf;
        }
        public void RemoveMasterLabelItem(int id)
        {
            try
            {

                itemRepository.RemoveMasterLabelItem(id);
            }
            catch (FisException fex)
            {
                logger.Error(fex.Message);
                throw fex;
            }
            catch (System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        public IList<IMES.DataModel.MasterLabelDef> GetMasterLabelByVCAndCode(string vc, string family)
        {
            IList<MasterLabelDef> lstMasterLabel = new List<MasterLabelDef>();
            IList<MasterLabelInfo> lstMasterLabelTmp = new List<MasterLabelInfo>();
            try 
            {
                if (String.IsNullOrEmpty(vc) && String.IsNullOrEmpty(family))
                {
                    lstMasterLabelTmp = (IList<MasterLabelInfo>)itemRepository.GetAllMasterLabels();
                }
                else if (String.IsNullOrEmpty(vc))
                {
                    lstMasterLabelTmp = (IList<MasterLabelInfo>)itemRepository.GetMasterLabelByCode(family);
                }
                else if (String.IsNullOrEmpty(family))
                {
                    lstMasterLabelTmp = (IList<MasterLabelInfo>)itemRepository.GetMasterLabelByVC(vc);
                }
                else
                {
                    lstMasterLabelTmp = (IList<MasterLabelInfo>)itemRepository.GetMasterLabelByVCAndCode(vc, family);
                }
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
            if(lstMasterLabelTmp!=null)
            {
                foreach(MasterLabelInfo info in lstMasterLabelTmp)
                {
                    MasterLabelDef def =VO2PO(info);
                    lstMasterLabel.Add(def);
                }
            }
            return lstMasterLabel;
        }

        public void SaveMasterLabelItem(IMES.DataModel.MasterLabelDef ml)
        {
            try 
            {
                IList<MasterLabelInfo> infoLst=new List<MasterLabelInfo>();
                infoLst=(IList<MasterLabelInfo>)itemRepository.GetMasterLabelByVCAndCode(ml.vc.Trim(), ml.family.Trim());
                FisException fe = null;

                if (infoLst.Count == 0)
                {
                    ml.cdt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    ml.udt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    MasterLabelInfo insertInfo = PO2VO(ml);
                    itemRepository.AddMasterLabelItem(insertInfo);
                }
                else 
                {
                    if (infoLst[0].id == ml.id)
                    {
                        ml.udt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        MasterLabelInfo insertInfo = PO2VO(ml);
                        itemRepository.UpdateMasterLabelItem(insertInfo, ml.vc, ml.family);
                    }
                    else 
                    {
                        List<string> param = new List<string>();
                        fe = new FisException("DMT099", param);
                        throw fe;
                    }
                }
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        #endregion

    }
}

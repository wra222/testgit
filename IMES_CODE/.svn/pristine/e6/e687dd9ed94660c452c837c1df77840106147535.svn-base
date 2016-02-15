using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class WeightSettingManager : MarshalByRefObject,IWeightSetting
    {

        #region IWeightSetting 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
        public int AddOrUpdateWeightSettingItem(IMES.DataModel.COMSettingDef def)
        {
            int id=0;
            try 
            {
                string name = def.name;
                IList<COMSettingInfo> tempList=itemRepository.FindCOMSettingByName(name);
                if (tempList != null && tempList.Count > 0)
                {
                    //update
                    //issue code
                    //ITC-1361-0062  itc210012  2012-02-01
                    if (tempList[0].id == def.id)
                    {
                        COMSettingInfo vo = PO2VO(def);
                        vo.id = tempList[0].id;
                        vo.cdt = Convert.ToDateTime(def.cdt);
                        vo.udt = DateTime.Now;
                        itemRepository.UpdateCOMSettingItem(vo);
                        id = vo.id;
                    }
                    else 
                    {
                        List<string> param = new List<string>();
                        throw new FisException("DMT144", param);
                    }
                    
                }
                else 
                {
                    //add
                    COMSettingInfo vo = PO2VO(def);
                    vo.cdt = DateTime.Now;
                    vo.udt = DateTime.Now;
                    itemRepository.AddCOMSettingItem(vo);
                    id = vo.id;
                }
            }
            catch(Exception)
            {
                throw;
            }
            return id;
        }

        private static COMSettingInfo PO2VO(IMES.DataModel.COMSettingDef def)
        {
            COMSettingInfo vo = new COMSettingInfo();
            vo.id = def.id;
            vo.name = def.name;
            vo.commPort = def.commport;
            vo.baudRate = def.baudRate;
            vo.rthreshold = Convert.ToInt32(def.rthreshold);
            vo.sthreshold = Convert.ToInt32(def.sthreshold);
            vo.handshaking = Convert.ToInt32(def.handshaking);
            vo.editor = def.editor;
            //vo.cdt = Convert.ToDateTime(def.cdt);
            //vo.udt = Convert.ToDateTime(def.udt);
            return vo;
        }

        public IList<COMSettingDef> GetAllWeightSettingItems()
        {
            IList<COMSettingDef> dataList = new List<COMSettingDef>();
            try 
            {
                IList<COMSettingInfo> voList = itemRepository.GetAllCOMSetting();
                if(voList!=null)
                {
                    foreach (COMSettingInfo inf in voList)
                    {
                        COMSettingDef po = new COMSettingDef();
                        po.id = inf.id;
                        po.name = inf.name;
                        po.commport = inf.commPort;
                        po.baudRate = inf.baudRate;
                        po.rthreshold = inf.rthreshold.ToString() ;
                        po.sthreshold = inf.sthreshold.ToString();
                        po.handshaking = inf.handshaking.ToString();
                        po.editor = inf.editor;
                        po.cdt = inf.cdt.ToString("yyyy-MM-dd hh:mm:ss") ;
                        po.udt = inf.udt.ToString("yyyy-MM-dd hh:mm:ss");
                        dataList.Add(po);
                    }
                }

            }
            catch(Exception)
            {
                throw;
            }
            return dataList;
        }

        public void RemoveWeightSettingItem(IMES.DataModel.COMSettingDef def)
        {
            try 
            {
                if(def.id!=0)
                {
                    itemRepository.RemoveCOMSettingItem(def.id);
                }
                
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using log4net;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.StandardWeight;



//using IMES.FisObject.Common.FisBOM;
//using IMES.FisObject.Common.Warranty;
//using IMES.FisObject.Common.NumControl;
//using IMES.FisObject.Common.Station;
//using IMES.FisObject.Common.Model;
//using IMES.FisObject.Common.Part;
//using IMES.FisObject.Common.Defect;
//using IMES.FisObject.FA.Product;
//using IMES.FisObject.PAK.DN;
//using IMES.FisObject.PAK.Pallet;
//using IMES.FisObject.Common.CheckItem;
//using IMES.FisObject.PCA.MB;
//using IMES.FisObject.Common.Repair;
//using IMES.FisObject.Common.Misc;
//using IMES.FisObject.Common.MO;
//using IMES.FisObject.PCA.MBModel;
//using IMES.FisObject.Common.Line;
//using IMES.FisObject.PCA.MBMO;
//using IMES.Infrastructure;
//using IMES.Infrastructure.FisObjectBase;


namespace IMES.Maintain.Implementation
{
    public class PalletTypeforICC : MarshalByRefObject, IPalletTypeforICC
    {
        IPalletTypeRepository iPalletTypeRepository = RepositoryFactory.GetInstance().GetRepository<IPalletTypeRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<string> GetShipWay()
        {
            logger.Debug("(PalletTypeforICC)GetShipWay start");
            try
            {
                return iPalletTypeRepository.GetShipWay();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)GetShipWay end");
            }
            
        }

        public IList<string> GetRegId()
        {
            logger.Debug("(PalletTypeforICC)GetRegId start");
            try
            {
                return iPalletTypeRepository.GetRegId();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)GetRegId end");
            }
            
        }

        public IList<PalletTypeInfo> GetPalletType(string shipWay, string regId)
        {
            logger.Debug("(PalletTypeforICC)GetPalletType start, [shipWay]:" + shipWay);
            IList<PalletTypeInfo> ret = null;

            try
            {
                IList<PalletType> lstPalletType = new List<PalletType>();
                if (shipWay == "ALL" && regId == "ALL")
                {
                    lstPalletType = iPalletTypeRepository.FindAll();
                }
                else if (shipWay == "ALL" && regId != "ALL")
                {
                    lstPalletType = iPalletTypeRepository.GetPalletTypeByRegId(regId);
                }
                else if (shipWay != "ALL" && regId == "ALL")
                {
                    lstPalletType = iPalletTypeRepository.GetPalletType(shipWay);
                }
                else if (shipWay != "ALL" && regId != "ALL")
                {
                    lstPalletType = iPalletTypeRepository.GetPalletType(shipWay, regId);
                }

                //IList<PalletType> lstPalletType = iPalletTypeRepository.GetPalletType(shipWay, regId);
                if (lstPalletType != null)
                {
                    ret = new List<PalletTypeInfo>();

                    foreach (PalletType e in lstPalletType)
                    {
                        ret.Add(PalletType_To_PalletTypeInfo(e));
                    }
                }
                
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)GetPalletType end, [shipWay]:" + shipWay);
            }
            return ret;
        }

        public void Remove(PalletTypeInfo item)
        {
            logger.Debug("(PalletTypeforICC)Remove start, [item]:" + item);
            PalletType lstPalletType = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                lstPalletType = iPalletTypeRepository.Find(item.ID);
                if (lstPalletType != null)
                {
                    iPalletTypeRepository.Remove(lstPalletType, work);
                }
                work.Commit();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)Remove end, [item]:" + item);
            } 
        }

        public IList<PalletTypeInfo> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int maxQty, int minQty)
        {
            logger.Debug("(PalletTypeforICC)CheckExistRangeQty start, [shipWay]:" + shipWay);
            IList<PalletTypeInfo> ret = null;

            try
            {
                IList<PalletType> lstPalletType = iPalletTypeRepository.CheckExistRangeQty(shipWay, regId, stdPltFullQty, maxQty, minQty);

                if (lstPalletType != null)
                {
                    ret = new List<PalletTypeInfo>();
                    foreach (PalletType e in lstPalletType)
                    {
                        ret.Add(PalletType_To_PalletTypeInfo(e));
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)CheckExistRangeQty end, [shipWay]:" + shipWay);
            }
            return ret;
        }

        public IList<PalletTypeInfo> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int palletLayer, string oceanType, int maxQty, int minQty)
        {
            logger.Debug("(PalletTypeforICC)CheckExistRangeQty start, [shipWay]:" + shipWay);
            IList<PalletTypeInfo> ret = null;
            
            try
            {
                //IList<PalletType> lstPalletType = iPalletTypeRepository.CheckExistRangeQty(shipWay, regId,stdPltFullQty,palletLayer,maxQty,minQty);
                IList<PalletType> lstPalletType = iPalletTypeRepository.CheckExistRangeQty(shipWay, regId, stdPltFullQty, palletLayer, oceanType, maxQty, minQty);
                
                if (lstPalletType != null)
                {
                    ret = new List<PalletTypeInfo>();
                    foreach (PalletType e in lstPalletType)
                    {
                        ret.Add(PalletType_To_PalletTypeInfo(e));
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)CheckExistRangeQty end, [shipWay]:" + shipWay);
            }
            return ret;
        }

        public void Update(PalletTypeInfo item)
        {
            logger.Debug("(PalletTypeforICC)Update start, [item]:" + item);
            PalletType PalletTypeOld = null;
            PalletType PalletTypeNew = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                PalletTypeOld = iPalletTypeRepository.Find(item.ID);
                PalletTypeNew = PalletTypeInfo_To_PalletType(item);

                PalletTypeOld.ID = PalletTypeNew.ID;
                PalletTypeOld.ShipWay = PalletTypeNew.ShipWay;
                PalletTypeOld.RegId = PalletTypeNew.RegId;
                PalletTypeOld.Type = PalletTypeNew.Type;
                PalletTypeOld.StdPltFullQty = PalletTypeNew.StdPltFullQty;
                PalletTypeOld.MinQty = PalletTypeNew.MinQty;
                PalletTypeOld.MaxQty = PalletTypeNew.MaxQty;
                PalletTypeOld.Code = PalletTypeNew.Code;
                PalletTypeOld.PltWeight = PalletTypeNew.PltWeight;
                PalletTypeOld.MinusPltWeight = PalletTypeNew.MinusPltWeight;
                PalletTypeOld.CheckCode = PalletTypeNew.CheckCode;
                PalletTypeOld.ChepPallet = PalletTypeNew.ChepPallet;
                PalletTypeOld.PalletLayer = PalletTypeNew.PalletLayer;
                PalletTypeOld.Editor = PalletTypeNew.Editor;
                PalletTypeOld.Cdt = PalletTypeNew.Cdt;
                PalletTypeOld.Udt = PalletTypeNew.Udt;
                PalletTypeOld.OceanType = PalletTypeNew.OceanType;

                if (PalletTypeOld.MinQty > PalletTypeOld.MaxQty)
                {
                    throw new Exception("[MinQty] must be less than [MaxQty]!");
                }
                else
                {
                    iPalletTypeRepository.Update(PalletTypeOld, work);
                    work.Commit();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)Update end, [item]:" + item);
            } 
        }

        public void Add(PalletTypeInfo item)
        {
            logger.Debug("(PalletTypeforICC)Add start, [item]:" + item);
            PalletType PalletType = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                PalletType = PalletTypeInfo_To_PalletType(item);
                if (PalletType.MinQty > PalletType.MaxQty)
                {
                    throw new Exception("[MinQty] must be less than [MaxQty]!");
                }
                else
                {
                    iPalletTypeRepository.Add(PalletType, work);
                    work.Commit();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PalletTypeforICC)Add end, [item]:" + item);
            } 
        }

        private PalletTypeInfo PalletType_To_PalletTypeInfo(PalletType version)
        {
            PalletTypeInfo e = new PalletTypeInfo();
            e.ID = version.ID;
            e.ShipWay = version.ShipWay;
            e.RegId = version.RegId;
            e.PalletType = version.Type;
            e.OceanType = version.OceanType;
            e.StdFullQty = version.StdPltFullQty;
            e.MinQty = version.MinQty;
            e.MaxQty = version.MaxQty;
            e.PalletCode = version.Code;
            e.Weight = version.PltWeight;
            e.InPltWeight = version.MinusPltWeight;
            e.CheckCode = version.CheckCode;
            e.ChepPallet = version.ChepPallet;
            e.PalletLayer = version.PalletLayer;
            e.Editor = version.Editor;
            e.Cdt = version.Cdt;
            e.Udt = version.Udt;
            return e;
        }

        private PalletType PalletTypeInfo_To_PalletType (PalletTypeInfo version)
        {
            PalletType e = new PalletType();
            e.ID = version.ID;
            e.ShipWay = version.ShipWay;
            e.RegId = version.RegId;
            e.Type = version.PalletType;
            e.OceanType = version.OceanType;
            e.StdPltFullQty = version.StdFullQty;
            e.MinQty = version.MinQty;
            e.MaxQty = version.MaxQty;
            e.Code = version.PalletCode;
            e.PltWeight = version.Weight;
            e.MinusPltWeight = version.InPltWeight;
            e.CheckCode = version.CheckCode;
            e.ChepPallet = version.ChepPallet;
            e.PalletLayer = version.PalletLayer;
            e.Editor = version.Editor;
            e.Cdt = version.Cdt;
            e.Udt = version.Udt;
            return e;
        }
    }
}

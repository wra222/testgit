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
using IMES.FisObject.Common.Model;

namespace IMES.Maintain.Implementation
{
    public class OSWIN : MarshalByRefObject, IOSWIN
    {
        IOSWINRepository iOSWINRepository = RepositoryFactory.GetInstance().GetRepository<IOSWINRepository>();
        IFamilyRepository iFamilyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<string> GetOSWINFamily()
        {
            logger.Debug("(OSWIN)GetOSWINList start");
            IList<string> ret = null;
            try
            {
                return ret = iOSWINRepository.GetOSWINFamily();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(OSWIN)GetOSWINList end");
            }
        }

        public IList<string> GetFamilyObjList()
        {
            logger.Debug("(OSWIN)GetOSWINList start");
            IList<IMES.FisObject.Common.Model.Family> lst = null;
            IList<string> ret = new List<string>();
            try
            {
                lst = iFamilyRepository.GetFamilyObjList();
                foreach (IMES.FisObject.Common.Model.Family item in lst)
                {
                    ret.Add(item.FamilyName.Trim());
                }
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(OSWIN)GetOSWINList end");
            }
        }

        public IList<OSWINInfo> GetOSWINList(string Family)
        {
            logger.Debug("(OSWIN)GetOSWINList start");
            IList<OSWINInfo> ret = null;
            IList<IMES.FisObject.Common.Model.OSWIN> lstOSWIN = new List<IMES.FisObject.Common.Model.OSWIN>() ;    
            try
            {
                if (Family == "ALL")
                {
                    lstOSWIN = iOSWINRepository.FindAll();
                }
                else
                {
                    lstOSWIN = iOSWINRepository.GetOSWIN(Family);
                }
                    if (lstOSWIN.Count != 0)
                {
                    ret = new List<OSWINInfo>();

                    foreach (IMES.FisObject.Common.Model.OSWIN e in lstOSWIN)
                    {
                        ret.Add(OSWIN_To_OSWINInfo(e));
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
                logger.Debug("(OSWIN)GetOSWINList end");
            }
            return ret;
        }

        public OSWINInfo CheckExistOSWIN(string Family, string Zmod)
        {
            logger.Debug("(OSWIN)GetOSWINByZmode start");
            OSWINInfo ret = new OSWINInfo();
            try
            {
                IMES.FisObject.Common.Model.OSWIN OSWINInfo = new IMES.FisObject.Common.Model.OSWIN();
                    
                OSWINInfo = iOSWINRepository.GetOSWINByZmode(Family,Zmod);
                if (OSWINInfo != null)
                {
                    ret = OSWIN_To_OSWINInfo(OSWINInfo);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(OSWIN)GetOSWINByZmode end");
            }
            return ret;
        }

        public void Remove(OSWINInfo item)
        {
            logger.Debug("(OSWINOSWIN)Remove start, [item]:" + item);
            IMES.FisObject.Common.Model.OSWIN lstOSWIN = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                lstOSWIN = iOSWINRepository.Find(item.ID);
                if (lstOSWIN != null)
                {
                    iOSWINRepository.Remove(lstOSWIN, work);
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
                logger.Debug("(OSWIN)Remove end, [item]:" + item);
            }
        }

        public void Update(OSWINInfo item)
        {
            logger.Debug("(OSWIN)Update start, [item]:" + item);
            IMES.FisObject.Common.Model.OSWIN OSWINOld = null;
            IMES.FisObject.Common.Model.OSWIN OSWINNew = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                OSWINOld = iOSWINRepository.Find(item.ID);
                OSWINNew = OSWINInfo_To_OSWIN(item);
                OSWINOld.ID = OSWINNew.ID;
                OSWINOld.Family = OSWINNew.Family;
                OSWINOld.Zmode = OSWINNew.Zmode;
                OSWINOld.OS = OSWINNew.OS;
                OSWINOld.AV = OSWINNew.AV;
                OSWINOld.Image = OSWINNew.Image;
                OSWINOld.Editor = OSWINNew.Editor;
                OSWINOld.Cdt = OSWINNew.Cdt;
                OSWINOld.Udt = OSWINNew.Udt;
                iOSWINRepository.Update(OSWINOld, work);
                work.Commit();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(OSWIN)Update end, [item]:" + item);
            }
        }

        public void Add(OSWINInfo item)
        {
            logger.Debug("(OSWIN)Add start, [item]:" + item);
            IMES.FisObject.Common.Model.OSWIN OSWIN = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                OSWIN = OSWINInfo_To_OSWIN(item);
                iOSWINRepository.Add(OSWIN, work);
                work.Commit();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(OSWIN)Add end, [item]:" + item);
            }
        }
        
        private OSWINInfo OSWIN_To_OSWINInfo(IMES.FisObject.Common.Model.OSWIN item)
        {
            OSWINInfo e = new OSWINInfo();
            e.ID = item.ID;
            e.Family = item.Family;
            e.Zmode = item.Zmode;
            e.OS = item.OS;
            e.AV = item.AV;
            e.Image = item.Image;
            e.Editor = item.Editor;
            e.Cdt = item.Cdt;
            e.Udt = item.Udt;
            return e;
        }

        private IMES.FisObject.Common.Model.OSWIN OSWINInfo_To_OSWIN(OSWINInfo item)
        {
            IMES.FisObject.Common.Model.OSWIN e = new IMES.FisObject.Common.Model.OSWIN();
            e.ID = item.ID;
            e.Family = item.Family;
            e.Zmode = item.Zmode;
            e.OS = item.OS;
            e.AV = item.AV;
            e.Image = item.Image;
            e.Editor = item.Editor;
            e.Cdt = item.Cdt;
            e.Udt = item.Udt;
            return e;
        }
    }
}

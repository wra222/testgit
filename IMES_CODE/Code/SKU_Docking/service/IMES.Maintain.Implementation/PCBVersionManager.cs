using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.PCA.EcrVersion;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PCA.PCBVersion;

namespace IMES.Maintain.Implementation
{
    class PCBVersionManager : MarshalByRefObject, IPCBVersion
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IPCBVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IPCBVersionRepository, PCBVersion>();
        #region IPCBVersion members

        public IList<PCBVersionInfo> GetPCBVersionInfoListByFamily(string family)
        {
            logger.Debug("(PCBVersionManager)GetPCBVersionInfoListByFamily start, [family]:" + family);
            IList<PCBVersionInfo> ret = null;
            

            try
            {
                IList<PCBVersion> lstEcrVersion = ier.GetPCBVersion(family);
                if (lstEcrVersion != null)
                {
                    ret = new List<PCBVersionInfo>();

                    foreach (PCBVersion e in lstEcrVersion)
                    {
                        ret.Add(GetPCBVersionInfo(e));
                    }
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCBVersionManager)GetPCBVersionInfoListByFamily end, [family]:" + family);
            }

            return ret;
        }

        public void SavePCBVersion(PCBVersionInfo info)
        {
            logger.Debug("(PCBVersionManager)SavePCBVersion start, [info]:" + info);
            IUnitOfWork work = new UnitOfWork();
            IList<PCBVersion> lstPCBVersion = null;

            try
            {
                lstPCBVersion = ier.GetPCBVersion(info.Family, info.MBCode);

                if (lstPCBVersion == null || lstPCBVersion.Count == 0)
                {
                    ier.Add(GetPCBVersion(info), work);
                }
                else
                {

                    var q =
                        (from p in lstPCBVersion
                        where p.PCBVer == info.PCBVer
                        select p).Count();

                    
                    if (q == 0)
                    {
                        ConvertPCBVersionInfoTOPCBVersion(lstPCBVersion[0], info);
                        ier.Add(lstPCBVersion[0], work);
                        
                    }
                    else if (q > 0)
                    {
                        foreach (PCBVersion items in lstPCBVersion)
                        {
                            if (items.PCBVer == info.PCBVer)
                            { 
                                ConvertPCBVersionInfoTOPCBVersion(items, info);
                                ier.Update(items, work);
                            }
                        }
                    }
                    else
                    {
                        List<string> param = new List<string>();
                        throw new FisException("DMT137", param);
                    }

                    //if (lstPCBVersion[0].PCBVer == info.PCBVer)
                    //{
                    //    ConvertPCBVersionInfoTOPCBVersion(lstPCBVersion[0], info);
                    //    ier.Update(lstPCBVersion[0], work);
                    //}
                    //else
                    //{
                    //    List<string> param = new List<string>();

                    //    throw new FisException("DMT137", param);
                    //}
                }

                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCBVersionManager)SavePCBVersion end, [info]:" + info);
            }            
        }

        public void DeletePCBVersion(PCBVersionInfo info)
        {
            logger.Debug("(PCBVersionManager)DeletePCBVersion start, [info]:" + info);
            IList<PCBVersion> lstPCBVersion = null;
            IUnitOfWork work = new UnitOfWork();
            try
            {
                lstPCBVersion = ier.GetPCBVersion(info.Family, info.MBCode);
                if (lstPCBVersion != null || lstPCBVersion.Count != 0)
                {
                    ier.Remove(GetPCBVersion(info),work);
                }
                work.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PCBVersionManager)DeletePCBVersion end, [info]:" + info);
            } 
        }

        #endregion

        private PCBVersionInfo GetPCBVersionInfo(PCBVersion version)
        {
            PCBVersionInfo e = new PCBVersionInfo();
            e.Family = version.Family;
            e.MBCode = version.MBCode;
            e.PCBVer = version.PCBVer;
            e.CTVer = version.CTVer;
            e.Supplier = version.Supplier;
            e.Remark = version.Remark;
            e.Editor = version.Editor;
            e.Cdt = version.Cdt;
            e.Udt = version.Udt;
            return e;
        }

        private PCBVersion GetPCBVersion(PCBVersionInfo info)
        {
            PCBVersion e = new PCBVersion();
            e.Family = info.Family;
            e.MBCode = info.MBCode;
            e.PCBVer = info.PCBVer;
            e.CTVer = info.CTVer;
            e.Supplier = info.Supplier;
            e.Remark = info.Remark;
            e.Editor = info.Editor;
            return e;
        }

        private void ConvertPCBVersionInfoTOPCBVersion(PCBVersion e, PCBVersionInfo info)
        {
            e.Family = info.Family;
            e.MBCode = info.MBCode;
            e.PCBVer = info.PCBVer;
            e.CTVer = info.CTVer;
            e.Supplier = info.Supplier;
            e.Remark = info.Remark;
            e.Editor = info.Editor;
        }
    }
}

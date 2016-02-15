/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: implementation for ECR Version
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-04-27 Tong.Zhi-Yong         Create 
 * Known issues:
 */
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

namespace IMES.Maintain.Implementation
{
    class ECRVersionManager : MarshalByRefObject, IECRVersion
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IECRVersion members

        public IList<FamilyInfo> GetFamilyInfoListForECRVersion()
        {
            IList<FamilyInfo> ret = null;
            IEcrVersionRepository ipr = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();

            try
            {
                IList<string> lstFamily = ipr.GetFamilyInfoListForECRVersion();
                
                if (lstFamily != null)
                {
                    ret = new List<FamilyInfo>();

                    foreach (string t in lstFamily)
                    {
                        ret.Add(GetFamilyInfo(t));
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

            return ret;
        }


        public IList<FamilyInfo> GetFamilyInfoListForSA()
        {
            IList<FamilyInfo> ret = null;
            IEcrVersionRepository ipr = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();

            try
            {
                // GetFamilyInfoListForSA();

                IList<string> lstFamily = ipr.GetFamilyInfoListForSA("MB");// .GetFamilyInfoListForECRVersion();

                if (lstFamily != null)
                {
                    ret = new List<FamilyInfo>();

                    foreach (string t in lstFamily)
                    {
                        ret.Add(GetFamilyInfo(t));
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

            return ret;
        }


        public IList<EcrVersionInfo> GetECRVersionInfoListByFamily(string family)
        {
            logger.Debug("(ECRVersionManager)GetECRVersionInfoListByFamily start, [family]:" + family);
            IList<EcrVersionInfo> ret = null;
            IEcrVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();

            try
            {
                IList<EcrVersion> lstEcrVersion = ier.GetECRVersionListByFamily(family);

                if (lstEcrVersion != null)
                {
                    ret = new List<EcrVersionInfo>();

                    foreach (EcrVersion e in lstEcrVersion)
                    {
                        ret.Add(GetEcrVersionInfo(e));
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
                logger.Debug("(ECRVersionManager)GetECRVersionInfoListByFamily end, [family]:" + family);
            }

            return ret;
        }

        public void SaveECRVersion(EcrVersionInfo info)
        {
            logger.Debug("(ECRVersionManager)SaveECRVersion start, [info]:" + info);
            IEcrVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            IUnitOfWork work = new UnitOfWork();
            IList<EcrVersion> lstEcrVersion = null;

            try
            {
                lstEcrVersion = ier.GetECRVersionByFamilyMBCodeAndECR(info.Family, info.MBCode, info.ECR);

                if (lstEcrVersion == null || lstEcrVersion.Count == 0)
                {
                    ier.Add(GetEcrVersion(info), work);
                }
                else
                {
                    if (lstEcrVersion[0].ID == info.ID)
                    {
                        ConvertEcrVersionForUpdate(lstEcrVersion[0], info);
                //        ier.Update(lstEcrVersion[0], work);
                        ier.UpdateEcrVersionMaintainDefered(work, lstEcrVersion[0],info.Family,info.MBCode,info.ECR);
                    }
                    else
                    {
                        List<string> param = new List<string>();

                        throw new FisException("DMT137", param);
                    }
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
                logger.Debug("(ECRVersionManager)SaveECRVersion end, [info]:" + info);
            }            
        }

        public void UpdateECRVersion(EcrVersionInfo info)
        {
            throw new NotImplementedException();
        }

        public void DeleteECRVersion(EcrVersionInfo info)
        {
            logger.Debug("(ECRVersionManager)DeleteECRVersion start, [info]:" + info);
            IEcrVersionRepository ier = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            EcrVersion ecrVersion = null;
            IUnitOfWork work = new UnitOfWork();

            try
            {
                ecrVersion = ier.Find(info.ID);
        //        IList<EcrVersion> tempList = ier.GetECRVersionByFamilyMBCodeAndECR(info.Family, info.MBCode, info.ECR);
       //         ecrVersion = tempList[0];

                if (ecrVersion != null)
                {
                    ier.Remove(ecrVersion, work);

                    work.Commit();
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
                logger.Debug("(ECRVersionManager)DeleteECRVersion end, [info]:" + info);
            } 
        }

        #endregion

        private FamilyInfo GetFamilyInfo(string family)
        {
            FamilyInfo f = new FamilyInfo();

            f.id = family;
            f.friendlyName = family;

            return f;
        }

        private EcrVersionInfo GetEcrVersionInfo(EcrVersion version)
        {
            EcrVersionInfo e = new EcrVersionInfo();

            e.ID = version.ID;
            e.Cdt = version.Cdt;
            e.Udt = version.Udt;
            e.Remark = version.Remark;
            e.ECR = version.ECR;
            e.Editor = version.Editor;
            e.Family = version.Family;
            e.IECVer = version.IECVer;
            e.MBCode = version.MBCode;

            return e;
        }

        private EcrVersion GetEcrVersion(EcrVersionInfo info)
        {
            EcrVersion e = new EcrVersion();

            //e.ID = info.ID;
            //e.Cdt = info.Cdt;
            //e.Udt = info.Udt;
            e.Remark = info.Remark;
            e.ECR = info.ECR;
            e.Editor = info.Editor;
            e.Family = info.Family;
            e.IECVer = info.IECVer;
            e.MBCode = info.MBCode;

            return e;
        }

        private void ConvertEcrVersionForUpdate(EcrVersion e, EcrVersionInfo info)
        {
            e.Remark = info.Remark;
            e.ECR = info.ECR;
            e.Editor = info.Editor;
            e.Family = info.Family;
            e.IECVer = info.IECVer;
            e.MBCode = info.MBCode;
        }
    }
}

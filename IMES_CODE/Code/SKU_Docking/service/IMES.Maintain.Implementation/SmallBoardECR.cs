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
using IMES.FisObject.Common.Misc;

namespace IMES.Maintain.Implementation
{
    class SmallBoardECR : MarshalByRefObject, ISmallBoardECR
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ISmallBoardECR members

        public IList<string> GetFamilyList(string customer)
        {
            IList<string> ret = new List<string>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                FamilyDefecr condition = new FamilyDefecr();
                condition.CustomerID = customer;
                IList<FamilyDefecr> familyList = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.Family, FamilyDefecr>(condition);
                familyList = familyList.OrderBy(x => x.Family).ToList();
                foreach (FamilyDefecr item in familyList)
                {
                    ret.Add(item.Family);
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

        public IList<MBType> GetMBTypeList()
        {
            IList<MBType> ret = new List<MBType>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                ConstValueTypeInfo condition = new ConstValueTypeInfo();
                condition.type = "SmallBoardMBType";
                IList<ConstValueTypeInfo> constValueList = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.ConstValueType, ConstValueTypeInfo>(condition);
                constValueList = constValueList.OrderBy(x => x.description).ToList();
                foreach (ConstValueTypeInfo item in constValueList)
                {
                    MBType value = new MBType();
                    value.Descr = item.description;
                    value.Value = item.value;
                    ret.Add(value);
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

        public IList<SmallBoardECRInfo> GetSmallBoardECRInfo(string family)
        {
            logger.Debug("(ISmallBoardECR)GetSmallBoardECRInfo start, [family]:" + family);
            IList<SmallBoardECRInfo> ret = new List<SmallBoardECRInfo>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                SmallBoardECRInfo condition = new SmallBoardECRInfo();
                condition.Family = family;
                ret = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardECR, SmallBoardECRInfo>(condition);
                ret = ret.OrderBy(x => x.MBCode).ThenBy(x => x.MBType).ThenBy(x => x.ECR).ToList();
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
                logger.Debug("(ISmallBoardECR)GetSmallBoardECRInfo end, [family]:" + family);
            }
            return ret;
        }

        public void SaveSmallBoardECRInfo(SmallBoardECRInfo info)
        {
            logger.Debug("(ISmallBoardECR)SaveECRVersion start, [info]:" + info);
            IList<SmallBoardECRInfo> check = new List<SmallBoardECRInfo>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                SmallBoardECRInfo condition = new SmallBoardECRInfo();
                condition.Family = info.Family;
                condition.MBCode = info.MBCode;
                condition.MBType = info.MBType;
                condition.ECR = info.ECR;
                check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardECR, SmallBoardECRInfo>(condition);
                if (check.Count != 0)
                {
                    if (info.ID != int.MinValue)
                    {
                        check = check.Where(x => x.ID != info.ID).ToList();
                        if (check.Count != 0)
                        {
                            throw new FisException("Data Exist");
                        }
                    }
                    else
                    {
                        throw new FisException("Data Exist");
                    }
                }
                if (info.ID == int.MinValue)
                {
                    info.Cdt = DateTime.Now;
                    info.Udt = DateTime.Now;
                    iMiscRepository.InsertDataWithID<IMES.Infrastructure.Repository._Metas.SmallBoardECR, SmallBoardECRInfo>(info);
                }
                else
                {
                    SmallBoardECRInfo condition2 = new SmallBoardECRInfo();
                    condition2.ID = info.ID;
                    check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardECR, SmallBoardECRInfo>(condition2);
                    SmallBoardECRInfo id = new SmallBoardECRInfo();
                    id.ID = info.ID;
                    info.Cdt = check[0].Cdt;
                    info.Udt = DateTime.Now;
                    iMiscRepository.UpdateDataByID<IMES.Infrastructure.Repository._Metas.SmallBoardECR, SmallBoardECRInfo>(id, info);
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
                logger.Debug("(ISmallBoardECR)SaveECRVersion end, [info]:" + info);
            }            
        }

        public void DeleteSmallBoardECRInfo(SmallBoardECRInfo info)
        {
            logger.Debug("(ISmallBoardECR)DeleteECRVersion start, [info]:" + info);
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                iMiscRepository.DeleteData<IMES.Infrastructure.Repository._Metas.SmallBoardECR, SmallBoardECRInfo>(info);
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
                logger.Debug("(ISmallBoardECR)DeleteECRVersion end, [info]:" + info);
            } 
        }

        #endregion
    }
}

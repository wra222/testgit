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
    class SmallBoardDefine : MarshalByRefObject, ISmallBoardDefine
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region SmallBoardDefine members

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

        public IList<string> GetPartNoList()
        {
            IList<string> ret = new List<string>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                PartDef condition = new PartDef();
                condition.bomNodeType="SB";
                IList<PartDef> temp = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.Part_NEW, PartDef>(condition);
                temp = temp.OrderBy(x => x.partNo).ToList();
                foreach (PartDef item in temp)
                {
                    ret.Add(item.partNo);
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

        public IList<SmallBoardDefineInfo> GetSmallBoardDefineInfo(string family)
        {
            logger.Debug("(SmallBoardDefine)GetSmallBoardDefineInfo start, [family]:" + family);
            IList<SmallBoardDefineInfo> ret = new List<SmallBoardDefineInfo>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                SmallBoardDefineInfo condition = new SmallBoardDefineInfo();
                if (family != "ALL")
                {
                    condition.Family = family;
                }
                ret = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(condition);
                ret = ret.OrderBy(x => x.Family).ThenBy(x => x.Priority).ToList();
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
                logger.Debug("(SmallBoardDefine)GetSmallBoardDefineInfo end, [family]:" + family);
            }
            return ret;
        }

        public void SaveSmallBoardDefineInfo(SmallBoardDefineInfo info)
        {
            logger.Debug("(SmallBoardDefine)SaveSmallBoardDefineInfo start, [info]:" + info);
            IList<SmallBoardDefineInfo> check = new List<SmallBoardDefineInfo>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                SmallBoardDefineInfo qtyCondition = new SmallBoardDefineInfo();
                qtyCondition.Family = info.Family;
                IList<SmallBoardDefineInfo> checkQty = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(qtyCondition);
                if (checkQty.Count != 0)
                {
                    if (checkQty[0].MaxQty != info.MaxQty)
                    {
                        throw new FisException("CQCHK50121", new string[] { });
                    }
                }
                if (info.ID == int.MinValue)
                {
                    throw new FisException("ID is null");
                }

                SmallBoardDefineInfo condition1 = new SmallBoardDefineInfo();
                condition1.Family = info.Family;
                condition1.MBType = info.MBType;
                condition1.PartNo = info.PartNo;
                check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(condition1);
                if (check.Count != 0)
                {
                    if (check[0].ID != info.ID)
                    {
                        throw new FisException(string.Format("此PartNo：{0}已存在Family：{1}、MBType：{2}的配置中。", info.PartNo, info.Family, info.MBType));
                    }
                }
                SmallBoardDefineInfo condition2 = new SmallBoardDefineInfo();
                condition2.Family = info.Family;
                condition2.MBType = info.MBType;
                condition2.Priority = info.Priority;
                check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(condition2);
                if (check.Count != 0)
                {
                    if (check[0].ID != info.ID)
                    {
                        throw new FisException(string.Format("此Priority：{0}已存在Family：{1}、MBType：{2}的配置中。", info.Priority, info.Family, info.MBType));
                    }
                }
                SmallBoardDefineInfo condition3 = new SmallBoardDefineInfo();
                condition3.ID = info.ID;
                check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(condition3);
                SmallBoardDefineInfo id = new SmallBoardDefineInfo();
                id.ID = info.ID;
                info.Cdt = check[0].Cdt;
                info.Udt = DateTime.Now;
                iMiscRepository.UpdateDataByID<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(id, info);
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
                logger.Debug("(SmallBoardDefine)SaveSmallBoardDefineInfo end, [info]:" + info);
            }            
        }

        public void AddSmallBoardDefineInfo(SmallBoardDefineInfo info)
        {
            logger.Debug("(SmallBoardDefine)SaveSmallBoardDefineInfo start, [info]:" + info);
            IList<SmallBoardDefineInfo> check = new List<SmallBoardDefineInfo>();
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                SmallBoardDefineInfo qtyCondition = new SmallBoardDefineInfo();
                qtyCondition.Family = info.Family;
                IList<SmallBoardDefineInfo> checkQty = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(qtyCondition);
                if (checkQty.Count != 0)
                {
                    if (checkQty[0].MaxQty != info.MaxQty)
                    {
                        throw new FisException("CQCHK50121", new string[] { });
                    }
                }
                if (info.ID == int.MinValue)
                {
                    throw new FisException("ID is null");
                }

                SmallBoardDefineInfo condition1 = new SmallBoardDefineInfo();
                condition1.Family = info.Family;
                condition1.MBType = info.MBType;
                condition1.PartNo = info.PartNo;
                check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(condition1);
                if (check.Count != 0)
                {
                    throw new FisException("Data Exist");
                }
                SmallBoardDefineInfo condition2 = new SmallBoardDefineInfo();
                condition2.Family = info.Family;
                condition2.MBType = info.MBType;
                condition2.Priority = info.Priority;
                check = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(condition2);
                if (check.Count != 0)
                {
                    throw new FisException("Data Exist");
                }
                info.Cdt = DateTime.Now;
                info.Udt = DateTime.Now;
                iMiscRepository.InsertDataWithID<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(info);
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
                logger.Debug("(SmallBoardDefine)SaveSmallBoardDefineInfo end, [info]:" + info);
            }
        }

        public void DeleteSmallBoardDefineInfo(SmallBoardDefineInfo info)
        {
            logger.Debug("(SmallBoardDefine)DeleteSmallBoardDefineInfo start, [info]:" + info);
            IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            try
            {
                iMiscRepository.DeleteData<IMES.Infrastructure.Repository._Metas.SmallBoardDefine, SmallBoardDefineInfo>(info);
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
                logger.Debug("(SmallBoardDefine)DeleteSmallBoardDefineInfo end, [info]:" + info);
            } 
        }

        #endregion
    }
}

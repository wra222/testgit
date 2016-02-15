/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-1-20  liu xiaoling          Create 
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.FisObject.Common.Part;
namespace IMES.Maintain.Implementation
{

    class PartForbiddenManager : MarshalByRefObject, IPartForbiddenManager
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();








        #region IPartForbiddenManager 成员

        public IList<PartForbiddenMaintainInfo> getPartForbiddenListByFamily(string strFamily)
        {
            IList<PartForbiddenMaintainInfo> partForbiddenList = new List<PartForbiddenMaintainInfo>();
            try
            {
                IList<PartForbidden> tmpPartForbiddenList = partRepository.GetPartForbiddenListByFamily(strFamily);

                foreach (PartForbidden temp in tmpPartForbiddenList)
                {
                    PartForbiddenMaintainInfo partForbidden = new PartForbiddenMaintainInfo();

                    partForbidden = convertToMaintainInfoFromObj(temp);

                    partForbiddenList.Add(partForbidden);
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

            return partForbiddenList;
        }

        //add/save partforbidden
        public int SavePartForbidden(PartForbiddenMaintainInfo infoPartForbidden)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                PartForbidden partForbiddenObj = null;
                if (infoPartForbidden.Id != 0)
                {
                    partForbiddenObj = partRepository.GetPartForbidden(infoPartForbidden.Id);
                }
                if (partForbiddenObj == null)
                {
                    //检查是否已存在相同的PartForbidden
                    int count = partRepository.CheckExistedPartForbidden(infoPartForbidden.Model, infoPartForbidden.Descr, infoPartForbidden.PartNo, infoPartForbidden.AssemblyCode, infoPartForbidden.Family);
                    if (count > 0)
                    {
                        ex = new FisException("DMT039", paraError);
                        throw ex;
                    }

                    partForbiddenObj = new PartForbidden();
                    partForbiddenObj = convertToObjFromMaintainInfo(partForbiddenObj, infoPartForbidden);

                    IUnitOfWork work = new UnitOfWork();
                    partRepository.AddPartForbiddenDefered(work, partForbiddenObj);
                    work.Commit();

                }
                else
                {

                    partForbiddenObj = convertToObjFromMaintainInfo(partForbiddenObj, infoPartForbidden);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.SavePartForbiddenDefered(work, partForbiddenObj);

                    work.Commit();
                }

                return partForbiddenObj.ID;

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
        }

        public void DeletePartForbidden(int partForbiddenId)
        {
            try
            {
                PartForbidden objPartForbidden = partRepository.GetPartForbidden(partForbiddenId);
                IUnitOfWork work = new UnitOfWork();
                partRepository.DeletePartForbiddenDefered(work, objPartForbidden);
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
        }

        private PartForbiddenMaintainInfo convertToMaintainInfoFromObj(PartForbidden temp)
        {
            PartForbiddenMaintainInfo partForbidden = new PartForbiddenMaintainInfo();

            partForbidden.Id = temp.ID;
            partForbidden.Family = temp.Family;
            partForbidden.Model = temp.Model;
            partForbidden.Descr = temp.Descr;
            partForbidden.PartNo = temp.PartNo;
            partForbidden.Status = temp.Status;
            partForbidden.AssemblyCode = temp.AssemblyCode;
            partForbidden.Editor = temp.Editor;
            partForbidden.Cdt = temp.Cdt;
            partForbidden.Udt = temp.Udt;

            return partForbidden;
        }

        private PartForbidden convertToObjFromMaintainInfo(PartForbidden obj, PartForbiddenMaintainInfo temp)
        {

            obj.ID = temp.Id;
            obj.Family = temp.Family;
            obj.Model = temp.Model;
            obj.PartNo = temp.PartNo;
            obj.Descr = temp.Descr;
            obj.Status = temp.Status;
            obj.AssemblyCode = temp.AssemblyCode;
            obj.Editor = temp.Editor;

            return obj;
        }
        #endregion
    }
}

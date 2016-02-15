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
using IMES.FisObject.Common.FisBOM;
namespace IMES.Maintain.Implementation
{

    class PartManagerEx :MarshalByRefObject,  IPartManagerEx
    {
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPartRepositoryEx partRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IPartRepositoryEx, IPart>();

        public void SavePartEx(PartDef newPart, string oldPartNo)
        {
            try
            {
                partRepositoryEx.SavePartEx(newPart, oldPartNo);
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
        
        public void DeletePartInfoByID(int id)
       {
           try
           {
               partRepositoryEx.DeletePartInfoByID(id);
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

        public void DeletePartInfoByID(int id, string partNo)
        {
            try
            {
                partRepositoryEx.DeletePartInfoByID(id, partNo);
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
        
        public void AddPartInfoForVendorCode(PartInfoMaintainInfo obj)
        {
            IList<PartInfo> lstPartTypePartInfo = new List<PartInfo>();
            try
            {
                    PartInfo partInfo = new PartInfo();
                    partInfo.PN = obj.PartNo;
                    partInfo.InfoType = obj.InfoType;
                    partInfo.InfoValue = obj.InfoValue;
                    partInfo.Editor = obj.Editor;
                    partInfo.Cdt = obj.Cdt;
                    partInfo.Udt = obj.Udt;
                    partRepository.AddPartInfo(partInfo);
               
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
        public IList<PartInfoMaintainInfo> GetPartInfoListByPartNo(string partNo)
        {
            IList<PartInfoMaintainInfo> partList = new List<PartInfoMaintainInfo>();

            try
            {
                partList = partRepositoryEx.GetPartInfoListByPartNo(partNo);
                if (partList.Count != 0)
                {
                    return partList;
                }
                else
                {
                    return null;
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
       
        }

        public IList<PartDef> GetPartByPartialPartNo(string partNo, int rowCount)
        {
            IList<PartDef> partList = new List<PartDef>();

            try
            {
                partList = partRepositoryEx.GetPartListByPartialPartNo(partNo,rowCount);
                if (partList.Count != 0)
                {
                    return partList;
                }
                else
                {
                    return null;
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
        
        
        }


        public IList<PartDef> GetPartListByPartType(string partType, int rowCount)
        {
            IList<PartDef> partList = new List<PartDef>();

            try
            {
                partList = partRepositoryEx.GetPartListByPartType(partType, rowCount);
                if (partList.Count != 0)
                {
                    return partList;
                }
                else
                {
                    return null;
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
        }


        public IList<string> GetProductsFromProduct_Part(string partNo, int rowCount)
        {
            IList<string> lst = null;

            try
            {
                lst = partRepositoryEx.GetProductsFromProduct_Part(partNo, rowCount);
                return lst;
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

        public void DeletePart(string partNo, string editor)
        {
            try
            {
                logger.Info("DeletePart partNo=" + partNo + ", editor=" + editor);
                partRepositoryEx.DeletePartEx(partNo);
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

    }

}

﻿/*
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

    class PartTypeManager :MarshalByRefObject, IPartTypeManager
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();





        #region IPartTypeManager 成员

        public IList<PartTypeMaintainInfo> GetPartTypeList()
        {
            IList<PartTypeMaintainInfo> partTypeList = new List<PartTypeMaintainInfo>();
            try
            {
                IList<PartType> tmpPartTypeList = partRepository.GetPartTypeObjList();

                foreach (PartType temp in tmpPartTypeList)
                {
                    PartTypeMaintainInfo partType = new PartTypeMaintainInfo();

                    partType = convertToMaintainInfoFromObj(temp);

                    partTypeList.Add(partType);
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

            return partTypeList;
        }

        public IList<PartTypeDescMaintainInfo> GetPartTypeDescList(string PartType)
        {
            IList<PartTypeDescMaintainInfo> partTypeDescList = new List<PartTypeDescMaintainInfo>();
            try
            {
                IList<PartTypeDescription> tmpPartTypeDescList = partRepository.GetPartTypeDescriptionList(PartType);

                foreach (PartTypeDescription temp in tmpPartTypeDescList)
                {
                    PartTypeDescMaintainInfo partTypeDesc = new PartTypeDescMaintainInfo();

                    partTypeDesc = convertToMaintainInfoFromObj(temp);

                    partTypeDescList.Add(partTypeDesc);
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

            return partTypeDescList;
        }

        public IList<PartTypeAttributeMaintainInfo> GetPartTypeAttributeList(string PartType)
        {
            IList<PartTypeAttributeMaintainInfo> partTypeAttrList = new List<PartTypeAttributeMaintainInfo>();
            try
            {
                IList<PartTypeAttribute> tmpPartTypeAttrList = partRepository.GetPartTypeAttributes(PartType);

                foreach (PartTypeAttribute temp in tmpPartTypeAttrList)
                {
                    PartTypeAttributeMaintainInfo partTypeAttr = new PartTypeAttributeMaintainInfo();

                    partTypeAttr = convertToMaintainInfoFromObj(temp);

                    partTypeAttrList.Add(partTypeAttr);
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

            return partTypeAttrList;
        }

        public IList<PartTypeMappingMaintainInfo> GetPartTypeMappingList(string PartType)
        {
            IList<PartTypeMappingMaintainInfo> partTypeMappingList = new List<PartTypeMappingMaintainInfo>();
            try
            {
                IList<PartTypeMapping> tmpPartTypeMappingList = partRepository.GetPartTypeMappingList(PartType);

                foreach (PartTypeMapping temp in tmpPartTypeMappingList)
                {
                    PartTypeMappingMaintainInfo partTypeMapping = new PartTypeMappingMaintainInfo();

                    partTypeMapping = convertToMaintainInfoFromObj(temp);

                    partTypeMappingList.Add(partTypeMapping);
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

            return partTypeMappingList;
        }


        public void AddPartType(PartTypeMaintainInfo Object)
        {
            try
            {
                PartType partTypeObj = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();

                partRepository.AddPartTypeDefered(work, partTypeObj);

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
                throw;//new SystemException(e.Message);
            }
        }

        public void SavePartType(PartTypeMaintainInfo Object, string strOldPartType)
        {
            try
            {
                PartType partTypeObj = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();

                partRepository.SavePartTypeDefered(work, partTypeObj, strOldPartType);

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

        public void DeletePartType(string strOldPartType)
        {
            try
            {
                IUnitOfWork work = new UnitOfWork();

                //partRepository.DeletePartTypeDefered(work, strOldPartType);

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

        public int AddPartTypeDescription(PartTypeDescMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                int count;
                count = partRepository.CheckExistedDesc(Object.PartType, Object.Description, "");

                //检查PartTypeDescription的内容是否已和该PartType的其他某个Description相同。若是，则提示异常。
                if (count > 0)
                {
                    ex = new FisException("DMT022", paraError);
                    throw ex;
                }
                else
                {
                    PartTypeDescription objPartTypeDescription = convertToObjFromMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.AddPartTypeDescDefered(work, objPartTypeDescription);

                    work.Commit();
                    return objPartTypeDescription.ID;
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

        public void SavePartTypeDescription(PartTypeDescMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                IList<int> lstId = new List<int>();
                int count;
                count = partRepository.CheckExistedDesc(Object.PartType, Object.Description, Object.ID.ToString());
                //检查PartTypeDescription的内容是否已和该PartType的其他某个Description相同。若是，则提示异常。
                if (count > 0)
                {
                    ex = new FisException("DMT022", paraError);
                    throw ex;
                }
                else//==1，保存
                {
                    PartTypeDescription objPartTypeDescription = convertToObjFromMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.SavePartTypeDescDefered(work, objPartTypeDescription);

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
        }

        public void DeletePartTypeDescription(string strId)
        {
            try
            {
                PartTypeDescMaintainInfo tmpMaintainInfo = new PartTypeDescMaintainInfo();
                tmpMaintainInfo.ID = Int32.Parse(strId);

                PartTypeDescription objPartTypeDescription = convertToObjFromMaintainInfo(tmpMaintainInfo);

                IUnitOfWork work = new UnitOfWork();

                partRepository.DeletePartTypeDescDefered(work, objPartTypeDescription);

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

        public void AddPartTypeAttribute(PartTypeAttributeMaintainInfo Object)
        {
            try
            {
                PartTypeAttribute partTypeAttributeObj = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();

                partRepository.AddPartTypeAttributeDefered(work, partTypeAttributeObj);

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
                throw;//new SystemException(e.Message);
            }
        }

        public void SavePartTypeAttribute(string strOldCode, PartTypeAttributeMaintainInfo Object)
        {
            try
            {
                PartTypeAttribute partTypeAttributeObj = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();

                partRepository.SavePartTypeAttributeDefered(work, partTypeAttributeObj, Object.PartType, strOldCode);

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

        public void DeletePartTypeAttribute(string strPartType, string strOldCode)
        {
            try
            {
                IUnitOfWork work = new UnitOfWork();

                partRepository.DeletePartTypeAttributeDefered(work, strPartType, strOldCode);

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



        public int AddPartTypeMapping(PartTypeMappingMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                int count;
                count = partRepository.CheckExistedSAPType(Object.FISType, Object.SAPType, "");

                //检查SAPType的内容是否已和该PartType的其他某个SAPType相同。若是，则提示异常。
                if (count > 0)
                {
                    ex = new FisException("DMT021", paraError);
                    throw ex;
                }
                else
                {
                    PartTypeMapping objPartTypeMapping = convertToObjFromMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.AddPartTypeMappingDefered(work, objPartTypeMapping);

                    work.Commit();
                    return objPartTypeMapping.ID;
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

        public void SavePartTypeMapping(PartTypeMappingMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                IList<int> lstId = new List<int>();
                int count;
                count = partRepository.CheckExistedSAPType(Object.FISType, Object.SAPType, Object.ID.ToString());
                //检查SAPType的内容是否已和该PartType的其他某个SAPType相同。若是，则提示异常。
                if (count > 0)
                {
                    ex = new FisException("DMT021", paraError);
                    throw ex;
                }
                else//==1，保存
                {
                    PartTypeMapping objPartTypeMapping = convertToObjFromMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.SavePartTypeMappingDefered(work, objPartTypeMapping);

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
        }

        public void DeletePartTypeMapping(string strId)
        {
            try
            {
                PartTypeMappingMaintainInfo tmpMaintainInfo = new PartTypeMappingMaintainInfo();
                tmpMaintainInfo.ID = Int32.Parse(strId);

                PartTypeMapping objPartTypeMapping = convertToObjFromMaintainInfo(tmpMaintainInfo);

                IUnitOfWork work = new UnitOfWork();

                partRepository.DeletePartTypeMappingDefered(work, objPartTypeMapping);

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



        #endregion

        private PartType convertToObjFromMaintainInfo(PartTypeMaintainInfo temp)
        {
            PartType part = new PartType
            {
                PartTypeName = temp.PartType,
                PartTypeGroup = temp.PartTypeGroup,
                Editor = temp.Editor,
                Cdt = temp.Cdt,
                Udt = temp.Udt
            };

            return part;
        }


        private PartTypeMaintainInfo convertToMaintainInfoFromObj(PartType temp)
        {
            PartTypeMaintainInfo part = new PartTypeMaintainInfo();

            part.PartType = temp.PartTypeName;
            part.PartTypeGroup = temp.PartTypeGroup;
            part.Editor = temp.Editor;
            part.Cdt = temp.Cdt;
            part.Udt = temp.Udt;

            return part;
        }


        private PartTypeAttribute convertToObjFromMaintainInfo(PartTypeAttributeMaintainInfo temp)
        {
            PartTypeAttribute part = new PartTypeAttribute();

            part.PartType = temp.PartType;
            part.Code = temp.Code;
            part.Description = temp.Description;
            part.Editor = temp.Editor;
            part.Cdt = temp.Cdt;
            part.Udt = temp.Udt;

            return part;
        }

        
        private PartTypeAttributeMaintainInfo convertToMaintainInfoFromObj(PartTypeAttribute temp)
        {
            PartTypeAttributeMaintainInfo part = new PartTypeAttributeMaintainInfo();

            part.PartType = temp.PartType;
            part.Code = temp.Code;
            part.Description = temp.Description;
            part.Editor = temp.Editor;
            part.Cdt = temp.Cdt;
            part.Udt = temp.Udt;

            return part;
        }


        private PartTypeDescription convertToObjFromMaintainInfo(PartTypeDescMaintainInfo temp)
        {
            PartTypeDescription part = new PartTypeDescription(temp.ID, temp.PartType, temp.Description);

            return part;
        }

        private PartTypeDescMaintainInfo convertToMaintainInfoFromObj(PartTypeDescription temp)
        {
            PartTypeDescMaintainInfo part = new PartTypeDescMaintainInfo();

            part.ID = temp.ID;
            part.PartType = temp.PartType;
            part.Description = temp.Description;

            return part;
        }


        private PartTypeMapping convertToObjFromMaintainInfo(PartTypeMappingMaintainInfo temp)
        {
            PartTypeMapping part = new PartTypeMapping();

            part.ID = temp.ID;
            part.FISType = temp.FISType;
            part.SAPType = temp.SAPType;
            part.Editor = temp.Editor;
            part.Cdt = part.Cdt;
            part.Udt = part.Udt;

            return part;
        }

        private PartTypeMappingMaintainInfo convertToMaintainInfoFromObj(PartTypeMapping temp)
        {
            PartTypeMappingMaintainInfo part = new PartTypeMappingMaintainInfo();

            part.ID = temp.ID;
            part.SAPType = temp.SAPType;
            part.FISType = temp.FISType;
            part.Editor = temp.Editor;
            part.Cdt = temp.Cdt;
            part.Udt = temp.Udt;

            return part;
        }

    }
}
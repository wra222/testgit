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

    class PartTypeManagerEx :MarshalByRefObject, IPartTypeManagerEx
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //     public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        public IPartRepositoryEx partRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IPartRepositoryEx, IPart>();


        #region PartRepositoryEx Function
        public IList<string> GetBomNodeTypeList()
        {
            IList<string> bomList = new List<string>();
            bomList = partRepositoryEx.GetBomNodeTypeList();
            return bomList;

        }
        public IList<string> GetPartTypeList(string bomNodeType)
        {
            IList<string> partTypeList = new List<string>();
            partTypeList = partRepositoryEx.GetPartTypeList(bomNodeType);
            return partTypeList;
            //     partRepositoryEx.DeletePartType(
        }
        public void DeletePartType(string strOldPartType)
        {
            try
            {
                partRepositoryEx.DeletePartTypeByPartType(strOldPartType);
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

        public void DeletePartType(string id, string strOldPartType)
        {
            try
            {
                int ID = int.Parse(id);
                IList<PartType> tmpPartTypeList = partRepositoryEx.GetPartTypeObjList();
                IList<PartType> checkList = tmpPartTypeList.Where(x => x.ID != ID && x.PartTypeName == strOldPartType).ToList();
                if (checkList.Count == 0)
                {
                    partRepositoryEx.DeletePartTypeAttAndDescByPartType(strOldPartType);
                }
                partRepositoryEx.DeletePartTypeByPartType(ID);
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

        private PartType convertToObjFromMaintainInfo(PartTypeMaintainInfo temp)
        {
            PartType part = new PartType
            {
                ID = temp.ID,
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
            part.ID = temp.ID;
            part.PartType = temp.PartTypeName;
            part.PartTypeGroup = temp.PartTypeGroup;
            part.Editor = temp.Editor;
            part.Cdt = temp.Cdt;
            part.Udt = temp.Udt;

            return part;
        }
        public void SavePartType(PartTypeMaintainInfo Object, string strOldPartType)
        {
            try
            {
                PartType partTypeObj = convertToObjFromMaintainInfo(Object);
                partRepositoryEx.SavePartType(partTypeObj, strOldPartType);

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

        public void SavePartType(PartTypeMaintainInfo Object)
        {
            try
            {
                IList<PartType> tmpPartTypeList = partRepositoryEx.GetPartTypeObjList();
                IList<PartType> checkList = tmpPartTypeList.Where(x => x.PartTypeName == Object.PartType && x.PartTypeGroup == Object.PartTypeGroup).ToList();
                if (checkList.Count != 0)
                {
                    throw new FisException("PartType:" + Object.PartType + " BomNodType:" + Object.PartTypeGroup + " is exist");
                }
                PartType partTypeObj = convertToObjFromMaintainInfo(Object);
                partRepositoryEx.SavePartType(partTypeObj);
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

        public void AddPartType(string partTypeName, string partTypeGroup, string editor)
        {
            try
            {
                IList<PartType> tmpPartTypeList = partRepositoryEx.GetPartTypeObjList();
                IList<PartType> checkList = tmpPartTypeList.Where(x => x.PartTypeName == partTypeName && x.PartTypeGroup == partTypeGroup).ToList();
                if (checkList.Count != 0)
                {
                    throw new FisException("PartType:" + partTypeName + " BomNodType:" + partTypeGroup + " is exist");
                }

                PartType partTypeObj = new PartType
                {
                    PartTypeName = partTypeName,
                    PartTypeGroup = partTypeGroup,
                    Editor = editor,
                    Cdt = DateTime.Now,
                    Udt = DateTime.Now
                };
                ////   PartType partTypeObj = convertToObjFromMaintainInfo(Object);
                //   PartType p=new PartType(Object.
                partRepositoryEx.AddPartType(partTypeObj);
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

        public IList<PartTypeMaintainInfo> GetPartTypeList()
        {
            IList<PartTypeMaintainInfo> partTypeList = new List<PartTypeMaintainInfo>();
            try
            {
                IList<PartType> tmpPartTypeList = partRepositoryEx.GetPartTypeObjList();

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



        #endregion
    }
}

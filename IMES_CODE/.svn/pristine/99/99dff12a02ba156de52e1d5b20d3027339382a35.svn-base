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

    class PartManager : MarshalByRefObject, IPartManager
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        public IAssemblyCodeRepository assCodeRepository = RepositoryFactory.GetInstance().GetRepository<IAssemblyCodeRepository>();
        public IBOMRepository bomRepostory = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();


        /// <summary>
        /// 取得所有family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <returns></returns>
        public IList<FamilyDef> GetFamilyInfoList()
        {
            List<IMES.DataModel.FamilyDef> retLst = new List<FamilyDef>();
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                IList<Family> getData = itemRepository.FindAll();
                for (int i = 0; i < getData.Count; i++)
                {
                    FamilyDef item = new FamilyDef();
                    item.CustomerID = getData[i].Customer;
                    item.Descr = getData[i].Description;
                    item.Family = getData[i].FamilyName;
                    item.Editor = getData[i].Editor;
                    retLst.Add(item);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }




        #region added by shhwang on 2011-11-14

        /// <summary>
        /// 从part type表获取PartNodeType
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public IList<PartTypeDef> getPartNodeType(string tp)
        {
            IList<PartTypeDef> partTypeLst = new List<PartTypeDef>();

            try
            {
                partTypeLst = partRepository.GetPartNodeType(tp);
                if (partTypeLst.Count != 0)
                {

                    return partTypeLst;
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

        /// <summary>
        /// 根据part node type获取part数据列表,按照partno排序
        /// </summary>
        /// <param name="PartType"></param>
        /// <returns></returns>
        public IList<PartDef> getListByPartType(string PartType)
        {
            IList<PartDef> partList = new List<PartDef>();

            try
            {
                partList = partRepository.GetListByPartType(PartType);
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

        /// <summary>
        /// 修改part数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partNo"></param>
        public void updatePart(PartDef obj, string partNo)
        {
            try
            {
                partRepository.UpdatePartByPartNo(obj, partNo);
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

        /// <summary>
        /// 添加一条part记录
        /// </summary>
        /// <param name="obj"></param>
        public void addPart(PartDef obj)
        {
            IList<PartDef> pdLst = new List<PartDef>();
            pdLst = partRepository.GetLstByPartNo(obj.partNo);
            try
            {
                if (pdLst != null && pdLst.Count > 0)
                {
                    PartDef pd = new PartDef();
                    if (pd.flag == 0)
                    {
                        pd = pdLst.First();
                        partRepository.UpdatePartByPartNo(pd, pd.partNo);
                    }
                    else
                    {
                        //已经存在具有相同part的记录
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT097", erpara);
                        throw ex;
                    }
                }
                partRepository.AddPart(obj);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 根据partno删除一条记录
        /// </summary>
        /// <param name="partNo"></param>
        public void deletePart(string partNo)
        {
            try
            {
                assCodeRepository.DeleteAssemblyCodeInfoByPN(partNo);
                bomRepostory.DeleteMoBOMByComponent(partNo);
                partRepository.DeletePartInfoByPN(partNo);
                partRepository.DeletePart(partNo);

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

        /// <summary>
        /// 写入PartInfo表一条数据
        /// </summary>
        /// <param name="obj"></param>
        public void addPartInfo(PartInfoMaintainInfo obj)
        {
            IList<PartInfo> lstPartTypePartInfo = new List<PartInfo>();
            try
            {
                lstPartTypePartInfo = partRepository.GetLstPartInfo(obj.PartNo, obj.InfoType, obj.InfoValue);
                if (lstPartTypePartInfo != null && lstPartTypePartInfo.Count > 0)
                {
                    //已经存在具有相同infovalue的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT098", erpara);
                    throw ex;
                }
                else
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

        /// <summary>
        /// 根据InfoType删除PartInfo一条数据
        /// </summary>
        /// <param name="item"></param>
        public void deletePartInfo(string item)
        {
            try
            {
                partRepository.DeletePartInfo(item);
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


        /// <summary>
        /// 联合查询partinfo与parttype表
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        public IList<PartTypeAndPartInfoValue> GetPartTypeAndPartInfoValueListByPartNo(string partNo, string partType)
        {
            IList<PartTypeAndPartInfoValue> lstPartTypePartInfo = new List<PartTypeAndPartInfoValue>();
            try
            {
                lstPartTypePartInfo = partRepository.GetPartTypeAndPartInfoValueListByPartNo(partNo, partType);
                if (lstPartTypePartInfo.Count != 0)
                {
                    return lstPartTypePartInfo;
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

        public IList<PartDef> getLstByBomNode(string bomNode)
        {
            IList<PartDef> partList = new List<PartDef>();

            try
            {
                partList = partRepository.GetLstByBomNode(bomNode);
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

        public IList<PartDef> getLstByPartNo(string partNo)
        {
            IList<PartDef> partList = new List<PartDef>();

            try
            {
                partList = partRepository.GetLstByPartNo(partNo);
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


        /// <summary>
        /// 根据type获取所有Description的数据列表
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public IList<DescTypeInfo> getDescriptionList(string tp)
        {
            IList<DescTypeInfo> lstDesc = new List<DescTypeInfo>();
            try
            {
                lstDesc = partRepository.GetDescriptionList(tp);
                if (lstDesc.Count != 0)
                {
                    return lstDesc;
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

        public IList<PartInfoMaintainInfo> getLstPartInfo(string partno, string infoType, string infoValue)
        {
            IList<PartInfoMaintainInfo> partList = new List<PartInfoMaintainInfo>();
            IList<PartInfo> pinfoLst = new List<PartInfo>();
            try
            {
                PartInfoMaintainInfo partInfo = new PartInfoMaintainInfo();
                pinfoLst = partRepository.GetLstPartInfo(partno, infoType, infoValue);
                if (partList.Count != 0)
                {
                    foreach (PartInfo pf in pinfoLst)
                    {
                        partInfo.PartNo = pf.PN;
                        partInfo.InfoType = pf.InfoType;
                        partInfo.InfoValue = pf.InfoValue;
                        partInfo.Editor = pf.Editor;
                        partInfo.Cdt = pf.Cdt;
                        partInfo.Udt = pf.Udt;
                        partList.Add(partInfo);
                    }
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

        /// <summary>
        /// 修改partinfo数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partno"></param>
        public void updatePartInfo(PartInfoMaintainInfo obj, string partno, string infoType, string infoValue)
        {
            try
            {
                PartInfo pinfo = new PartInfo();
                pinfo.PN = obj.PartNo;
                pinfo.InfoValue = obj.InfoValue;
                pinfo.InfoType = obj.InfoType;
                pinfo.Editor = obj.Editor;
                pinfo.Cdt = obj.Cdt;
                pinfo.Udt = obj.Udt;
                partRepository.UpdatePartInfo(pinfo, partno, infoType, infoValue);
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


        public IList<string> GetPartInfoValueByPartDescr(string desc, string infoType1,string infoType2,string infoValue)
        {
            IList<string> iCode = new List<string>();
            try
            {
                if (infoType2 == "" && infoValue == "")
                {
                    iCode = partRepository.GetPartInfoValueByPartDescr(desc, infoType1);
                }
                else
                {
                    iCode = partRepository.GetPartInfoValueByPartDescr(desc, infoType1, infoType2, infoValue);
                }
                if (iCode != null && iCode.Count > 0)
                {
                    return iCode;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<string> GetInfoValue(string Type)
        {
            IList<string> lstString = new List<string>();
            try
            {
                lstString = partRepository.GetInfoValue(Type);
                if (lstString != null && lstString.Count > 0)
                {
                    return lstString;
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

        #endregion

        #region 原part方法
        /*
        #region IPartManager 成员

        public IList<PartTypeMaintainInfo> GetPartTypeList()
        {
            IList<PartTypeMaintainInfo> partTypeList = new List<PartTypeMaintainInfo>();
            try
            {
                IList<PartType> tmpPartTypeList = partRepository.GetPartTypeObjList();

                foreach (PartType temp in tmpPartTypeList)
                {
                    PartTypeMaintainInfo partType = new PartTypeMaintainInfo();

                    partType = convertToPartTypeMaintainInfoFromPartTypeObj(temp);

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

        public IList<PartTypeDescMaintainInfo> GetPartTypeDescList(string partType)
        {
            IList<PartTypeDescMaintainInfo> partTypeDescList = new List<PartTypeDescMaintainInfo>();
            try
            {
                IList<PartTypeDescription> tmpPartTypeDescList = partRepository.GetPartTypeDescriptionList(partType);

                foreach (PartTypeDescription temp in tmpPartTypeDescList)
                {
                    PartTypeDescMaintainInfo partTypeDesc = new PartTypeDescMaintainInfo();

                    partTypeDesc = convertToPartTypeDescMaintainInfoFromPartTypeDescObj(temp);

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

        public IList<PartMaintainInfo> GetPartList(string PartNo)
        {
            IList<PartMaintainInfo> partList = new List<PartMaintainInfo>();

            try
            {
                IList<IPart> tmpPartList = partRepository.GetPartListByPreStr(PartNo);

                foreach (IPart temp in tmpPartList)
                {
                    PartMaintainInfo part = new PartMaintainInfo();

                    part = convertToPartMaintainInfoFromPartObj(temp);

                    partList.Add(part);
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

            return partList;
        }

        public PartMaintainInfo GetPart(string PartNo)
        {
            PartMaintainInfo part = new PartMaintainInfo();

            try
            {
                IPart partObj = partRepository.Find(PartNo.ToUpper());

                part = convertToPartMaintainInfoFromPartObj(partObj);
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

            return part;
        }
        
        public void AddPart(PartMaintainInfo partInfo)
        {
            try
            {
                IPart partObj = convertToPartObjFromPartMaintainInfo(partInfo);

                IUnitOfWork work = new UnitOfWork();

                partRepository.Add(partObj, work);

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
      
        public void SavePart(PartMaintainInfo partInfo)
        {
            try
            {
                logger.Debug("SavePart");
                logger.Debug("PartNo:" + partInfo.PartNo);
                logger.Debug("PartType:" + partInfo.PartType);
                logger.Debug("Descr:" + partInfo.Descr);
                logger.Debug("Editor:" + partInfo.Editor);
                logger.Debug("CustPartNo:" + partInfo.CustPartNo);

                IPart partObj = partRepository.Find(partInfo.PartNo.ToUpper());

                if (partObj == null)
                {
                    partObj = new Part();

                    partObj = convertToPartObjFromPartMaintainInfo(partObj, partInfo);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.Add(partObj, work);

                    work.Commit();

                }
                else
                {
                    partObj = convertToPartObjFromPartMaintainInfo(partObj, partInfo);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.Update(partObj, work);

                    work.Commit();
                }

            }
            catch (FisException e)
            {
                logger.Debug("FisException:" + e.mErrmsg);
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error("Exception:" + e.Message);
                logger.Error(e.Message);
                throw;
            }
        }

        public void DeletePart(string PartNo)
        {
            try
            {
                IPart part = partRepository.Find(PartNo);

                IUnitOfWork work = new UnitOfWork();

                partRepository.Remove(part, work);

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

        private PartTypeMaintainInfo convertToPartTypeMaintainInfoFromPartTypeObj(PartType temp)
        {
            PartTypeMaintainInfo part = new PartTypeMaintainInfo();

            part.PartType = temp.PartTypeName;
            part.PartTypeGroup = temp.PartTypeGroup;

            return part;
        }

        private PartTypeDescMaintainInfo convertToPartTypeDescMaintainInfoFromPartTypeDescObj(PartTypeDescription temp)
        {
            PartTypeDescMaintainInfo part = new PartTypeDescMaintainInfo();

            part.ID = temp.ID;
            part.PartType = temp.PartType;
            part.Description = temp.Description;

            return part;
        }


        private PartMaintainInfo convertToPartMaintainInfoFromPartObj(IPart temp)
        {
            PartMaintainInfo part = new PartMaintainInfo();

            part.Cdt = temp.Cdt;
            part.CustPartNo = temp.CustPn;
            part.Descr = temp.Descr;
            part.Editor = temp.Editor;
            part.PartNo = temp.PN;
            part.Remark = temp.Remark;
            part.PartType = temp.Type;
            part.Udt = temp.Udt;
            part.AutoDL = temp.AutoDL;
            part.Descr2 = temp.Descr2;

            return part;
        }


        private IPart convertToPartObjFromPartMaintainInfo(IPart objPart, PartMaintainInfo temp)
        {
            //IPart part = new Part(temp.PartNo, temp.PartType, temp.CustPartNo, temp.Descr, temp.FruNo, temp.Vendor, temp.IECVersion, temp.Remark, temp.AutoDL, temp.Editor, temp.Cdt, temp.Udt);

            objPart.CustPn = temp.CustPartNo;
            objPart.Descr = temp.Descr;
            objPart.Editor = temp.Editor;
            objPart.PN = temp.PartNo.ToUpper();
            objPart.Type = temp.PartType;
            objPart.AutoDL = temp.AutoDL;
            objPart.Remark = temp.Remark;
            objPart.Descr2 = temp.Descr2;

            return objPart;
        }

        public IList<AssemblyCodeMaintainInfo> GetAssemblyCodeList(string PartNo)
        {
            IList<AssemblyCodeMaintainInfo> assemblyCodeList = new List<AssemblyCodeMaintainInfo>();

            try
            {
                IList<AssemblyCode> objAssemblyCodeList = partRepository.GetAssemblyCodeList(PartNo);


                foreach (AssemblyCode temp in objAssemblyCodeList)
                {
                    AssemblyCodeMaintainInfo modelinfo = new AssemblyCodeMaintainInfo();

                    modelinfo = convertToAssemblyCodeMaintainInfoFromAssemblyCodeObj(temp);

                    assemblyCodeList.Add(modelinfo);
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

            return assemblyCodeList;
        }


        public IList<AssemblyCodeMaintainInfo> GetAssemblyCodeList(string Model, string PartNo)
        {
            IList<AssemblyCodeMaintainInfo> assemblyCodeList = new List<AssemblyCodeMaintainInfo>();

            try
            {
                IList<AssemblyCode> objAssemblyCodeList = partRepository.GetAssemblyCodeList(Model, PartNo);


                foreach (AssemblyCode temp in objAssemblyCodeList)
                {
                    AssemblyCodeMaintainInfo modelinfo = new AssemblyCodeMaintainInfo();

                    modelinfo = convertToAssemblyCodeMaintainInfoFromAssemblyCodeObj(temp);

                    assemblyCodeList.Add(modelinfo);
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

            return assemblyCodeList;
        }

        public AssemblyCodeMaintainInfo GetAssemblyCode(string strId)
        {
            AssemblyCodeMaintainInfo assemblyCodeInfo = new AssemblyCodeMaintainInfo();
            try
            {
                AssemblyCode objAssemblyCode = partRepository.FindAssemblyCodeById(Int32.Parse(strId));
                assemblyCodeInfo = convertToAssemblyCodeMaintainInfoFromAssemblyCodeObj(objAssemblyCode);
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

            return assemblyCodeInfo;
        }

        public int AddAssemblyCode(AssemblyCodeMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                int count, countSameAssemblyCode;
                count = partRepository.CheckExistedAssemblyCode(Object.PartNo, Object.Family, Object.Model, Object.Region, "");
                countSameAssemblyCode = assCodeRepository.CheckSameAssemblyCode(Object.PartNo, Object.AssemblyCode);

                //检查Assembly Code框中内容是否与其他PartNo的Assembly Code相同。若是，则提示用户
                if (countSameAssemblyCode > 0)
                {
                    ex = new FisException("DMT034", paraError);
                    throw ex;
                }


                //检查Assembly Code的内容是否已和该Part NO的其他某个Assembly Code相同。若是，则提示异常。
                if (count > 0)
                {
                    //paraError.Add(Object.PartNo);
                    //paraError.Add(Object.AssemblyCode);
                    //ex = new FisException("CHK020", paraError);
                    ex = new FisException("DMT009", paraError);//new SystemException("There is the same Rule!");//
                    throw ex;
                }
                else
                {
                    AssemblyCode objAssemblyCode = convertToAssemblyCodeObjFromAssemblyCodeMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.AddAssemblyCodeDefered(work, objAssemblyCode);

                    work.Commit();
                    return objAssemblyCode.ID;
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

        public int SaveAssemblyCode(AssemblyCodeMaintainInfo Object)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                IList<int> lstId = new List<int>();
                int count, countSameAssemblyCode;
                count = partRepository.CheckExistedAssemblyCode(Object.PartNo, Object.Family, Object.Model, Object.Region, Object.Id.ToString());
                countSameAssemblyCode = assCodeRepository.CheckSameAssemblyCode(Object.PartNo, Object.AssemblyCode);

                //检查Assembly Code框中内容是否与其他PartNo的Assembly Code相同。若是，则提示用户
                if (countSameAssemblyCode > 0)
                {
                    ex = new FisException("DMT034", paraError);
                    throw ex;
                }


                //检查Family、Region和Model的内容是否已和其他某个Assembly Code的对应数据相同（若Model不为空，则不比较Region）
                if (count > 0)
                {
                    //paraError.Add(Object.PartNo);
                    //paraError.Add(Object.AssemblyCode);
                    ex = new FisException("DMT009", paraError);//new SystemException("There is the same Rule!");//
                    throw ex;
                }
                else//==1，保存
                {
                    AssemblyCode objAssemblyCode = convertToAssemblyCodeObjFromAssemblyCodeMaintainInfo(Object);

                    IUnitOfWork work = new UnitOfWork();

                    partRepository.UpdateAssemblyCodeDefered(work, objAssemblyCode);

                    work.Commit();
                    return objAssemblyCode.ID;
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

        public void DeleteAssemblyCode(string strId)
        {
            try
            {
                IUnitOfWork work = new UnitOfWork();

                partRepository.DeleteAssemblyCodeDefered(work, Int32.Parse(strId));

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

        private AssemblyCodeMaintainInfo convertToAssemblyCodeMaintainInfoFromAssemblyCodeObj(AssemblyCode temp)
        {
            AssemblyCodeMaintainInfo assemblyCode = new AssemblyCodeMaintainInfo();

            assemblyCode.AssemblyCode = temp.AssCode;
            assemblyCode.Family = temp.Family;
            assemblyCode.PartNo = temp.Pn;
            assemblyCode.Model = temp.Model;
            assemblyCode.Region = temp.Region;
            assemblyCode.Id = temp.ID;
            assemblyCode.Editor = temp.Editor;
            assemblyCode.Cdt = temp.Cdt;
            assemblyCode.Udt = temp.Udt;


            return assemblyCode;
        }

        private AssemblyCode convertToAssemblyCodeObjFromAssemblyCodeMaintainInfo(AssemblyCodeMaintainInfo temp)
        {
            AssemblyCode assemblyCode = new AssemblyCode();

            assemblyCode.Pn = temp.PartNo;
            assemblyCode.Family = temp.Family;
            assemblyCode.AssCode = temp.AssemblyCode;
            assemblyCode.Model = temp.Model;
            assemblyCode.Region = temp.Region;
            assemblyCode.ID = temp.Id;
            assemblyCode.Editor = temp.Editor;
            assemblyCode.Cdt = temp.Cdt;
            assemblyCode.Udt = temp.Udt;

            return assemblyCode;
        }

        /*
        public IList<PartInfoMaintainInfo> GetPartInfoList(string PartNo, string PartType)
        {
            IList<PartInfoMaintainInfo> partInfoList = new List<PartInfoMaintainInfo>();

            try
            {
                IList<PartTypeAttributeAndPartInfoValue> objPartList = partRepository.GetPartTypeAttributeAndPartInfoValueList(PartNo, PartType);


                foreach (PartTypeAttributeAndPartInfoValue temp in objPartList)
                {
                    PartInfoMaintainInfo partinfo = new PartInfoMaintainInfo();

                    partinfo = convertToPartInfoMaintainInfoFromPartInfoObj(temp);

                    partInfoList.Add(partinfo);
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

            return partInfoList;
        }
        

        public int AddPartInfo(string PartNo, PartInfoMaintainInfo Object)
        {
            try
            {
                IPart objPart = partRepository.Find(PartNo);

                PartInfo partInfo = convertToObjFromMaintainInfo(Object);

                objPart.AddAttribute(partInfo);
                IUnitOfWork work = new UnitOfWork();
                partRepository.Update(objPart, work);
                work.Commit();
                return partInfo.ID;
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

        public void SavePartInfo(string PartNo, PartInfoMaintainInfo Object)
        {
            try
            {
                IPart objPart = partRepository.Find(PartNo);

                PartInfo partInfo = convertToObjFromMaintainInfo(Object);

                objPart.ChangeAttribute(partInfo);
                IUnitOfWork work = new UnitOfWork();
                partRepository.Update(objPart, work);
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

        public void DeletePartInfo(string PartNo, PartInfoMaintainInfo Object)
        {

            try
            {
                IPart objPart = partRepository.Find(PartNo);

                PartInfo partInfo = convertToObjFromMaintainInfo(Object);

                objPart.DeleteAttribute(partInfo);
                IUnitOfWork work = new UnitOfWork();
                partRepository.Update(objPart, work);
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

        private PartInfo convertToObjFromMaintainInfo(PartInfoMaintainInfo temp)
        {
            PartInfo partInfo = new PartInfo();

            partInfo.Editor = temp.Editor;
            partInfo.ID = temp.Id;
            partInfo.InfoType = temp.InfoType;
            partInfo.InfoValue = temp.InfoValue;
            partInfo.PN = temp.PartNo;

            return partInfo;
        }

       
        #endregion

        #region IPartManager 成员


        private RegionMaintainInfo convertToMaintainInfoFromObj(Region temp)
        {
            RegionMaintainInfo region = new RegionMaintainInfo();

            region.Region = temp.region;
            region.Description = temp.Description;
            region.Editor = temp.Editor;
            region.Cdt = temp.Cdt;
            region.Udt = temp.Udt;

            return region;
        }

        private PartTypeAttributeAndPartInfoValueMaintainInfo convertToMaintainInfoFromObj(PartTypeAttributeAndPartInfoValue temp)
        {
            PartTypeAttributeAndPartInfoValueMaintainInfo info = new PartTypeAttributeAndPartInfoValueMaintainInfo();

            info.Id = temp.Id;
            info.MainTableKey = temp.MainTblKey;
            info.Editor = temp.Editor;
            info.Cdt = temp.Cdt;
            info.Udt = temp.Udt;
            info.Item = temp.Item;
            info.Content = temp.Content;
            info.Description = temp.Description;

            return info;
        }

        private AssemblyCodeInfo convertToObjFromMaintainInfo(AssemblyCodeInfoMaintainInfo temp)
        {
            AssemblyCodeInfo assemblyCodeInfo = new AssemblyCodeInfo();

            assemblyCodeInfo.Editor = temp.Editor;
            assemblyCodeInfo.ID = temp.Id;
            assemblyCodeInfo.InfoType = temp.InfoType;
            assemblyCodeInfo.InfoValue = temp.InfoValue;
            assemblyCodeInfo.AssemblyCode = temp.AssemblyCode;
            assemblyCodeInfo.Cdt = temp.Cdt;

            return assemblyCodeInfo;
        }


        public IList<RegionMaintainInfo> GetRegionList()
        {
            IList<RegionMaintainInfo> regionList = new List<RegionMaintainInfo>();
            try
            {
                IList<Region> tmpRegionList = partRepository.GetRegionList();

                foreach (Region temp in tmpRegionList)
                {
                    RegionMaintainInfo region = new RegionMaintainInfo();

                    region = convertToMaintainInfoFromObj(temp);

                    regionList.Add(region);
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

            return regionList;
        }


        public PartMaintainInfo getPartByAssemblyCode(string strAssemblyCode)
        {
            PartMaintainInfo part = new PartMaintainInfo();
            FisException ex;
            List<string> paraError = new List<string>();

            try
            {
                IPart partObj = partRepository.GetPartByAssemblyCode(strAssemblyCode);

                if (partObj == null)
                {
                    ex = new FisException("DMT008", paraError);//new SystemException("Invalid Assembly Code!");
                    throw ex;
                }
                else
                {
                    part = convertToPartMaintainInfoFromPartObj(partObj);
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

            return part;
        }

        public IList<PartTypeAttributeAndPartInfoValueMaintainInfo> GetPartTypeAttributeAndPartInfoValueListByPartNo(string partNo, string partType)
        {
            IList<PartTypeAttributeAndPartInfoValueMaintainInfo> partInfoList = new List<PartTypeAttributeAndPartInfoValueMaintainInfo>();

            try
            {
                IList<PartTypeAttributeAndPartInfoValue> objPartList = partRepository.GetPartTypeAttributeAndPartInfoValueListByPartNo(partNo, partType);


                foreach (PartTypeAttributeAndPartInfoValue temp in objPartList)
                {
                    PartTypeAttributeAndPartInfoValueMaintainInfo partinfo = new PartTypeAttributeAndPartInfoValueMaintainInfo();

                    partinfo = convertToMaintainInfoFromObj(temp);

                    partInfoList.Add(partinfo);
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

            return partInfoList;
        }

        public IList<PartTypeAttributeAndPartInfoValueMaintainInfo> GetPartTypeAttributeAndPartInfoValueListByAssemblyCode(string assemblyCode, string partType)
        {
            IList<PartTypeAttributeAndPartInfoValueMaintainInfo> partInfoList = new List<PartTypeAttributeAndPartInfoValueMaintainInfo>();

            try
            {
                IList<PartTypeAttributeAndPartInfoValue> objPartList = partRepository.GetPartTypeAttributeAndPartInfoValueListByAssemblyCode(assemblyCode, partType);


                foreach (PartTypeAttributeAndPartInfoValue temp in objPartList)
                {
                    PartTypeAttributeAndPartInfoValueMaintainInfo partinfo = new PartTypeAttributeAndPartInfoValueMaintainInfo();

                    partinfo = convertToMaintainInfoFromObj(temp);

                    partInfoList.Add(partinfo);
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

            return partInfoList;
        }

        public int AddAssemblyCodeInfo(AssemblyCodeInfoMaintainInfo Object)
        {
            try
            {
                AssemblyCodeInfo assemblyCodeInfo = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();
                partRepository.AddAssemblyCodeInfoDefered(work, assemblyCodeInfo);
                work.Commit();
                return assemblyCodeInfo.ID;
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

        public void SaveAssemblyCodeInfo(AssemblyCodeInfoMaintainInfo Object)
        {
            try
            {
                AssemblyCodeInfo assemblyCodeInfo = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();
                partRepository.SaveAssemblyCodeInfoDefered(work, assemblyCodeInfo);
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

        public void DeleteAssemblyCodeInfo(AssemblyCodeInfoMaintainInfo Object)
        {
            try
            {
                AssemblyCodeInfo assemblyCodeInfo = convertToObjFromMaintainInfo(Object);

                IUnitOfWork work = new UnitOfWork();
                partRepository.DeleteAssemblyCodeInfoDefered(work, assemblyCodeInfo);
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
        */
        #endregion







    }

}

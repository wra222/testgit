// 2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0169
// 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0231
// 2011-01-12 zhulei                  Modify 
// 2011-01-18 zhulei                  Modify 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.DataModel;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.Common.Process;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository.Common;
using IMES.Infrastructure.Utility.Cache;
using IMES.Station.Interface.CommonIntf;
using System.Data;
using IMES.Infrastructure.Repository;
using IMES.Infrastructure.UnitOfWork;
using log4net;
using IMES.Common;
using IMES.Resolve.Common;


namespace IMES.Station.Implementation
{
    public class CommonImpl : MarshalByRefObject, IMB_CODE, I111Level, IPdLine, ISamplea, IMO, IDocType, ITestStation, IFamily, I1397Level, IDefect, ICause, IMajorPart, IComponent, IObligation, IMark, IFloor, IPPIDType, IPPIDDescription,
                              IPartNo, ISubDefect, IResponsibility, I4M, ICover, IUncover, ITrackingStatus, IDistribution, IModel,
                              IProdIdRange, IKPType, IChangeKPType, IBOLNo, IPallet, ISession, IPrintItem,
                              IRepair, IPrintTemplate,
                              IMES.Station.Interface.CommonIntf.IMB,
                              IMES.Station.Interface.CommonIntf.IProduct, IPartType, IQCStatistic, 
                              IDCode, ISMTMO, IShipment, ICache, IKittingCode, ILightCode,IDataMigration,IDeliveryByModel,
                              IConstValue, IDismantleType, ILabelKittingCode, IStationByType, IQty, IPartQuery, IBOMQuery, IExecSP, ICauseItem, IDeliveryByCarton
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Implementation of IMB_CODE


        /// <summary>
        /// 获取全部MBCode信息
        /// </summary>
        /// <returns>MBCode信息列表</returns>
        public IList<MB_CODEAndMDLInfo> GetMbCodeAndMdlList()
        {
            IList<MB_CODEAndMDLInfo> retLst = null;
            try
            {

                IMBModelRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                retLst = mbRepository.GetMBCodeAndMdlList();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// 获取全部未打印的MBCode信息
        /// </summary>
        /// <returns>MBCode信息列表</returns>
        public IList<MB_CODEAndMDLInfo> GetMBCodeAndMdlListExceptPrinted()
        {
            IList<MB_CODEAndMDLInfo> retLst = null;
            try
            {

                IMBModelRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                retLst = mbRepository.GetMBCodeAndMdlListExceptPrinted();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 获取全部MBCode信息(返回值为string,而非结构)
        /// </summary>
        /// <returns>MBCode信息(string)</returns>
        public IList<string> GetMbCodeList()
        {
            IList<string> retLst = null;
            try
            {

                IMBModelRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                retLst = mbRepository.GetMBCodeListFromEcrVersion();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }

        }

        // add by zhulei
        /// <summary>
        /// 获取全部MBCode信息(MB Label Print)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mdlType"></param>
        /// <param name="mdlPostfix"></param>
        /// <returns>MBCode信息列表</returns>
        public IList<MB_CODEAndMDLInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix)
        {
            IList<MB_CODEAndMDLInfo> retLst = new List<MB_CODEAndMDLInfo>();
            IList<MbCodeAndMdlInfo> mbCodeLst = null;
            MB_CODEAndMDLInfo ret;
            try
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                mbCodeLst = mbRepository.GetMbCodeAndMdlInfoList(bomNodeType, mbType, mdlType, mdlPostfix);
                foreach (var mbCode in mbCodeLst)
                {
                    ret.mbCode = mbCode.mbCode;
                    ret.mdl = mbCode.mdl;
                    retLst.Add(ret);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// 获取全部MBCode信息(VGA Label Print)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mdlType"></param>
        /// <param name="mdlPostfix"></param>
        /// <returns>MBCode信息列表</returns>
        public IList<MB_CODEAndMDLInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix, string vgaType, string vgaValue)
        {
            IList<MB_CODEAndMDLInfo> retLst = new List<MB_CODEAndMDLInfo>();
            IList<MbCodeAndMdlInfo> mbCodeLst = null;
            MB_CODEAndMDLInfo ret;
            try
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                mbCodeLst = mbRepository.GetMbCodeAndMdlInfoList(bomNodeType, mbType, mdlType, mdlPostfix, vgaType, vgaValue);
                foreach (var mbCode in mbCodeLst)
                {
                    ret.mbCode = mbCode.mbCode;
                    ret.mdl = mbCode.mdl;
                    retLst.Add(ret);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Implementation of I111Level
        /// <summary>
        /// 根据MBCode获取对应的111 Level信息列表
        /// </summary>
        /// <param name="MB_CODEid">MB_CODE id</param>
        /// <returns>111 Level信息列表</returns>
        public IList<_111LevelInfo> Get111LevelList(string mbCodeId)
        {

            IList<_111LevelInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(mbCodeId))
                {
                    IMBModelRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                    retLst = mbRepository.Get111LevelList(mbCodeId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// 根据MBCode获取对应的所有未打印的111 Level信息
        /// </summary>
        /// <param name="MB_CODEid">MB_CODE id</param>
        /// <returns>111 Level信息列表</returns>
        public IList<_111LevelInfo> Get111LevelListExceptPrinted(string mbCodeId)
        {

            IList<_111LevelInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(mbCodeId))
                {
                    IMBModelRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                    retLst = mbRepository.Get111LevelListExceptPrinted(mbCodeId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }


        }

        //add by zhulei
        /// <summary>
        /// 根据MBCode获取对应的111 Level信息列表(MB and VGA)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mbCode"></param>
        /// <returns>111 Level信息列表</returns>
        public IList<_111LevelInfo> GetPartNoListByInfo(string bomNodeType, string mbType, string mbCode)
        {
            IList<_111LevelInfo> retLst = new List<_111LevelInfo>();
            IList<string> modelLst = null;
            _111LevelInfo ret;

            try
            {

                if (!String.IsNullOrEmpty(mbCode))
                {
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                    modelLst = mbRepository.GetPartNoListByInfo(bomNodeType, mbType, mbCode);
                    foreach (var model in modelLst)
                    {
                        ret.friendlyName = model;
                        ret.id = model;
                        retLst.Add(ret);
                    }

                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Implementation of ISamplea
        public IList<OfflineLableSettingDef> GetSampleaList()
        {
            IList<OfflineLableSettingDef> retLst = new List<OfflineLableSettingDef>();
            ILabelTypeRepository lblTpRepository = null;
            try
            {
                lblTpRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
                retLst = lblTpRepository.GetAllOfflineLabelSetting();
                return retLst;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }    
        }
        #endregion

        #region Implementation of IPdLine

        /// <summary>
        /// 根据customer获取对应的PdLine信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>PdLine信息列表</returns>
        public IList<PdLineInfo> GetPdLineList(string customerId)
        {

            IList<PdLineInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(customerId))
                {
                    ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                    retLst = lineRepository.GetAllPdLineListByCust(customerId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// 根据station,customer获取对应的PdLine信息列表
        /// </summary>
        /// <param name="stationId">Station Identifier</param>
        /// <param name="customerId">customer</param>
        /// <returns>PdLine信息列表</returns>
        public IList<PdLineInfo> GetPdLineList(string stationId, string customerId)
        {

            IList<PdLineInfo> retLst = new List<PdLineInfo>();
            IList<LineInfo> linelst = new List<LineInfo>();
            PdLineInfo ret = new PdLineInfo();
            try
            {

                if (!String.IsNullOrEmpty(stationId) && !String.IsNullOrEmpty(customerId))
                {
                    ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                    //----------------------------------------------------------------
                    //Modify 2012/01/13 common rule UC change：
                    //SELECT a.[Line] AS Code, [Descr] as Line
	                //FROM IMES_GetData..Line_Station a, IMES_GetData..Line b
	                //WHERE a.Station = @Station
		            //and a.Line = b.Line
		            //and CustomerID = @Customer
	                //ORDER BY [Descr]
                    //------------------------------------------------------------------
                    //retLst = lineRepository.GetPdLineList(customerId, stationId);
                    //GetLineListByStationAndCustomer
                    linelst = lineRepository.GetLineListByStationAndCustomer(stationId, customerId);
                    foreach (var lineitem in linelst)
                    {
                        ret.friendlyName = lineitem.descr;
                        ret.id = lineitem.line;
                        retLst.Add(ret);
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// 根据stage,customer获取对应的PdLine信息列表
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="customerId">customer</param>
        /// <returns>PdLine信息列表</returns>
        public IList<PdLineInfo> GetPdLineListByStageAndCustomer(string stage, string customerId)
        {

            IList<PdLineInfo> retLst = new List<PdLineInfo>();
            DataTable dtLine = new DataTable();
            PdLineInfo ret = new PdLineInfo();

            try
            {
                if (!String.IsNullOrEmpty(stage) && !String.IsNullOrEmpty(customerId))
                {
                    ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();

                    dtLine = lineRepository.GetLineByCustomerAndStage(customerId, stage);
                    foreach (DataRow row in dtLine.Rows)
                    {
                        ret.friendlyName = row["Descr"].ToString();
                        ret.id = row["Line"].ToString();
                        retLst.Add(ret);
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据PdLine获取对应的PdLine详细信息
        /// </summary>
        /// <param name="id">PdLineID</param>
        /// <returns>PdLine详细信息</returns>
        public PdLineInfo GetPdLine(string id)
        {
            ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            PdLineInfo ret = new PdLineInfo();
            ret.id = id;
            ret.friendlyName = lineRepository.Find(id).Descr;
            return ret;
        }

        #endregion

        #region Implementation of ISMTMO

        /// <summary>
        /// 根据111阶码取得SMTMO信息列表
        /// </summary>
        /// <param name="_111LevelId">111阶码</param>
        /// <returns>SMTMO信息列表</returns>
        public IList<SMTMOInfo> GetSMTMOList(string _111LevelId)
        {

            IList<SMTMOInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(_111LevelId))
                {
                    IMBMORepository mbMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                    retLst = mbMORepository.GetSMTMOList(_111LevelId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 根据SMTMO标识，取得SMTMO信息
        /// </summary>
        /// <param name="SMTMOId">SMTMO标识</param>
        /// <returns>SMTMO信息</returns>
        public SMTMOInfo GetSMTMOInfo(string sMTMOId)
        {

            SMTMOInfo sMTMOInfo = new SMTMOInfo();
            try
            {

                if (!String.IsNullOrEmpty(sMTMOId))
                {
                    IMBMORepository mbMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                    sMTMOInfo = mbMORepository.GetSMTMOInfo(sMTMOId);

                }
                return sMTMOInfo;

            }
            catch (Exception)
            {
                throw;
            }


        }

        //add by zhulei
        /// <summary>
        /// 根据111阶码取得SMTMO信息列表(MB and VGA)
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns>SMTMO信息列表</returns>
        public IList<SMTMOInfo> GetSmtMoListByPno(string partNo)
        {
            IList<SMTMOInfo> retLst = new List<SMTMOInfo>();
            IList<string> partNoLst = null;
            SMTMOInfo ret = new SMTMOInfo();

            try
            {

                if (!String.IsNullOrEmpty(partNo))
                {
                    IMBMORepository mbMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                    partNoLst = mbMORepository.GetSmtMoListByPno(partNo);
                    foreach (var part in partNoLst)
                    {
                        ret.friendlyName = part;
                        ret.id = part;
                        retLst.Add(ret);
                    }

                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据SMTMO标识，取得SMTMO信息(MB and VGA)
        /// </summary>
        /// <param name="SMTMO"></param>
        /// <returns>SMTMO信息</returns>
        public SMTMOInfo GetSmtmoInfoList(string SMTMO)
        {
            IList<SmtmoInfo> smtmoLst = null;
            SMTMOInfo ret = new SMTMOInfo();

            try
            {

                if (!String.IsNullOrEmpty(SMTMO))
                {
                    IMBMORepository mbMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                    SmtmoInfo smtmoInfo = new SmtmoInfo();
                    smtmoInfo.smtmo = SMTMO;
                    smtmoLst = mbMORepository.GetSmtmoInfoList(smtmoInfo);
                    if (smtmoLst.Count() > 0)
                    {
                        ret._111LevelId = smtmoLst[0].iecpartno;
                        ret.cdt = smtmoLst[0].cdt;
                        ret.description = string.Empty;
                        ret.friendlyName = smtmoLst[0].smtmo;
                        ret.id = smtmoLst[0].smtmo;
                        ret.MB_CODEId = string.Empty;
                        ret.remark = smtmoLst[0].remark;
                        ret.totalMBQty = smtmoLst[0].qty;
                        ret.printedMBQty = smtmoLst[0].printQty;
                    }
                    else
                    {
                        ret._111LevelId = string.Empty;
                        ret.cdt = DateTime.Now;
                        ret.description = string.Empty;
                        ret.friendlyName = string.Empty;
                        ret.id = string.Empty;
                        ret.MB_CODEId = string.Empty;
                        ret.remark = string.Empty;
                        ret.totalMBQty = 0;
                        ret.printedMBQty = 0;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Implementation of IMO

        /// <summary>
        /// 根据机器Model号，取得MO信息列表
        /// </summary>
        /// <param name="modelId">机器Model号码</param>
        /// <returns>MO信息列表</returns>
        public IList<MOInfo> GetMOList(string modelId)
        {

            IList<MOInfo> retLst = null;
            try
            {
                if (!String.IsNullOrEmpty(modelId))
                {
                    IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                    retLst = moRepository.GetMOList(modelId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }



        }

        /// <summary>
        /// 根据MO标识，取得MO信息
        /// </summary>
        /// <param name="MOId">MO标识</param>
        /// <returns>MO信息</returns>
        public MOInfo GetMOInfo(string moId)
        {

            MOInfo moInfo = new MOInfo();
            try
            {

                if (!String.IsNullOrEmpty(moId))
                {


                    IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                    MO mo = moRepository.Find(moId);
                    if (mo != null)
                    {
                        moInfo.qty = mo.Qty;
                        moInfo.pqty = mo.PrtQty;
                    }



                }
                return moInfo;

            }
            catch (Exception)
            {
                throw;
            }


        }

        #endregion

        #region Implementation of IDocType
        /// <summary>
        /// 取得文档类型列表
        /// </summary>
        /// <returns>文档类型列表</returns>
        public IList<DocTypeInfo> GetDocTypeList()
        {

            IList<DocTypeInfo> retLst = null;

            try
            {

                IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
               // retLst = miscRepository.GetDocTypeList();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }


        }


        #endregion

     

        #region Implementation of IFamily


        /// <summary>
        /// 取得Product的Family信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <returns>Family信息列表</returns>
        public IList<FamilyInfo> GetFamilyList(string customerId)
        {

            IList<FamilyInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(customerId))
                {

                    IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                    retLst = familyRepository.FindFamiliesByCustomer(customerId);

                }
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 取得Product的Family信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Family信息列表</returns>
        public IList<FamilyInfo> FindFamiliesByCustomerOrderByFamily(string customer)
        {
            
            try
            {
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> customerLst = new List<string>();
                customerLst = partRepository.GetValueFromSysSettingByName("Customer");
                if (customerLst != null && customerLst.Count > 0)
                {
                    customer = customerLst[0];
                }
                else
                {
                    // throw new FisException("PAK087", new List<string>() { "Customer" });
                    return null;
                }

                IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                IList<FamilyInfo> retLst = null;
                retLst = familyRepository.FindFamiliesByCustomerOrderByFamily(customer);
                return retLst;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

       
        #endregion

        #region Implementation of I1397Level

        /// <summary>
        /// 取得1397阶信息列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>1397阶信息列表</returns>
        public IList<_1397LevelInfo> Get1397LevelList(string familyId)
        {

            IList<_1397LevelInfo> retLst = null;


            try
            {
                if (!String.IsNullOrEmpty(familyId))
                {

                    IMBModelRepository mbModelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                    retLst = mbModelRepository.Get1397LevelList(familyId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }



        }


        #endregion

      
        #region Implementation of IDefect
        /// <summary>
        /// 取得Defect列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Defect信息列表</returns>
        public IList<IMES.DataModel.DefectInfo> GetDefectList(string type)
        {

            IList<IMES.DataModel.DefectInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(type))
                {
                    IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                    retLst = defectRepository.GetDefectList(type);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 取得Defect信息
        /// </summary>
        /// <param name="defectId">Defect标识</param>
        /// <returns>Defect信息</returns>
        public IMES.DataModel.DefectInfo GetDefectInfo(string defectId)
        {

            IMES.DataModel.DefectInfo defectInfo = new IMES.DataModel.DefectInfo();
            try
            {

                if (!String.IsNullOrEmpty(defectId))
                {
                    IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                    defectInfo = defectRepository.GetDefectInfo(defectId);



                }
                return defectInfo;

            }
            catch (Exception)
            {
                throw;
            }


        }

        #endregion

        #region Implementation of ICause

        /// <summary>
        /// 取得Cause信息列表
        /// </summary>
        /// <param name="customerId">customer</param>
        /// <param name="stage">customer</param>
        /// <returns>Cause信息列表</returns>
        public IList<CauseInfo> GetCauseList(string customerId, string stage)
        {

            IList<CauseInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(customerId))
                {
                    IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                    retLst = defectInfoRepository.GetCauseList(customerId, stage);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IMajorPart
        /// <summary>
        /// 取得MajorPart信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>MajorPart信息列表</returns>
        public IList<MajorPartInfo> GetMajorPartList(string customerId)
        {

            IList<MajorPartInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(customerId))
                {
                    IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                    retLst = defectInfoRepository.GetMajorPartList(customerId);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IComponent
        /// <summary>
        /// 取得Component信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Component信息列表</returns>
        public IList<ComponentInfo> GetComponentList(string customerId)
        {

            IList<ComponentInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(customerId))
                {
                    IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                    retLst = defectInfoRepository.GetComponentList(customerId);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IObligation
        /// <summary>
        /// 取得Obligation信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Obligation信息列表</returns>
        public IList<ObligationInfo> GetObligationList(string customerId)
        {

            IList<ObligationInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(customerId))
                {
                    IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                    retLst = defectInfoRepository.GetObligationList(customerId);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IMark
        /// <summary>
        /// 取得Mark信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Mark信息列表</returns>
        public IList<MarkInfo> GetMarkList(string customerId)
        {

            IList<MarkInfo> retLst = null;

            try
            {


                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.GetMarkList(customerId);
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }




        }

        #endregion

        #region Implementation of IFloor
        /// <summary>
        /// 取得Floor信息列表
        /// </summary>
        /// <returns>Floor信息列表</returns>
        public IList<FloorInfo> GetFloorList()
        {

            IList<FloorInfo> retLst = null;

            try
            {


                IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                retLst = miscRepository.GetFloorList();
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }





        }

        #endregion

        #region Implementation of IPPIDType
        /// <summary>
        /// 取得PPID类型信息列表
        /// </summary>
        /// <returns>PPID类型信息列表</returns>
        public IList<PPIDTypeInfo> GetPPIDTypeList()
        {

            IList<PPIDTypeInfo> retLst = null;

            try
            {


                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                retLst = partRepository.GetPPIDTypeList();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }






        }

        #endregion

        #region Implementation of IPPIDDescription

        /// <summary>
        /// 取得PPID描述信息列表
        /// </summary>
        /// <param name="PPIDTypeId">PPIDType id</param>
        /// <returns>PPID描述信息列表</returns>
        public IList<PPIDDescriptionInfo> GetPPIDDescriptionList(string PPIDTypeId)
        {

            IList<PPIDDescriptionInfo> retLst = null;

            try
            {


                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                retLst = partRepository.GetPPIDDescriptionList(PPIDTypeId);
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }




        }

        #endregion

        #region Implementation of IPartNo
        /// <summary>
        /// 取得PartNo信息列表
        /// </summary>
        /// <param name="PPIDTypeId">PPID类型标识</param>
        /// <param name="PPIDDescrptionId">PPID描述标识</param>
        /// <returns>PartNo信息列表</returns>
        public IList<PartNoInfo> GetPartNoList(string PPIDTypeId, string PPIDDescrptionId)
        {

            IList<PartNoInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(PPIDTypeId) && !String.IsNullOrEmpty(PPIDDescrptionId))
                {

                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    retLst = partRepository.GetPartNoList(PPIDTypeId, PPIDDescrptionId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }




        }

        #endregion

        #region Implementation of ISubDefect
        /// <summary>
        /// 取得Sub-Defect信息列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Sub-Defect信息列表</returns>
        public IList<SubDefectInfo> GetSubDefectList(string type)
        {

            IList<SubDefectInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();

                    retLst = defectRepository.GetSubDefectList(type);//Dean 20110513 由空白改為傳type
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IResponsibility
        /// <summary>
        /// 取得Responsibility列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Responsibility列表</returns>
        public IList<ResponsibilityInfo> GetResponsibilityList(string customerId)
        {

            IList<ResponsibilityInfo> retLst = null;

            try
            {

                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.GetResponsibilityList(customerId);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of I4M

        /// <summary>
        /// 取得4M列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>4M列表</returns>
        public IList<_4MInfo> Get4MList(string customerId)
        {

            IList<_4MInfo> retLst = null;

            try
            {

                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.Get4MList(customerId);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of ICover
        /// <summary>
        /// 取得Cover列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Cover列表</returns>
        public IList<CoverInfo> GetCoverList(string customerId)
        {

            IList<CoverInfo> retLst = null;
            try
            {

                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.GetCoverList(customerId);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IUncover
        /// <summary>
        /// 取得Uncover列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>取得Uncover列表</returns>
        public IList<UncoverInfo> GetUncoverList(string customerId)
        {

            IList<UncoverInfo> retLst = null;

            try
            {

                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.GetUncoverList(customerId);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of ITrackingStatus
        /// <summary>
        /// 取得Tracking Status列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Tracking Status列表</returns>
        public IList<TrackingStatusInfo> GetTrackingStatusList(string customerId)
        {

            IList<TrackingStatusInfo> retLst = null;

            try
            {

                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.GetTrackingStatusList(customerId);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IDistribution
        /// <summary>
        /// 取得Distribution列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Distribution列表</returns>
        public IList<DistributionInfo> GetDistributionList(string customerId)
        {

            IList<DistributionInfo> retLst = null;

            try
            {

                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                retLst = defectInfoRepository.GetDistributionList(customerId);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of Imodel

        /// <summary>
        /// 取得Model列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>Model列表</returns>
        public IList<IMES.DataModel.ModelInfo> GetModelList(string familyId)
        {

            IList<IMES.DataModel.ModelInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(familyId))
                {
                    IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    retLst = modelRepository.GetModelList(familyId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }



        }


        /// <summary>
        /// 检查用户输入的Model在Model表中是否存在
        /// </summary>
        /// <param name="model">Model</param>
        public void checkModel(string model)
        {
            try
            {
                var currentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                var currentModel = currentModelRepository.Find(model);
                if (currentModel == null || currentModel.Status != "1")
                {
                    throw new FisException("CHK038", new List<string>() { model });
                }
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

        /// <summary>
        /// 检查用户输入的Model在是否属于该Family
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="model">model</param>
        public void checkModelinFamily(string family, string model)
        {
            try
            {
                var currentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                var currentModel = currentModelRepository.Find(model);
                if (currentModel == null || currentModel.Status != "1")
                {
                    throw new FisException("CHK038", new List<string>() { model });
                }

                IList<Model> modelLst = new List<Model>();
                modelLst = currentModelRepository.GetModelListByModel(family, model);       //支持like功能
                if (modelLst == null || modelLst.Count <= 0)
                {
                    throw new FisException("PAK096", new List<string>() { model, family });
                }
                else
                {
                    Boolean modelFlag = false;
                    foreach (Model imodel in modelLst)
                    {
                        if (imodel.ModelName == model)
                        {
                            modelFlag = true;
                            break;
                        }
                    }

                    if (!modelFlag)
                    {
                        throw new FisException("PAK096", new List<string>() { model, family });
                    }
                }

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

      
        #endregion

        #region Implementation of IProdIdRange
        /// <summary>
        /// 取得ProdIdRange列表
        /// </summary>
        /// <param name="MOId">MO标识</param>
        /// <returns>ProdIdRange列表</returns>
        public IList<ProdIdRangeInfo> GetProdIdRangeList(string moId)
        {

            IList<ProdIdRangeInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(moId))
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    retLst = productRepository.GetProdIdRangeList(moId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IKPType
        /// <summary>
        /// 取得KP类型列表
        /// </summary>
        /// <returns>KP类型列表</returns>
        public IList<KPTypeInfo> GetKPTypeList()
        {

            IList<KPTypeInfo> retLst = null;

            try
            {


                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                retLst = partRepository.GetKPTypeList();
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }




        }

        #endregion

        #region Implementation of IChangeKPType
        /// <summary>
        /// 取得ChangeKPType列表
        /// </summary>
        /// <returns>ChangeKPType列表</returns>
        public IList<ConstValueInfo> GetChangeKPTypeList()
        {

            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();

            try
            {
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                retLst = partRepository.GetConstValueListByType("ChangeKP");
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }




        }

        #endregion

        #region Implementation of IBOLNo
        /// <summary>
        /// 取得BOLNo列表
        /// </summary>
        /// <returns>BOLNo列表</returns>
        public IList<BOLNoInfo> GetBOLNoList()
        {

            IList<BOLNoInfo> retLst = null;

            try
            {

                IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                retLst = deliveryRepository.GetBolNo();
                return retLst;


            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IPallet
        /// <summary>
        /// 取得Pallet列表
        /// </summary>
        /// <param name="DNId">DN标识</param>
        /// <returns>Pallet列表</returns>
        public IList<PalletInfo> GetPalletList(string dnId)
        {

            IList<PalletInfo> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(dnId))
                {
                    IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                    retLst = palletRepository.GetPalletList(dnId);


                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }


        }

        #endregion

        #region Implementation of ICauseItem

        /// <summary>
        /// 取得Cause Item
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>Cause Item</returns>
        public IList<string> GetCauseItemByType(string type)
        {

            IList<string> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(type))
                {
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    retLst = partRepository.GetCauseItemListByType(type);
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        //#region Implementation of IMES.Station.Interface.CommonIntf.ICheckItem 

        ////get data from session, edit by 207006
        //public IList<IMES.DataModel.CheckItemInfo> GetCheckItemList(string mbId)
        //{

        //    //IList<IMES.DataModel.CheckItemInfo> retLst = null;



        //    //try
        //    //{
        //    //    if (!String.IsNullOrEmpty(mbId))
        //    //    {
        //    //        ICheckItemRepository checkItemRepository = RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, IMES.FisObject.Common.CheckItem.ICheckItem>();
        //    //        retLst = checkItemRepository.GetCheckItemList(mbId);


        //    //    }
        //    //    return retLst;
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    throw;
        //    //}

        //    try
        //    {
        //        Session Session = SessionManager.GetInstance.GetSession(mbId, Session.SessionType.MB);
        //        BOM BOMItem = (BOM)Session.GetValue(Session.SessionKeys.SessionBom);
        //        IList<IMES.FisObject.Common.CheckItem.ICheckItem> checkItems = (IList<IMES.FisObject.Common.CheckItem.ICheckItem>)Session.GetValue(Session.SessionKeys.ExplicityCheckItemList);
        //        IList<IMES.DataModel.CheckItemInfo> retLst = new List <IMES.DataModel.CheckItemInfo >();
        //        if (BOMItem != null)
        //        {
        //            foreach (BOMItem item in BOMItem.Items)
        //            {
        //                IMES.DataModel.CheckItemInfo checkItemInfo= new IMES.DataModel.CheckItemInfo();
        //                checkItemInfo.qty = item.Qty;
        //                checkItemInfo.scannedQty = 0;
        //                checkItemInfo.id = item.AlterParts[0].PN;
        //                retLst.Add(checkItemInfo);
        //            }

        //        }

        //        if (checkItems != null)
        //        {
        //            foreach (IMES.FisObject.Common.CheckItem.ICheckItem item in checkItems)
        //            {
        //                IMES.DataModel.CheckItemInfo checkItemInfo = new IMES.DataModel.CheckItemInfo();
        //                checkItemInfo.qty = 1;
        //                checkItemInfo.scannedQty = 0;
        //                checkItemInfo.id = item.CheckItem;
        //                retLst.Add(checkItemInfo);
        //            }

        //        }
        //        return retLst;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //#endregion

        #region Implementation of IRepairLog
        /// <summary>
        /// 取得MB Repair列表
        /// </summary>
        /// <param name="MBId">MB标识</param>
        /// <returns>MB Repair列表</returns>
        public IList<RepairInfo> GetMBRepairList(string mbId)
        {

            IList<RepairInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(mbId))
                {
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                    retLst = mbRepository.GetMBRepairLogList(mbId);

                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 取得Product Repair列表
        /// </summary>
        /// <param name="ProdId">Product标识</param>
        /// <returns>Product Repair列表</returns>
        public IList<RepairInfo> GetProdRepairList(string prodId)
        {

            IList<RepairInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(prodId))
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    retLst = productRepository.GetProdRepairLogList(prodId);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public IList<RepairInfo> GetProdRepairList(string prodId,out bool IsOQCRepair)
        {
            IsOQCRepair = false;

            IList<RepairInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(prodId))
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    
                    retLst = productRepository.GetProdRepairLogList(prodId);
                    IMES.FisObject.FA.Product.IProduct product = productRepository.GetProductByIdOrSn(prodId);
                    IList<IMES.FisObject.FA.Product.ProductInfo> productinfos = product.ProductInfoes;
                    
                    foreach (IMES.FisObject.FA.Product.ProductInfo info in productinfos)
                    {
                        if (info.InfoType == ExtendSession.SessionKeys.OQCRepairStation && info.InfoValue != "")
                        {
                            IsOQCRepair = true;
                            break;
                        }
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Implementation of IMB
        /// <summary>
        /// 根据MBSNO取得MB相关信息
        /// </summary>
        /// <param name="MBId">MB SNO</param>
        /// <returns>MB相关信息</returns>
        public IMES.DataModel.MBInfo GetMBInfo(string mbId)
        {
            IMES.DataModel.MBInfo mbInfo = new IMES.DataModel.MBInfo();


            try
            {
                if (!String.IsNullOrEmpty(mbId))
                {
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                    ILineRepository lr = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                    Line line = null;

                    mbInfo = mbRepository.GetMBInfo(mbId);

                    if (mbInfo.line != null)
                    {
                        line = lr.Find(mbInfo.line);

                        if (line != null)
                        {
                            mbInfo.lineDesc = line.Descr;
                        }
                    }
                }
                return mbInfo;
            }
            catch (Exception)
            {
                throw;
            }



        }


        /// <summary>
        /// 根据MBSNO取得MB相关信息
        /// </summary>
        /// <param name="MBId">MB SNO</param>
        /// <returns>MB相关信息</returns>
        /// 
        //Dean 20100329
        public IMES.DataModel.MBInfo GetMBInfo(string mbId, out int MultiQTY)
        {
            MultiQTY = 0;
            IMES.DataModel.MBInfo mbInfo = new IMES.DataModel.MBInfo();


            try
            {
                if (!String.IsNullOrEmpty(mbId))
                {
                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                    ILineRepository lr = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                    Line line = null;

                    //Dean 20110329 
                    IMES.FisObject.PCA.MB.IMB MBData=mbRepository.Find(mbId);
                    if (MBData == null)
                    {
                        var ex = new FisException("SFC001", new string[] { mbId });
                        throw ex;
                    }

                    mbRepository.FillModelObj(MBData);
                    //MBData.ModelObj                    
                    MBCode MBCodeData= MBData.ModelObj.MbCodeObj;

                    //如果MB Code 没有维护 [Multi Q’ty]，则报告错误
                    if (MBCodeData != null)
                    {
                        MultiQTY = MBCodeData.MultQty;
                    }
                    else
                    {
                        //請維護 MCode [%1] 的連板數！
                        var ex = new FisException("CHK153", new string[] { MBData.ModelObj.Mbcode });
                        throw ex;                        
                    }
                    //如果MB Code Maintain的[Multi Q’ty] 不在Range of Multi Q’ty 允许范围内，则报告错误
                    //該MB SNo為子板MB Sno，不能進行切板!!
                    //MBCode 升位，CheckCode需要同步修改
                    // 
                    // CheckCode：若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码
                    // CheckCode为数字，则为子板，为’R’，则为RCTO
                    // ============================================================
                    string strCheckCode = "";
                    if (mbId.Substring(4, 1) == "M")
                    {
                        strCheckCode = mbId.Substring(5, 1);
                    }
                    else
                    {
                        strCheckCode = mbId.Substring(6, 1);
                    }
                    // ============================================================
                    //if (mbId.Substring(5, 1).ToString() != "0" && MultiQTY > 1)//子板
                    if (strCheckCode != "0" && MultiQTY > 1)//子板
                    {
                        var ex = new FisException("CHK161", new string[] { });                     
                        throw ex;
                    }

                   // if (mbId.Substring(5, 1).ToString() == "0")//母板
                   // {
                        if (MultiQTY < 10 && MultiQTY > 0)
                        {
                            //当Multi Q’ty 大于1时，若MB Sno 为子板序列号，则不进行切板 – MB Sno第6位为0，则为母板，否则为子板
                            if (MultiQTY > 1)
                            {
                                /*if (mbId.Substring(5, 1).ToString() != "0")
                                {
                                    //数据越界，请检查[%1] MultiQty！
                                    var ex = new FisException("CHK154", new string[] { MBData.ModelObj.Mbcode });
                                    throw ex;
                                }*/
                            }
                        }
                        else
                        {
                            //数据越界，请检查[%1] MultiQty！
                            var ex = new FisException("CHK154", new string[] { MBData.ModelObj.Mbcode });
                            throw ex;
                        }
                   // }
                    //Dean 20110329 


                    mbInfo = mbRepository.GetMBInfo(mbId);

                    if (mbInfo.line != null)
                    {
                        line = lr.Find(mbInfo.line);
                        

                        if (line != null)
                        {
                            mbInfo.lineDesc = line.Descr;
                        }                       
                    }                    
                }
                return mbInfo;
            }
            catch (Exception)
            {
                throw;
            }



        }
        #endregion

        #region Implementation of IProduct
        /// <summary>
        /// 根据ProdId取得Product相关信息
        /// </summary>
        /// <param name="productId">ProdId</param>
        /// <returns>Product相关信息</returns>
        public IMES.DataModel.ProductInfo GetProductInfo(string productId)
        {
            IMES.DataModel.ProductInfo productInfo = new IMES.DataModel.ProductInfo();


            try
            {
                if (productId != null)
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    productInfo = productRepository.GetProductInfo(productId);
                   
                }
                return productInfo;
            }
            catch (Exception)
            {
                throw;
            }



        }

        /// <summary>
        /// 根据ProdId取得Product相关信息
        /// </summary>
        /// <param name="customerSn">Customer SN</param>
        /// <returns>Product相关信息</returns>
        public IMES.DataModel.ProductInfo GetProductInfoByCustomSn(string customerSn)
        {
            IMES.DataModel.ProductInfo productInfo = new IMES.DataModel.ProductInfo();


            try
            {
                if (customerSn != null)
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    productInfo = productRepository.GetProductInfoByCustomSn(customerSn);
                  
                }
                return productInfo;
            }
            catch (Exception)
            {
                throw;
            }



        }
        
        /// <summary>
        /// 根据ProdId取得Product Status相关信息
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Product Status相关信息</returns>
        public ProductStatusInfo GetProductStatusInfo(string productId)
        {
            ProductStatusInfo productStatusInfo = new ProductStatusInfo();


            try
            {
                if (productId != null)
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    productStatusInfo = productRepository.GetProductStatusInfo(productId);

                }
                return productStatusInfo;
            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IPartType

        /// <summary>
        /// 获取PartType列表
        /// </summary>
        /// <returns>获取PartType列表</returns>
        public IList<PartTypeInfo> GetPartTypeList()
        {

            IList<PartTypeInfo> retLst = null;

            try
            {

                IPartRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                retLst = mbRepository.GetPartTypeList();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        #endregion

        #region Implementation of IQCStatistic
        /// <summary>
        /// 获取QC统计信息列表
        /// </summary>
        /// <param name="pdLine">PdLine</param>
        /// <returns>QC统计信息列表</returns>
        public IList<QCStatisticInfo> GetQCStatisticList(string pdLine, string type)
        {

            IList<QCStatisticInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(pdLine))
                {
                    IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                    retLst = miscRepository.GetQCStatisticList(pdLine, type);


                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }



        }


        #endregion

        #region Implementation of IDCode
        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <param name="isFRU">是否为FRU</param>
        /// <returns></returns>
        public IList<DCodeInfo> GetDCodeRuleListForMB(bool isFRU, string customer)
        {
            IWarrantyRepository wr =
                RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            return (from w in wr.GetDCodeRuleListForMB(isFRU, customer)
                    orderby w.Descr // 2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0169
                    select new DCodeInfo { id = w.Id.ToString(), friendlyName = w.Descr }).ToArray();
        }

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        public IList<DCodeInfo> GetDCodeRuleListForMB(string customer)
        {
            IWarrantyRepository wr =
                RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            return (from w in wr.GetDCodeRuleListForMB(customer)
                    orderby w.Descr
                    select new DCodeInfo { id = w.Id.ToString(), friendlyName = w.Descr }).ToArray();
        }

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        public IList<DCodeInfo> GetDCodeRuleListForVB(string customer)
        {
            IWarrantyRepository wr =
                RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            return (from w in wr.GetDCodeRuleListForVB(customer)
                    orderby w.Descr // 2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0169
                    select new DCodeInfo { id = w.Id.ToString(), friendlyName = w.Descr }).ToArray();
        }

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        public IList<DCodeInfo> GetDCodeRuleListForKP(string customer)
        {
            IWarrantyRepository wr =
                RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            return (from w in wr.GetDCodeRuleListForKP(customer)
                    orderby w.Descr
                    select new DCodeInfo { id = w.Id.ToString(), friendlyName = w.Descr }).ToArray();
        }

        /// <summary>
        /// 获取DCode信息列表
        /// </summary>
        /// <returns></returns>
        public IList<DCodeInfo> GetDCodeRuleListForDK(string customer)      //Jessica Liu, 2012-5-29
        {
            IWarrantyRepository wr =
                RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            return (from w in wr.GetDCodeRuleListForDK(customer)
                    orderby w.WarrantyCode
                    select new DCodeInfo { id = w.Id.ToString(), friendlyName = w.Descr }).ToArray();
        }

        #endregion

        #region ISession Members

        /// <summary>
        /// 强制结束一个指定Session
        /// </summary>
        /// <param name="sessionKey">MBSN或ProdId等</param>
        /// <param name="type">type包括MB,Product,Common</param>
        public void TerminateSession(string sessionKey, IMES.DataModel.SessionType type)
        {
            try
            {
                Session.SessionType currentSessionType = Session.SessionType.Common;
                switch (type)
                {
                    case IMES.DataModel.SessionType.Common:
                        currentSessionType = Session.SessionType.Common;
                        break;
                    case IMES.DataModel.SessionType.MB:
                        currentSessionType = Session.SessionType.MB;
                        break;
                    case IMES.DataModel.SessionType.Product:
                        currentSessionType = Session.SessionType.Product;
                        break;

                }

                Session tempSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (tempSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(tempSession);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根据type获取Session信息
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>Session信息</returns>
        public IList<SessionInfo> GetSessionByType(SessionType type)
        {
            IList<SessionInfo> retlist = null;
            try
            {
                SessionManager seM = SessionManager.GetInstance;
                List<Session> sessionlist = seM.GetSessionByType((Session.SessionType)type);
                retlist = new List<SessionInfo>();
                if ((sessionlist != null) && (sessionlist.Count > 0))
                {
                    SessionInfo seInfo = null;
                    foreach (Session se in sessionlist)
                    {
                        seInfo = new SessionInfo();
                        seInfo.Cdt = se.Cdt;
                        seInfo.Operator = se.Editor;
                        seInfo.PdLine = se.Line;
                        seInfo.SessionKey = se.Key;
                        seInfo.sessiontype = (SessionType)se.Type;
                        seInfo.StationId = se.Station;
                        retlist.Add(seInfo);
                    }
                }

                return retlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 获取全部站信息列表
        /// </summary>
        /// <returns>站信息列表</returns>
        public IList<StationInfo> GetAllStationInfo()
        {
            try
            {
                IList<StationInfo> staInfolist = null;
                var stationRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
                IList<IStation> stalist = stationRepository.FindAll();
                staInfolist = new List<StationInfo>();
                if (stalist != null && stalist.Count > 0)
                {
                    StationInfo staInfo = null;
                    foreach (IStation sta in stalist)
                    {
                        staInfo = new StationInfo();
                        staInfo.StationId = sta.StationId;
                        staInfo.Descr = sta.Descr;
                        staInfolist.Add(staInfo);
                    }
                }
                return staInfolist;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region ITestStation Members

        /// <summary>
        /// 取得FA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        public IList<TestStationInfo> GetFATestStationList()
        {
            IList<TestStationInfo> retLst = null;

            try
            {


                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                retLst = stationRepository.GetFATestStationList();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得SA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        public IList<TestStationInfo> GetSATestStationList()
        {
            IList<TestStationInfo> retLst = null;

            try
            {


                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                retLst = stationRepository.GetSATestStationList();
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据站号取得站名称
        /// </summary>
        /// <param name="stationId">station id</param>
        /// <returns>站名称</returns>
        public string GeStationDescr(string stationId)
        {
            string descr = String.Empty;

            try
            {

                if (!String.IsNullOrEmpty(stationId))
                {
                    IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                    IStation iStation = stationRepository.Find(stationId);
                    if (iStation != null)
                    {
                        descr = iStation.Descr;
                    }

                }
                return descr;

            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 根据station type来获取站信息列表
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>站信息列表</returns>
        public IList<StationInfo> GetStationListByType(string type)
        {
           
            IList<StationInfo> retLst = new List<StationInfo>();

            try
            {


                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                IList<IStation> tempLst = stationRepository.GetStationList((StationType)Enum.Parse(typeof(StationType), type));
                if (tempLst != null && tempLst.Count > 0)
                {
                    StationInfo stationInfo = null;
                    foreach (IStation temp in tempLst)
                    {
                        stationInfo = new StationInfo();
                        stationInfo.StationId = temp.StationId;
                        stationInfo.Descr = temp.Descr;
                        retLst.Add(stationInfo);
                    }
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StationInfo> GetStationListByLineAndType(string pdline, string type)
        {

            IList<StationInfo> retLst = new List<StationInfo>();

            try
            {


                ILineRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                IList<IStation> tempLst = stationRepository.GetStationListByLineAndStationType(pdline, type);
                if (tempLst != null && tempLst.Count > 0)
                {
                    StationInfo stationInfo = null;
                    foreach (IStation temp in tempLst)
                    {
                        stationInfo = new StationInfo();
                        stationInfo.StationId = temp.StationId;
                        stationInfo.Descr = temp.Descr;
                        retLst.Add(stationInfo);
                    }
                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //#region ICollectionData Members
        ///// <summary>
        ///// 取得CollectionData信息列表
        ///// </summary>
        ///// <param name="MBId">session key标识</param>
        /////  <param name="MBId">type</param>
        ///// <returns>CollectionData信息列表</returns>
        //IList<BomItemInfo> ICollectionData.GetCheckItemList(string sessionKey, IMES.DataModel.SessionType type)
        //{
        //    try
        //    {
        //        Session.SessionType currentSessionType = Session.SessionType.Common;
        //        switch (type)
        //        {
        //            case IMES.DataModel.SessionType.Common:
        //                currentSessionType = Session.SessionType.Common;
        //                break;
        //            case IMES.DataModel.SessionType.MB:
        //                currentSessionType = Session.SessionType.MB;
        //                break;
        //            case IMES.DataModel.SessionType.Product:
        //                currentSessionType = Session.SessionType.Product;
        //                break;

        //        }

        //        Session tempSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);
        //        if (tempSession == null)
        //        {
        //            FisException ex;
        //            List<string> erpara = new List<string>();
        //            erpara.Add(sessionKey);
        //            ex = new FisException("CHK021", erpara);
        //            throw ex;
        //        }

        //        IList<IMES.DataModel.BomItemInfo> retLst = GetCheckItemList(tempSession);
        //        return retLst;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //#endregion

        /// <summary>
        /// 取得CollectionData信息列表
        /// </summary>
        /// <param name="MBId">session</param>
        ///  <param name="MBId">type</param>
        /// <returns>CollectionData信息列表</returns>
        //public static IList<BomItemInfo> GetCheckItemList(Session Session)
        //{
        //    try
        //    {
        //        //if (Session == null)
        //        //{
        //        //    FisException ex;
        //        //    List<string> erpara = new List<string>();
        //        //    erpara.Add(sessionKey);
        //        //    ex = new FisException("CHK021", erpara);
        //        //    throw ex;
        //        //}
        //        BOM BOMItem = (BOM)Session.GetValue(Session.SessionKeys.SessionBom);
        //        IList<IMES.FisObject.Common.CheckItem.ICheckItem> checkItems = (IList<IMES.FisObject.Common.CheckItem.ICheckItem>)Session.GetValue(Session.SessionKeys.ExplicityCheckItemList);
        //        IList<IMES.DataModel.BomItemInfo> retLst = new List<BomItemInfo>();
        //        if (BOMItem != null)
        //        {
        //            foreach (BOMItem item in BOMItem.Items)
        //            {
        //                BomItemInfo ItemInfo = new BomItemInfo();
        //                ItemInfo.qty = item.Qty;
        //                if (item.StationPreCheckedPart != null)
        //                {
        //                    ItemInfo.scannedQty = item.StationPreCheckedPart.Count();
        //                    ItemInfo.collectionData = new List<pUnit>();
        //                    foreach (PartUnit preItem in item.StationPreCheckedPart)
        //                    {
        //                        pUnit temp = new pUnit();
        //                        temp.sn = preItem.Sn;
        //                        temp.pn = preItem.Pn;
        //                        temp.valueType = item.ValueType ;
        //                        ItemInfo.collectionData.Add(temp);
        //                    }
        //                }
        //                else
        //                {
        //                    ItemInfo.scannedQty = 0;
        //                    ItemInfo.collectionData = new List<pUnit>();
        //                }

        //                List<PartNoInfo> allPart = new List<PartNoInfo>();


        //                foreach (BOMPart part in item.AlterParts)
        //                {
        //                    PartNoInfo aPart = new PartNoInfo();
        //                    aPart.description = part.Descr;
        //                    aPart.id = part.PN;
        //                    aPart.friendlyName = aPart.id;
        //                    aPart.partTypeId = part.Type;
        //                    aPart.iecPartNo = part.PN;
        //                    aPart.valueType = item.ValueType ;
        //                    allPart.Add(aPart);
        //                }
        //                allPart.Sort(delegate(PartNoInfo p1, PartNoInfo p2) { return p1.iecPartNo.CompareTo(p2.iecPartNo); });

        //                ItemInfo.parts = allPart;
        //                retLst.Add(ItemInfo);
        //            }

        //        }

        //        if (checkItems != null)
        //        {
        //            foreach (ICheckItem item in checkItems)
        //            {
        //                BomItemInfo checkItemInfo = new BomItemInfo();
        //                checkItemInfo.qty = 1;
        //                checkItemInfo.scannedQty = 0;

        //                IList<PartNoInfo> allPart = new List<PartNoInfo>();
        //                PartNoInfo aPart = new PartNoInfo();
        //                aPart.description = string.Empty;
        //                aPart.id = item.ItemDisplayName;
        //                aPart.friendlyName = aPart.id;
        //                aPart.partTypeId = string.Empty;
        //                aPart.iecPartNo = aPart.id;
        //                allPart.Add(aPart);
        //                checkItemInfo.parts = allPart;

        //                retLst.Add(checkItemInfo);
        //            }

        //        }
        //        return retLst;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static IList<BomItemInfo> GetCheckItemList(Session Session, string MBSN, string MBModel)
        //{
        //    try
        //    {
        //        BOM BOMItem = (BOM)Session.GetValue(Session.SessionKeys.SessionBom);
        //        IList<IMES.FisObject.Common.CheckItem.ICheckItem> checkItems = (IList<IMES.FisObject.Common.CheckItem.ICheckItem>)Session.GetValue(Session.SessionKeys.ExplicityCheckItemList);
        //        IList<IMES.DataModel.BomItemInfo> retLst = new List<BomItemInfo>();
        //        if (BOMItem != null)
        //        {
        //            foreach (BOMItem item in BOMItem.Items)
        //            {
        //                BomItemInfo ItemInfo = new BomItemInfo();
        //                ItemInfo.qty = item.Qty;
        //                if (item.StationPreCheckedPart != null)
        //                {
        //                    ItemInfo.scannedQty = item.StationPreCheckedPart.Count();
        //                    ItemInfo.collectionData = new List<pUnit>();
        //                    foreach (PartUnit preItem in item.StationPreCheckedPart)
        //                    {
        //                        pUnit temp = new pUnit();
        //                        temp.sn = preItem.Sn;
        //                        temp.pn = preItem.Pn;
        //                        temp.valueType = item.ValueType;
        //                        ItemInfo.collectionData.Add(temp);
        //                    }
        //                }
                 
        //                else
        //                {
        //                    ItemInfo.scannedQty = 0;
        //                    ItemInfo.collectionData = new List<pUnit>();
        //                }

        //                List<PartNoInfo> allPart = new List<PartNoInfo>();


        //                foreach (BOMPart part in item.AlterParts)
        //                {
        //                    PartNoInfo aPart = new PartNoInfo();
        //                    aPart.description = part.Descr;
        //                    aPart.id = part.PN;
        //                    aPart.friendlyName = aPart.id;
        //                    aPart.partTypeId = part.Type;
        //                    aPart.iecPartNo = part.PN;
        //                    aPart.valueType = item.ValueType;
        //                    allPart.Add(aPart);
        //                    if ( !string.IsNullOrEmpty(MBModel) &&
        //                        !string.IsNullOrEmpty(MBSN) && 
        //                        aPart.id == MBModel  )
        //                    {
        //                        ItemInfo.scannedQty = 1; //item.StationPreCheckedPart.Count();
        //                        ItemInfo.collectionData = new List<pUnit>();

        //                        pUnit temp = new pUnit();
        //                        temp.sn = MBSN;
        //                        temp.pn = MBModel;
        //                        temp.valueType = "SN"; //item.ValueType;
        //                        ItemInfo.collectionData.Add(temp);


        //                    }


        //                }
        //                allPart.Sort(delegate(PartNoInfo p1, PartNoInfo p2) { return p1.iecPartNo.CompareTo(p2.iecPartNo); });

        //                ItemInfo.parts = allPart;
        //                retLst.Add(ItemInfo);
        //            }

        //        }

        //        if (checkItems != null)
        //        {
        //            foreach (ICheckItem item in checkItems)
        //            {
        //                BomItemInfo checkItemInfo = new BomItemInfo();
        //                checkItemInfo.qty = 1;
        //                checkItemInfo.scannedQty = 0;

        //                IList<PartNoInfo> allPart = new List<PartNoInfo>();
        //                PartNoInfo aPart = new PartNoInfo();
        //                aPart.description = string.Empty;
        //                aPart.id = item.ItemDisplayName;
        //                aPart.friendlyName = aPart.id;
        //                aPart.partTypeId = string.Empty;
        //                aPart.iecPartNo = aPart.id;
        //                allPart.Add(aPart);
        //                checkItemInfo.parts = allPart;

        //                retLst.Add(checkItemInfo);
        //            }

        //        }
        //        return retLst;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #region Implementation of IPrintTemplate
        /// <summary>
        /// 取得所有可用的打印模板
        /// </summary>
        /// <param name="labelType">标签类型</param>
        /// <returns>可用的打印模板列表</returns>
        public IList<PrintTemplateInfo> GetPrintTemplateList(string labelType)
        {

            IList<PrintTemplateInfo> retLst = null;

            try
            {

                if (!String.IsNullOrEmpty(labelType))
                {
                    ILabelTypeRepository lblTpRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
                    retLst = lblTpRepository.GetPrintTemplateList(labelType);

                }
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }



        }

        /// <summary>
        /// 获取指定pcode可能打印的pcode
        /// </summary>
        /// <param name="pcode">pcode</param>
        /// <returns>指定pcode可能打印的LabelType</returns>
        public IList<string> GetPrintLabelTypeList(string pcode)
        {

            IList<string> retLst = null;

            try
            {
                if (!String.IsNullOrEmpty(pcode))
                {
                    ILabelTypeRepository lblTpRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
                    retLst = lblTpRepository.GetPrintLabelTypeList(pcode);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 使用PCode 找是否要使用RemoteBat 打印
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        public string GetRemoteBatPath(string pcode)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var values = partRep.GetValueFromSysSettingByName("RemoteBatPath");
            if (values != null && values.Count > 0 && !string.IsNullOrEmpty(values[0]))
            {
                var remotePCode = partRep.GetConstValueTypeList("RemoteBatPCode", pcode);
                if (remotePCode != null && remotePCode.Count > 0)
                {
                    return values[0];
                }
            }

            return "";

        }

        /// <summary>
        ///   select * from SysSetting a, ConstValueType b where a.Name ='RemoteBartenderPath' and b.Type='RemoteBartenderPCode' and b.Value =@PCode
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        public string GetRemoteBartenderPath(string pcode)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var values = partRep.GetValueFromSysSettingByName("RemoteBartenderPath");
            if (values != null && values.Count > 0 && !string.IsNullOrEmpty(values[0]))
            {
                var remotePCode = partRep.GetConstValueTypeList("RemoteBartenderPCode", pcode);
                if (remotePCode != null && remotePCode.Count > 0)
                {
                    return values[0];
                }
            }

            return "";
        }

        /// <summary>
        /// 取得LabelType的打印模式(Template/Batch: "1" / "0")
        /// </summary>
        /// <param name="labelType">标签类型</param>
        /// <returns>打印模式</returns>
        public string GetPrintMode(string labelType)
        {
            string ret = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(labelType))
                {
                    ILabelTypeRepository lblTpRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
                    ret = lblTpRepository.GetPrintMode(labelType).ToString();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得LabelType的Rule模式
        /// </summary>
        /// <param name="labelType">标签类型</param>
        /// <returns>Rule模式</returns>
        public string GetRuleMode(string labelType)
        {
            string ret = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(labelType))
                {
                    ILabelTypeRepository lblTpRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
                    ret = lblTpRepository.GetRuleMode(labelType).ToString();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        /// <summary>
        /// 根据type,customer获取对应的Defect信息列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="customer">customer</param>
        /// <returns>Defect信息列表</returns>
        public IList<IMES.DataModel.DefectInfo> GetDefectInfoByTypeAndCustomer(string type, string customer)
        {
            IList<IMES.DataModel.DefectInfo> retLst = null;

            try
            {
                IDefectInfoRepository defectInfoRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();

                retLst = defectInfoRepository.FindDefectInfoesByType(type, customer);
                return retLst;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// bat打印时,根据存储过程名字和参数执行存储过程,获取主Bat
        /// </summary>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns></returns>
        public string GetMainBat(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues)
        {
            ILabelTypeRepository lblTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
            return lblTypeRepository.GetMainBat(currentSPName, parameterKeys, parameterValues);
        }
		
		/// <summary>
        /// Bartender打印时,根据存储过程名字和参数执行存储过程,获取主Bartender Name/Value
        /// </summary>
        /// <param name="currentSPName">存储过程名字</param>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns>主bat名称</returns>
        public IList<NameValueDataTypeInfo> GetBartenderNameValueList(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues)
        {
            ILabelTypeRepository lblTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
            return lblTypeRepository.GetBartendarNameValueInfo(currentSPName, parameterKeys, parameterValues);
        }	

        /// <summary>
        /// 获取IProduct
        /// </summary>
        /// <param name="inputNo"></param>
        /// <param name="InputType"></param>
        /// <returns></returns>
        public static IMES.FisObject.FA.Product.IProduct GetProductByInput(string inputNo, InputTypeEnum InputType)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            IMES.FisObject.FA.Product.IProduct currentProduct = null;

            switch (InputType)
            {
                case InputTypeEnum.CustSN:
                    currentProduct = productRepository.GetProductByCustomSn(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.GetProductByCustomSn(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.Carton:
                    List<string> productIDList = productRepository.GetProductIDListByCarton(inputNo);
                    if (productIDList == null || productIDList.Count == 0 && inputNo.Length > 1)
                    {
                        productIDList = productRepository.GetProductIDListByCarton(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    if (productIDList != null && productIDList.Count != 0)
                    {
                        currentProduct = productRepository.Find(productIDList[0]);
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSN:
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSN(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSN(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSNOrCarton:
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrCarton(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrCarton(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;
                case InputTypeEnum.ProductIDOrCustSNOrPallet:
                    currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrPallet(inputNo);
                    if (currentProduct == null && inputNo.Length > 1)
                    {
                        currentProduct = productRepository.FindOneProductWithProductIDOrCustSNOrPallet(inputNo.Substring(0, inputNo.Length - 1));
                    }
                    break;

            }

            if (currentProduct == null)
            {
                FisException fe = new FisException("CHK079", new string[] { inputNo });
                throw fe;
            }
            return currentProduct;
        }

        /// <summary>
        /// 输入的类型，共有两种CustSN，Carton
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 输入的是Session.CustSN
            /// </summary>
            CustSN = 1,

            /// <summary>
            /// 输入的是Session.Carton
            /// </summary>
            Carton = 2,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSN
            /// For:Unit Label Print
            /// </summary>
            ProductIDOrCustSN = 4,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSNOrCarton
            /// </summary>
            ProductIDOrCustSNOrCarton = 8,

            /// <summary>
            /// 输入的是Session.ProductIDOrCustSNOrPallet
            /// </summary>
            ProductIDOrCustSNOrPallet = 16
        }

        #region Implementation of IShipment
        /// <summary>
        /// 获取Shipment信息列表
        /// </summary>
        /// <param name="shipDate">Shipment date</param>
        /// <returns>Shipment信息列</returns>
        public IList<ShipmentInfo> GetShipmentList(DateTime shipDate)
        {
            IList<ShipmentInfo> retLst = null;
            try
            {

               
                IDeliveryRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                retLst = mbRepository.GetDeliveryInfoValueByTypeAndModelPrefix("RedShipment", "60", shipDate); 

                return retLst;
            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion

        #region ICache Members
        /// <summary>
        /// 刷新所有Cache
        /// </summary>
        public void RefreshAllCache()
        {
            logger.Warn("RefreshAllCache!!");
            DataChangeMediator.RefreshAllCache();
        }
        /// <summary>
        /// 更新所有Cache
        /// </summary>
        public void UpdateCacheNow()
        {
            logger.Warn("UpdateCacheNow!!");
            DataChangeMediator.UpdateCacheNow();
        }

        /// <summary>
        /// 更新PartMatch dll load into MEF cache 
        /// </summary>
        public void RefreshPartMatchAssembly()
        {
            logger.Warn("RefreshPartMatchAssembly!!");
            IMES.FisObject.Common.Part.PartPolicy.PartSpecialCodeContainer.GetInstance.RefreshParts();
        }

        static readonly IMES.Infrastructure.Utility.ICache bomCacheRep = (IMES.Infrastructure.Utility.ICache)RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        static readonly IMES.Infrastructure.Utility.ICache partCacheRep = (IMES.Infrastructure.Utility.ICache)RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        static readonly IMES.Infrastructure.Utility.ICache modelCacheRep = (IMES.Infrastructure.Utility.ICache)RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        static readonly IMES.Infrastructure.Utility.ICache familyCacheRep = (IMES.Infrastructure.Utility.ICache)RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

        /// <summary>
        /// 刪除Cache Item by Key
        /// </summary>
        /// <param name="cacheType">Bom, Part, Model, Family</param>
        /// <param name="key"></param>
        public void RemoveCacheItem(string cacheType, string key)
        {
            if (logger.IsInfoEnabled)
            {
                logger.InfoFormat("Remove CacheType:{0} Key:{1}", cacheType, key);
            }
            switch (cacheType)
            {
                case DataChangeMediator.CacheSwitchType.BOM:

                    if (bomCacheRep.IsCached())
                    {
                        bomCacheRep.ProcessItem(new CacheUpdateInfo { Item = key, Type = cacheType });
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, key);
                    }
                    break;
                case DataChangeMediator.CacheSwitchType.Part:

                    if (partCacheRep.IsCached())
                    {
                        partCacheRep.ProcessItem(new CacheUpdateInfo { Item = key, Type = cacheType });
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, key);
                    }
                    break;
                case DataChangeMediator.CacheSwitchType.Model:

                    if (modelCacheRep.IsCached())
                    {
                        modelCacheRep.ProcessItem(new CacheUpdateInfo { Item = key, Type = cacheType });
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, key);
                    }
                    break;
                case DataChangeMediator.CacheSwitchType.Family:

                    if (familyCacheRep.IsCached())
                    {
                        familyCacheRep.ProcessItem(new CacheUpdateInfo { Item = key, Type = cacheType });
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, key);
                    }
                    break;
            }

        }

        static readonly IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        static readonly IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        static readonly IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        static readonly IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

        /// <summary>
        /// 刪除Cache Item by KeyList
        /// </summary>
        /// <param name="cacheType">Bom, Part, Model, Family</param>
        /// <param name="keyList"></param>
        public void RemoveCacheItem(string cacheType, string[] keyList)
        {
            if (logger.IsInfoEnabled)
            {
                logger.InfoFormat("Remove CacheType:{0} Key:{1}", cacheType, string.Join(",", keyList));
            }
            switch (cacheType)
            {
                case DataChangeMediator.CacheSwitchType.BOM:
                    if (bomCacheRep.IsCached())
                    {
                        bomRep.RemoveCacheByKeyList(keyList);
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, string.Join(",", keyList));
                    }
                    break;
                case DataChangeMediator.CacheSwitchType.Part:
                    if (partCacheRep.IsCached())
                    {
                        partRep.RemoveCacheByKeyList(keyList);
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, string.Join(",", keyList));
                    }
                    break;
                case DataChangeMediator.CacheSwitchType.Model:
                    if (modelCacheRep.IsCached())
                    {
                        modelRep.RemoveCacheByKeyList(keyList);
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, string.Join(",", keyList));
                    }
                    break;
                case DataChangeMediator.CacheSwitchType.Family:
                    if (familyCacheRep.IsCached())
                    {
                        familyRep.RemoveCacheByKeyList(keyList);
                    }
                    else
                    {
                        logger.WarnFormat("Cache Disable CacheType:{0} Key:{1}", cacheType, string.Join(",", keyList));
                    }
                    break;
            }
        }


        #endregion

        #region Implementation of IKittingCode
        /// <summary>
        /// 获取全部KittingCode信息列表
        /// </summary>
        /// <returns>KittingCode信息列表</returns>
        public IList<KittingCodeInfo> GetKittingCodeList()
        {
            IList<KittingCodeInfo> retLst = null;
            try
            {

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                retLst = productRepository.GetKittingCodeList();
               
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion

        #region Implementation of ILightCode
        /// <summary>
        /// 获取全部LightCode列表
        /// </summary>
        /// <returns>LightCode列表</returns>
        public IList<string> GetLightCodeList()
        {
            IList<string> retLst = new List<string>();
            try
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                retLst = productRepository.GetLabelKittingCodeList("FA Label");

                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Implementation of IDataMigration
        ///<summary>
        /// Data migration from imes to old fis
        ///</summary>
        ///<param name="keyid">ProductID， CartonID，PalletID</param>
        ///<param name="pcode">UI PCode</param>
        ///<param name="no1">reserved parameter</param>
        ///<param name="no2">reserved parameter</param>
        public void ImesToFis(string keyid, string pcode, string no1, string no2)
        {
            if (no1 == null)
            {
                no1 = string.Empty;
            }

            if (no2 == null)
            {
                no2 = string.Empty;
            }

            IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
           // miscRepository.ImesToFis(keyid, pcode, no1, no2);
            
            
        }

        #endregion

        #region Implementation of IPalletType

        /// <summary>
        /// 根据type从ConstValue获取对应的信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns>信息列表</returns>
        public IList<ConstValueInfo> GetConstValueListByType(string type,string orderby)
        {
            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var resultLst = palletRepository.GetConstValueListByType(type);
                    if (!string.IsNullOrEmpty(orderby))
                    {
                        var tmpLst = from item in resultLst orderby item.id select item;
                        retLst = tmpLst.ToList();
                    }
                    else
                    {
                        retLst = resultLst;
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
      
        }

        public IList<ConstValueTypeInfo> GetConstValueTypeListByType(string type)
        {
            IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    retLst = partRep.GetConstValueTypeList(type);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Implementation of IDismantleType
        
        /// <summary>
        /// 取得ConstValue列表
        /// </summary>
        /// <returns>ConstValue列表</returns>
        public IList<ConstValueInfo> GetDismantleTypeList(string type)
        {
            IList<ConstValueInfo> retLst = null;
            try
            {
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                retLst = partRepository.GetConstValueListByType(type);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Implementation of ILabelKittingCode

        /// <summary>
        /// 根据type获取对应的LabelKittingCode信息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns>LabelKittingCode信息列表</returns>
        public IList<LabelKittingCode> GetLabelKittingCodeList(string type)
        {
            IList<LabelKittingCode> retLst = null;
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                    retLst = productRepository.GetLabelKittingCodeListByType(type);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion
        // add by dorothy
        #region Implementation of IStationByType

        /// <summary>
        /// 根据type获取对应的Station信息列表按照Descr排序
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Station信息列表</returns>
        public IList<StationInfo> GetStationByTypeList(string type)
        {
            IList<StationInfo> retlist = new List<StationInfo>();
            try
            {
                IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                IList<IStation> stalist = stationRepository.GetStationListFromStation(type);

               
                if (stalist != null && stalist.Count > 0)
                {
                    StationInfo staInfo = null;
                    foreach (IStation sta in stalist)
                    {
                        staInfo = new StationInfo();
                        staInfo.StationId = sta.StationId;
                        staInfo.Descr = sta.Descr;
                        retlist.Add(staInfo);
                    }
                }

                return retlist;
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        #endregion

        #region Implementation of IDeliveryByModel

        /// <summary>
        /// 根据model获取对应的delivery信息列表按照ShipDate排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Delivery信息列表</returns>
        public IList<DNForUI> GetDeliveryListByModel(string model,string status)
        {

            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IList<Delivery> dnlist = dnRep.GetDeliveryListByShipDateAndModelAndStatus(model);
            IList<DNForUI> retlist = new List<DNForUI>();

            foreach (Delivery dn in dnlist)
            {
                DNForUI dnInfo = new DNForUI();
                dnInfo.DeliveryNo = dn.DeliveryNo;
                dnInfo.ShipDate = dn.ShipDate;
                dnInfo.Qty = dn.Qty;
                dnInfo.ShipWay = dn.DeliveryEx.ShipWay;
                retlist.Add(dnInfo);
                
            }
            return retlist;
        }

        #endregion

        //add by zhulei
        #region Implementation of IQty

        /// <summary>
        /// Get Qty
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public int GetQty(string line)
        {
            int ret = 0;
            DateTime dt = Convert.ToDateTime("1900-01-01 00:00:00.000");
            try
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                IList<int> qtylist = mbRepository.GetQtyListFromPcaIctCountByCdtAndPdLine(dt,line);


                if (qtylist.Count > 0)
                {
                    ret = qtylist[0];
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Clear Qty
        /// </summary>
        /// <param name="line"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public void ClearQty(string line, string qty)
        {
            DateTime dt = Convert.ToDateTime("1900-01-01 00:00:00.000");
            PcaIctCountInfo pcaIctInfo = new PcaIctCountInfo();
            try
            {
                pcaIctInfo.cdt = DateTime.Now;
                pcaIctInfo.pdLine = line;
                pcaIctInfo.qty = Convert.ToInt32(qty);

                SqlTransactionManager.Begin();

                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                mbRepository.UpdateQtyFromPcaIctCountByCdtAndPdLine(0, dt, line);
                mbRepository.InsertPcaIctCountInfo(pcaIctInfo);

                SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }

        }

        #endregion

        #region impliementation of IPartQuery
        public IList<PartQty> QueryPartByModel(IList<ModelQty> models, IList<string> stations)
        {
            IDictionary<string, PartQty> tempResult = new Dictionary<string, PartQty>();
            foreach (var modelQtyObj in models)
            {
                foreach (var station in stations)
                {
                    IFlatBOM sessionBOM = null;
                    var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    string customer = string.Empty;
                    string family = string.Empty;
                    IFlatBOM bom = bomRepository.GetModelFlatBOMByStationModel(customer, station, modelQtyObj.Line, family, modelQtyObj.Model, null);
                    if (bom != null)
                    {
                        foreach (var bomitem in bom.BomItems)
                        {
                            if (bomitem.AlterParts != null)
                            {
                                var part = bomitem.AlterParts.First();
                                string pn = part.PN;
                                string key = pn + "|" + modelQtyObj.Line;
                                if (tempResult.ContainsKey(key))
                                {
                                    tempResult[key].Qty += bomitem.Qty * modelQtyObj.Qty;
                                }
                                else
                                {
                                    var value = new PartQty();
                                    value.Pn = pn;
                                    value.Line = modelQtyObj.Line;
                                    value.Qty = bomitem.Qty * modelQtyObj.Qty;
                                    value.Description = part.Descr;
                                    tempResult.Add(key, value);
                                }
                            }
                        }
                    }
                }

            }
            return tempResult.Values.ToList();
        }
        #endregion
        
        #region IBOMQuery
            public IList<StationInfo> GetAllPartCollectionStation()
            {
                var bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IList<StationInfo> stations = bomRep.GetAllPartCollectionStation();
                return stations;
            }

            public IDictionary<string, IList<IMES.DataModel.BomItemInfo>> GetBOM(string customer, string model, string line)
            {
                ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                IDictionary<string, IList<IMES.DataModel.BomItemInfo>> ret =
                    new Dictionary<string, IList<IMES.DataModel.BomItemInfo>>();
                var modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                var bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<StationInfo> stations = bomRep.GetAllPartCollectionStation();
                var modelObj = modelRep.Find(model);
                if (modelObj == null)
                {
                    return null;
                }


                string pkStation = "-PK01-PK02-PK03-PK04-PK05-PKOK-";
                string DDDKitStation6 = "PKOK";

                foreach (var station in stations)
                {
                    if (!pkStation.Contains(station.StationId))
                    {
                        try
                        {
                            IFlatBOM bom = bomRep.GetModelFlatBOMByStationModel(customer, station.StationId, line, modelObj.FamilyName, model, null);
                            if (bom != null)
                            {
                                IList<IMES.DataModel.BomItemInfo> bomInfos = bom.ToBOMItemInfoList();
                                ret.Add(station.StationId, bomInfos);
                            }
                        }
                        catch (FisException e)
                        {
                            if (string.Compare(e.mErrcode, "BOM001") != 0)
                            {
                                logger.Error(e.Message, e);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Error(e.Message, e);
                        }
                    }
                }

                try
                {
                    IFlatBOM pizzakittingBOM = bomRep.GetModelFlatBOMByStationModel(customer, DDDKitStation6, line, modelObj.FamilyName, model, null);
                    if (pizzakittingBOM != null)
                    {
                        IList<IMES.DataModel.BomItemInfo> bomInfos = pizzakittingBOM.ToBOMItemInfoList();
                        ret.Add(DDDKitStation6, bomInfos);
                    }
                }
                catch (FisException e)
                {
                    if (string.Compare(e.mErrcode, "BOM001") != 0)
                    {
                        logger.Error(e.Message, e);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e.Message, e);
                }
                
                return ret;
            }


        #endregion


            #region IExecSP Members
            ILog ExecSQLLogger = LogManager.GetLogger("ExecSQLLogger");

            public DataTable GetSPResult(string editor, string dbName, string spName, string[] ParameterNameArray, string[] ParameterValueArray)
            {

                
                if (ParameterNameArray == null || ParameterValueArray==null)
                {
                    return null;
                }
                
                string ParameterValuesStr ="";
                System.Data.SqlClient.SqlParameter[] paramsArray = new System.Data.SqlClient.SqlParameter[ParameterNameArray.Length];
                for (int i = 0; i < ParameterNameArray.Length;i++ ) {
                    paramsArray[i] = new System.Data.SqlClient.SqlParameter(ParameterNameArray[i].StartsWith("@") ? ParameterNameArray[i] : "@"+ParameterNameArray[i], ParameterValueArray[i]);
                    ParameterValuesStr = ParameterValuesStr + paramsArray[i].ParameterName + "=" + ParameterValueArray[i] + ",";
                }
                ExecSQLLogger.Info("editor is: " + editor + ", Database is: " + dbName + ", SPName is: " + spName + ", Parameters is :" + ParameterValuesStr.TrimEnd(new char[]{','}));


                IProductRepository MyRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                string ConnectString = string.Format(IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString, dbName);
                try{
                return IMES.Infrastructure.Repository._Schema.SqlHelper.ExecuteDataFillConsiderOutParams(ConnectString, CommandType.StoredProcedure, spName, paramsArray);
                }
                catch (Exception e)
                {
                    ExecSQLLogger.Error(e.Message, e);
                    throw e;
                }
            }

            public DataSet GetSQLResult(string editor,string dbName, string sqlText, string[] ParameterNameArray, string[] ParameterValueArray)
            {
                ExecSQLLogger.Info("editor is: " + editor + ", Database is: " + dbName + ", ExecSQL is: " + sqlText);
                System.Data.SqlClient.SqlParameter[] paramsArray = null;
                if (ParameterNameArray != null && ParameterValueArray != null && ParameterNameArray.Length == ParameterValueArray.Length)
                {
                    paramsArray = new System.Data.SqlClient.SqlParameter[ParameterNameArray.Length];
                    for (int i = 0; i < ParameterNameArray.Length; i++)
                    {
                        paramsArray[i] = new System.Data.SqlClient.SqlParameter(ParameterNameArray[i].StartsWith("@") ? ParameterNameArray[i] : "@" + ParameterNameArray[i], ParameterValueArray[i]);
                    }
                }
                
                string ConnectString = string.Format(IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString, dbName);
                if (string.IsNullOrEmpty(dbName)) {
                    ConnectString = IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_FA;
                }

                IProductRepository MyRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                try
                {
                    return IMES.Infrastructure.Repository._Schema.SqlHelper.ExecSPorSql(ConnectString, CommandType.Text, sqlText, paramsArray);
                }
                catch (Exception e)
                {
                    ExecSQLLogger.Error(e.Message, e);
                    throw e;
                }
            }

            #endregion

            #region Implementation of IDeliveryByCarton

            /// <summary>
            /// 根据model获取对应的delivery信息列表按照ShipDate排序
            /// </summary>
            /// <param name="model"></param>
            /// <returns>Delivery信息列表</returns>
            public IList<DNForUI> GetDeliveryListByCarton(string model, string status)
            {
                IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<Delivery> dnlist = dnRep.GetDeliveryListByShipDateAndModelAndStatus(model);
                IList<DNForUI> retlist = new List<DNForUI>();

                foreach (Delivery dn in dnlist)
                {
                    DNForUI dnInfo = new DNForUI();
                    dnInfo.DeliveryNo = dn.DeliveryNo;
                    dnInfo.ShipDate = dn.ShipDate;
                    dnInfo.Qty = dn.Qty;
                    dnInfo.ModelName = dn.ModelName;
                    retlist.Add(dnInfo);
                }
                return retlist;

            }

            #endregion

        private static CommonImpl _Instance = null;

        /// <summary>
        /// GetInstance
        /// </summary>
        /// <returns></returns>
        public static CommonImpl GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new CommonImpl();
            }
            return _Instance;
        }

        /// <summary>
        /// 檢查 BlockStation
        /// 參數: 
        /// </summary>
        public void CheckProductBlockStation(Product currentProduct, string pdLine, string editor, string Station, string Customer)
        {
            if (currentProduct.Status == null)
            {
                throw new FisException("SFC002", new string[] { currentProduct.ProId });
            }

            IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            string notEmpytLine = pdLine;

            if (string.IsNullOrEmpty(notEmpytLine) && currentProduct.Status != null)
            {
                notEmpytLine = currentProduct.Status.Line;
            }

            if (currentProduct.Status != null && string.IsNullOrEmpty(currentProduct.Status.ReworkCode))
            {
                string firstLine = "";
                if (!string.IsNullOrEmpty(notEmpytLine))
                {
                    firstLine = notEmpytLine.Substring(0, 1);
                }
                IList<ModelProcess> currentModelProcess = CurrentProcessRepository.GetModelProcessByModelLine(currentProduct.Model, firstLine);
                if (currentModelProcess == null || currentModelProcess.Count == 0)
                {
                    //CurrentProcessRepository.CreateModelProcess(currentProduct.Model, editor, firstLine);
                    ResolveProcess.CreateModelProcess(currentProduct.ModelObj, editor, firstLine);
                }
            }

            CurrentProcessRepository.SFC(notEmpytLine, Customer, Station, currentProduct.ProId, "Product");
        }

        /// <summary>
        /// 檢查 BlockStation
        /// 參數: 
        /// </summary>
        public void CheckProductBlockStation(MB mb, string pdLine, string editor, string Station, string Customer)
        {
            IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            string notEmpytLine = pdLine;

            if (mb == null || mb.MBStatus == null)
            {
                throw new FisException("SFC001", new string[] { "" });
            }
            string keyOfSFC = mb.Sn;

            if (string.IsNullOrEmpty(notEmpytLine))
            {
                notEmpytLine = mb.MBStatus.Line;
            }

            CurrentProcessRepository.SFC(notEmpytLine, Customer, Station, keyOfSFC, "MB");
        }

    }



}

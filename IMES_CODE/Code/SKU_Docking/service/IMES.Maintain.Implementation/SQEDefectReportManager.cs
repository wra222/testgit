using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using log4net;
using IMES.Infrastructure.Repository;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Maintain.Implementation
{
    public class SQEDefectReportManager : MarshalByRefObject, ISQEDefectReport
    {

        IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
        #region ISQEDefectReport Members

        /// <summary>
        /// 添加一条iqckp记录
        /// </summary>
        /// <param name="iqcKp"></param>
        public void AddIqcKp(IqcKpDef iqcKp)
        {
            try
            {
                defectRepository.AddIqcKp(iqcKp);
            }

            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据type获取sqedefect数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<DefectCodeInfo> GetDefecSQEDefectInfo(string type)
        {
            try
            {
                IList<DefectCodeInfo> lstDefectInfo = new List<DefectCodeInfo>();
                lstDefectInfo = defectRepository.GetDefectCodeList(type);
                if (lstDefectInfo != null && lstDefectInfo.Count > 0)
                {
                    return lstDefectInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (FisException ex)
            {
                throw ex;
            }

            catch (ExecutionEngineException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据tp,ctno,defect和cause获取不良信息记录
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="CtLabel"></param>
        /// <param name="Defect"></param>
        /// <param name="Cause"></param>
        /// <returns></returns>
        public IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string CtLabel, string Defect, string Cause)
        {
            try
            {
                IList<IqcKpDef> lstIqcKp = new List<IqcKpDef>();
                lstIqcKp = defectRepository.GetIqcKpByTypeCtLabelAndDefect(tp, CtLabel, Defect, Cause);
                if (lstIqcKp != null && lstIqcKp.Count > 0)
                {
                    return lstIqcKp;

                }
                else
                {
                    return null;
                }

            }
            catch (FisException ex)
            {
                throw ex;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据tp,ctno,defect获取不良信息记录
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="CtLabel"></param>
        /// <param name="Defect"></param>
        /// <returns></returns>
        public IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string CtLabel, string Defect)
        {
            try
            {
                IList<IqcKpDef> lstIqcKp = new List<IqcKpDef>();
                lstIqcKp = defectRepository.GetIqcKpByTypeCtLabelAndDefect(tp, CtLabel, Defect);
                if (lstIqcKp != null && lstIqcKp.Count > 0)
                {
                    return lstIqcKp;
                }
                else
                {
                    return null;
                }
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据ctno获取parttype信息记录
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public IList<string> GetPartTypeSQEDefectReport(string sn)
        {
            IList<string> lstPartType = new List<string>();
            try
            {
                lstPartType = defectRepository.GetPartTypeSQEDefectReport(sn);
                if (lstPartType != null && lstPartType.Count > 0)
                {
                    return lstPartType;
                }
                else
                {
                    return null;
                }

            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// CTNo相关的不良记录信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public IList<SQEDefectCTNoInfo> GetSQEDefectCTNoInfo(string sn)
        {
            try
            {
                IList<SQEDefectCTNoInfo> lstDefectCode = new List<SQEDefectCTNoInfo>();
                lstDefectCode = defectRepository.GetSQEDefectCTNoInfo(sn);
                if (lstDefectCode != null && lstDefectCode.Count > 0)
                {
                    return lstDefectCode;
                }
                else
                {
                    return null;
                }

            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据ctno获取不良信息详细记录
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public IList<SQEDefectProductRepairReportInfo> GetSQEDefectProductRepairInfo(string sn)
        {
            try
            {
                IList<SQEDefectProductRepairReportInfo> lstProductRepairInfo = new List<SQEDefectProductRepairReportInfo>();
                lstProductRepairInfo = defectRepository.GetSQEDefectProductRepairInfo(sn);
                if (lstProductRepairInfo != null && lstProductRepairInfo.Count > 0)
                {
                    return lstProductRepairInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据tp,ctno和defect更细一条iqckp记录
        /// </summary>
        /// <param name="iqcKp"></param>
        /// <param name="tp"></param>
        /// <param name="ctLabel"></param>
        /// <param name="defect"></param>
        public void UpdateIqcKp(IqcKpDef iqcKp, string tp, string ctLabel, string defect)
        {

            IqcKpDef condition = new IqcKpDef();
            //IList<IqcKpDef> lstSetIqckp = defectRepository.GetIqcKpByTypeCtLabelAndDefect(tp, ctLabel, defect);
            IqcKpDef iqcKpSet = new IqcKpDef();
            //condition = lstSetIqckp.First();
            iqcKpSet.Cause = iqcKp.Cause;
            iqcKpSet.Editor = iqcKp.Editor;
            iqcKpSet.Location = iqcKp.Location;
            iqcKpSet.Obligation = iqcKp.Obligation;
            iqcKpSet.Model = iqcKp.Model;
            iqcKpSet.Remark = iqcKp.Remark;
            iqcKpSet.Result = iqcKp.Result;
            iqcKpSet.Udt = iqcKp.Udt;
                        
            condition.Tp = tp;
            condition.CtLabel = ctLabel;
            condition.Defect = defect;
            try
            {
            defectRepository.UpdateIqcKp(iqcKpSet, condition);
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据type获取defectinfo数据list
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<DefectInfoDef> GetDefectInfoByType(string type)
        {
            IList<DefectInfoDef> retLst = null;
            try
            {
                IDefectInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository>();
                retLst = itemRepository.GetRepairInfoByCondition(type);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// GetIECPNList()
        /// </summary>
        /// <param name="ctNo"></param>
        /// <returns></returns>
        public IList<string> GetIecpnList(string ctNo)
        {
            try
            {
                IList<string> retList = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                string[] param = { "RDESC", "RRRDESC", "RRRRRDESC" };
                //'RDESC','RRRDESC','RRRDESC' -'RRRRRDESC'
                retList = partRepository.GetPartInfoValueByInfoTypesAndInfoValuePrefix(param, ctNo);
                return retList;
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion
    }
}

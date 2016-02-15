/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Defect Station
* UI:CI-MES12-SPEC-PAK-DATA MAINTAIN（II）.docx –2012/05/15 
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-15   Du.Xuan               Create   
* ITC-1361-0147 对defect cur pre相同的数据Add&update进行保护
* Known issues:
* TODO：
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using log4net;

using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Defect;

using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;


namespace IMES.Maintain.Implementation
{
    public class DefectStationManager : MarshalByRefObject, IDefectStation
    {

        #region IGrade Members
        
        public const string PUB = "<PUB>";
        public const string HP = "HP";
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static IList<DefectCodeStationInfo> defectList = new List<DefectCodeStationInfo>();
        IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
        IDefectRepository defectRep = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();


        public IList<DefectCodeStationInfo> GetDefectList()
        {

            IList<DefectCodeStationInfo> defectList = new List<DefectCodeStationInfo>();
            try 
            {
                DefectCodeStationInfo cond = new DefectCodeStationInfo();
                //defectList = stationRep.GetDefectCodeStationList(cond);
                defectList = stationRep.GetDefectCodeStationList2(cond);

            }
            catch(FisException fe)
            {
                logger.Error(fe.Message);
                throw fe;
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return defectList;
        }

        public bool CheckFamily(string family)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            bool ret = false;
            try
            {
                IFamilyRepository iFamilyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                Family item = iFamilyRepository.FindFamily(family);
                if (item == null)
                {
                    ret = CheckModel(family);
                }
                else
                {
                    ret = true;
                }
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public bool CheckModel(string model)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            bool ret = false;
            try
            {
                IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                Model item = iModelRepository.Find(model);
                if (item != null)
                {
                    ret = true;
                }
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public string AddDefectStation(DefectCodeStationInfo stationItem)
        {
            try 
            {
                if (stationRep.CheckDefectStationUnique(stationItem.pre_stn, stationItem.crt_stn, stationItem.majorPart, stationItem.cause, stationItem.defect,stationItem.family))
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT150", erpara);
                    throw ex;
                }    
                DefectCodeStationInfo newitem = new DefectCodeStationInfo();
                stationItem.cdt = DateTime.Now;
                stationItem.udt = DateTime.Now;
                stationRep.InsertDefectCodeStationInfo(stationItem);
                return Convert.ToString(stationItem.id);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        public void DeleteDefectStation(int id)
        {
            try 
            {
                DefectCodeStationInfo conItem = new DefectCodeStationInfo();
                conItem.id = id;
                stationRep.DeleteDefectCodeStationInfo(conItem);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        public void UpdateDefectStation(DefectCodeStationInfo stationItem)
        {
            try 
            {
                //if (stationRep.CheckDefectStationUnique(stationItem.pre_stn, stationItem.crt_stn, stationItem.majorPart, stationItem.cause, stationItem.defect))
                //{
                //    List<string> erpara = new List<string>();
                //    FisException ex;
                //    ex = new FisException("DMT150", erpara);
                //    throw ex;
                //}  
                DefectCodeStationInfo conItem = new DefectCodeStationInfo();
                conItem.id = stationItem.id;
                stationItem.id = int.MinValue;
                stationItem.udt = DateTime.Now;
                stationRep.UpdateDefectCodeStationInfo(stationItem,conItem);    
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        #endregion

        public IList<IMES.DataModel.DefectInfo> GetDefectCodeList()
        {
            //Select Defect, Descr from DefectCode where Type=’PRD’ order by Defect
            IList<IMES.DataModel.DefectInfo> defectList = null;
            try
            {
                defectList = defectRep.GetDefectList("PRD");
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return defectList;
        }

        public IList<StationMaintainInfo> GetStationList()
        {
            //Select Station, Name from Station order by Station
            IList<StationMaintainInfo> stationList = new List<StationMaintainInfo>();

            try
            {
                IList<IStation> tmpList = null;

                tmpList = stationRep.FindAll();

                foreach (IStation temp in tmpList)
                {
                    
                    StationMaintainInfo station = new StationMaintainInfo();

                    station.Station = temp.StationId;
                    station.Descr = temp.Name;

                    stationList.Add(station);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return stationList;
        }

        public IList<string> GetPreStationFromDefectStation()
        {
            IList<string> stationList = new List<string>();

            try
            {
                return stationRep.GetPreStationFromDefectStation();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// 取得MajorPart信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>MajorPart信息列表</returns>
        public IList<List<string>> GetMajorPartList(string type, string customer)
        {
            try
            {
                string strSQL = @"select Code, Description from DefectInfo where [Type]=@DType order by Code";

                SqlParameter paraNameType = new SqlParameter("@DType", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = type;

                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL,
                    paraNameType);

                List<List<string>> list = new List<List<string>>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string Code = dr[0] as string;
                    string EngDescr = dr[1] as string;
                    List<string> l = new List<string>(new string[] { Code.Trim(), EngDescr.Trim() });
                    list.Add(l);
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeStationInfo> GetDefectStationByPreStation(string preStation)
        {
            try
            {
                DefectCodeStationInfo pre_stn = new DefectCodeStationInfo();
                pre_stn.pre_stn = preStation;
                return stationRep.GetDefectCodeStationList2(pre_stn);
            }
            catch (FisException fe)
            {
                logger.Error(fe.Message);
                throw fe;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        public bool CheckDefectStationUnique(string preStation, string curStation, string majorPart, string cause, string defect)
        {
            try
            {
                return stationRep.CheckDefectStationUnique(preStation, curStation, majorPart, cause, defect);
            }
            catch (FisException fe)
            {
                logger.Error(fe.Message);
                throw fe;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }
    }
}

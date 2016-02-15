using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Hold;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using IMES.Maintain.Implementation;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;

namespace IMES.Maintain.Implementation
{
    public class DefectHoldRule : MarshalByRefObject, IDefectHoldRule
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IHoldRepository iHoldRepository = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();

        public IList<string> GetLineTop()
        {
            logger.Debug("(DefectHoldRule)GetLineTop starts");
            try
            {
                string strSQL = @"select distinct Line from DefectHoldRule order by Line ";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)GetLineTop end");
            }
        }

        public IList<string> GetLine()
        {
            logger.Debug("(DefectHoldRule)GetLine starts");
            try
            {
                string strSQL = @"select distinct AliasLine from LineEx order by AliasLine";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)GetLine end");
            }
        }

        public IList<string> GetCheckInStation()
        {
            logger.Debug("(DefectHoldRule)GetCheckInStation starts");
            try
            {
                string strSQL = @"select distinct Value from ConstValueType
                                    where Type='HoldCheckInStation'
                                    order by Value";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)GetCheckInStation end");
            }
        }

        public IList<string> GetHoldStation()
        {
            logger.Debug("(DefectHoldRule)GetHoldStation starts");
            try
            {
                string strSQL = @"select distinct Station from StationAttr
                                    where AttrName='IsHold' and AttrValue='Y'
                                    order by Station";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)GetHoldStation end");
            }
        }

        public IList<string> GetHoldCode()
        {
            logger.Debug("(DefectHoldRule)GetHoldCode starts");
            try
            {
                string strSQL = @"select distinct Defect,Descr from DefectCode
                                    where Type='HOLD' order by Defect";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)GetHoldCode end");
            }
        }

        public bool CheckFamilyAndModel(string inputDate)
        {
            logger.Debug("(DefectHoldRule)CheckFamilyAndModel starts");
            try
            {
                string strSQL = @"select * from Family where Family = @Family";
                SqlParameter paraNameType = new SqlParameter("@Family", SqlDbType.VarChar, 30);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = inputDate;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);


                strSQL = @"select * from Model where Model = @Model";
                paraNameType = new SqlParameter("@Model", SqlDbType.VarChar, 30);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = inputDate;
                DataTable dt2 = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);

                if (dt.Rows.Count == 0 && dt2.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)CheckFamilyAndModel end");
            }
        }

        public IList<DefectHoldRuleInfo> GetDefectHoldRule(DefectHoldRuleInfo condition)
        {
            logger.Debug("(DefectHoldRule)GetDefectHoldRule starts");
            try
            {
                return iHoldRepository.GetDefectHoldRule(condition);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)GetDefectHoldRule end");
            }
        }

        public void InsertDefectHoldRule(DefectHoldRuleInfo item)
        {
            logger.Debug("(DefectHoldRule)InsertDefectHoldRule starts");
            try
            {
                DefectHoldRuleInfo checkInfo = new DefectHoldRuleInfo();
                if (item.Line != "")
                {
                    checkInfo.Line = item.Line;
                }
                if (item.Family != "")
                {
                    checkInfo.Family = item.Family;
                }
                if (item.DefectCode != "")
                {
                    checkInfo.DefectCode = item.DefectCode;
                }
                if (item.CheckInStation != "")
                {
                    checkInfo.CheckInStation = item.CheckInStation;
                }

                
                checkInfo.EqualSameDefectCount = item.EqualSameDefectCount;
           
                //checkInfo.OverDefectCount = item.OverDefectCount;
                
                IList<DefectHoldRuleInfo> checkInfoList = iHoldRepository.GetDefectHoldRule(checkInfo);

                if (checkInfoList != null && checkInfoList.Count != 0)
                {
                    IList<DefectHoldRuleInfo> checkcount = new List<DefectHoldRuleInfo>();
                    
                    if (item.OverDefectCount != 0)
                    {
                        checkcount = (from q in checkInfoList
                                      where q.OverDefectCount != 0
                                      select q).ToList<DefectHoldRuleInfo>();
                        if (checkcount != null && checkcount.Count != 0)
                        {
                            throw new Exception("This OverDefectCode is Exists, Please check it.");
                        }
                    }
                    throw new Exception("This Info is Exists");
                }
                iHoldRepository.InsertDefectHoldRule(item);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)InsertDefectHoldRule end");
            }
        }

        public void UpdateDefectHoldRule(DefectHoldRuleInfo item)
        {
            logger.Debug("(DefectHoldRule)UpdateDefectHoldRule starts");
            try
            {
                iHoldRepository.UpdateDefectHoldRule(item);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)UpdateDefectHoldRule end");
            }
        }

        public void DeleteDefectHoldRule(int id)
        {
            logger.Debug("(DefectHoldRule)DeleteDefectHoldRule starts");
            try
            {
                iHoldRepository.DeleteDefectHoldRule(id);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DefectHoldRule)DeleteDefectHoldRule end");
            }
        }
    }
}

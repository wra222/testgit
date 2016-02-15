using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.Maintain.Implementation;
using log4net;
using IMES.FisObject.Common.Hold;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.Model;

namespace IMES.Maintain.Implementation
{
    public class HoldRule : MarshalByRefObject, IHoldRule
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IHoldRepository iHoldRepository = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();

        private IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();

        public IList<string> GetLineTop()
        {
            logger.Debug("(HoldRule)GetLineTop starts");
            try
            {
                string strSQL = @"select distinct Line from HoldRule order by Line";
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
                logger.Debug("(HoldRule)GetLineTop end");
            }
        }

        public IList<string> GetFamilyTop()
        {
            logger.Debug("(HoldRule)GetFamilyTop starts");
            try
            {
                string strSQL = @"select distinct Family from HoldRule order by Family";
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
                logger.Debug("(HoldRule)GetFamilyTop end");
            }
        }

        public IList<string> GetModelTop(string family)
        {
            logger.Debug("(HoldRule)GetModelTop starts");
            try
            {
                string strSQL = @"select distinct Model from HoldRule Where Family=@Family order by Model";
                SqlParameter paraNameType = new SqlParameter("@Family", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = family;
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
                logger.Debug("(HoldRule)GetModelTop end");
            }
        }

        public IList<string> GetLine()
        {
            logger.Debug("(HoldRule)GetLine starts");
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
                logger.Debug("(HoldRule)GetLine end");
            }
        }

        public IList<string> GetFamily()
        {
            logger.Debug("(HoldRule)GetFamily starts");
            try
            {
                string strSQL = @"select distinct Family from Family order by Family";
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
                logger.Debug("(HoldRule)GetFamily end");
            }
        }

        public IList<string> GetCheckInStation()
        {
            logger.Debug("(HoldRule)GetCheckInStation starts");
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
                logger.Debug("(HoldRule)GetCheckInStation end");
            }
        }

        public IList<string> GetHoldStation()
        {
            logger.Debug("(HoldRule)GetHoldStation starts");
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
                logger.Debug("(HoldRule)GetHoldStation end");
            }
        }

        public IList<string> GetHoldCode()
        {
            logger.Debug("(HoldRule)GetHoldCode starts");
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
                logger.Debug("(HoldRule)GetHoldCode end");
            }
        }

        public IList<HoldRuleInfo> GetHoldRule(HoldRuleInfo condition)
        {
            logger.Debug("(HoldRule)GetHoldRule starts");
            try
            {
                return iHoldRepository.GetHoldRule(condition);
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
                logger.Debug("(HoldRule)GetHoldRule end");
            }
        }

        public void InsertMultiCustSNHoldRule(IList<string> custSnList, HoldRuleInfo item)
        {
            logger.Debug("(HoldRule)InsertMultiCustSNHoldRule starts");
            try
            {
                if (!string.IsNullOrEmpty(item.Family) && !string.IsNullOrEmpty(item.Model))
                {
                    IList<Model> checkModelList = iModelRepository.GetModelListByModel(item.Family, item.Model);
                    if (checkModelList.Count == 0 || checkModelList == null)
                    {
                        throw new Exception("This Model:[" + item.Model.ToString() + "] is not Exists in This Family :[" + item.Family.ToString() + "]");
                    }
                }
                //else if (!string.IsNullOrEmpty(item.Model))
                //{
                //    Model checkModel = iModelRepository.Find(item.Model);
                //    if (checkModel == null)
                //    {
                //        throw new Exception("This Model :[" + item.Model.ToString() + "] is not Exists");
                //    }
                //}

                if (iHoldRepository.ExistHoldRuleByCustSn(custSnList))
                {
                    throw new Exception("Input custSn is Exists");
                }
                iHoldRepository.InsertMultiCustSNHoldRule(custSnList,item);
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
                logger.Debug("(HoldRule)InsertMultiCustSNHoldRule end");
            }
        }

        public void InsertHoldRule(HoldRuleInfo item)
        {
            logger.Debug("(HoldRule)InsertHoldRule starts");
            try
            {
                if (!string.IsNullOrEmpty(item.Family) && !string.IsNullOrEmpty(item.Model))
                {
                    IList<Model> checkModelList = iModelRepository.GetModelListByModel(item.Family, item.Model);
                    if (checkModelList.Count == 0 || checkModelList == null)
                    {
                        throw new Exception("This Model:[" + item.Model.ToString() + "] is not Exists in This Family :[" + item.Family.ToString() + "]");
                    }
                }
                //else if (!string.IsNullOrEmpty(item.Model))
                //{
                //    Model checkModel = iModelRepository.Find(item.Model);
                //    if (checkModel == null)
                //    {
                //        throw new Exception("This Model :[" + item.Model.ToString() + "] is not Exists");
                //    }
                //}
                HoldRuleInfo checkInfo = new HoldRuleInfo();
                if (item.Line != "")
                {
                    checkInfo.Line = item.Line;
                }
                if (item.Family != "")
                {
                    checkInfo.Family = item.Family;
                }
                if (item.Model != "")
                {
                    checkInfo.Model = item.Model;
                }
                if (item.CUSTSN != "")
                {
                    checkInfo.CUSTSN = item.CUSTSN;
                }
                if (item.CheckInStaion != "")
                {
                    checkInfo.CheckInStaion = item.CheckInStaion;
                }
                IList<HoldRuleInfo> checkInfoList = iHoldRepository.GetHoldRule(checkInfo);
                if (checkInfoList != null && checkInfoList.Count != 0)
                {
                    throw new Exception("This Info is Exists");
                }
                iHoldRepository.InsertHoldRule(item);
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
                logger.Debug("(HoldRule)InsertHoldRule end");
            }
        }

        public void UpdateHoldRule(HoldRuleInfo item)
        {
            logger.Debug("(HoldRule)UpdateHoldRule starts");
            try
            {
                iHoldRepository.UpdateHoldRule(item);
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
                logger.Debug("(HoldRule)UpdateHoldRule end");
            }
        }

        public void DeleteHoldRule(int id)
        {
            logger.Debug("(HoldRule)DeleteHoldRule starts");
            try
            {
                iHoldRepository.DeleteHoldRule(id);
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
                logger.Debug("(HoldRule)DeleteHoldRule end");
            }
        }
    }
}

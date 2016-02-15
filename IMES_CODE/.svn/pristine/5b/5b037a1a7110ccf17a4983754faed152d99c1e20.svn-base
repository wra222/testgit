using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;

namespace IMES.Maintain.Implementation
{
    public class ModuleApprovalItem : MarshalByRefObject, IModuleApprovalItem     
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();

        #region ModuleApprovalItem

        public IList<string> GetFamilyList()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select Distinct Family 
                                    from Family a
                                    where a.Family <> ''
                                    order by a.Family";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public DataTable GetApprovalItemAttrList(string approvalItemID)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select distinct Case when (AttrValue='ALL')
                                           then b.Family 
                                           else a.AttrValue
                                           end as Family, 
                                           a.Editor, a.Cdt, a.Udt
                                    from ApprovalItemAttr a, Family b
                                    where ApprovalItemID=@ApprovalItemID and 
                                          AttrName='FamilyNeedUploadFile' and
                                          b.Family <>''
                                    order by Family";
                SqlParameter paraNameID = new SqlParameter("@ApprovalItemID", SqlDbType.VarChar, 30);
                paraNameID.Direction = ParameterDirection.Input;
                paraNameID.Value = approvalItemID;
                return SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameID);
                //List<string> list = new List<string>(dt.Rows.Count);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    string item = dr[0].ToString().Trim();
                //    list.Add(item);
                //}
                //return list;
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

        public IList<string> GetModuleListTop()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"Select Distinct Module 
                                    From ApprovalItem";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public IList<string> GetModuleList()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"Select Value From ConstValueType 
                                    Where [Type]='ModuleApprovalItem'";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public IList<string> GetActionNAmeList(string module)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string type = module + "Action";

                //switch (module)
                //{
                //    case "FAIBTO":
                //        type = "FAIBTOAction";
                //        break;
                //    case "FAIBTF":
                //        type = "FAIBTFAction";
                //        break;
                //    case "MO":
                //        type = "MOAction";
                //        break;
                //    default:
                //        type = "";
                //        break;
                //}
                string strSQL = @"Select Value From ConstValueType where [Type]=@Type";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 30);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = type;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL,paraNameType);
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public IList<string> GetDepartmentList(string module)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string type = module + "Dept";
                //switch (module)
                //{
                //    case "FAIBTO":
                //        type = "FAIBTODept";
                //        break;
                //    case "FAIBTF":
                //        type = "FAIBTFDept";
                //        break;
                //    case "MO":
                //        type = "MODept";
                //        break;
                //    default:
                //        type = "FAIBTODept";
                //        break;
                //}
                string strSQL = @"Select Value From ConstValueType where [Type]=@Type";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 30);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = type;
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public DataTable GetModuleList(string module)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select (SELECT  AttrValue + ',' FROM ApprovalItemAttr WHERE ApprovalItemID = ApprovalItem.ID FOR XML Path('')),* 
                                    from ApprovalItem 
                                    where Module = @Type ";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 30);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = module;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                return dt;
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

        public IList<ApprovalItemInfo> GetModuleList(ApprovalItemInfo condition)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                return iModelRepository.GetApprovalItem(condition);
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

        //public bool CHeckCheckItemTypeExExisit(string checkItemType)
        //{
        //    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //    logger.DebugFormat("BEGIN: {0}()", methodName);
        //    try
        //    {
        //        IList<CheckItemTypeExInfo> item = iModelRepository.GetCheckItemTypeEx(new CheckItemTypeExInfo { CheckItemType = checkItemType });
        //        if (item.Count == 0)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg, e);
        //        throw new Exception(e.mErrmsg);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message, e);
        //        throw;
        //    }
        //    finally
        //    {
        //        logger.DebugFormat("END: {0}()", methodName);
        //    }
        //}

        public void UpdateApprovalItem(ApprovalItemInfo item)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iModelRepository.UpdateApprovalItem(item);
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

        public void InsertApprovalItem(ApprovalItemInfo item)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iModelRepository.InsertApprovalItem(item);
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

        public void DeleteApprovalItem(int id)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iModelRepository.DeleteApprovalItem(id);
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

        public void InsertApprovalItemAttr(IList<ApprovalItemAttrInfo> condition)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //DeleteApprovalItemAttr(condition[0].ApprovalItemID);
                iModelRepository.DeleteApprovalItemAttr(condition[0].ApprovalItemID,"FamilyNeedUploadFile");
                foreach (ApprovalItemAttrInfo item in condition)
                {
                    iModelRepository.InsertApprovalItemAttr(item);
                }
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

        public void DeleteApprovalItemAttr(long id)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                iModelRepository.DeleteApprovalItemAttr(id, "FamilyNeedUploadFile");
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

        #endregion

        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }


    }
}

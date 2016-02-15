using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.StandardWeight;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
//using IMES.Station.Implementation;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Common;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class FAIFARelease : MarshalByRefObject, IFAIFARelease
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        
        #region FAIFARelease
        public DataTable GetFAIModelList(string department, string model, string from, string to,string state)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = "";
                if (state != "Release")
                {
                    if (department == "OQC")
                    {
                        strSQL = @" select a.Model, b.Family, a.PlanInputDate, a.FAQty, a.ModelType,
                                               a.InFAQty, a.FAState, a.Editor, a.Cdt, a.Udt 
                                          from FAIModel a, Model b
                                          where a.Model =b.Model and a.Cdt > @FromDate and a.Cdt < dateadd(dd, 1, @ToDate) ";
                        if (state == "Waiting")
                        {
                            strSQL += " and a.FAState in ('Waiting', 'InApproval') ";
                        }
                        else if (state == "Approval")
                        {
                            strSQL += " and a.FAState in ('Approval', 'Pilot') ";
                        }

                        if (!string.IsNullOrEmpty(model))
                        {
                            strSQL += " and a.Model = @Model ";
                        }
                    }
                    else
                    {

                        strSQL = @" select a.Model, b.Family, a.PlanInputDate, a.FAQty, a.ModelType,
                                              a.InFAQty, a.FAState, a.Editor, a.Cdt, a.Udt
                                    from FAIModel a
                                    inner join Model b on a.Model = b.Model
                                    inner join ApprovalStatus c on a.Model = c.ModuleKeyValue ";
                        if (state == "Waiting")
                        {
                            strSQL += " and c.Status in ('Waiting', 'Option') ";
                        }
                        else if (state == "Approval")
                        {
                            strSQL += " and c.Status in ('Approved') ";
                        }
                        strSQL += @" inner join ApprovalItem d on c.ApprovalItemID = d.ID and 
                                                          d.Module = 'FAI'+ rtrim(a.ModelType) and
                                                                 d.ActionName = 'ReleaseFA' and 
                                    d.Department = @Department
                                    where a.Cdt > @FromDate and a.Cdt < dateadd(dd, 1, @ToDate) ";

                        if (!string.IsNullOrEmpty(model))
                        {
                            strSQL += " and a.Model = @Model ";
                        }
                        strSQL += " order by a.Cdt, b.Family, a.Model ";
                    }
                }
                else
                {
                    strSQL = @"select a.Model, b.Family, a.PlanInputDate, a.FAQty, a.ModelType,
                                      a.InFAQty, a.FAState, a.Editor, a.Cdt, a.Udt
                            from FAIModel a
                            inner join Model b on a.Model = b.Model
                            where a.FAState = 'Release' and
                                   a.Cdt > @FromDate and a.Cdt < dateadd(dd, 1, @ToDate)";
                }

                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                SqlParameter paraNameFrom = new SqlParameter("@FromDate", SqlDbType.VarChar, 20);
                paraNameFrom.Direction = ParameterDirection.Input;
                paraNameFrom.Value = from;
                SqlParameter paraNameTo = new SqlParameter("@ToDate", SqlDbType.VarChar, 20);
                paraNameTo.Direction = ParameterDirection.Input;
                paraNameTo.Value = to;
                SqlParameter paraNameDepartment = new SqlParameter("@Department", SqlDbType.VarChar, 20);
                paraNameDepartment.Direction = ParameterDirection.Input;
                paraNameDepartment.Value = department;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel,
                                                                                                                        paraNameFrom, 
                                                                                                                        paraNameTo, 
                                                                                                                        paraNameDepartment);
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

        public DataTable GetDepartmentApproveStatus(string model)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
			logger.DebugFormat("BEGIN: {0}()", methodName);
			try	 
            {
                
                string strSQL = @" select a.ModuleKeyValue as Model, c.Department,
                                          a.[Status],d.UploadFileGUIDName as guid,
                                                isnull(d.UploadFileName,'') as UploadFileName, 
                                                a.Comment, 
                                                a.Editor, a.Udt,
                                                c.IsNeedUploadFile, a.ID as ApprovalStatusID, a.ApprovalItemID
                                        from ApprovalStatus a
                                        inner join FAIModel b on a.ModuleKeyValue = b.Model
                                        inner join ApprovalItem c on a.ApprovalItemID = c.ID and 
                                                       c.Module = 'FAI'+ rtrim(b.ModelType) and 
                                                       c.ActionName='ReleaseFA'
                                        left join UploadFiles d on a.ID=d.ApprovalStatusID
                                        where a.ModuleKeyValue=@Model";
//                string strSQL = @" SELECT '' as Model,'' as Department,
//                                    '' as [Status], guid,filename as UploadFileName,
//                                    '' as Comment,'' as Editor, '' as Cdt
//                                    FROM [UpLoadFile_GUID]";
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
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

        public void InsertUpLoadFileInfo(string model, string Department, string uploadFileGUIDName, string uploadFileName, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select b.ID
                                    from ApprovalItem a
                                    inner join ApprovalStatus b on b.ApprovalItemID=a.ID 
                                    and b.ModuleKeyValue=@Model
                                    and a.Department=@Department
                                    and a.ActionName = 'ReleaseFA' ";
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                SqlParameter paraNameDepartment = new SqlParameter("@Department", SqlDbType.VarChar, 20);
                paraNameDepartment.Direction = ParameterDirection.Input;
                paraNameDepartment.Value = Department;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel,paraNameDepartment);
                if (dt.Rows.Count == 0)
                {
                    //throw new FisException(string.Format("Model:{0},Department:{1} 查無ApprovalItemID,請確認資料是否正確...", model, Department));
                    throw new FisException("CQCHK50111", new string[] { model, Department });
                }
                string temp = dt.Rows[0]["ID"].ToString().Trim();
                UploadFilesInfo condition = new UploadFilesInfo();
                condition.ApprovalStatusID = (long)Convert.ToInt32(temp);
                IList<UploadFilesInfo> uploadlist = new List<UploadFilesInfo>();
                uploadlist = iModelRepository.GetUploadFiles(condition);
                UploadFilesInfo item = new UploadFilesInfo();
                item.ApprovalStatusID = Convert.ToInt32(temp);
                item.UploadFileGUIDName = uploadFileGUIDName;
                item.UploadFileName = uploadFileName;
                item.UploadServerName = "";
                item.Editor = editor;
                item.Cdt = DateTime.Now;
                if (uploadlist.Count == 0)
                {
                    iModelRepository.InsertUploadFiles(item);
                }
                else
                {
                    item.ID = uploadlist[0].ID;
                    iModelRepository.UpdateUploadFiles(item);   
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

        public IList<string> GetDepartmentList(string model)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            IList<string> ret = new List<string>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select a.Department
                                from ApprovalItem a
                                inner join ApprovalStatus b on b.ApprovalItemID=a.ID 
                                and a.ActionName='ReleaseFA'
                                and b.ModuleKeyValue=@Model";
                //                string strSQL = @" SELECT '' as Model,'' as Department,
                //                                    '' as [Status], guid,filename as UploadFileName,
                //                                    '' as Comment,'' as Editor, '' as Cdt
                //                                    FROM [UpLoadFile_GUID]";
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
                ret.Add(string.Empty);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ret.Add(item["Department"].ToString().Trim());
                    }
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

        public void CheckApprovalStatusAndChengeStatus(string model, string approvalStatusID, string approvalItemID, string comment, string editor, string family, 
                                                        string department, string isNeedUploadFile, string filename)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = "";
                if (isNeedUploadFile == "Y" && string.IsNullOrEmpty(filename))
                {
                    strSQL = @"select * from ApprovalItemAttr 
                                        where ApprovalItemID =@approvalID
                                        and AttrName = 'FamilyNeedUploadFile'
                                        and AttrValue in ('ALL', @family) ";
                    SqlParameter paraNameApprovalID = new SqlParameter("@approvalID", SqlDbType.VarChar, 20);
                    paraNameApprovalID.Direction = ParameterDirection.Input;
                    paraNameApprovalID.Value = approvalItemID;
                    SqlParameter paraNameFamily = new SqlParameter("@family", SqlDbType.VarChar, 20);
                    paraNameFamily.Direction = ParameterDirection.Input;
                    paraNameFamily.Value = family;
                    DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameApprovalID,
                                                                                                                            paraNameFamily);
                    if (dt.Rows.Count > 0)
                    {
                        throw new FisException("CQCHK50112", new string[] { });
                    }
                }

                ApprovalStatusInfo approvalstatusItem = new ApprovalStatusInfo();
                approvalstatusItem.ID = Convert.ToInt32(approvalStatusID);
                approvalstatusItem.Status = "Approved";
                approvalstatusItem.Comment = comment;
                approvalstatusItem.Editor = editor;
                approvalstatusItem.Udt = DateTime.Now;
                iModelRepository.UpdateApprovalStatus(approvalstatusItem);

                if (department == "OQC")
                {
                    strSQL = @" select a.Status,COUNT(1)as Qty
                                from ApprovalStatus a
                                inner join ApprovalItem b on a.ApprovalItemID = b.ID
                                inner join FAIModel c on a.ModuleKeyValue = c.Model
                                where b.Module = 'FAI'+ rtrim(c.ModelType) and 
                                            b.ActionName = 'ReleaseFA' and 
                                            b.IsNeedApprove = 'Y' and
                                            a.ModuleKeyValue=@Model
                                            group by a.Status";
                    SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                    paraNameModel.Direction = ParameterDirection.Input;
                    paraNameModel.Value = model;
                    DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
                    int waitingQty = 0;
                    int approvedQty = 0;
                    string faStatus = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Status"].ToString() == "Waiting")
                        {
                            waitingQty = Convert.ToInt32(dr["Qty"].ToString());
                        }
                        if (dr["Status"].ToString() == "Approved")
                        {
                            approvedQty = Convert.ToInt32(dr["Qty"].ToString());
                        }
                    }
                    if (waitingQty > 0)
                    {
                        throw new FisException("CQCHK50113", new string[] { });
                    }
                    if (waitingQty == 0)
                    {
                        faStatus = "Release";
                        SendMails(model);
                    }
                    FAIModelInfo faiModelItem = new FAIModelInfo();
                    faiModelItem.Model = model;
                    faiModelItem.FAState = faStatus;
                    faiModelItem.Editor = editor;
                    faiModelItem.Udt = DateTime.Now;
                    iModelRepository.UpdateFAIModel(faiModelItem);
                }
                else
                { 
                    strSQL = @"select a.Status,COUNT(1)as Qty
                                        from ApprovalStatus a
                                        inner join ApprovalItem b on a.ApprovalItemID = b.ID and b.Department <> 'OQC' 
                                        inner join FAIModel c on a.ModuleKeyValue = c.Model
                                        and b.Module = 'FAI'+ rtrim(c.ModelType)
                                        and b.IsNeedApprove = 'Y' and b.ActionName='ReleaseFA'
                                        where a.ModuleKeyValue =@Model
                                        group by a.Status";
                    SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                    paraNameModel.Direction = ParameterDirection.Input;
                    paraNameModel.Value = model;
                    DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
                    int waitingQty = 0;
                    int approvedQty = 0;
                    string faStatus = "Waiting";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Status"].ToString() == "Waiting")
                        {
                            waitingQty = Convert.ToInt32(dr["Qty"].ToString());
                        }
                        if (dr["Status"].ToString() == "Approved")
                        {
                            approvedQty = Convert.ToInt32(dr["Qty"].ToString());
                        }
                    }
                    if (waitingQty == 0)
                    {
                        faStatus = "Approval";
                        SendMails(model);
                    }
                    else if (approvedQty == 1)
                    {
                        faStatus = "InApproval";
                    }
                    FAIModelInfo faiModelItem = new FAIModelInfo();
                    faiModelItem.Model = model;
                    faiModelItem.FAState = faStatus;
                    faiModelItem.Editor = editor;
                    faiModelItem.Udt = DateTime.Now;
                    iModelRepository.UpdateFAIModel(faiModelItem);
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

        private void SendMails(string model)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select distinct c.OwnerEmail, c.CCEmail
                                    from ApprovalStatus a
                                    inner join FAIModel b on a.ModuleKeyValue = b.Model
                                    inner join ApprovalItem c on a.ApprovalItemID=c.ID and 
                                               c.Module = 'FAI'+ rtrim(b.ModelType) and 
                                               c.ActionName='ReleaseFA' and
                                               c.Department='OQC'
                                    where a.ModuleKeyValue=@Model";
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
                if (dt.Rows.Count == 0)
                {
                    return;
                }

                string FromAddress = ConfigurationManager.AppSettings["FromAddress"];
                string[] ToAddress = dt.Rows[0]["OwnerEmail"].ToString().Trim().Split(',');
                string[] CcAddress = dt.Rows[0]["CCEmail"].ToString().Trim().Split(',');
                string MailSubject = ConfigurationManager.AppSettings["MailSubject"];
                string EmailServer = ConfigurationManager.AppSettings["MailServer"];
                string content = string.Format(@"Dear QC:
                                                    IMG,DMI,SIE,PA 四個部門已對已下機型確認OK，請貴部儘快確認！
                                                    MODEL:{0}",model);

                ActivityCommonImpl.Instance.Mail.Send(FromAddress,
                            ToAddress,
                            CcAddress,
                            MailSubject,
                            content,
                            EmailServer);
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

        public void RemoveStatus(string model, string approvalID, string comment, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                ApprovalStatusInfo approvalstatusItem = new ApprovalStatusInfo();
                approvalstatusItem.ID = Convert.ToInt32(approvalID);
                approvalstatusItem.Status = "Waiting";
                approvalstatusItem.Comment = comment;
                approvalstatusItem.Editor = editor;
                approvalstatusItem.Udt = DateTime.Now;
                iModelRepository.UpdateApprovalStatus(approvalstatusItem);


                string strSQL = @"select a.Status,COUNT(1)as Qty
                                    from ApprovalStatus a
                                    inner join ApprovalItem b on a.ApprovalItemID = b.ID and b.IsNeedApprove = 'Y'
                                    inner join FAIModel c on a.ModuleKeyValue = c.Model
                                    where a.ModuleKeyValue =@Model
                                    and b.Module = 'FAI'+ rtrim(c.ModelType)
                                    and b.ActionName='ReleaseFA'
                                    group by a.Status";
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
                int approvedQty = 0;
                string faStatus = "";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Status"].ToString() == "Approved")
                    {
                        approvedQty = Convert.ToInt32(dr["Qty"].ToString());
                    }
                }
                if (approvedQty == 0)
                {
                    faStatus = "Waiting";
                }
                else
                {
                    faStatus = "InApproval";
                }
                FAIModelInfo faiModelItem = new FAIModelInfo();
                faiModelItem.Model = model;
                faiModelItem.FAState = faStatus;
                faiModelItem.Editor = editor;
                faiModelItem.Udt = DateTime.Now;
                iModelRepository.UpdateFAIModel(faiModelItem);
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

        public DataTable GetExeclData(string model,string department, string from, string to, string state)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                SqlParameter paraNameDepartment = new SqlParameter("@Department", SqlDbType.VarChar, 20);
                paraNameDepartment.Direction = ParameterDirection.Input;
                paraNameDepartment.Value = department;
                SqlParameter paraNameFrom = new SqlParameter("@FromDate", SqlDbType.VarChar, 20);
                paraNameFrom.Direction = ParameterDirection.Input;
                paraNameFrom.Value = from;
                SqlParameter paraNameTo = new SqlParameter("@ToDate", SqlDbType.VarChar, 20);
                paraNameTo.Direction = ParameterDirection.Input;
                paraNameTo.Value = to;
                SqlParameter paraNamestate = new SqlParameter("@State", SqlDbType.VarChar, 20);
                paraNamestate.Direction = ParameterDirection.Input;
                paraNamestate.Value = state;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, 
                                                                                                        "sp_Query_FAIFARelease_Excel", 
                                                                                                        paraNameModel,
                                                                                                        paraNameDepartment,
                                                                                                        paraNameFrom,
                                                                                                        paraNameTo,
                                                                                                        paraNamestate);
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

        public string UploadFile(string guid)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @" select * from [UploadFiles] where UploadFileGUIDName=@guid ";

                SqlParameter paraNameGUID = new SqlParameter("@guid", SqlDbType.VarChar, 48);
                paraNameGUID.Direction = ParameterDirection.Input;
                paraNameGUID.Value = guid;

                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameGUID);
                if (dt.Rows.Count == 0)
                {
                    throw new FisException("CQCHK50114", new string[] { });
                }
                string filename = dt.Rows[0]["UploadFileName"].ToString().Trim();
                return filename;
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
    }
}

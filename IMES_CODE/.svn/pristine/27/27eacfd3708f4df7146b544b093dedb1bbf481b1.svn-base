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

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class FAIPAKRelease : MarshalByRefObject, IFAIPAKRelease
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        
        #region FAIPAKRelease
        //public IList<FAIModelInfo> GetFAIModelList(FAIModelInfo condition)
        //{
        //    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //    logger.DebugFormat("BEGIN: {0}()", methodName);
        //    try
        //    {
        //        return iModelRepository.GetFAIModel(condition);
                
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

        public DataTable GetDepartmentApproveStatus(string model, string department, string state)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
			logger.DebugFormat("BEGIN: {0}()", methodName);
			try	 
            {

                string strSQL = @" select a.Model, a.ModelType, a.PAKQty, 
                                           a.InPAKQty, a.PAKStartDate, a.PAKState,d.UploadFileGUIDName as guid,
                                           isnull(d.UploadFileName,'') as UploadFileName,
                                           a.Editor, a.Cdt, a.Udt,
                                           b.ID as ApprovalStatusID
                                    from FAIModel a
                                    inner join ApprovalStatus b on a.Model = b.ModuleKeyValue
                                    inner join ApprovalItem c on b.ApprovalItemID = c.ID and
                                                              c.Module = 'FAI'+ rtrim(a.ModelType) and
                                                                 c.ActionName = 'ReleasePAK' and
                                                                 Department = @Department 
                                    left join UploadFiles d on b.ID = d.ApprovalStatusID
                                    where a.PAKState = @State";

                if(!string.IsNullOrEmpty(model))
                {
                    strSQL += " and a.Model=@Model";
                }
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                SqlParameter paraNameDepartment = new SqlParameter("@Department", SqlDbType.VarChar, 20);
                paraNameDepartment.Direction = ParameterDirection.Input;
                paraNameDepartment.Value = department;
                SqlParameter paraNameState = new SqlParameter("@State", SqlDbType.VarChar, 20);
                paraNameState.Direction = ParameterDirection.Input;
                paraNameState.Value = state;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel,paraNameDepartment,paraNameState);
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
                                    and a.ActionName = 'ReleasePAK' ";
                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameModel.Direction = ParameterDirection.Input;
                paraNameModel.Value = model;
                SqlParameter paraNameDepartment = new SqlParameter("@Department", SqlDbType.VarChar, 20);
                paraNameDepartment.Direction = ParameterDirection.Input;
                paraNameDepartment.Value = Department;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel,paraNameDepartment);
                if (dt.Rows.Count == 0)
                {
                    throw new FisException("CQCHK50111", new string[] {model, Department});
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

//        public IList<string> GetDepartmentList(string model)
//        {
//            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
//            IList<string> ret = new List<string>();
//            logger.DebugFormat("BEGIN: {0}()", methodName);
//            try
//            {
//                string strSQL = @"select a.Department
//                                from ApprovalItem a
//                                inner join ApprovalStatus b on b.ApprovalItemID=a.ID 
//                                and b.ModuleKeyValue=@Model";
//                //                string strSQL = @" SELECT '' as Model,'' as Department,
//                //                                    '' as [Status], guid,filename as UploadFileName,
//                //                                    '' as Comment,'' as Editor, '' as Cdt
//                //                                    FROM [UpLoadFile_GUID]";
//                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
//                paraNameModel.Direction = ParameterDirection.Input;
//                paraNameModel.Value = model;
//                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
//                ret.Add(string.Empty);
//                if (dt.Rows.Count != 0)
//                {
//                    foreach (DataRow item in dt.Rows)
//                    {
//                        ret.Add(item["Department"].ToString().Trim());
//                    }
//                }
//                return ret;
//            }
//            catch (FisException e)
//            {
//                logger.Error(e.mErrmsg, e);
//                throw new Exception(e.mErrmsg);
//            }
//            catch (Exception e)
//            {
//                logger.Error(e.Message, e);
//                throw;
//            }
//            finally
//            {
//                logger.DebugFormat("END: {0}()", methodName);
//            }
//        }

        public void SaveStatus(string model, string approvalID, int pakQty, string shipdate, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                FAIModelInfo faiModelItem = new FAIModelInfo();
                faiModelItem.Model = model;
                faiModelItem.PAKQty = pakQty;
                faiModelItem.PAKStartDate =Convert.ToDateTime(shipdate);
                faiModelItem.Editor = editor;
                faiModelItem.Udt = DateTime.Now;
                iModelRepository.UpdateFAIModel(faiModelItem);


//                ApprovalStatusInfo approvalstatusItem = new ApprovalStatusInfo();
//                approvalstatusItem.ID = Convert.ToInt32(approvalID);
//                approvalstatusItem.Status = "Approval";
//                approvalstatusItem.Comment = comment;
//                approvalstatusItem.Editor = editor;
//                approvalstatusItem.Udt = DateTime.Now;
//                iModelRepository.UpdateApprovalStatus(approvalstatusItem);


//                string strSQL = @"select Status,COUNT(1)as Qty
//                                from ApprovalStatus group by Status";
//                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
//                int waitingQty = 0;
//                int approvedQty = 0;
//                //int totoQty = 0;
//                string faStatus = "";
//                foreach (DataRow dr in dt.Rows)
//                {
//                    if (dr["Status"].ToString() == "Waiting")
//                    {
//                        waitingQty = Convert.ToInt32(dr["Qty"].ToString());
//                    }
//                    if (dr["Status"].ToString() == "Approval")
//                    {
//                        approvedQty = Convert.ToInt32(dr["Qty"].ToString());
//                    }
//                    //totoQty += Convert.ToInt32(dr["Qty"].ToString());
//                }
//                if (waitingQty == 0 || approvedQty == 1)
//                {
//                    if (waitingQty == 0)
//                    {
//                        faStatus = "Approval";
//                        SendMails(model);
//                    }
//                    else if (approvedQty == 1)
//                    {
//                        faStatus = "InApproval";
//                    }
//                    FAIModelInfo faiModelItem = new FAIModelInfo();
//                    faiModelItem.Model = model;
//                    faiModelItem.FAState = faStatus;
//                    faiModelItem.Editor = editor;
//                    faiModelItem.Udt = DateTime.Now;
//                    iModelRepository.UpdateFAIModel(faiModelItem);

//                }
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

//        private void SendMails(string model)
//        {
//            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
//            logger.DebugFormat("BEGIN: {0}()", methodName);
//            try
//            {
//                string strSQL = @"select distinct c.OwnerEmail, c.CCEmail
//                                    from ApprovalStatus a
//                                    inner join FAIModel b on a.ModuleKeyValue = b.Model
//                                    inner join ApprovalItem c on a.ApprovalItemID=c.ID and 
//                                               c.Module = (Case when b.ModelType='BTO' 
//                                                           then 'FAIBTO' 
//                                                           ELSE 'FAIBTF' END) and 
//                                               c.ActionName='ReleaseFA' and
//                                               c.Department='OQC'
//                                    where a.ModuleKeyValue=@Model";
//                SqlParameter paraNameModel = new SqlParameter("@Model", SqlDbType.VarChar, 20);
//                paraNameModel.Direction = ParameterDirection.Input;
//                paraNameModel.Value = model;
//                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameModel);
//                if (dt.Rows.Count == 0)
//                {
//                    return;
//                }

//                string FromAddress = ConfigurationManager.AppSettings["FromAddress"];
//                string[] ToAddress = dt.Rows[0]["OwnerEmail"].ToString().Trim().Split(',');
//                string[] CcAddress = dt.Rows[0]["CCEmail"].ToString().Trim().Split(',');
//                string MailSubject = ConfigurationManager.AppSettings["MailSubject"];
//                string EmailServer = ConfigurationManager.AppSettings["MailServer"];
//                string content = string.Format(@"Dear QC:
//                                                    IMG,DMI,SIE,PA 四個部門已對已下機型確認OK，請貴部儘快確認！
//                                                    MODEL:{0}",model);

//                SendMail.Send(FromAddress,
//                            ToAddress,
//                            CcAddress,
//                            MailSubject,
//                            content,
//                            EmailServer);
//            }
//            catch (FisException e)
//            {
//                logger.Error(e.mErrmsg, e);
//                throw new Exception(e.mErrmsg);
//            }
//            catch (Exception e)
//            {
//                logger.Error(e.Message, e);
//                throw;
//            }
//            finally
//            {
//                logger.DebugFormat("END: {0}()", methodName);
//            }
//        }

        public void ReleaseStatus(string model, string approvalID, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                ApprovalStatusInfo approvalstatusItem = new ApprovalStatusInfo();
                approvalstatusItem.ID = Convert.ToInt32(approvalID);
                approvalstatusItem.Status = "Approved";
                approvalstatusItem.Editor = editor;
                approvalstatusItem.Udt = DateTime.Now;
                iModelRepository.UpdateApprovalStatus(approvalstatusItem);


                FAIModelInfo faiModelItem = new FAIModelInfo();
                faiModelItem.Model = model;
                faiModelItem.PAKState = "Release";
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
        #endregion
    }
}

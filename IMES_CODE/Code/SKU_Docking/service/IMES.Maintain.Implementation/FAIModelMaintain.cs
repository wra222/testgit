using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IMES.DataModel;
using IMES.FisObject.Common.Hold;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
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
    public class FAIModelMaintain : MarshalByRefObject, IFAIModelMaintain
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IHoldRepository iHoldRepository = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();

        private IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();

        public DataTable Query(string model)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(model={1})", methodName, model);
            try
            {
                string strSQL = @"select a.Model, b.Family, a.ModelType,
        a.FAQty, a.InFAQty, a.FAState,
        a.PAKQty, a.InPAKQty, a.PAKState, 
        a.Editor, a.Cdt, a.Udt 
from FAIModel a
inner join Model b on a.Model = b.Model
where a.Model = @Model";

                SqlParameter paraName = new SqlParameter("@Model", SqlDbType.VarChar, 16);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = model;

                DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_FA,
                                                                                    System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    paraName);
                return tb;
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
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private void UpdateFAIModel(string model, string editor)
        {
            IUnitOfWork uow = new UnitOfWork();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IList<IProduct> lstPrds = prodRep.GetProductListByModel(model);
            if (null != lstPrds && lstPrds.Count > 0)
            {
                lstPrds = lstPrds.Where(x => x.ProductInfoes.Any(y => (y.InfoType == "FAIinFA" || y.InfoType == "FAIinPAK") && y.InfoValue == "Y")).ToList();
                foreach (IProduct p in lstPrds)
                {
                    prodRep.BackUpProductInfoDefered(uow, p.ProId, editor, "FAIinFA");
                    prodRep.BackUpProductInfoDefered(uow, p.ProId, editor, "FAIinPAK");

                    IMES.FisObject.FA.Product.ProductInfo item = new IMES.FisObject.FA.Product.ProductInfo();
                    item.ProductID = p.ProId;
                    item.InfoType = "FAIinFA";
                    item.InfoValue = "";
                    item.Editor = editor;

                    IMES.FisObject.FA.Product.ProductInfo cond = new IMES.FisObject.FA.Product.ProductInfo();
                    cond.ProductID = p.ProId;
                    cond.InfoType = "FAIinFA";
                    prodRep.UpdateProductInfoDefered(uow, item, cond);

                    //

                    item = new IMES.FisObject.FA.Product.ProductInfo();
                    item.ProductID = p.ProId;
                    item.InfoType = "FAIinPAK";
                    item.InfoValue = "";
                    item.Editor = editor;

                    cond = new IMES.FisObject.FA.Product.ProductInfo();
                    cond.ProductID = p.ProId;
                    cond.InfoType = "FAIinPAK";
                    prodRep.UpdateProductInfoDefered(uow, item, cond);

                }
            }

            string OnlyNeedOQCApprove = CommonImpl.GetInstance().GetValueFromSysSetting("OnlyNeedOQCApprove");
            string FAIFAQty = CommonImpl.GetInstance().GetValueFromSysSetting("FAIFAQty");
            string FAIPAKQty = CommonImpl.GetInstance().GetValueFromSysSetting("FAIPAKQty");

            // 刪除
            ApprovalStatusInfo condApprovalStatus = new ApprovalStatusInfo();
            condApprovalStatus.ModuleKeyValue = model;
            IList<ApprovalStatusInfo> lstApprovalStatus = iModelRepository.GetApprovalStatus(condApprovalStatus);
            if (null != lstApprovalStatus && lstApprovalStatus.Count > 0)
            {
                foreach (ApprovalStatusInfo approvalStatusInfo in lstApprovalStatus)
                {
                    UploadFilesInfo condUploadFiles = new UploadFilesInfo();
                    condUploadFiles.ApprovalStatusID = approvalStatusInfo.ID;
                    IList<UploadFilesInfo> lstUploadFilesInfo = iModelRepository.GetUploadFiles(condUploadFiles);
                    if (null != lstUploadFilesInfo && lstUploadFilesInfo.Count > 0)
                    {
                        foreach (UploadFilesInfo uploadFilesInfo in lstUploadFilesInfo)
                            iModelRepository.DeleteUploadFiles(uploadFilesInfo.ID);
                    }

                    iModelRepository.DeleteApprovalStatus(approvalStatusInfo.ID);
                }
            }

            string FAState = "";
            // 新增ApprovalStatus
            if ("Y" == OnlyNeedOQCApprove)
            {
                string strSQL = @"insert into ApprovalStatus(
ApprovalItemID, ModuleKeyValue, 
Status, Editor, Cdt, Udt)
select 
a.ID, b.Model as ModuleKeyValue, 
case when IsNeedApprove='Y' 
	  then 'Waiting'
	  else 'Option'
      end as [Status], @CurrentUser, 
GETDATE() as Cdt, GETDATE() as Udt
from ApprovalItem a , FAIModel b
where b.Model=@OpenModel and 
a.Module= 'FAI'+b.ModelType and 
a.Department = 'OQC'";

                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@CurrentUser", SqlDbType.VarChar);
                paramsArray[0].Value = editor;
                paramsArray[1] = new SqlParameter("@OpenModel", SqlDbType.VarChar);
                paramsArray[1].Value = model;

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_FA,
                                                                                    System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    paramsArray);

                FAState = "Approval";
            }
            else if (string.IsNullOrEmpty(OnlyNeedOQCApprove) || "N" == OnlyNeedOQCApprove)
            {
                string strSQL = @"insert into ApprovalStatus(
ApprovalItemID, ModuleKeyValue, 
Status, Editor, Cdt, Udt)
select a.ID, b.Model as ModuleKeyValue, 
		case when IsNeedApprove='Y' 
		then 'Waiting'
		else 'Option'
		end as [Status], @CurrentUser, GETDATE() as Cdt, GETDATE() as Udt
from ApprovalItem a , FAIModel b
where b.Model=@OpenModel and 
       a.Module= 'FAI'+b.ModelType
";

                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@CurrentUser", SqlDbType.VarChar);
                paramsArray[0].Value = editor;
                paramsArray[1] = new SqlParameter("@OpenModel", SqlDbType.VarChar);
                paramsArray[1].Value = model;

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_FA,
                                                                                    System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    paramsArray);

                FAState = "Waiting";
            }

            DateTime now = DateTime.Now;

            FAIModelInfo itemFai = iModelRepository.GetFAIModelByModel(model);
            itemFai.Model = model;
            itemFai.PlanInputDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            itemFai.FAQty = int.Parse(FAIFAQty);
            itemFai.inFAQty = 0;
            itemFai.PAKQty = int.Parse(FAIPAKQty);
            itemFai.inPAKQty = 0;
            itemFai.FAState = FAState;
            itemFai.PAKState = "Hold";
            itemFai.Remark = "Reopen";
            itemFai.Editor = editor;
            itemFai.Cdt = now;
            itemFai.Udt = now;
            
            iModelRepository.UpdateFAIModelDefered(uow, itemFai);

            uow.Commit();
        }

        private void InsertFAIModel(string model, string editor)
        {
            string OnlyNeedOQCApprove = CommonImpl.GetInstance().GetValueFromSysSetting("OnlyNeedOQCApprove");
            string FAIFAQty = CommonImpl.GetInstance().GetValueFromSysSetting("FAIFAQty");
            string FAIPAKQty = CommonImpl.GetInstance().GetValueFromSysSetting("FAIPAKQty");

            string modelType = GetModelType(model);
            string FAState = "";
            // 新增ApprovalStatus
            if ("Y" == OnlyNeedOQCApprove)
            {
                string strSQL = @"insert into ApprovalStatus(
ApprovalItemID, ModuleKeyValue, 
Status, Editor, Cdt, Udt)
select 
a.ID, @AddModel as ModuleKeyValue, 
case when IsNeedApprove='Y' 
	  then 'Waiting'
	  else 'Option'
      end as [Status], @CurrentUser, 
GETDATE() as Cdt, GETDATE() as Udt
from ApprovalItem a 
where 
a.Module= @ModelType and 
a.Department = 'OQC'";

                SqlParameter[] paramsArray = new SqlParameter[3];
                paramsArray[0] = new SqlParameter("@CurrentUser", SqlDbType.VarChar);
                paramsArray[0].Value = editor;
                paramsArray[1] = new SqlParameter("@AddModel", SqlDbType.VarChar);
                paramsArray[1].Value = model;
                paramsArray[2] = new SqlParameter("@ModelType", SqlDbType.VarChar);
                paramsArray[2].Value = "FAI" + modelType;

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_FA,
                                                                                    System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    paramsArray);

                FAState = "Approval";
            }
            else if (string.IsNullOrEmpty(OnlyNeedOQCApprove) || "N" == OnlyNeedOQCApprove)
            {
                string strSQL = @"insert into ApprovalStatus(
ApprovalItemID, ModuleKeyValue, 
Status, Editor, Cdt, Udt)
select a.ID, @AddModel as ModuleKeyValue, 
		case when IsNeedApprove='Y' 
		then 'Waiting'
		else 'Option'
		end as [Status], @CurrentUser, GETDATE() as Cdt, GETDATE() as Udt
from ApprovalItem a 
where 
a.Module= @ModelType
";

                SqlParameter[] paramsArray = new SqlParameter[3];
                paramsArray[0] = new SqlParameter("@CurrentUser", SqlDbType.VarChar);
                paramsArray[0].Value = editor;
                paramsArray[1] = new SqlParameter("@AddModel", SqlDbType.VarChar);
                paramsArray[1].Value = model;
                paramsArray[2] = new SqlParameter("@ModelType", SqlDbType.VarChar);
                paramsArray[2].Value = "FAI" + modelType;

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_FA,
                                                                                    System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    paramsArray);

                FAState = "Waiting";
            }

            DateTime now = DateTime.Now;

            FAIModelInfo itemFai = new FAIModelInfo()
            {
                Model = model,
                ModelType = modelType,
                PlanInputDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0),
                FAQty = int.Parse(FAIFAQty),
                inFAQty = 0,
                PAKQty = int.Parse(FAIPAKQty),
                inPAKQty = 0,
                PAKStartDate = now,
                FAState = FAState,
                PAKState = "Hold",
                Remark = "KeyIn",
                Editor = editor,
                Cdt = now,
                Udt = now
            };

            IUnitOfWork uow = new UnitOfWork();

            iModelRepository.InsertFAIModelDefered(uow, itemFai);

            uow.Commit();
        }

        public void ReOpen(string model, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(model={1})", methodName, model);
            try
            {
                UpdateFAIModel(model, editor);
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
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private string GetModelType(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                
            }
            else if ("Y" == model.Substring(model.Length - 1))
            {
                if (model.Length >= 7)
                {
                    string m7 = model.Substring(6, 1);
                    if ("Y" == m7)
                        return "Special";
                    else if (Regex.IsMatch(m7, "^[0-9]*$"))
                        return "CTO";
                    else if (Regex.IsMatch(m7, "^[A-Z]*$"))
                        return "BTO";
                }
            }
            else
            {
                if (model.Length >= 3 && "173" == model.Substring(0, 3))
                {
                    return "RCTO";
                }
            }

            // 此機型：@addModel非適用FAI管控之機型 !
            throw new FisException("CQCHK1099", new string[] { model });
        }

        public void Add(string model, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(model={1})", methodName, model);
            try
            {
                string modelType = GetModelType(model);
                CheckModelType(modelType);
                
                FAIModelInfo modelFai = new FAIModelInfo();
                Model m = new Model();
                bool existsFAI = CheckFAIModel(model, out modelFai, out m);
                
                if (existsFAI)
                    UpdateFAIModel(model, editor);
                else
                {
                    InsertFAIModel(model, editor);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private bool CheckFAIModel(string model, out FAIModelInfo modelFai, out Model m)
        {
            modelFai = iModelRepository.GetFAIModelByModel(model);
            bool existsFAI = false;
            if (null != modelFai)
            {
                if (!("Release" == modelFai.FAState && "Release" == modelFai.PAKState))
                {
                    // 此Model：@addModel 執行FAI 中，不可Reopen !
                    throw new FisException("CQCHK00054", new string[] { model });
                }
                existsFAI = true;
            }

            m = iModelRepository.Find(model);
            if (null == m)
            {
                // 不能找到Model:%1
                throw new FisException("CHK804", new string[] { model });
            }

            return existsFAI;
        }

        private void CheckModelType(string modelType)
        {
            ApprovalItemInfo condApprovalItem = new ApprovalItemInfo();
            condApprovalItem.Module = "FAI" + modelType;
            IList<ApprovalItemInfo> lstApprovalItem = iModelRepository.GetApprovalItem(condApprovalItem);
            if (null == lstApprovalItem || lstApprovalItem.Count == 0)
            {
                // 此Model類型：@ModelType未維護審核部門[ApprovalItem] ! 請先進行維護!
                throw new FisException("CQCHK1100", new string[] { modelType });
            }
        }

        public ArrayList GetFAIModelInfo(string model)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(model={1})", methodName, model);
            try
            {
                ArrayList ret = new ArrayList();

                string modelType = GetModelType(model);
                CheckModelType(modelType);

                FAIModelInfo modelFai = new FAIModelInfo();
                Model m = new Model();
                bool existsFAI = CheckFAIModel(model, out modelFai, out m);

                ret.Add(m.Family.FamilyName);
                ret.Add(modelType);

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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}

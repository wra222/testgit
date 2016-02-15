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
using IMES.FisObject.Common.Part;

namespace IMES.Maintain.Implementation
{
    public class PartForbidRule : MarshalByRefObject, IPartForbidRule
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

        public IList<string> GetCustomer()
        {
            logger.Debug("(PartForbidRule)GetCustomer starts");
            try
            {
                string strSQL = @"select Code from Customer ";
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PartForbidRule)GetCustomer end");
            }
        }

        public IList<string> GetCategory()
        {
            logger.Debug("(PartForbidRule)GetCategory starts");
            try
            {
                string strSQL = @"select Value from ConstValueType where Type='PartForbidCategory' ";
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PartForbidRule)GetCategory end");
            }
        }

        public IList<string> GetLine(string customer)
        {
            logger.Debug("(PartForbidRule)GetLine starts");
            try
            {
                string strSQL = @"select distinct AliasLine as Line 
                                    from Line a, LineEx b
                                    where a.Line=b.Line and a.CustomerID=@Customer ";
                SqlParameter paraNameCustomer = new SqlParameter("@Customer", SqlDbType.VarChar, 20);
                paraNameCustomer.Direction = ParameterDirection.Input;
                paraNameCustomer.Value = customer;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameCustomer);
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
                logger.Debug("(PartForbidRule)GetLine end");
            }
        }

        public IList<PartForbidRuleInfo> GetPartForbidRule(PartForbidRuleInfo condition)
        {
            logger.Debug("(PartForbidRule)GetPartForbidRule starts");
            try
            {
                return iPartRepository.GetPartForbid(condition);
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
                logger.Debug("(PartForbidRule)GetPartForbidRule end");
            }
        }

        public void InsertPartForbidRule(PartForbidRuleInfo item)
        {
            logger.Debug("(PartForbidRule)InsertDefectHoldRule starts");
            try
            {
                PartForbidRuleInfo checkInfo = new PartForbidRuleInfo();
                checkInfo.Customer = item.Customer;
                checkInfo.Category = item.Category;
                checkInfo.Line = item.Line;
                checkInfo.Family = item.Family;
                checkInfo.ExceptModel = item.ExceptModel;
                checkInfo.BomNodeType = item.BomNodeType;
                checkInfo.VendorCode = item.VendorCode;
                checkInfo.PartNo = item.PartNo;
                IList<PartForbidRuleInfo> checkInfoList = iPartRepository.GetPartForbid(checkInfo);

                if (checkInfoList != null && checkInfoList.Count != 0)
                {
                    item.ID = checkInfoList[0].ID;
                    iPartRepository.UpdatePartForbid(item);
                }
                else
                {
                    iPartRepository.AddPartForbid(item);
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
                logger.Debug("(PartForbidRule)InsertDefectHoldRule end");
            }
        }

        public void UpdatePartForbidRule(PartForbidRuleInfo item)
        {
            logger.Debug("(PartForbidRule)UpdateDefectHoldRule starts");
            try
            {
				PartForbidRuleInfo checkInfo = new PartForbidRuleInfo();
                checkInfo.Customer = item.Customer;
                checkInfo.Category = item.Category;
                checkInfo.Line = item.Line;
                checkInfo.Family = item.Family;
                checkInfo.ExceptModel = item.ExceptModel;
                checkInfo.BomNodeType = item.BomNodeType;
                checkInfo.VendorCode = item.VendorCode;
                checkInfo.PartNo = item.PartNo;
                IList<PartForbidRuleInfo> checkInfoList = iPartRepository.GetPartForbid(checkInfo);

                if (checkInfoList != null && checkInfoList.Count != 0)
                {
                    throw new FisException("Input Data is exist");
                }
                else
                {
                    //iPartRepository.AddPartForbid(item);
                    iPartRepository.UpdatePartForbid(item);
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
                logger.Debug("(PartForbidRule)UpdateDefectHoldRule end");
            }
        }

        public void DeletePartForbidRule(int id)
        {
            logger.Debug("(PartForbidRule)DeleteDefectHoldRule starts");
            try
            {
                iPartRepository.DeletePartForbid(id);
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
                logger.Debug("(PartForbidRule)DeleteDefectHoldRule end");
            }
        }
    }
}

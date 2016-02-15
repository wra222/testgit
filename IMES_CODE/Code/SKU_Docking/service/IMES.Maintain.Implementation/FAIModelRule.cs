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
    public class FAIModelRule : MarshalByRefObject, IFAIModelRule
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IHoldRepository iHoldRepository = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();

        public DataTable Query(string family)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(family={1})", methodName, family);
            try
            {
                string strSQL = @"select ID, Family, Value as ModelType, 
Descr as MoLimitQty, Editor, Cdt, Udt
from FamilyInfo
where Family = @Family and Name='FAI'
order by Family
";

                SqlParameter paraName = new SqlParameter("@Family", SqlDbType.VarChar, 50);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = family;

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

        public void Delete(string idFamilyInfo, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(idFamilyInfo={1})", methodName, idFamilyInfo);
            try
            {
                IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

                FamilyInfoDef cond = new FamilyInfoDef();
                cond.id = int.Parse(idFamilyInfo);

                IUnitOfWork uow = new UnitOfWork();

                familyRep.RemoveFamilyInfoDefered(uow, cond);

                uow.Commit();
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

        public void Save(string idFamilyInfo, string family, string modelType, string moLimitQty, string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(family={1}, modelType={2}, moLimitQty={3})", methodName, family, modelType, moLimitQty);
            try
            {
                IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

                FamilyInfoDef cond = new FamilyInfoDef();
                cond.family = family;
                cond.name = "FAI";
                cond.value = modelType;

                IList<FamilyInfoDef> lstFamilyInfo = familyRep.GetExistFamilyInfo(cond);
                if (null != lstFamilyInfo && lstFamilyInfo.Count > 0)
                {
                    if (lstFamilyInfo[0].id.ToString() != idFamilyInfo)
                    {
                        // 紀錄已存在! 請重新輸入!
                        throw new FisException("CQCHK1098", new string[] { });
                    }

                }

                IUnitOfWork uow = new UnitOfWork();
                cond.descr = moLimitQty;
                cond.editor = editor;
                cond.udt = DateTime.Now;
                
                if (null != lstFamilyInfo && lstFamilyInfo.Count > 0)
                {
                    FamilyInfoDef condFind = new FamilyInfoDef();
                    condFind.id = int.Parse(idFamilyInfo);

                    familyRep.UpdateFamilyInfoDefered(uow, cond, condFind);
                }
                else
                {
                    if (string.IsNullOrEmpty(idFamilyInfo))
                    {
                        cond.cdt = DateTime.Now;
                        familyRep.AddFamilyInfoDefered(uow, cond);
                    }
                    else
                    {
                        FamilyInfoDef condFind = new FamilyInfoDef();
                        condFind.id = int.Parse(idFamilyInfo);

                        familyRep.UpdateFamilyInfoDefered(uow, cond, condFind);
                    }
                }
                uow.Commit();

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

		public DataTable GetFamilyFromFamilyInfo()
		{
			string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"select distinct Family
from FamilyInfo
where Name='FAI'
";

                DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_FA,
                                                                                    System.Data.CommandType.Text,
                                                                                    strSQL);
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
		
		public DataTable GetFamily(string custid)
		{
			string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(custid={1})", methodName, custid);
            try
            {
                string strSQL = @"select Family from Family 
where CustomerID = @Customer
order by Family
";

                SqlParameter paraName = new SqlParameter("@Customer", SqlDbType.VarChar, 16);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = custid;

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
		
		public IList<string> GetModelType()
		{
			string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                IList<string> ret = new List<string>();

                IList<ConstValueTypeInfo> lstConstValueType = CommonImpl.GetInstance().GetConstValueTypeListByType("ModelType");
                if (null != lstConstValueType && lstConstValueType.Count > 0)
                {
                    for (int i = 0; i < lstConstValueType.Count; i++)
                    {
                        ret.Add(lstConstValueType[i].value);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
		}
		
    }
}

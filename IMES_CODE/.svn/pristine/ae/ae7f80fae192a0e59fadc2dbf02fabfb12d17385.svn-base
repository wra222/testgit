using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using IMES.Entity.Infrastructure.Framework;
using IMES.Entity.Infrastructure.Interface;
using IMES.Entity.Repository.Meta.IMESSKU;

namespace IMES.Query.Implementation
{
    public class FA_ECOModelQuery : MarshalByRefObject, IFA_ECOModelQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<ECOModelInfo> GetECOModelList(string model, DateTime froms, DateTime to)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            IList<ECOModelInfo> ret = new List<ECOModelInfo>();
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<ECOModel> ecoModelRep = new Repository<ECOModel>("HPIMESDB");
                    if (model != "" && froms != DateTime.MinValue && to != DateTime.MinValue)
                    {
                        ret = (from q in ecoModelRep.Query()
                               where q.Cdt >= froms
                               && q.Cdt <= to
                               && q.Model == model
                               select new ECOModelInfo
                               {
                                   ID = q.ID,
                                   Cdt = q.Cdt,
                                   ECRNo = q.ECRNo,
                                   ECONo = q.ECONo,
                                   Editor = q.Editor,
                                   Model = q.Model,
                                   Plant = q.Plant,
                                   PreStatus = q.PreStatus,
                                   Remark = q.Remark,
                                   Status = q.Status,
                                   Udt = q.Udt,
                                   ValidateFromDate = q.ValidateFromDate
                               }).ToList();
                    }
                    else if (model != "" && froms == DateTime.MinValue)
                    {
                        ret = (from q in ecoModelRep.Query()
                               where q.Model == model
                               select new ECOModelInfo
                               {
                                   ID = q.ID,
                                   Cdt = q.Cdt,
                                   ECRNo = q.ECRNo,
                                   ECONo = q.ECONo,
                                   Editor = q.Editor,
                                   Model = q.Model,
                                   Plant = q.Plant,
                                   PreStatus = q.PreStatus,
                                   Remark = q.Remark,
                                   Status = q.Status,
                                   Udt = q.Udt,
                                   ValidateFromDate = q.ValidateFromDate
                               }).ToList();
                    }
                    else if (model == "" && froms != DateTime.MinValue && to != DateTime.MinValue)
                    {
                        ret = (from q in ecoModelRep.Query()
                               where q.Cdt >= froms
                               && q.Cdt <= to
                               select new ECOModelInfo
                               {
                                   ID = q.ID,
                                   Cdt = q.Cdt,
                                   ECRNo = q.ECRNo,
                                   ECONo = q.ECONo,
                                   Editor = q.Editor,
                                   Model = q.Model,
                                   Plant = q.Plant,
                                   PreStatus = q.PreStatus,
                                   Remark = q.Remark,
                                   Status = q.Status,
                                   Udt = q.Udt,
                                   ValidateFromDate = q.ValidateFromDate
                               }).ToList();
                    }
                    return ret;
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public IList<ECOModelInfo> GetECOModelList(long id)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            IList<ECOModelInfo> ret = new List<ECOModelInfo>();
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<ECOModel> ecoModelRep = new Repository<ECOModel>("HPIMESDB");
                    ret = (from q in ecoModelRep.Query()
                           where q.ID == id
                           select new ECOModelInfo
                           {
                               ID = q.ID,
                               Cdt = q.Cdt,
                               ECRNo = q.ECRNo,
                               ECONo = q.ECONo,
                               Editor = q.Editor,
                               Model = q.Model,
                               Plant = q.Plant,
                               PreStatus = q.PreStatus,
                               Remark = q.Remark,
                               Status = q.Status,
                               Udt = q.Udt,
                               ValidateFromDate = q.ValidateFromDate
                           }).ToList();
                    return ret;
                }
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public IList<ECOModelInfo> SaveECOModelChange(ECOModelInfo item)
		{
			string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                ECOModel items = new ECOModel();
                string preStatus = item.PreStatus;
                string model = item.Model;
                items.Udt = DateTime.Now;
                items.PreStatus = preStatus;
                items.Status = item.Status;
                items.Remark = item.Remark;
                items.Editor = item.Editor;
                items.Cdt = item.Cdt;
                items.Plant = item.Plant;
                items.ValidateFromDate = item.ValidateFromDate;
                items.ECONo = item.ECONo;
                items.ECRNo = item.ECRNo;
                items.Model = model;
                items.ID = item.ID;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IRepository<ECOModel> ecoModelRep = new Repository<ECOModel>("HPIMESDB");
                    IRepository<ConstValueType> constValueTypeRep = new Repository<ConstValueType>("HPIMESDB");
                    IList<ConstValueType> constvaluetemp = new List<ConstValueType>();
                    if (preStatus == "HoldTravelCard")
                    {
                        constvaluetemp = (from q in constValueTypeRep.Query()
                                          where q.Type == "HolTravelCardModel"
                                          && q.Value.Contains(model)
                                          select q).ToList();
                        if (constvaluetemp.Count != 0)
                        {
                            constValueTypeRep.Delete(constvaluetemp[0], false);
                        }
                    }
                    ecoModelRep.Update(items);
                    uow.Commit();
                }
                return GetECOModelList(item.ID);
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
		}

    }
}

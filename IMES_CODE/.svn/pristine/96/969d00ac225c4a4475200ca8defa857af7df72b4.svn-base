using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using System.Data;
using System.Configuration;

namespace IMES.Maintain.Implementation
{
    class RunInTimeControlManager : MarshalByRefObject, IRunInTimeControl
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        public IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

        //从表RunInTimeControl取得Family列表 
        //SELECT Code AS Family 
        //FROM RunInTimeControl
        //         WHERE Type = 'Family'
        //         ORDER BY Family 
        public IList<string> GetFamilyListFromRunInTimeControl()
        {
            return productRepository.GetFamilyListFromRunInTimeControl();
        }

        //根据Type取得RunInTimeControl列表
        //SELECT ID,Code, [Type], [Hour], Remark,Editor, Cdt, Udt 
        //FROM RunInTimeControl
        //         WHERE Type = ? 
        //         ORDER BY Code   
        public IList<RunInTimeControlInfoMaintain> GetRunInTimeControlListByType(string type)
        {
            try
            {
                IList<RunInTimeControl> result = productRepository.GetRunInTimeControlListByType(type);
                IList<RunInTimeControlInfoMaintain> returnList = new List<RunInTimeControlInfoMaintain>();
                foreach (RunInTimeControl runInTimeControl in result)
                {
                    RunInTimeControlInfoMaintain runInTimeControlInfoMaintain = new RunInTimeControlInfoMaintain();
                    runInTimeControlInfoMaintain.ID = runInTimeControl.ID;
                    runInTimeControlInfoMaintain.Code = runInTimeControl.Code;
                    runInTimeControlInfoMaintain.Hour = runInTimeControl.Hour.Trim();
                    runInTimeControlInfoMaintain.TestStation = runInTimeControl.TestStation;
                    runInTimeControlInfoMaintain.ControlType = runInTimeControl.ControlType;
                    runInTimeControlInfoMaintain.Type = runInTimeControl.Type;
                    runInTimeControlInfoMaintain.Remark = runInTimeControl.Remark;
                    runInTimeControlInfoMaintain.Editor = runInTimeControl.Editor;
                    runInTimeControlInfoMaintain.Cdt = runInTimeControl.Cdt;
                    runInTimeControlInfoMaintain.Udt = runInTimeControl.Udt;
                    returnList.Add(runInTimeControlInfoMaintain);
                }
                return returnList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        //根据CPQSNO取得RunInTimeControl列表
        //SELECT ID,Code, [Type], [Hour], Remark,Editor, Cdt, Udt 
        //FROM RunInTimeControl
        //         WHERE Type = ? 
        //         ORDER BY Code   
        public IList<RunInTimeControlInfoMaintain> GetRunInTimeControlListByCPQSNO(string type)
        {                                           
            try
            {
                string Connection = ConfigurationManager.ConnectionStrings["DBServer"].ToString().Trim();
                Connection = string.Format(Connection, "HPIMES");
                string cmdText = @"select a.*
                                    from RunInTimeControl a 
                                    join Product b  on a.Code = b.CUSTSN 
                                    join ProductStatus  c  on c.ProductID = b.ProductID 
                                    where not exists(select 1 from ConstValue d 
                                                        where c.Station = d.Value  and d.Type='SpecialRuninTime') and 
                                                        a.Type ='CPQSNO'";
                DataTable dt = new DataTable();
                dt = IMES.Infrastructure.Repository._Schema.SqlHelper.ExecuteDataFill(Connection, System.Data.CommandType.Text, cmdText);
                IList<RunInTimeControlInfoMaintain> returnList = new List<RunInTimeControlInfoMaintain>();
                for(int i = 0; i<dt.Rows.Count; i++)
                {

                    RunInTimeControlInfoMaintain runInTimeControlInfoMaintain = new RunInTimeControlInfoMaintain();

                    runInTimeControlInfoMaintain.ID = (int)dt.Rows[i]["ID"];
                    runInTimeControlInfoMaintain.Code = (string)dt.Rows[i]["Code"];
                    runInTimeControlInfoMaintain.Hour = (string)dt.Rows[i]["Hour"];
                    runInTimeControlInfoMaintain.TestStation = (string)dt.Rows[i]["TestStation"];
                    runInTimeControlInfoMaintain.ControlType = (bool)dt.Rows[i]["ControlType"];
                    runInTimeControlInfoMaintain.Type = (string)dt.Rows[i]["Type"];
                    if (dt.Rows[i]["Remark"] == DBNull.Value)
                    { runInTimeControlInfoMaintain.Remark = "";}
                    else { runInTimeControlInfoMaintain.Remark = (string)dt.Rows[i]["Remark"]; }
                    runInTimeControlInfoMaintain.Editor = (string)dt.Rows[i]["Editor"];
                    runInTimeControlInfoMaintain.Cdt = (DateTime)dt.Rows[i]["Cdt"];
                    runInTimeControlInfoMaintain.Udt = (DateTime)dt.Rows[i]["Udt"];
                    returnList.Add(runInTimeControlInfoMaintain);
                }


                return returnList;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                throw;
            }
        }

        //根据Type和Code取得RunInTimeControl 
        //SELECT ID,Code, [Type], [Hour], Remark,Editor, Cdt, Udt  
        //FROM RunInTimeControl
        //         WHERE Type = ?
        //                   AND Code = ? 
        public RunInTimeControlInfoMaintain GetRunInTimeControlByTypeAndCode(string type, string code)
        {
            try
            {
                RunInTimeControl runInTimeControl = productRepository.GetRunInTimeControl(type, code);
                if (runInTimeControl == null)
                {
                    return null;
                }
                RunInTimeControlInfoMaintain runInTimeControlInfoMaintain = new RunInTimeControlInfoMaintain();
                runInTimeControlInfoMaintain.ID = runInTimeControl.ID;
                runInTimeControlInfoMaintain.Code = runInTimeControl.Code;
                runInTimeControlInfoMaintain.Type = runInTimeControl.Type;
                runInTimeControlInfoMaintain.Hour = runInTimeControl.Hour.Trim();
                runInTimeControlInfoMaintain.TestStation = runInTimeControl.TestStation;
                runInTimeControlInfoMaintain.ControlType = runInTimeControl.ControlType;
                runInTimeControlInfoMaintain.Type = runInTimeControl.Type;
                runInTimeControlInfoMaintain.Remark = runInTimeControl.Remark;
                runInTimeControlInfoMaintain.Editor = runInTimeControl.Editor;
                runInTimeControlInfoMaintain.Cdt = runInTimeControl.Cdt;
                runInTimeControlInfoMaintain.Udt = runInTimeControl.Udt;
                return runInTimeControlInfoMaintain;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runInTimeControlInfoMaintain"></param>
        /// <returns></returns>
        //public int UpdateRunInTimeControlByTypeAndCode(RunInTimeControlInfoMaintain runInTimeControlInfoMaintain)
        //{
        //    RunInTimeControl runInTimeControl = new RunInTimeControl();
        //    runInTimeControl.Code = runInTimeControlInfoMaintain.Code;
        //    runInTimeControl.Type = runInTimeControlInfoMaintain.Type;
        //    runInTimeControl.Hour = runInTimeControlInfoMaintain.Hour;
        //    runInTimeControl.Remark = runInTimeControlInfoMaintain.Remark;
        //    runInTimeControl.Editor = runInTimeControlInfoMaintain.Editor;
        //    runInTimeControl.TestStation = runInTimeControlInfoMaintain.TestStation;
        //    runInTimeControl.ControlType = runInTimeControlInfoMaintain.ControlType;
        //    runInTimeControl.Udt = runInTimeControlInfoMaintain.Udt;
        //    try
        //    {
        //        RunInTimeControlInfoMaintain runInTimeContr = productRepository.GetRunInTimeControlByTypeCodeAndStation(runInTimeControl.Type, runInTimeControl.Code, runInTimeControl.TestStation);
        //        if (runInTimeContr == null)
        //        {
        //            UnitOfWork uow = new UnitOfWork();
        //            productRepository.UpdateRunInTimeControlByTypeAndCodeDefered(uow, runInTimeControl);
        //            productRepository.InsertRunInTimeControlLogDefered(uow, runInTimeControl.Type, runInTimeControl.Code);
        //            uow.Commit();
        //        }
        //        else
        //        {
        //            //已经存在具有相同runintimecontrol记录
        //            List<string> erpara = new List<string>();
        //            FisException ex;
        //            ex = new FisException("DMT102", erpara);
        //            throw ex;
        //        }
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw;
        //    }
        //    return runInTimeControlInfoMaintain.ID;
        //}
        //记录log信息
        //INSERT RunInTimeControlLog(Code, [Type], [Hour], Remark, Editor, Cdt)
        //                  SELECT Code, [Type], Hour, Remark, Editor, Udt
        //                            FROM RunInTimeControl
        //                            WHERE [Type] = ? AND Code = ?  
        //public void InsertRunInTimeControlLog(string type, string code)
        //{
        //    try
        //    {
        //        productRepository.InsertRunInTimeControlLog(type, code);
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw;
        //    }
        //}

        //新增RunInTimeControl纪录，返回ID (对象带出)
        //INSERT RunInTimeControl(Code, [Type], [Hour], Remark, Editor, Cdt, Udt)
        //                   VALUES(?, ?, ?, ?, ?, GETDATE(), GETDATE()) 
        public int InsertRunInTimeControl(RunInTimeControlInfoMaintain runInTimeControlInfoMaintain)
        {
            RunInTimeControl runInTimeControl = new RunInTimeControl();
            runInTimeControl.Code = runInTimeControlInfoMaintain.Code;
            runInTimeControl.Type = runInTimeControlInfoMaintain.Type;
            runInTimeControl.Hour = runInTimeControlInfoMaintain.Hour;
            runInTimeControl.Remark = runInTimeControlInfoMaintain.Remark;
            runInTimeControl.Editor = runInTimeControlInfoMaintain.Editor;
            runInTimeControl.TestStation = runInTimeControlInfoMaintain.TestStation;
            runInTimeControl.ControlType = runInTimeControlInfoMaintain.ControlType;
            runInTimeControl.Cdt = runInTimeControlInfoMaintain.Cdt;
            runInTimeControl.Udt = runInTimeControlInfoMaintain.Udt;
            try
            {
                RunInTimeControlInfoMaintain runInTimeContr = productRepository.GetRunInTimeControlByTypeCodeAndStation(runInTimeControl.Type, runInTimeControl.Code, runInTimeControl.TestStation);
                if (runInTimeContr == null)
                {
                    UnitOfWork uow = new UnitOfWork();
                    productRepository.InsertRunInTimeControlDefered(uow, runInTimeControl);
                    //productRepository.InsertRunInTimeControlLogDefered(uow, runInTimeControlInfoMaintain.Type, runInTimeControlInfoMaintain.Code);
                    uow.Commit();
                }
                else
                { //已经存在具有相同runintimecontrol记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT102", erpara);
                    throw ex;

                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            return runInTimeControl.ID;
        }

        //删除RunInTimeControl纪录
        //DELETE FROM RunInTimeControl WHERE [Type] = ? AND Code = ?
        //public void DeleteRunInTimeControl(string type, string code)
        //{
        //    try
        //    {
        //        RunInTimeControl runInTimeContr = productRepository.GetRunInTimeControl(type, code);
        //        if (runInTimeContr == null)
        //        {
        //            //已经不存在具有相同runintimecontrol记录
        //            List<string> erpara = new List<string>();
        //            FisException ex;
        //            ex = new FisException("DMT103", erpara);
        //            throw ex;
        //        }
        //        else
        //        {
        //            UnitOfWork uow = new UnitOfWork();
        //            productRepository.InsertRunInTimeControlLogDefered(uow, type, code);
        //            productRepository.DeleteRunInTimeControlDefered(uow, type, code);
        //            uow.Commit();
        //        }
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw;
        //    }
        //}

        //判断Product是否存在
        public bool IfProductIsExists(string custSN)
        {
            IProduct product = productRepository.GetProductByCustomSn(custSN);
            if (product == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //判断Family是否存在
        public bool IfFamilyIsExists(string familyId)
        {
            Family family = familyRepository.Find(familyId);
            if (family == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 根据type、code、teststation修改数据
        /// </summary>
        /// <param name="runInTimeControl"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="testStation"></param>
        //public void UpdateRunInTimeControlByTypeCodeAndTestStation(RunInTimeControlInfoMaintain runInTimeControlInfoMaintain, string type, string code, string testStation)
        //{
        //    RunInTimeControl runInTimeControl = new RunInTimeControl();
        //    runInTimeControl.Code = runInTimeControlInfoMaintain.Code;
        //    runInTimeControl.Type = runInTimeControlInfoMaintain.Type;
        //    runInTimeControl.Hour = runInTimeControlInfoMaintain.Hour;
        //    runInTimeControl.Remark = runInTimeControlInfoMaintain.Remark;
        //    runInTimeControl.Editor = runInTimeControlInfoMaintain.Editor;
        //    runInTimeControl.TestStation = runInTimeControlInfoMaintain.TestStation;
        //    runInTimeControl.ControlType = runInTimeControlInfoMaintain.ControlType;
        //    runInTimeControl.Udt = runInTimeControlInfoMaintain.Udt;
        //    try
        //    {
        //        if ((!runInTimeControl.Code.Equals(code)) || (!runInTimeControl.TestStation.Equals(testStation)))
        //        {
        //            RunInTimeControlInfoMaintain runInTimeContr = productRepository.GetRunInTimeControlByTypeCodeAndStation(runInTimeControl.Type, runInTimeControl.Code, runInTimeControl.TestStation);
        //            if (runInTimeContr != null)
        //            {
        //                //已经存在具有相同runintimecontrol记录
        //                List<string> erpara = new List<string>();
        //                FisException ex;
        //                ex = new FisException("DMT102", erpara);
        //                throw ex;
        //            }
        //        }
        //        UnitOfWork uow = new UnitOfWork();
        //        productRepository.UpdateRunInTimeControlByTypeCodeAndTestStation(runInTimeControl, type, code, testStation);
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw;
        //    }
        //}
        public void UpdateRunInTimeControlById(RunInTimeControlInfoMaintain runInTimeControlInfoMaintain)
        {
            RunInTimeControl runInTimeControl = new RunInTimeControl();
            runInTimeControl.Code = runInTimeControlInfoMaintain.Code;
            runInTimeControl.Type = runInTimeControlInfoMaintain.Type;
            runInTimeControl.Hour = runInTimeControlInfoMaintain.Hour;
            runInTimeControl.Remark = runInTimeControlInfoMaintain.Remark;
            runInTimeControl.Editor = runInTimeControlInfoMaintain.Editor;
            runInTimeControl.TestStation = runInTimeControlInfoMaintain.TestStation;
            runInTimeControl.ControlType = runInTimeControlInfoMaintain.ControlType;
            runInTimeControl.Udt = DateTime.Now;// runInTimeControlInfoMaintain.Udt;
            runInTimeControl.ID = runInTimeControlInfoMaintain.ID;

            try
            {

                RunInTimeControlInfoMaintain runInTimeContr = productRepository.GetRunInTimeControlExceptId(runInTimeControl.Type, runInTimeControl.Code, runInTimeControl.TestStation, runInTimeControl.ID);
                if (runInTimeContr != null)
                {
                    //已经存在具有相同runintimecontrol记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT102", erpara);
                    throw ex;
                }
            
                UnitOfWork uow = new UnitOfWork();
                productRepository.InsertRunInTimeControlLogDefered(uow, runInTimeControl.ID);
                productRepository.UpdateRunInTimeControlByIdDefered(uow,runInTimeControl);
                uow.Commit();

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 根据type，code和teststation获取runintimecontrol数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="testStation"></param>
        /// <returns></returns>
        public RunInTimeControlInfoMaintain getRunintimeControlByCodeTypeAndTestStation(string type, string code, string testStation)
        {
            try
            {
                RunInTimeControlInfoMaintain runInTimeControl = productRepository.GetRunInTimeControlByTypeCodeAndStation(type, code, testStation);
                if (runInTimeControl == null)
                {
                    return null;
                }
                else
                {
                    return runInTimeControl;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

        }

        //根据type，code和teststation删除runintimecontrol数据
        //public void DeleteRunInTimeControlByTypeCodeAndTeststation(string type, string code, string teststation)
        //{
        //    try
        //    {
        //        RunInTimeControl runInTimeContr = productRepository.GetRunInTimeControl(type, code);
        //        if (runInTimeContr == null)
        //        {
        //            //已经不存在具有相同runintimecontrol记录
        //            List<string> erpara = new List<string>();
        //            FisException ex;
        //            ex = new FisException("DMT103", erpara);
        //            throw ex;
        //        }
        //        else
        //        {
        //            UnitOfWork uow = new UnitOfWork();
        //            productRepository.InsertRunInTimeControlLogDefered(uow, type, code);
        //            productRepository.DeleteRunInTimeControlByTypeCodeAndTeststation(type, code, teststation);
        //            uow.Commit();
        //        }
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw;
        //    }
        //}


        public void DeleteRunInTimeControlById(string idStr)
        {
            try
            {
                int id = Int32.Parse(idStr);
                UnitOfWork uow = new UnitOfWork();
                productRepository.InsertRunInTimeControlLogDefered(uow,id);
                productRepository.DeleteRunInTimeControlByIdDefered(uow,id);
                uow.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

    }
}

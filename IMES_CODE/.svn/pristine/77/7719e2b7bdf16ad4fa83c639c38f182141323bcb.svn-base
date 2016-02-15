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
using IMES.FisObject.Common.QTime;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;

namespace IMES.Maintain.Implementation
{
    public class QTime : MarshalByRefObject, IQTime
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IQTimeRepository iQTimeRepository = RepositoryFactory.GetInstance().GetRepository<IQTimeRepository>();
        private ILineRepository iLineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
        private IStationRepository iStationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
        private IFamilyRepository iFamilyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
        private IDefectRepository iDefectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
        private IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
        private IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

        public IList<QTimeinfo> GetQTimeList(string line)
        {
            logger.Debug("(QTime)GetQTimeList starts");
            try
            {
                IList<IMES.FisObject.Common.QTime.QTime> lstQTime = iQTimeRepository.GetQTimeByLine(line);
                IList<QTimeinfo> ret = null;
                if (lstQTime != null)
                {
                    ret = new List<QTimeinfo>();
                    foreach (IMES.FisObject.Common.QTime.QTime items in lstQTime)
                    {
                        ret.Add(QTime_To_QTimeinfo(items));
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
                logger.Debug("(QTime)GetQTimeList end");
            }
        }

        public QTimeinfo CheckExistDefectCodeList(string Line, string Station, string Family)
        {
            logger.Debug("(QTime)CheckExistDefectCodeList starts");
            try
            {
                string[] ObjKey = new string[3];
                ObjKey[0] = Line;
                ObjKey[1] = Station;
                ObjKey[2] = Family;
                IMES.FisObject.Common.QTime.QTime items = new IMES.FisObject.Common.QTime.QTime();
                items = iQTimeRepository.Find(ObjKey);
                return QTime_To_QTimeinfo(items);
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
                logger.Debug("(QTime)CheckExistDefectCodeList end");
            }
        }

        public IList<string> GetAliasLineList()
        {
            logger.Debug("(QTime)GetAliasLineList starts");
            try
            {
                return iLineRepository.GetAllAliasLine();
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
                logger.Debug("(QTime)GetAliasLineList end");
            }    
        }

        public DataTable GetHoldStationList()
        {
            logger.Debug("(QTime)GetStationList starts");
            try
            {
                return iStationRepository.GetStationList();
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
                logger.Debug("(QTime)GetStationList end");
            }
        }

        public IList<ConstValueTypeInfo> GetStationList()
        {
            logger.Debug("(QTime)GetStationList starts");
            try
            {
                return iPartRepository.GetConstValueTypeList("QTimeStation");
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
                logger.Debug("(QTime)GetStationList end");
            }
        }

        public IList<DefectCodeInfo> GetDefectCodeList()
        {
            logger.Debug("(QTime)GetDefectCodeList starts");
            try
            {
                return iDefectRepository.GetDefectCodeList();
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
                logger.Debug("(QTime)GetDefectCodeList end");
            }
        }

        public bool CheckFamily(string InputType)
        {
            logger.Debug("(QTime)CheckFamily starts");
            try
            {
                int ret = iProductRepository.CheckExistsProductIDOrModelOrFamily(InputType);
                if (ret == 1 || ret == 2 || ret ==3)
                {
                    return true;
                }
                else
                {
                    return false;
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
                logger.Debug("(QTime)CheckFamily end");
            }
        }

        public void Add(QTimeinfo item)
        {
            logger.Debug("(QTime)Add starts");
            IUnitOfWork work = new UnitOfWork();
            try
            {
                iQTimeRepository.Add(QTimeInfo_To_QTime(item),work);
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
                work.Commit();
                logger.Debug("(QTime)Add end");
            }
        }

        public void Update(QTimeinfo item)
        {
            logger.Debug("(QTime)Update starts");
            IUnitOfWork work = new UnitOfWork();
            IMES.FisObject.Common.QTime.QTime itemsOld = null;
            IMES.FisObject.Common.QTime.QTime itemsNew = null;
            try
            {
                string[] ObjKey = new string[3];
                ObjKey[0] = item.Line;
                ObjKey[1] = item.Station;
                ObjKey[2] = item.Family  ;
                itemsOld = iQTimeRepository.Find(ObjKey);
                itemsNew = QTimeInfo_To_QTime(item);
                itemsOld.Line = itemsNew.Line;
                itemsOld.Station = itemsNew.Station;
                itemsOld.Family = itemsNew.Family;
                itemsOld.Category = itemsNew.Category;
                itemsOld.TimeOut = itemsNew.TimeOut;
                itemsOld.StopTime = itemsNew.StopTime;
                itemsOld.DefectCode = itemsNew.DefectCode;
                itemsOld.HoldStation = itemsNew.HoldStation;
                itemsOld.HoldStatus = itemsNew.HoldStatus;
                itemsOld.ExceptStation = itemsNew.ExceptStation;
                itemsOld.Editor = itemsNew.Editor;
                iQTimeRepository.Update(itemsOld, work);
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
                work.Commit();
                logger.Debug("(QTime)Update end");
            }
        }

        public void Delete(string Line, string Station, string Family)
        {
            logger.Debug("(QTime)Delete starts");
            try
            {
                iQTimeRepository.RemoveQTime(Line,Station,Family);
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
                logger.Debug("(QTime)Delete end");
            }
        }

        private QTimeinfo QTime_To_QTimeinfo(IMES.FisObject.Common.QTime.QTime list)
        {
            QTimeinfo e = new QTimeinfo();
            if (list != null)
            {
                //QTimeStationStatusEnum enumitem = (QTimeStationStatusEnum)Enum.Parse(typeof(QTimeStationStatusEnum), e.Catagory, true);//enum to string
                e.Line = list.Line;
                e.Station = list.Station;
                e.Family = list.Family;
                e.Catagory = list.Category.ToString();
                e.TimeOut = list.TimeOut;
                e.StopTime = list.StopTime;
                e.DefectCode = list.DefectCode;
                e.HoldStation = list.HoldStation;
                e.HoldStatus = list.HoldStatus.ToString();
                e.ExceptStation = list.ExceptStation;
                e.Editor = list.Editor;
                e.Cdt = list.Cdt;
                e.Udt = list.Udt;
            }
            return e;
        }

        private IMES.FisObject.Common.QTime.QTime QTimeInfo_To_QTime(QTimeinfo list)
        {
            IMES.FisObject.Common.QTime.QTime e = new IMES.FisObject.Common.QTime.QTime();
            if (list != null)
            {
                QTimeStationStatusEnum enumPass_Fail = (QTimeStationStatusEnum)Enum.Parse(typeof(QTimeStationStatusEnum), list.HoldStatus, true);
                QTimeCategoryEnum enumMax_Min = (QTimeCategoryEnum)Enum.Parse(typeof(QTimeCategoryEnum), list.Catagory, true);
                e.Line = list.Line;
                e.Station = list.Station;
                e.Family = list.Family;
                e.Category = enumMax_Min;
                e.TimeOut = list.TimeOut;
                e.StopTime = list.StopTime;
                e.DefectCode = list.DefectCode;
                e.HoldStation = list.HoldStation;
                e.HoldStatus = enumPass_Fail;
                e.ExceptStation = list.ExceptStation;
                e.Editor = list.Editor;
                e.Cdt = list.Cdt;
                e.Udt = list.Udt;
            }
            return e;
        }
    }
}

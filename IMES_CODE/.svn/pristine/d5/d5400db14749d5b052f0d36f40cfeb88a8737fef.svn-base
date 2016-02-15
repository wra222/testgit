using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;
using IMES.Common;
using metas = IMES.Infrastructure.Repository._Metas;
using log4net;
using IMES.Infrastructure;

namespace IMES.Station.Implementation
{
    public class MaterialRequest : MarshalByRefObject, IMaterialRequest
   {
        static ActivityCommonImpl utl = ActivityCommonImpl.Instance;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<MBRepairControlInfo> GeMaterialRequest(MBRepairControlInfo condition)
        {
          
            IList<MBRepairControlInfo> sbInfoList = utl.miscRep.GetData<metas.MBRepairControl, MBRepairControlInfo>(condition);
            return sbInfoList;
             
        }
         public IList<MBRepairControlInfo> GeMaterialRequest(MBRepairControlInfo condition,MBRepairControlInfo betweencondition,string betweenColumnName, DateTime beginValue, DateTime endValue)
        {
          
            IList<MBRepairControlInfo> sbInfoList =  utl.miscRep.GetDataByBetween<metas.MBRepairControl, MBRepairControlInfo>(condition, betweencondition, betweenColumnName, beginValue, endValue);
            return sbInfoList;
             
        }

        public void AddMBRepairControl(MBRepairControlInfo condition)
        {
             logger.Debug("(MBRepairControlInfo)AddMBRepairControlInfo start, [info]:" + condition);
            try
            {
               
                condition.Cdt = DateTime.Now;
                condition.Udt = DateTime.Now;
                utl.miscRep.InsertDataWithID<metas.MBRepairControl, MBRepairControlInfo>(condition);
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
            finally
            {
                logger.Debug("(MBRepairControlInfo)AddInfo end, [info]:" + condition);
            }


         
        }
        public void DelMBRepairControl(MBRepairControlInfo condition)
        {
            logger.Debug("(MBRepairControlInfo)DelMBRepairControlInfo start, [info]:" + condition);
            try
            {
                utl.miscRep.DeleteData<metas.MBRepairControl, MBRepairControlInfo>(new MBRepairControlInfo { ID = condition.ID });
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
            finally
            {
                logger.Debug("(MBRepairControlInfo)DelInfo end, [info]:" + condition);
            }

           
        }
        public void UpdateMBRepairControl(MBRepairControlInfo condition)
        {
            logger.Debug("(MBRepairControlInfo)UpdateMBRepairControlInfo start, [info]:" + condition);
            try
            {
                  MBRepairControlInfo id = new MBRepairControlInfo();
                id.ID = condition.ID;
                condition.Udt = DateTime.Now;

                utl.miscRep.UpdateDataByID<metas.MBRepairControl, MBRepairControlInfo>(id, condition);
                  

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
            finally
            {
                logger.Debug("(MBRepairControlInfo)UpdateInfo end, [info]:" + condition);
            }

        }
   }
}

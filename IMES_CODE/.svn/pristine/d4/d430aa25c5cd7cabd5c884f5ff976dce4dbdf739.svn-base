using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime; 
using IMES.DataModel;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Route;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Station.Implementation
{
     public class MultiUnPack : MarshalByRefObject,IMultiUnPack
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
        public DataTable InputSnList(List<string> snList, string pdline, string editor, string station, string customer)
        {
            Unpack objUnpack = new Unpack();
            DataTable dt = IniTable();
            foreach (string sn in snList)
            { 
                if(string.IsNullOrEmpty(sn)){continue;};
                try
                {
                    objUnpack.UnpackAllBySNCheck(sn, pdline, editor, station, customer);
                    objUnpack.UnpackAllbySNSave(sn);
                    DataRow newRow = dt.NewRow();
                    newRow[0] =sn;
                    newRow[1] = "OK";
                    newRow[2] = "";
                    dt.Rows.Add(newRow);
              
                }
                catch (FisException e)
                {
                    DataRow newRow = dt.NewRow();
                    newRow[0] = sn;
                    newRow[1] = "FAIL";
                    newRow[2] = e.mErrmsg.Replace("\r\n","<br />");
                    dt.Rows.Add(newRow);
                    logger.Error(e.mErrmsg);
                }
                catch (Exception e)
                {
                    DataRow newRow = dt.NewRow();
                    newRow[0] = sn;
                    newRow[1] = "FAIL";
                    newRow[2] = e.Message;
                    dt.Rows.Add(newRow);
                    logger.Error(e.Message);
           
                }
                finally
                {
               //     logger.Debug("(PrintInatelICASAImpl)Cancel end, [prodId]:" + prodId);
                }
            
            }
           return dt;
        }


        public DataTable IniTable()
        {
            DataTable retTable = new DataTable();
            retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
            retTable.Columns.Add("Result", Type.GetType("System.String"));
            retTable.Columns.Add("Error Message", Type.GetType("System.String"));
            return retTable;
        
        }
    }
}

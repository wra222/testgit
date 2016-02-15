using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using carton = IMES.FisObject.PAK.CartonSSCC;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckCartonCTForRCTO : MarshalByRefObject, ICheckCartonCTForRCTO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

		
		/// <summary>
        /// 刷CartonSN，获取该CartonSN下结合的所有机器MBCT2
        /// </summary>
        /// <param name="CartonSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputSN(string CartonSN, string line, string editor, string station, string customer)
		{
			logger.Debug("(CheckCartonCTForRCTOImpl)InputSN start, [CartonSN]:" + CartonSN + " [editor]:" + editor + " [station]:" + station);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList ret = new ArrayList();
			
			try
            {
                string errInfoValue = "请核对CartonSN是否正确";
                bool haveData = false;
                string strSQL = @"select distinct InfoValue from dbo.ProductInfo where ProductID in(
                                 select ProductID from Product where CartonSN=@CartonSN)
                                 and InfoType='ModelCT' order by InfoValue";
                SqlParameter paraName = new SqlParameter("@CartonSN", SqlDbType.VarChar, 20);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = CartonSN;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_FA, System.Data.CommandType.Text,
                        strSQL, paraName);
                foreach (DataRow dr in dt.Rows)
                {
                    string InfoValue = dr[0] as string;
                    if (string.IsNullOrEmpty(InfoValue))
                    {
                        throw new Exception(errInfoValue);
                    }
                    else
                    {
                        haveData = true;
                        ret.Add(InfoValue);
                    }
                }

                if (!haveData)
                {
                    throw new Exception(errInfoValue);
                }
                return ret;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CheckCartonCTForRCTOImpl)InputSN end, [CartonSN]:" + CartonSN + " [editor]:" + editor + " [station]:" + station);
            }
		}


        /// <summary>
        /// check MBCT
        /// </summary>
        /// <param name="CartonSN"></param>
        /// <param name="mbct"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public bool checkMBCT(string CartonSN, string mbct, string line, string editor, string station, string customer)
		{
			logger.Debug("(CheckCartonCTForRCTOImpl)checkMBCT start, [CartonSN]:" + CartonSN + " [editor]:" + editor + " [station]:" + station);
            FisException ex;
            List<string> erpara = new List<string>();
			bool matched = false;

			try
            {
                return matched;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CheckCartonCTForRCTOImpl)checkMBCT end, [CartonSN]:" + CartonSN + " [editor]:" + editor + " [station]:" + station);
            }
		}


        /// <summary>
        /// 结束
        /// 
        /// </summary>
        /// <param name="CartonSN"></param>
		/// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        public void save(string CartonSN, string line, string editor, string station)
		{
			logger.Debug("(CheckCartonCTForRCTOImpl)save start, [CartonSN]:" + CartonSN + " [editor]:" + editor + " [station]:" + station);
            FisException ex;
            List<string> erpara = new List<string>();
			
			try
            {
                carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                CartonLogInfo linfo = new CartonLogInfo();
                linfo.cartonNo = CartonSN;
                linfo.station = station;
                linfo.status = 1;//pass
                linfo.line = line;
                linfo.editor = editor;
                linfo.cdt = DateTime.Now;
                cartRep.AddCartonLogInfo(linfo);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CheckCartonCTForRCTOImpl)save end, [CartonSN]:" + CartonSN + " [editor]:" + editor + " [station]:" + station);
            }
		}
		

    }
}

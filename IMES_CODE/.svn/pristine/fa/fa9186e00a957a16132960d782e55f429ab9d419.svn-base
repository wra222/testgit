/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for COAStatusChange Page            
 * CI-MES12-SPEC-PAK COA Status Change.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;

using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for COAStatusChange.
    /// </summary>
    public class _COAStatusChange : MarshalByRefObject, ICOAStatusChange
    { 
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICOAStatusRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        #region ICOAStatusChange Members
        /// <summary>
        /// 获取COA表相关信息
        /// </summary>
        public IList<S_RowData_COAStatus> GetCOAList(string begNO, string endNO)
        {
            logger.Debug("(_COAStatusChange)GetCOAList start, begin NO:" + begNO + " end NO:" + endNO);
            try
            {
                IList<S_RowData_COAStatus> ret = new List<S_RowData_COAStatus>();
                IList<COAStatus> coaList = currentRepository.GetCOAStatusRange(begNO, endNO);
                foreach (COAStatus tmp in coaList)
                {
                    S_RowData_COAStatus ele;
                    ele.Status = tmp.Status;
                    ele.Pno = tmp.IECPN;
                    ele.pdLine = tmp.LineID;
                    ret.Add(ele);
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
                logger.Debug("(_COAStatusChange)GetCOAList end, begin NO:" + begNO + " end NO:" + endNO);
            }
        }
        /// <summary>
        /// Update COA
        /// </summary>
        public void UpdateCOA(string begNO, string endNO, string editor, string status, string pdLine, string station)
        {
            logger.Debug("(_COAStatusChange)UpdateCOA start, begin NO:" + begNO + " end NO:" + endNO);
            try
            {
                string pno = "";
                string preStatus = "";
                IList<COAStatus> coaList = currentRepository.GetCOAStatusRange(begNO, endNO);
                foreach (COAStatus tmp in coaList)
                {
                    string preLine = "";
                    if ("" == pno)
                    {
                        pno = tmp.IECPN;
                    }
                    if ("" == preStatus)
                    {
                        preStatus = tmp.Status;
                    }


                    if (status != null && status != "")
                    {
                        tmp.Status = status.Substring(0,2);
                        if (status.Length > 3)
                        {
                            pdLine = status.Substring(3, 1);
                        }
                    }
                    if (editor != null && editor != "")
                    {
                        tmp.Editor = editor;
                    }
                    
                    tmp.Udt = DateTime.Now;

                    preLine = tmp.LineID;
                    if (pdLine!= null && pdLine != "")
                    {
                        tmp.LineID = pdLine;
                    }

                    currentRepository.UpdateCOAStatus(tmp);
                    COALog newItem = new COALog();
                    newItem.Cdt = DateTime.Now;
                    newItem.Tp = "COA";
                    newItem.COASN = tmp.COASN;
                    newItem.Editor = editor;
                    newItem.LineID = preLine;
                    if (preStatus == "")
                    {
                        newItem.StationID = "16";
                    }
                    else
                    {
                        newItem.StationID = preStatus;
                    }
                    
                    currentRepository.InsertCOALog(newItem);
                }
                COATransLog newLog = new COATransLog();
                newLog.begNo = begNO;
                newLog.endNo = endNO;
                newLog.cdt = DateTime.Now;
                newLog.status = status.Substring(0, 2);
                newLog.editor = editor;
                newLog.pno = pno;
                newLog.preStatus = preStatus;
                currentRepository.InsertCOATransLog(newLog);
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
                logger.Debug("(_COAStatusChange)UpdateCOA end, begin NO:" + begNO + " end NO:" + endNO);
            }
        }
        /// <summary>
        /// Receive COA
        /// </summary>
        public void ReceiveCOA(string begNO, string endNO, string editor)
        {
            logger.Debug("(_COAStatusChange)ReceiveCOA start, begin NO:" + begNO + " end NO:" + endNO);
            try
            {
                string pno = "";
                IList<COAStatus> coaList = currentRepository.GetCOAStatusRange(begNO, endNO);
                foreach (COAStatus tmp in coaList)
                {
                    string station = "";
                    station = currentRepository.GetStationOfNewestCOALog(tmp.COASN);

                    if (null == editor)
                    {
                        editor = "";
                    }
                    if (null == station || "" == station)
                    {
                        station = "16";
                    }
                    if ("" == pno)
                    {
                        pno = tmp.IECPN;
                    }
                    COALog newItem = new COALog();
                    newItem.Cdt = DateTime.Now;
                    newItem.COASN = tmp.COASN;
                    newItem.Editor = editor;
                    newItem.Tp="COA";
                    newItem.LineID = "RCV";
                    newItem.StationID = station;
                    currentRepository.InsertCOALog(newItem);
                    tmp.Status = station;
                    tmp.Editor = editor;
                    tmp.Udt = DateTime.Now;
                    currentRepository.UpdateCOAStatus(tmp);
                }
                COATransLog newLog = new COATransLog();
                newLog.begNo = begNO;
                newLog.endNo = endNO;
                newLog.cdt = DateTime.Now;
                newLog.status = "01";
                newLog.editor = editor;
                newLog.pno = pno;
                newLog.preStatus = "RE";
                currentRepository.InsertCOATransLog(newLog);
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
                logger.Debug("(_COAStatusChange)ReceiveCOA end, begin NO:" + begNO + " end NO:" + endNO);
            }
        }
        /// <summary>
        /// query
        /// </summary>
        public string  QueryEarly(string begNO)
        {
            logger.Debug("(_COAStatusChange)QueryEarly start, begin NO:" + begNO );
            string ret = "";
            DateTime cdtEnd = DateTime.Now;
            string iecPn = "";
            string[] statuses = new string[2];
            bool empty = true;
            try
            {
                statuses[0] = "A0";
                statuses[1] = "P0";
                IList<COAStatus> coaList = currentRepository.GetCOAStatusRange(begNO, begNO);
                foreach (COAStatus tmp in coaList)
                {
                    iecPn = tmp.IECPN;
                    cdtEnd = tmp.Cdt;
                    empty = false;
                    break;
                }
                if (empty)
                {
                    return ret;
                }
                IList<COAStatus> reList = currentRepository.GetCOAStatusListByStatusAndIecPn(statuses, iecPn , cdtEnd,  begNO);

                foreach (COAStatus tmp in reList)
                {
                    ret = tmp.COASN;
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
                logger.Debug("(_COAStatusChange)QueryEarly end, begin NO:" + begNO );
            }
        }
        #endregion
    }
}

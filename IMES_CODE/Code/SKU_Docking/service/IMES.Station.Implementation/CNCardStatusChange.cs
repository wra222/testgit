/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CNCardStatusChange Page            
 * CI-MES12-SPEC-PAK CN Card Status Change.docx
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
    /// IMES service for CNCardStatusChange.
    /// </summary>
    public class _CNCardStatusChange : MarshalByRefObject, ICNCardStatusChange
    { 
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICOAStatusRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        #region ICNCardStatusChange Members
        /// <summary>
        /// 获取CSNMas表相关信息
        /// </summary>
        public IList<S_RowData_CNCardStatus> GetCSNList(string begNO, string endNO)
        {
            logger.Debug("(_CNCardStatusChange)GetCSNList start, begin NO:" + begNO + " end NO:" + endNO);
            try
            {
                IList<S_RowData_CNCardStatus> ret = new List<S_RowData_CNCardStatus>();
                IList<CSNMasInfo> csnMasList = currentRepository.GetCSNMasRange(begNO, endNO);
                foreach (CSNMasInfo tmp in csnMasList)
                {
                    S_RowData_CNCardStatus ele;
                    ele.Status = tmp.status;
                    ele.Pno = tmp.pno;
                    ele.pdLine = tmp.pdLine;
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
                logger.Debug("(_CNCardStatusChange)GetCSNList end, begin NO:" + begNO + " end NO:" + endNO);
            }
        }
        /// <summary>
        /// Update CSNMas
        /// </summary>
        public void UpdateCSN(string begNO, string endNO, string editor, string udt, string status, string pdLine)
        {
            logger.Debug("(_CNCardStatusChange)UpdateCSN start, begin NO:" + begNO + " end NO:" + endNO);
            try
            {
                string pno = "";
                string preStatus = "";
                IList<CSNMasInfo> csnMasList = currentRepository.GetCSNMasRange(begNO, endNO);
                foreach (CSNMasInfo tmp in csnMasList)
                {
                    if ("" == pno)
                    {
                        pno = tmp.pno;
                    }
                    if ("" == preStatus)
                    {
                        preStatus = tmp.status;
                    }

                    if (status != "")
                    {
                        tmp.status = status.Substring(0, 2);
                        if (status.Length > 3)
                        {
                            pdLine = status.Substring(3, 1);
                        }
                    }
                    if (editor != "")
                    {
                        tmp.editor = editor;
                    }
                    if (udt != "")
                    {
                        tmp.udt = DateTime.Parse(udt);
                    }
                    if (pdLine != "" && pdLine != null)
                    {
                        tmp.pdLine = pdLine;
                    }
                    else
                    {
                        tmp.pdLine = "";
                    }
                    currentRepository.UpdateCSNMas(tmp);
                     
                    
                    
                    COALog newItem = new COALog();
                    newItem.Cdt = DateTime.Now;
                    newItem.Tp = "CNCard";
                    newItem.COASN = tmp.csn2;
               
                    newItem.Editor = editor;
                    newItem.StationID = "";
                    if (pdLine != "" && pdLine != null)
                    {
                        newItem.LineID = pdLine;
                    }
                    else if (tmp.pdLine != null && tmp.pdLine != "")
                    {
                        newItem.LineID = tmp.pdLine;
                    }
                    else
                    {
                        newItem.LineID = "";
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
                logger.Debug("(_CNCardStatusChange)UpdateCSN end, begin NO:" + begNO + " end NO:" + endNO);
            }
        }

        #endregion
    }
}

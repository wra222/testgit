/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for DOA MB Upload Page
 *             
 * UI:CI-MES12-SPEC-FA-UI DOA MB List Upload.docx
 * UC:CI-MES12-SPEC-FA-UC DOA MB List Upload.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-11-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization; 
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for DOA MB Upload.
    /// </summary>
    public class _DOAMBUploadImpl : MarshalByRefObject, IDOAMBUpload
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

        #region IDOAMBUpload Members

        /// <summary>
        /// Save DOA MBSN List.
        /// </summary>
        public void SaveDOAMBList(IList<string> mbList, string editor, out IList<string> passList, out IList<S_RowData_FailMBSN> failList)
        {
            logger.Debug("(_DOAMBUploadImpl)CheckDOAMBList starts");
            try
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                //Fail cause:
                //0: "该MB已作为海退板上传过"
                //1: "错误的MBSN状态"
                //2: "号码不存在，请到结转页面（IMES BackUp To Online）结转数据"
                //3: "错误的MBSN"

                List<string> pList = new List<string>();
                List<S_RowData_FailMBSN> fList = new List<S_RowData_FailMBSN>();

                IList<string> needCheckInFISList = new List<string>();

                S_RowData_FailMBSN item = new S_RowData_FailMBSN();

                //UC<4>: Check [MBSN]
                foreach (string tmp in mbList)
                {
                    if (tmp.Length != 10 && tmp.Length != 11)
                    {
                        item.m_mbsn = tmp;
                        item.m_cause = 3;
                        fList.Add(item);
                        continue;
                    }

                    IMB obj = mbRepository.Find(tmp);

                    if (null != obj)
                    {
                        //HT
                        if (obj.MBStatus.Station == "HT")
                        {
                            item.m_mbsn = tmp;
                            item.m_cause = 0;
                            fList.Add(item);
                            continue;
                        }
                        //32
                        if (obj.MBStatus.Station == "32")
                        {
                            pList.Add(tmp);
                            continue;
                        }
                        //Not HT/32
                        item.m_mbsn = tmp;
                        item.m_cause = 1;
                        fList.Add(item);
                        continue;
                    }
                    else
                    {
                        needCheckInFISList.Add(tmp);
                        continue;
                    }
                }

                if (needCheckInFISList.Count > 0)
                {
                    IList<string> param = new List<string>();
                    int callCnt = 0;
                    param.Add("");
                    foreach (string tmp in needCheckInFISList)
                    {
                        if (param[callCnt].Length + tmp.Length <= 8000)
                        {
                            param[callCnt] += tmp + ";";
                        }
                        else
                        {
                            callCnt++;
                            param.Add("");
                            param[callCnt] += tmp + ";";
                        }
                    }

                    string rlt = "";
                    for (int i = 0; i <= callCnt; i++)
                    {
                        param[i] = param[i].Substring(0, param[i].Length - 1);
                        //Call SP: op_DOA_CheckExistForPCBInFIS
                        //SP in-args:
                        //  @PCBNos varchar(8000)
                        try
                        {
                            DataTable dt = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_DOA,
                                                                       "op_DOA_CheckExistForPCBInFIS",
                                                                       new SqlParameter("PCBNos", param[i]));

                            rlt += dt.Rows[0][0].ToString().Trim() + ";";
                        }
                        catch
                        {
                            throw new Exception("Failed to execute SP 'op_DOA_CheckExistForPCBInFIS', please check the DB linked server!");
                        }
                    }
                    
                    string[] YNList = rlt.Split(';');
                    for (int i = 0; i < needCheckInFISList.Count; i++)
                    {
                        item.m_mbsn = needCheckInFISList[i];
                        item.m_cause = (YNList[i] == "Y") ? 2 : 3;
                        fList.Add(item);
                    }
                }

                UnitOfWork uow = new UnitOfWork();                
                //UC<5>: Save State For Each MBSN
                if (pList.Count > 0)
                {
                    IList<string> param = new List<string>();
                    int callCnt = 0;
                    param.Add("");
                    foreach (string tmp in pList)
                    {
                        if (param[callCnt].Length + tmp.Length <= 8000)
                        {
                            param[callCnt] += tmp + ";";
                        }
                        else
                        {
                            callCnt++;
                            param.Add("");
                            param[callCnt] += tmp + ";";
                        }
                    }
                    for (int i = 0; i <= callCnt; i++)
                    {
                        param[i] = param[i].Substring(0, param[i].Length - 1);
                        //Call SP: op_DOA_UpdateData to save state for each MBSN
                        //SP in-args:
                        //  @MBSNs varchar(8000)
                        //  @Editor varchar(30)
                        productRepository.ExecSpForNonQueryDefered(uow, SqlHelper.ConnectionString_DOA,
                                                                   "op_DOA_UpdateData",
                                                                   new SqlParameter("MBSNs", param[i]),
                                                                   new SqlParameter("Editor", editor));
                    }
                }

                //UC<6>: Save DOA Data
                string groupNo = "";
                try
                {
                    object _syncRoot_GetSeq = new object();
                    SqlTransactionManager.Begin();
                    lock (_syncRoot_GetSeq)
                    {
                        INumControlRepository numControlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                        NumControl nc = numControlRepository.GetMaxValue("DOAGroupNo", "DOAGroupNo", "HP");
                        if (nc != null)
                        {
                            groupNo = nc.Value;
                            string pre = string.Format("{0:yyMMdd}", DateTime.Now);
                            if (groupNo.Length > 6 && groupNo.Substring(0, 6) == pre)
                            {
                                long curMax = long.Parse(groupNo.Substring(6)) + 1;
                                groupNo = pre + curMax.ToString("D6");
                            }
                            else
                            {
                                groupNo = pre + "000001";
                            }
                            nc.Value = groupNo;
                            numControlRepository.SaveMaxNumber(nc, false);
                        }
                        else
                        {
                            groupNo = string.Format("{0:yyMMdd}", DateTime.Now) + "000001";
                            nc = new NumControl();
                            nc.NOName = "DOAGroupNo";
                            nc.NOType = "DOAGroupNo";
                            nc.Value = groupNo;
                            nc.Customer = "HP";
                            numControlRepository.SaveMaxNumber(nc, true);
                        }
                        SqlTransactionManager.Commit();

                    }
                }
                catch (Exception e)
                {
                    SqlTransactionManager.Rollback();
                    throw e;
                }
                finally
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }

                IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                foreach (string tmp in pList)
                {
                    DOAMBListInfo doaItem = new DOAMBListInfo();
                    doaItem.groupNo = groupNo;
                    doaItem.pcbno = tmp;
                    doaItem.status = "1";
                    doaItem.message = "PASS";
                    doaItem.editor = editor;
                    miscRepository.AddDOAMBListInfoDefered(uow, doaItem);
                }

                IList<string> causeList = GetFailCauseList();
                foreach (S_RowData_FailMBSN tmp in fList)
                {
                    DOAMBListInfo doaItem = new DOAMBListInfo();
                    doaItem.groupNo = groupNo;
                    doaItem.pcbno = tmp.m_mbsn;
                    doaItem.status = "0";
                    doaItem.message = causeList[tmp.m_cause];
                    doaItem.editor = editor;
                    miscRepository.AddDOAMBListInfoDefered(uow, doaItem);
                }

                uow.Commit();

                pList.Sort(new IcpString());
                fList.Sort(new IcpFailMBSN());

                passList = pList;
                failList = fList;

                return;
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
                logger.Debug("(_DOAMBUploadImpl)CheckDOAMBList end");
            }
        }

        /// <summary>
        /// Fail cause list.
        /// </summary>
        public IList<string> GetFailCauseList()
        {
            IList<string> ret = new List<string>();
            ret.Add("该MB已作为海退板上传过");
            ret.Add("错误的MBSN状态");
            ret.Add("号码不存在，请到结转页面（IMES BackUp To Online）结转数据");
            ret.Add("错误的MBSN");
            return ret;
        }

        public class IcpString : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return x.CompareTo(y);
            }
        }

        public class IcpFailMBSN : IComparer<S_RowData_FailMBSN>
        {
            public int Compare(S_RowData_FailMBSN x, S_RowData_FailMBSN y)
            {
                return x.m_mbsn.CompareTo(y.m_mbsn);
            }
        }

        #endregion
    }
}

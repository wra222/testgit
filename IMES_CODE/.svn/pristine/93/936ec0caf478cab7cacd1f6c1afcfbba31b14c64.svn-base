/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// PAQCOutput接口的实现类
    /// </summary>
    public class PAQCSortingImpl : MarshalByRefObject, IPAQCSorting
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string custSn, string line, string editor, string station, string customer)
        {
            logger.Debug("(PAQCSortingImpl)InputSN start, custSn:" + custSn);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSn, CommonImpl.InputTypeEnum.CustSN);

                ArrayList retList = new ArrayList();
                //d.	如果Customer S/N 所在的线别与页面选择不同，则报告错误：“Wrong Line!”
                //取得Customer S/N 所在的线别（ProductStatus.Line）
                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                ProductStatusInfo info = productRep.GetProductStatusInfo(currentProduct.ProId);

                if (info.pdLine.Substring(0, 1) != line.Substring(0, 1))
                {
                    List<string> erpara = new List<string>();
                    FisException e;
                    erpara.Add("");
                    e = new FisException("CHK935", erpara);
                    throw e;
                }

                retList.Add(currentProduct.ProId);
                //retList.Add(info.pdLine);
                return retList;
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
                logger.Debug("(PAQCSortingImpl)InputSN end, uutSn:" + custSn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public ArrayList save(int sortingID, string custSN, string editor, string station, string customer)
        {
            logger.Debug("(PAQCSortingImpl)save start [sortingID]: " + sortingID);

            try
            {
                ArrayList retList = new ArrayList();
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                //1.	Insert PAQCSorting_Product
                //INSERT INTO [PAQCSorting_Product]([PAQCSortingID],[CUSTSN],[Status],[Editor],[Cdt])
                //VALUES(@PAQCSortingID, @CustomerSN, 1, @Editor, GETDATE())
                //ITC-1413-0089
                PaqcsortingProductInfo item = new PaqcsortingProductInfo();
                item.paqcsortingid = sortingID;
                item.custsn = custSN;
                item.status = 1;
                item.editor = editor;
                item.cdt = DateTime.Now;
                modelRep.InsertPqacSortingProductInfo(item);

                //Least Pass Qty
                PaqcsortingInfo conf = new PaqcsortingInfo();
                conf.id = sortingID;
                IList<PaqcsortingInfo> sortList = modelRep.GetPaqcsortingInfoList(conf);
                int leastqty = 0;//getLeastPassQty(sortingID, sortList[0].previousFailTime);

                //@PAKSortingQty 取自SysSetting
                int sortingQty = getSortingQty();

                //2.	Update PAQCSorting
                //如果@PAQCSortingID 对应的Least Pass Qty >= @PAKSortingQty 
                //则需要将PAQCSorting 表中ID = @PAQCSortingID 的记录的Status 置为'C'，
                //并于Message 区显示：“本次PAQC Sorting 完毕!”
                //参考方法：
                //UPDATE PAQCSorting SET Stauts = 'C', Editor = @Editor, Udt = GETDATE() WHERE ID = @PAQCSortingID
                bool endFlag = false;
                if (leastqty >= sortingQty)
                {
                    PaqcsortingInfo udata = new PaqcsortingInfo();
                    udata.status = "C";
                    udata.editor = editor;
                    udata.udt = DateTime.Now;
                    PaqcsortingInfo uconf = new PaqcsortingInfo();
                    uconf.id = sortingID;
                    modelRep.UpdatePqacSortingInfo(udata, uconf);
                    endFlag = true;
                }

                retList.Add(endFlag);
                return retList;
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
                logger.Debug("(PAQCSortingImpl)save end [sortingID]: " + sortingID);
            }
        }

        public ArrayList updateStation(string sortingID, string editor, string station)
        {
            logger.Debug("(PAQCSortingImpl)updateStation start [sortingID]: " + sortingID);

            try
            {
                ArrayList retList = new ArrayList();
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                //Update PAQCSorting.PreviousFailTime，Status
                //UPDATE PAQCSorting SET Status = 'O', PreviousFailTime = GETDATE(), Editor = @Editor, Udt = GETDATE() 
                //WHERE ID = @PAQCSortingID

                PaqcsortingInfo udata = new PaqcsortingInfo();
                udata.status = "O";
                udata.previousFailTime = DateTime.Now;
                udata.editor = editor;
                udata.udt = DateTime.Now;

                PaqcsortingInfo uconf = new PaqcsortingInfo();
                uconf.id = Convert.ToInt32(sortingID);
                modelRep.UpdatePqacSortingInfo(udata, uconf);

                return retList;
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
                logger.Debug("(PAQCSortingImpl)save end [sortingID]: " + sortingID);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ArrayList addSorting( string customer,string line, string station,string editor,string remark)
        {
            logger.Debug("(PAQCSortingImpl)addSorting start [station]: " + station);

            try
            {
                ArrayList retList = new ArrayList();
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                //c.如果用户输入的Customer S/N 在数据库中不存在，则报告错误：“Invalid Customer S/N!”
                var currentProduct = CommonImpl.GetProductByInput(customer, CommonImpl.InputTypeEnum.CustSN);
                //d.如果用户输入的Customer S/N 在ProductLog 表中不存在69 站的记录，
                //则报告错误：“此产品尚未进入包装，不能进行Sorting!”
                var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId, "69");
                if (logList.Count <= 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("PAK167", erpara);
                    throw ex;
                }
                
                //1.Insert PAQCSorting
                //INSERT INTO PAQCSorting([Station],[Line],[Status],[PreviousFailTime],[Remark],[Editor],[Cdt],[Udt])
	            //VALUES(@Station, @Line, 'O', GETDATE(), @Remark, @Editor, GETDATE(), GETDATE())
                PaqcsortingInfo info = new PaqcsortingInfo();
                info.station = station;
                info.line = line;

                info.status = "O";
                info.previousFailTime = DateTime.Now;
                info.remark = remark;
                info.editor = editor;
                info.cdt = DateTime.Now;
                info.udt = DateTime.Now;
                modelRep.InsertPqacSortingInfo(info);

                //2.Insert PAQCSorting_Product
                //INSERT INTO [PAQCSorting_Product]([PAQCSortingID],[CUSTSN],[Status],[Editor],[Cdt])
                //VALUES(@PAQCSortingID, @CustomerSN, 2, @Editor, GETDATE())
                PaqcsortingProductInfo pinfo = new PaqcsortingProductInfo();
                pinfo.paqcsortingid = info.id;
                pinfo.custsn = customer;
                pinfo.status = 2;
                pinfo.editor = editor;
                pinfo.cdt = DateTime.Now;
                modelRep.InsertPqacSortingProductInfo(pinfo);

                retList.Add(info.id);
                return retList;
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
                logger.Debug("(PAQCSortingImpl)addSorting end [Station]: " + station);
            }
        }
        #endregion
        public IList<PaqcsortingInfo> GetStationList(string line, string editor, out string message)
        {
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();

            IList<PaqcsortingInfo> stationList = new List<PaqcsortingInfo>();

            message = "";
            ArrayList retList = new ArrayList();
            if (line == "")
            {
                return stationList;
            }
            //1.	获取PAQC Sorting 支持的站点
            //PAQC Sorting 支持的站点定义在SysSetting表，参考如下方法获取：
            //SELECT @PAQCSortingStation = Value FROM SysSetting NOLOCK
            //WHERE Name = 'PAQCSortingStation'
            //其格式是逗号分隔的站号，例如：69,8C,95

            IList<string> valueList = new List<string>();
            valueList = partRep.GetValueFromSysSettingByName("PAQCSortingStation");
            //ITC-1413-0087
            if (valueList.Count == 0)
            {
                /* List<string> erpara = new List<string>();
                 FisException e;
                 erpara.Add("");
                 e = new FisException("PAK146", erpara);//请配置PAQC Sorting Station
                 throw e;*/
                return stationList;
            }

            string[] stationArray = valueList[0].Split(',');
            foreach (string stationID in stationArray)
            {

                //SELECT @PAQCSortingID = ID FROM PAQCSorting NOLOCK
                //WHERE Station = @Station 
                //AND LEFT(Line, 1) = LEFT(@Line, 1)
                //AND Status = 'O'
                IList<PaqcsortingInfo> sortingList = modelRep.GetPaqcsortingInfoList(line, stationID);
                foreach (var item in sortingList)
                {
                    if (item.status == "O")
                    {
                        insertSortingProduct(item.id, line, stationID,item.previousFailTime);
                    }
                    if (item.status == "O" || item.status == "I")
                    {
                        PaqcsortingInfo newStation = new PaqcsortingInfo();
                        //Descr Station.Descr
                        IStation station = stationRep.Find(stationID);
                        newStation.station = stationID;
                        newStation.Descr = station.Descr;

                        //SELECT CUSTSN FROM PAQCSorting_Product
                        //WHERE PAQCSortingID = @PAQCSortingID AND Status = '2'              
                        PaqcsortingProductInfo conf = new PaqcsortingProductInfo();
                        conf.paqcsortingid = item.id;
                        conf.status = 2;
                        PaqcsortingProductInfo nconf = new PaqcsortingProductInfo();
                        IList<PaqcsortingProductInfo> pinfoList = modelRep.GetPaqcsortingProductInfoList(conf, nconf);
                        if (pinfoList.Count > 0)
                        {
                            newStation.editor = pinfoList[0].custsn;
                        }

                        //SELECT Status FROM PAQCSorting
                        //WHERE ID = @PAQCSortingID
                        //取得的Status 值，使用如下字符串显示
                        //I – Initial 	O– Open 	C – Close
                        switch (item.status)
                        {
                            case "I":
                                newStation.status = "Initial";
                                break;
                            case "O":
                                newStation.status = "Open";
                                break;
                            case "C":
                                newStation.status = "Close";
                                break;
                        }
                        //SELECT PreviousFailTime FROM PAQCSorting
                        //WHERE ID = @PAQCSortingID
                        newStation.previousFailTime = item.previousFailTime;
 
                        //Qty
                        //SELECT COUNT(CUSTSN) FROM PAQCSorting_Product NOLOCK
	                    //WHERE PAQCSortingID = @PAQCSortingID AND Status <> 2
                        PaqcsortingProductInfo cond = new PaqcsortingProductInfo();
                        cond.paqcsortingid = item.id;
                        PaqcsortingProductInfo ncond = new PaqcsortingProductInfo();
                        ncond.status = 2;
                        pinfoList= modelRep.GetPaqcsortingProductInfoList(cond,ncond);
                        newStation.LeastPassQty = pinfoList.Count;

                        newStation.id = item.id;

                        stationList.Add(newStation);

                        //5.	Update PAQCSorting
                        //检查每一条记录，如果@PAQCSortingID 对应的Qty >= @PAKSortingQty 
                        //则需要将PAQCSorting 表中ID = @PAQCSortingID 的记录的Status 置为'C'，
                        //并于Message 区显示：“本次PAQC Sorting 完毕!  Line: ”+ LEFT(@Line, 1) + 
                        //“ Station: ” + @Station + “（” + @StationDescr + “） ” + “Sorting ID: ”+ @PAQCSortingID
                        //UPDATE PAQCSorting SET Stauts = 'C', Editor = @Editor, Udt = GETDATE() WHERE ID = @PAQCSortingID
                        int pakSortingQty = getSortingQty();
                        if (newStation.LeastPassQty >= pakSortingQty)
                        {
                            PaqcsortingInfo udata = new PaqcsortingInfo();
                            udata.status = "C";
                            udata.editor = editor;
                            udata.udt = DateTime.Now;

                            PaqcsortingInfo uconf = new PaqcsortingInfo();
                            uconf.id = newStation.id;
                            modelRep.UpdatePqacSortingInfo(udata, uconf);

                            message = message + "Line: " + line.Substring(0, 1)
                                     + " Station: " + stationID + "(" + newStation.Descr + ")"
                                     + " Sorting ID: " + item.id + " ;";
                        }
                    }
                }
            }
            //所有记录按照Status, Station 排序显示
            return (from item in stationList orderby item.status, item.station select item).ToList(); ;
        }

        /// <summary>
        /// 新增Product 记录至PAQCSortiing_Product
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        public void insertSortingProduct(int sortingID,string line,string station,DateTime failTime)
        {

            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

            //SELECT @StartTime = MIN(Cdt) FROM PAQCSorting_Product NOLOCK
	        //WHERE PAQCSortingID = @PAQCSortingID
            DateTime starttime = modelRep.GetMinCdtFromPaqcSortingProduct(sortingID);
            if (starttime.Year == 1)
            {
                starttime = failTime;
            }

            DateTime openTime = DateTime.Now;
            if (DateTime.Compare(failTime,starttime) >= 0)
            {
                openTime = failTime;
            }
            else
            {
                openTime = starttime;
            }

            //SELECT @N1 = COUNT(*) FROM PAQCSorting_Product NOLOCK
	        //WHERE PAQCSortingID = @PAQCSortingID AND Status <> '2'
            PaqcsortingProductInfo conf = new PaqcsortingProductInfo();
            conf.paqcsortingid = sortingID;
            PaqcsortingProductInfo nconf = new PaqcsortingProductInfo();
            nconf.status = 2;
            IList<PaqcsortingProductInfo> pinfoList = modelRep.GetPaqcsortingProductInfoList(conf,nconf);
            int n1 = pinfoList.Count;

            //SELECT @N2 = CONVERT(int, Value)
	        //FROM SysSetting NOLOCK WHERE Name = 'PAKSortingQty'
            int n2 = getSortingQty();

            //SET @N3 = @N2 - @N1 + 1
            int n3 = n2 - n1 + 1;
            if (n3 < 0)
            {
                n3 = 0;
            }

            //INSERT INTO PAQCSorting_Product (PAQCSortingID, CUSTSN, Status, Editor, Cdt)		
	        //SELECT TOP (@N3) @PAQCSortingID, a.CUSTSN, b.Status, b.Editor, b.Cdt
		    //FROM Product a (NOLOCK), ProductLog b (NOLOCK)
		    //WHERE a.ProductID = b.ProductID
			//AND b.Cdt > @StartTime
            //AND b.Cdt > @PreviousFailTime
			//AND LEFT(b.Line, 1) = LEFT(@Line, 1)
			//AND b.Station = @Station 
			//AND a.CUSTSN NOT IN (SELECT CUSTSN FROM PAQCSorting_Product NOLOCK WHERE PAQCSortingID = @PAQCSortingID)
            //ORDER BY b.Cdt
            modelRep.InsertIntoPaqCSortingProductFromProductAndProductLog(openTime, line, station, sortingID, n3);

            return ;

        }
        /// <summary>
        /// 取得Pass Qty
        /// </summary>
        /// <param name="sortingID"></param>
        /// <returns></returns>
        public int getPassQty(int sortingID)
        {
            //参考如下方法取得Pass Qty:
            //SELECT COUNT(CUSTSN) FROM PAQCSorting_Product NOLOCK
            //WHERE PAQCSortingID = @PAQCSortingID //AND Status = 1
            //Remark：@PAQCSortingID 为前面取得Previous Fail Time 时，同时取得的PAQCSorting.ID
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            PaqcsortingProductInfo conf = new PaqcsortingProductInfo();
            conf.paqcsortingid = sortingID;
            //conf.status = 1;
            int qty = modelRep.GetCountOfPqacSortingProduct(conf);
            //Modify 2012/08/30  Status <>2 
            //==============================================================
            PaqcsortingProductInfo conf2 = new PaqcsortingProductInfo();
            conf2.paqcsortingid = sortingID;
            conf2.status = 2;
            int qty2 = modelRep.GetCountOfPqacSortingProduct(conf2);
            qty = qty - qty2;
            //==============================================================
            return qty;
        }
        /// <summary>
        /// @PAKSortingQty
        /// </summary>
        /// <returns></returns>
        public int getSortingQty()
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            int sortingQty = 0;
            IList<string> tmpList = new List<string>();

            //@PAKSortingQty 取自SysSetting，参考如下方法获取：
            //SELECT @PAKSortingQty = Value FROM SysSetting NOLOCK WHERE Name = 'PAKSortingQty'
            tmpList = partRep.GetValueFromSysSettingByName("PAKSortingQty");

            if (tmpList.Count > 0)
            {
                sortingQty = Convert.ToInt32(tmpList[0]);
            }

            return sortingQty;
        }
    }
}

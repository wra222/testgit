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
                int leastqty = getLeastPassQty(sortingID, sortList[0].previousFailTime);

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

        public ArrayList updateStation(int sortingID, string editor, string station, string customer)
        {
            logger.Debug("(PAQCSortingImpl)updateStation start [sortingID]: " + sortingID);

            try
            {
                ArrayList retList = new ArrayList();
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                //Update PAQCSorting.PreviousFailTime
                //参考方法：
                //UPDATE PAQCSorting SET PreviousFailTime = GETDATE(), Editor = @Editor, Udt = GETDATE() WHERE ID = @PAQCSortingID

                PaqcsortingInfo udata = new PaqcsortingInfo();
                udata.previousFailTime = DateTime.Now;
                udata.editor = editor;
                udata.udt = DateTime.Now;
                PaqcsortingInfo uconf = new PaqcsortingInfo();
                uconf.id = sortingID;
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
                PaqcsortingInfo newStation = new PaqcsortingInfo();

                //Descr Station.Descr
                IStation station = stationRep.Find(stationID);
                newStation.station = stationID;
                newStation.Descr = station.Descr;

                //Previous Fail Time
                //SELECT ID, PreviousFailTime FROM PAQCSorting 
                //WHERE Status = 'O' AND LEFT(Line, 1) = LEFT(@Line, 1) AND Station = @Station
                //Remark：@Line – 页面选择的Line
                //Note：如果没有满足条件的记录，则Previous Fail Time显示为空
                IList<PaqcsortingInfo> sortingList = modelRep.GetPreviousFailTimeList(line, stationID);
                if (sortingList.Count > 0)
                {
                    newStation.id = sortingList[0].id;
                    newStation.previousFailTime = sortingList[0].previousFailTime;

                    DateTime startTime = modelRep.GetMinCdtFromPaqcSortingProduct(newStation.id);
                    if (startTime != DateTime.MinValue)
                    {
                        modelRep.InsertIntoPaqCSortingProductFromProductAndProductLog(startTime, line, stationID, newStation.id);
                    }
                }
                else
                {
                    newStation.id = -1;
                }

                //Pass Qty
                //如果前文没有得到Previous Fail Time，则显示为空；否则，参考如下方法取得Pass Qty:

                if (newStation.id != -1)
                {
                    int passQty = getPassQty(newStation.id);
                    newStation.PassQty = passQty;
                }

                //Least Pass Qty
                //如果前文没有得到Previous Fail Time，则显示为空；否则，参考如下方法取得Least Pass Qty:
                if (newStation.id != -1)
                {
                    int leastQty = getLeastPassQty(newStation.id, newStation.previousFailTime);
                    newStation.LeastPassQty = leastQty;
                }

                //将其中Least Pass Qty >= @PAKSortingQty 或者Previous Fail Time 为空的记录Disable
                int sortingQty = getSortingQty();
                if (newStation.id != -1 && newStation.LeastPassQty >= sortingQty)
                {
                    //5.	Update PAQCSorting
                    //检查每一条记录，如果@PAQCSortingID 对应的Least Qty >= @PAKSortingQty 
                    //则需要将PAQCSorting 表中ID = @PAQCSortingID 的记录的Status 置为'C'，
                    //并于Message 区显示：“本次PAQC Sorting 完毕!  Line: ”+ LEFT(@Line, 1) + “ Station: ” + @Station + “（” + @StationDescr + “）”
                    //message = message + station.StationId + "(" + station.Descr + ")";
                    //UPDATE PAQCSorting SET Stauts = 'C', Editor = @Editor, Udt = GETDATE() WHERE ID = @PAQCSortingID
                    PaqcsortingInfo udata = new PaqcsortingInfo();
                    udata.status = "C";
                    //udata.previousFailTime = DateTime.Now;
                    udata.editor = editor;
                    udata.udt = DateTime.Now;

                    PaqcsortingInfo uconf = new PaqcsortingInfo();
                    uconf.id = newStation.id;
                    modelRep.UpdatePqacSortingInfo(udata, uconf);

                    message = message + station.StationId + "(" + station.Descr + ")";

                    newStation.id = -2;

                }
               
                stationList.Add(newStation);

            }

            return stationList;

        }
        /// <summary>
        /// Least Pass Qty
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="failTime"></param>
        /// <returns></returns>
        public int getLeastPassQty(int sortingID, DateTime failTime)
        {

            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            PaqcsortingProductInfo conf = new PaqcsortingProductInfo();
            conf.paqcsortingid = sortingID;
            //conf.status = 1;
            //Least Pass Qty
            //参考如下方法取得Least Pass Qty:
            //SELECT COUNT(CUSTSN) FROM PAQCSorting_Product NOLOCK
            //WHERE PAQCSortingID = @PAQCSortingID AND Cdt > @PreviousFailTime //AND Status = 1
            int qty = modelRep.GetCountOfPqacSortingProduct(conf, failTime);
            //Modify 2012/08/30  Status <>2 
            //==============================================================
            PaqcsortingProductInfo conf2 = new PaqcsortingProductInfo();
            conf2.paqcsortingid = sortingID;
            conf2.status = 2;
            int qty2 = modelRep.GetCountOfPqacSortingProduct(conf2, failTime);
            qty = qty - qty2;
            //==============================================================
            return qty;
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

// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新ProductStatus
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-02-15   Vincent                 create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;
using System.Text.RegularExpressions;
using IMES.Common;

namespace IMES.Activity
{
  /// <summary>
  /// 
  /// </summary>
    public partial class UnPackProductPartByStation : BaseActivity
    {
        
        /// <summary>
        /// constructor
        /// </summary>
        public UnPackProductPartByStation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", 
                                                                                                                                    typeof(bool),
                                                                                                                                    typeof(UnPackProductPartByStation),
                                                                                                                                    new PropertyMetadata(false));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.ProdList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(UnPackProductPartByStation.IsSingleProperty)));
            }
            set
            {
                base.SetValue(UnPackProductPartByStation.IsSingleProperty, value);
            }
        }

        /// <summary>
        /// Write Relase Code
        /// </summary>
        public static DependencyProperty IsReleaseHoldProperty = DependencyProperty.Register("IsReleaseHold",
                                                                                                                                    typeof(bool),
                                                                                                                                    typeof(UnPackProductPartByStation),
                                                                                                                                    new PropertyMetadata(true));

        /// <summary>
        /// Release Hold Code
        /// </summary>
        [DescriptionAttribute("IsReleaseHold")]
        [CategoryAttribute("IsReleaseHold Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsReleaseHold
        {
            get
            {
                return ((bool)(base.GetValue(UnPackProductPartByStation.IsReleaseHoldProperty)));
            }
            set
            {
                base.SetValue(UnPackProductPartByStation.IsReleaseHoldProperty, value);
            }
        }


        /// <summary>
        /// Update Product Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPartRepository  partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

            IList<string> prodIDList = new List<string>();
            if (this.IsSingle)
            {
                IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (currentProduct == null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.Product });
                }
                prodIDList.Add(currentProduct.ProId);
            }
            else
            {
               prodIDList = CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) as IList<string>;
                if (prodIDList == null || prodIDList.Count == 0)
                {
                    IList<IProduct> productList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
                    if (productList == null || productList.Count == 0)
                    {
                        throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.ProdList + " and " + Session.SessionKeys.NewScanedProductIDList });
                    }
                    prodIDList = (from p in productList
                                  select p.ProId).ToList();                   
                }
                
            }

            //ConstValue.Type='GoToStationWithUnPack'
            //ConstValue.Name=GoToStation
            //ConstValue.Value=unpack Station 可以設置多個Station以,~;都可當分隔號 
            string goToStation =(string) CurrentSession.GetValue("GoToStation");
            if (string.IsNullOrEmpty(goToStation))
            {
                throw new FisException("CQCHK0006", new string[] { "GoToStation" });
            }

            IList <ConstValueInfo> unpackStationList=partRep.GetConstValueInfoList(new ConstValueInfo() { type="GoToStationWithUnPack",
                                                                                                                                                             name=goToStation});
            //if(unpackStationList==null || unpackStationList.Count ==0)
            //{
            //    throw new FisException("CQCHK0026 ", new string[] {"Type:GoToStationWithUnPack Name:" +goToStation });
            //}

            if (unpackStationList != null && unpackStationList.Count > 0 && !string.IsNullOrEmpty(unpackStationList[0].value))
            {
                IList<string> unPackStationList = unpackStationList[0].value.Split(new char[] { '~' }).ToList();

                IList<string> unpackCheckItemTypeList = prodRep.GetProductPartCheckItemTypeByStation(prodIDList, unPackStationList);

                if (unpackCheckItemTypeList.Contains("MB"))
                {
                    //1.get MBSnList from Product 
                    IList<ProductMBInfo> combinedPcbInfo= prodRep.GetPCBListByProductId(prodIDList);

                    IList<ConstValueInfo> checkOnBoardList = partRep.GetConstValueListByType("CPUOnBoard").Where(x=>!string.IsNullOrEmpty(x.value)).ToList();
                    IList<string> unpackCPUProductIDList = new List<string>();
                    if (checkOnBoardList != null && checkOnBoardList.Count > 0)
                    {
                        unpackCPUProductIDList = (from p in combinedPcbInfo
                                                                   where checkOnBoardCPU(p,checkOnBoardList)
                                                                   select p.ProductID).ToList();
                    }

                    

                     //2.Update Product.PCBID, PCBModelID,MAC,MBCER, CVSN to empty (for have MB CPU SN) 
                    prodRep.UnPackMBDefered(CurrentSession.UnitOfWork, prodIDList);
                    if (unpackCPUProductIDList != null && unpackCPUProductIDList.Count > 0)
                    {
                        prodRep.UnPackCPUDefered(CurrentSession.UnitOfWork, unpackCPUProductIDList);
                    }
                    //3.Update PCBStatus 15 station , PCBStatusEx 32 station, write pcbLog
                    var unpackMBSnList =(from p in combinedPcbInfo
                                                  where !string.IsNullOrEmpty(p.PCBID)
                                                  select p.PCBID).ToList();
                    if (unpackMBSnList!=null && unpackMBSnList.Count>0)
                    {
                        IList<TbProductStatus> mbStatusList= mbRep.GetMBStatus(unpackMBSnList);
                        mbRep.UpdatePCBPreStationDefered(CurrentSession.UnitOfWork, mbStatusList);
                        mbRep.UpdatePCBStatusByMultiMBDefered(CurrentSession.UnitOfWork,
                                                                                            unpackMBSnList,
                                                                                            "15A",
                                                                                            1,
                                                                                            mbStatusList[0].Line,
                                                                                            this.Editor);
                        mbRep.WritePCBLogByMultiMBDefered(CurrentSession.UnitOfWork,
                                                                                            unpackMBSnList,
                                                                                            "15A",
                                                                                            1,
                                                                                            mbStatusList[0].Line,
                                                                                            this.Editor);
                    }
                }

                if (unpackCheckItemTypeList.Contains("CPU"))
                {
                    // mantis 538
                    Session session = CurrentSession;
                    foreach (string proId in prodIDList)
                    {
                        IProduct currentProduct = prodRep.Find(proId);
                        //if (ActivityCommonImpl.Instance.NeedCheckMaterialCpuStatus(currentProduct, currentProduct.CVSN, ref session))
                        //{
                        ActivityCommonImpl.Instance.Material.UpdateMaterialCpuStatus(currentProduct.CVSN, "GoToStation", "Dismantle", true, true, ref session);
                        //}
                    }
                    
                    prodRep.UnPackCPUDefered(CurrentSession.UnitOfWork, prodIDList);
                }
                
                prodRep.DeleteProductPartByProductIDAndStationDefered(CurrentSession.UnitOfWork, prodIDList, unPackStationList, this.Editor);


                

               
            }


            if (this.IsReleaseHold)
            {
                IList<HoldInfo> holdInfoList = ( IList<HoldInfo> )CurrentSession.GetValue(ExtendSession.SessionKeys.ProdHoldInfoList);
                if (holdInfoList == null || holdInfoList.Count==0)
                {
                    throw new FisException("CQCHK0006", new string[] { ExtendSession.SessionKeys.ProdHoldInfoList });
                }

                string  releaseCode = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.ReleaseCode);
                if (string.IsNullOrEmpty(releaseCode))
                {
                    throw new FisException("CQCHK0006", new string[] { ExtendSession.SessionKeys.ReleaseCode });
                }

                prodRep.ReleaseHoldProductIDDefered(CurrentSession.UnitOfWork, holdInfoList, releaseCode, this.Editor);
            }

            return base.DoExecute(executionContext);
        }


        private bool checkOnBoardCPU(ProductMBInfo mbInfo, IList<ConstValueInfo> checkRule)
        {
            bool ret = false;

            foreach (ConstValueInfo item in checkRule)
            {
                if (Regex.IsMatch(mbInfo.PCBID, item.value))
                {
                    ret = true;
                    break;
                }

                if (Regex.IsMatch(mbInfo.Family, item.value))
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }
       
    }
}

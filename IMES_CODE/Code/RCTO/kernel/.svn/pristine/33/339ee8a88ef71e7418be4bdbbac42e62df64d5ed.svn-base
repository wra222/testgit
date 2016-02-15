using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.Linq;
namespace IMES.CheckItemModule.CQ.CT.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CT.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            if (part_unit != null)
            {
                //没有结合其它Product
                string partSn = ((PartUnit)part_unit).Sn.Trim();
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct product = product_repository.GetProductByIdOrSn(partSn);
                if (product != null)
                {
                    if (product.ProId != ((PartUnit) part_unit).ProductId) //将会在PartUnit中增加ProId。
                    {
                        throw new FisException("CHK184", new string[] {});
                    }
                }
                if (!string.IsNullOrEmpty(partSn)
                    && partSn.Length == 14
                    && string.Compare(partSn, 0, "6", 0, 1) == 0)
                {
                  
                    string hppn = partSn.Substring(0, 5);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var btInfo = partRepository.FindBattery(hppn);
                 
                    if (btInfo == null || string.IsNullOrEmpty(btInfo.hssn))
                    {
                        throw new FisException("CHK873", new[] { hppn });
                    }
                    //add Check Battery  

                    if (((PartUnit)part_unit).CurrentSession != null)
                    {
                        Session session = (Session)((PartUnit)part_unit).CurrentSession;
                        bool check = session.GetValue("EnableBatteryCtCheck")==null?false:(bool)session.GetValue("EnableBatteryCtCheck");
                        if (check)
                        {
                            if (session.GetValue("InputBatteryQty") == null) { session.AddValue("IsHavePassBattery", false); }
                            if (session.GetValue("InputBatteryQty") == null)
                            { session.AddValue("InputBatteryQty", 1); }
                            else
                            {
                                int battQty = (int)session.GetValue("InputBatteryQty");
                                battQty++;
                                session.AddValue("InputBatteryQty", battQty);
                            }

                            bool onlyCheckOne = session.GetValue("OnlyCheckOneBattery") == null ? true : (bool)session.GetValue("OnlyCheckOneBattery");
                            IFlatBOMItem bItem = (IFlatBOMItem)bom_item;
                            int qty=bItem.Qty;
                            if (qty == 1) // 只針對收1個 Battery Check CHK974
                            {
                                if (!CheckConsTypeAndFACombine(partRepository, session.Key, partSn))
                                { throw new FisException("CHK974", new List<string>()); }
                                
                            }
                            else if (!onlyCheckOne) // 2個以上Battery and onlyCheckOne=false
                            {
                             // int battQty = (int)session.GetValue("InputBatteryQty");
                                if ((int)session.GetValue("InputBatteryQty") != qty)
                                {
                                    if (session.GetValue("IsHavePassBattery") != null && !(bool)session.GetValue("IsHavePassBattery")) //if IsHavePassBattery=true, need not check
                                    {
                                        bool isPass = CheckConsTypeAndFACombine(partRepository, session.Key, partSn);
                                        session.AddValue("IsHavePassBattery", isPass);
                                    }
                                }
                                else //收最後一個battery
                                {
                                    if (!(bool)session.GetValue("IsHavePassBattery"))
                                    {
                                        if (!CheckConsTypeAndFACombine(partRepository, session.Key, partSn))
                                        {
                                            { throw new FisException("CHK974", new List<string>()); }
                                        }
                                    
                                    }
                                }
  
                            }

                        }// if (check)
                    } //    if (((PartUnit)part_unit).CurrentSession != null)

                    //add Check Battery
                }
            
            }
        }

        private bool CheckConsTypeAndFACombine(IPartRepository partRepository,string productId,string ct)
        {
            IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<ProductPart> lst = product_repository.GetProductPartsByPartSn(ct);
            IList<IMES.DataModel.ConstValueTypeInfo> lstConst= partRepository.GetConstValueTypeList("BATTCT");
            bool contains = lstConst.Any(p => p.value == ct);
            return (contains || lst.Count > 0);
        }
   
    }
}

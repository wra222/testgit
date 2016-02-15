using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.FisObject.Common.Process;
using IMES.DataModel;
using System.Text.RegularExpressions;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;

namespace IMES.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UnPackPart
	{
        private static readonly IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="product"></param>
        /// <param name="conditionInfoList"></param>
        /// <param name="productPart"></param>
        /// <param name="editor"></param>
        public static void UnPackProduct(Session session,
                                                            IProduct product,
                                                          IList<ConstValueInfo> conditionInfoList,
                                                          IProductPart productPart,
                                                          string editor)
        {

            var matchInfo = conditionInfoList.Where(x => x.name == productPart.PartType).FirstOrDefault();
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            if (matchInfo != null)
            {
                string returnStation = matchInfo.value;
                string snRE = matchInfo.description;
                string custSN = null;
                if (string.IsNullOrEmpty(snRE)  ||
                    !snRE.Contains("<SN>"))
                {
                    custSN = productPart.PartSn;
                }
                else
                {
                    Match match = Regex.Match(productPart.PartSn, snRE, RegexOptions.Compiled);
                    if (match.Success)
                    {                       
                        Group groupSN =match.Groups["SN"];
                        if (groupSN != null && 
                            groupSN.Success)
                        {
                            custSN = groupSN.Value;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(custSN))
                {
                    IProduct unpackProd = prodRep.FindOneProductWithProductIDOrCustSN(custSN);
                    if (unpackProd != null)
                    {
                        DateTime now = DateTime.Now;

                        unpackProd.AddLog(new ProductLog
                        {
                            ProductID = unpackProd.ProId,
                            Model = unpackProd.Model,
                            Line = unpackProd.Status.Line,
                            Station = returnStation,
                            Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                            Editor = editor,
                            Cdt = now
                        });

                        var newStatus = new IMES.FisObject.FA.Product.ProductStatus();

                        newStatus.Status = IMES.FisObject.Common.Station.StationStatus.Pass;
                        newStatus.StationId = returnStation;
                        newStatus.Editor = editor;
                        newStatus.Line = unpackProd.Status.Line;
                        newStatus.TestFailCount = 0;
                        newStatus.ReworkCode = string.Empty;
                        unpackProd.UpdateStatus(newStatus);

                        IList<IMES.DataModel.TbProductStatus> stationList = prodRep.GetProductStatus(new List<string> { unpackProd.ProId });
                        prodRep.UpdateProductPreStationDefered(session.UnitOfWork, stationList);

                        prodRep.Update(unpackProd, session.UnitOfWork);
                    }
                }
            }
        }
	}
}

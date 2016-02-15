// INVENTEC corporation (c)2012 all rights reserved. 
// Description: QueryBorrow
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-11   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class QueryBorrow : MarshalByRefObject, IQueryBorrow
    {
        /// <summary>
        /// Get BorrowLog List by Status
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.BorrowLog> GetBorrowList(string type)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            return productRepository.GetBorrowLogByStatus(type);

        }
    }
}

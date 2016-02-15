using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Entity.Repository.Meta;
using IMES.Entity.Infrastructure.Interface;

namespace IMES.Entity.Repository.Interface
{
    public interface IProductRepository: IRepository<Product>
    {
        IList<Product> GetProductByModel(string model);
    }
}

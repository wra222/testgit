using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Configuration;
using IMES.Entity.Repository.Interface;
using IMES.Entity.Infrastructure.Interface;
using IMES.Entity.Infrastructure.Framework;
using IMES.Entity.Repository.Meta;

namespace IMES.Entity.Repository.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository():base("DBServer")
        {
        }       

        public IList<Product> GetProductByModel(string model)
        {            
           return this.Find(x => x.Model == model); 
        }        
    }
}

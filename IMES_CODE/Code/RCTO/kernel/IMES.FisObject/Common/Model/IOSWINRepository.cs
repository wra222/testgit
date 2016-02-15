using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.Model
{
    public interface  IOSWINRepository:IRepository<OSWIN>
    {
        IList<OSWIN> GetOSWIN(string family);
        OSWIN GetOSWINByZmode(string family, string zmode);
        /// <summary>
        /// select a.*
        ///     from OSWIN a
        ///     inner join ModelBOM b on  b.Component like '%' +a.Zmode
        ///      where b.Material = @model 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        OSWIN GetOSWINByModelBOM(string model);
        IList<string> GetOSWINFamily();
    }
}

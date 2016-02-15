using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PCA.PCBVersion
{
    public interface IPCBVersionRepository : IRepository<PCBVersion>
    {
        IList<PCBVersion> GetPCBVersion(string family);
        IList<PCBVersion> GetPCBVersion(string family, string mbCode);

    }
}

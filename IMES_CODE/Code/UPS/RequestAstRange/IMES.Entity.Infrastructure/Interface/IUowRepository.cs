using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace IMES.Entity.Infrastructure.Interface
{
    public interface IUowRepository
    {
        void Save();
    }
}

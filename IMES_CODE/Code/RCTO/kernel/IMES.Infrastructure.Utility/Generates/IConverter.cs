using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates
{
    public interface IConverter
    {
        string Convert(object obj);
        bool IsHaveDefautValue();
        object GetDefautValue();
    }
}

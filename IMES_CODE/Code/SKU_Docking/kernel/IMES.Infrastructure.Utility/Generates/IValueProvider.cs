using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates
{
    public interface IValueProvider
    {
        object GetValue(string key);

        void AddValue(string key, object value);

        void RemoveValue(string key);
    }
}

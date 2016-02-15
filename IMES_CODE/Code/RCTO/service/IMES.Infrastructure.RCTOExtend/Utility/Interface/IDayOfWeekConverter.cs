using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.intf
{
    public abstract class IDayOfWeekConverter : IConverter
    {
        

        protected string _fieldKey = null;

        public IDayOfWeekConverter(string fieldKey)
        {
            this._fieldKey = fieldKey;
        }

        public abstract string Convert(object obj);

        public virtual bool IsHaveDefautValue()
        {
            return false;
        }

        public virtual object GetDefautValue()
        {
            return null;
        }
    }
}

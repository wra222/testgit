using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMES.Infrastructure.Utility.Generates.intf;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class YearConverterNormal : IYearConverter
    {
        public YearConverterNormal(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if(obj is IValueProvider)
            {
                DateTime dt = (DateTime)((IValueProvider)obj).GetValue(_fieldKey);
                string sYear = dt.Year.ToString();
                if (this._bit <= sYear.Length)
                    return sYear.Substring(sYear.Length - this._bit);
                else
                    return sYear.PadLeft(this._bit, this._padChar);
            }
            return null;
        }

    }
}

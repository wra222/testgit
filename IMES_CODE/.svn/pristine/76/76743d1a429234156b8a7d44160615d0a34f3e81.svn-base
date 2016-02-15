using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMES.Infrastructure.Utility.Generates.intf;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class DayConverterNormal : IDayConverter
    {
        public DayConverterNormal(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if(obj is IValueProvider)
            {
                DateTime dt = (DateTime)((IValueProvider)obj).GetValue(_fieldKey);
                string sDay = dt.Day.ToString();
                if (this._bit <= sDay.Length)
                    return sDay.Substring(sDay.Length - this._bit);
                else
                    return sDay.PadLeft(this._bit, this._padChar);
            }
            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class MonthConverter : intf.IMonthConverter
    {
        public MonthConverter(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if (obj != null)
            {
                if (obj is IValueProvider)
                {
                    DateTime dt = (DateTime)((IValueProvider)obj).GetValue(_fieldKey);

                    string str = dt.Month.ToString();

                    #region
                    //Month	Month Code
                    // 1	    01
                    // 2	    02
                    // 3	    03
                    // 4	    04
                    // 5	    05
                    // 6	    06
                    // 7	    07
                    // 8	    08
                    // 9	    09
                    //10        10
                    //11        11
                    //12        12
                    #endregion

                    if (this._bit <= str.Length)
                        return str.Substring(str.Length - this._bit);
                    else
                        return str.PadLeft(this._bit, this._padChar);
                }
            }
            return null;
        }
    }
}

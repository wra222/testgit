using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class MonthConverterOneBitCodeXYZ : intf.IMonthConverter
    {
        public MonthConverterOneBitCodeXYZ(string fieldKey)
            : base(1, '0', fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if (obj != null)
            {
                if (obj is IValueProvider)
                {
                    DateTime dt = (DateTime)((IValueProvider)obj).GetValue(_fieldKey);

                    #region
                    //Month	Month Code
                    // 1	    1
                    // 2	    2
                    // 3	    3
                    // 4	    4
                    // 5	    5
                    // 6	    6
                    // 7	    7
                    // 8	    8
                    // 9	    9
                    //10        X   88
                    //11        Y   89
                    //12        Z   90
                    #endregion

                    if (dt.Month < 10)
                        return dt.Month.ToString();
                    else
                        return System.Convert.ToChar(dt.Month + 78).ToString();
                }
            }
            return null;
        }
    }
}

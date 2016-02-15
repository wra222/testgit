using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class WeekConverter : intf.IWeekConverter
    {
        public WeekConverter(int bit, char padChar, string fieldKey,DayOfWeek firstDayOfWeek)
            : base(bit, padChar, fieldKey, firstDayOfWeek)
        {

        }

        public override string Convert(object obj)
        {
            if (obj != null)
            {
                if (obj is IValueProvider)
                {
                    DateTime dt = (DateTime)((IValueProvider)obj).GetValue(_fieldKey);
                    string str = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(dt, 
                                                                                                                                         CalendarWeekRule.FirstFullWeek,
                                                                                                                                           DayOfWeek.Monday).ToString();
                    #region
                    //Week	 Code
                    // 1	    01
                    // 2	    02
                    // 3	    03
                    // 4	    04
                    // 5	    05
                    // 6	    06
                    // 7	    07
                    // 8	    08
                    // 9	    09
                    //10      10
                    //11      11
                    //
                    //53      53
                    
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


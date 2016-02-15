using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class DayOfWeekConverter : intf.IDayOfWeekConverter
    {
        public DayOfWeekConverter(string fieldKey)
            : base(fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if (obj != null)
            { string strDay="";
                if (obj is IValueProvider)
                {
                    DateTime dt = (DateTime)((IValueProvider)obj).GetValue(_fieldKey);
                    switch (dt.DayOfWeek)
                    { 
                        case DayOfWeek.Monday:
                            strDay = "1";
                            break;
                        case DayOfWeek.Tuesday:
                            strDay = "2";
                            break;
                        case DayOfWeek.Wednesday:
                            strDay = "3";
                            break;
                        case DayOfWeek.Thursday:
                            strDay = "4";
                            break;
                        case DayOfWeek.Friday:
                            strDay = "5";
                            break;
                        case DayOfWeek.Saturday:
                            strDay = "6";
                            break;
                        case DayOfWeek.Sunday:
                            strDay = "7";
                            break;
                    }

                    return strDay;
                }
            }
            return null;
        }
    }
}


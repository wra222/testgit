using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class ModelTypeConverter : intf.IManufactoryModeConverter
    {
        public ModelTypeConverter(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if (obj is IValueProvider)
            {
               string str = (string)((IValueProvider)obj).GetValue(_fieldKey);
               
               if (this._bit <= str.Length)
                    return str.Substring(str.Length - this._bit);
               else
                   return str.PadLeft(this._bit, this._padChar);
            }
            return null;
        }
    }
}

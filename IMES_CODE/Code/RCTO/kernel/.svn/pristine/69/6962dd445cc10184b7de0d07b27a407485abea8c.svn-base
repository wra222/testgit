using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class ManufactoryModeConverter : intf.IManufactoryModeConverter
    {
        public ManufactoryModeConverter(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if(obj is IValueProvider)
            {
                //bool isExp = (bool)((IValueProvider)obj).GetValue(_fieldKey);

                string str = (string)((IValueProvider)obj).GetValue(_fieldKey); //isExp ? "P" : "M";

                //=P，试产
                //=M，量产

                if (this._bit <= str.Length)
                    return str.Substring(str.Length - this._bit);
                else
                    return str.PadLeft(this._bit, this._padChar);
            }
            return null;
        }
    }
}

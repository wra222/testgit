using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class SFGCustomizingSiteConverter : intf.ISFGCustomizingSiteConverter
    {
        public SFGCustomizingSiteConverter(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if(obj is IValueProvider)
            {
                //Blank for FG shipment(ModelInfo.Name='Type',Value='FG')

                //若非FG,
                //G : Model.Region = TRO 
                //H : Model.Region = TIH 
                //J : Model.Region = OME
                //U : Model.Region = TAIS

                string str = (string)((IValueProvider)obj).GetValue(_fieldKey);
                if (string.Empty == str)
                    return str;
                else if (this._bit <= str.Length)
                    return str.Substring(0, this._bit);
                else
                    return str.PadLeft(this._bit, this._padChar);
            }
            return null;
        }
    }
}

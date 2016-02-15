using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class ProductionSiteConverter : intf.IProductionSiteConverter
    {
        public ProductionSiteConverter(int bit, char padChar, string fieldKey)
            : base(bit, padChar, fieldKey)
        {

        }

        public override string Convert(object obj)
        {
            if(obj is IValueProvider)
            {
                //TSB目前写死为Q

                //initial Manufacturing Site (1st touch)
                //B = STI 
                //C = Compal (Taiwan)
                //G = TRO 
                //H = TIH 
                //J  = Ome
                //K = Compal (Kunshan) 
                //L = PDA - (PDA/Taiwan)
                //M = TCSS 
                //N = DT-PC (TCOT/JAPAN)
                //P = TIP
                //Q = Inventec (Pudong)
                //R = Asus (Pudong) &Pegatron(Pudong)
                //S = Inventec (Shanghai) 
                //T = Quanta (Taiwan)
                //U = TAIS
                //V = Inventec (Taiwan)
                //W = Quanta (Songjiang)

                string str = (string)((IValueProvider)obj).GetValue(_fieldKey);
                if (this._bit <= str.Length)
                    return str.Substring(0, this._bit);
                else
                    return str.PadLeft(this._bit, this._padChar);
            }
            return null;
        }
    }
}

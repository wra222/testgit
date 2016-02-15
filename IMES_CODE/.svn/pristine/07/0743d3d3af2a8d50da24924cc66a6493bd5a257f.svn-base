using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.intf
{
    public abstract class ILineConverter : IConverter
    {
        protected int _bit = 1;

        protected char _padChar = '0';

        protected string _fieldKey = null;

        public ILineConverter(int bit, char padChar, string fieldKey)
        {
            if (bit < 1)
                throw new FisException("GEN024", new string[] { });
            this._bit = bit;
            this._padChar = padChar;
            this._fieldKey = fieldKey;
        }

        public abstract string Convert(object obj);

        public virtual bool IsHaveDefautValue()
        {
            return false;
        }

        public virtual object GetDefautValue()
        {
            return null;
        }
    }
}

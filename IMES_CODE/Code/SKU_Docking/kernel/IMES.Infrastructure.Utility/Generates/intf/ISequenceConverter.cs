using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.intf
{
    public abstract class ISequenceConverter : IConverter
    {
        protected int _bit = 1;

        protected char _padChar = '0';

        /// <summary>
        /// 進位規則
        /// </summary>
        /// <remarks></remarks>
        protected MathSequenceWithCarryNumberRule _mathSequenceWithCarryNumberRule = null;

        public MathSequenceWithCarryNumberRule NumberRule
        {
            get
            {
                return this._mathSequenceWithCarryNumberRule;
            }
        }

        public ISequenceConverter(string charCollection, int bit, string maxNumber, string minNumber, char padChar)
        {
            this._bit = bit;
            this._padChar = padChar;
            this._mathSequenceWithCarryNumberRule = new MathSequenceWithCarryNumberRule(bit, charCollection, maxNumber, minNumber);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates.impl
{
    public class SequenceConverterNormal : intf.ISequenceConverter
    {
        public SequenceConverterNormal(string charCollection, int bit, string maxNumber, string minNumber, char padChar)
            : base(charCollection, bit, maxNumber, minNumber, padChar)
        {
        }

        public override string Convert(object obj)
        {
            if (obj != null && obj is string)
                return (string)obj;
            return null;
        }
    }
}

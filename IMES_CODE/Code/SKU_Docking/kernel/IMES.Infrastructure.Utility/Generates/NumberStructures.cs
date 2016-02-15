using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates
{
    public class ContinuousNumbers
    {
        public string NumberBegin = string.Empty;
        public string NumberEnd = string.Empty;
    }
    public class SequencialNumberSegment : ContinuousNumbers
    {
        public int iStep = 1;
    }

    public class SequencialNumberRanges
    {
        public SequencialNumberSegment[] Ranges = null;
    }
}

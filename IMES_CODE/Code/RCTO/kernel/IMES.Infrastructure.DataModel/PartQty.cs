using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class PartQty
    {
        public string Pn;
        public string Description;
        public int Qty;
        public string Line;

        /// <summary>
        /// key
        /// </summary>
        public string Key
        {
            get { return Pn + "|" + Line; }
        }
    }
}

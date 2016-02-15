using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ReworkInfoMaintain
    {
        private string reworkCode;

        public string ReworkCode
        {
            get { return reworkCode; }
            set { reworkCode = value; }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string editor;

        public string Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        private string cdt;

        public string Cdt
        {
            get { return cdt; }
            set { cdt = value; }
        }

        private string udt;

        public string Udt
        {
            get { return udt; }
            set { udt = value; }
        }
    }
}

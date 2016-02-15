using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class PartCheckMatchRuleDef
    {
        public	string	ID;
        public	string	PartCheckID;
        public	string	RegExp;
        public	string	PnExp;
        public	string	PartPropertyExp;
        public	string	ContainCheckBit;
        public  string  Editor;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class DefectCodeNextStationInfo
    {
        private string _NXT_STN = "";
       
        int ID { get; set; }
        public string NXT_STN
        {
            get { return _NXT_STN; }
            set { _NXT_STN = value.Trim(); } 
        }
        public string MajorPart {get;set;}
        public string Cause {get;set;}
        public string Defect {get;set;}
        public int Priority { get; set; }
        public int FamilyPriority { get; set; }
    }
}

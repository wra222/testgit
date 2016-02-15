using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public class PartTypeAttributeAndPartInfoValue
    {
        public int Id { get; set; }  //PartInfo.ID或AssemblyCodeInfo.ID

        public string MainTblKey { get; set; }  //PartInfo.PartNo或AssemblyCodeInfo.AssemblyCode

        public string Item { get; set; }     //PartTypeAttribute.Code
        public string Content { get; set; }  //PartInfo.InfoValue或AssemblyCodeInfo.InfoValue
        public string Description { get; set; }//PartTypeAttribute.Description
        public string Editor { get; set; }  //PartInfo.Editor或AssemblyCodeInfo.Editor
        public DateTime Cdt { get; set; }   //PartInfo.Cdt或AssemblyCodeInfo.Cdt
        public DateTime Udt { get; set; }   //PartInfo.Udt或AssemblyCodeInfo.Udt
    }
}

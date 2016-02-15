using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    public struct XRay
    {
        public string PdLine;
        public string Model;
        public string PCBNo;
        public string Location;
        public string Obligation;
        public string IsPass;
        public string Remark;
        public string Editor;
        public DateTime Cdt;

    }
    public interface ISAInputXRay
    {
        
        /// <summary>
        /// 
        /// </summary>

        void Save(string input, string pdline, string model,string location,string obligation,string remark,string state,string customer,string editor);

        IList<XRay> GetMaterialByTypeList(string type);
                
    }

}
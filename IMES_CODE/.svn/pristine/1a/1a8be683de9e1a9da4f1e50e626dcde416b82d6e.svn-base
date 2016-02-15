using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;



namespace IMES.Maintain.Interface.MaintainIntf
{
    [Serializable]
    public class OSWINInfo
    {
        public int ID;
        public string Family;
        public string Zmode;
        public string OS;
        public string AV;
        public string Image;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

    public interface IOSWIN
    {
        IList<string> GetOSWINFamily();

        IList<string> GetFamilyObjList();

        IList<OSWINInfo> GetOSWINList(string Family);

        OSWINInfo CheckExistOSWIN(string Family, string Zmod);

        void Remove(OSWINInfo item);

        void Update(OSWINInfo item);

        void Add(OSWINInfo item);

    }
}

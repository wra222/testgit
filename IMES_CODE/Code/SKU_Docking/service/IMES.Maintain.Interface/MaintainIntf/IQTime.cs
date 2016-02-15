using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    [Serializable]
    public class QTimeinfo
    {
        public string Line;
        public string Station;
        public string Family;
        public string Catagory;
        public int TimeOut;
        public int StopTime;
        public string DefectCode;
        public string HoldStation;
        public string HoldStatus;
        public string ExceptStation;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }

    public interface IQTime
    {
        IList<QTimeinfo> GetQTimeList(string line);

        QTimeinfo CheckExistDefectCodeList(string Line ,string Station , string Family);

        IList<string> GetAliasLineList();

        DataTable GetHoldStationList();

        IList<ConstValueTypeInfo> GetStationList();
        
        IList<DefectCodeInfo> GetDefectCodeList();

        bool CheckFamily(string InputType);

        void Add(QTimeinfo item);

        void Update(QTimeinfo item);

        void Delete(string Line, string Station, string Family);
        
    }
}

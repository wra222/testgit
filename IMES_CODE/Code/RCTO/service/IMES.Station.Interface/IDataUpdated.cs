using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface
{

    interface IDataUpdated
    {
        void DataUpdated(UpdatedItem item);
    }


    public class UpdatedItem
    {
        public ItemType type;
        public string item;
    }

    public enum ItemType
    {
        Part,       //Part, PartInfo
        Model,      //Model, ModelInfo
        ModelBom,   //ModelBOM
        Mo,
        MoBom,      //MoBom
        DefectCode,   //DefectCode
        DefectInfo,  //DefectInfo
        PartCheckSetting, //PartCheckSetting
        CheckItem,  //CheckItem
        Station,    //Station
        Line, //Line, Line_Station
        Process,    //Process,Process_Station
    }

}

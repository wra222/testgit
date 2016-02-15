using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BizRule.Config
{
    # region define message filed index
    static class ParseMsgRule
    {
        static public char Delimiter = GetDelimiter().ToCharArray()[0];
        public static string GetDelimiter()
        {
            string s = ConfigurationManager.AppSettings["Delimiter"];
            return string.IsNullOrEmpty(s) ? "," : s;
        }
    }
    static class QueryBoxInfoIndex
    {
        static public int MsgArrayLength = 6;
        static public Dictionary<int, string> SetIndex()
      {

          Dictionary<int, string> SqlNameIndx = new Dictionary<int, string>();
          SqlNameIndx.Add(0, "PCBNo");
          SqlNameIndx.Add(1, "IsPass");
          SqlNameIndx.Add(2, "TestCase");
          SqlNameIndx.Add(3, "Descr");
          SqlNameIndx.Add(4, "ErrorCode");
          SqlNameIndx.Add(5, "TestTime");
          SqlNameIndx.Add(6, "ProductID");
          SqlNameIndx.Add(7, "SerialNumber");
          SqlNameIndx.Add(8, "DateManufactured");
          SqlNameIndx.Add(9, "PublicKey");
          SqlNameIndx.Add(10, "MACAddress");
          SqlNameIndx.Add(11, "IMEI");
          SqlNameIndx.Add(12, "IMSI");
          SqlNameIndx.Add(13, "PrivateKey");
          SqlNameIndx.Add(14, "EventType");
          SqlNameIndx.Add(15, "DeviceAttribute");
          SqlNameIndx.Add(16, "Platform");
          SqlNameIndx.Add(17, "ICCID");
          SqlNameIndx.Add(18, "Model");
          SqlNameIndx.Add(19, "EAN");
          SqlNameIndx.Add(20, "Backlight");
          SqlNameIndx.Add(21, "BatteryType");
          SqlNameIndx.Add(22, "OperationMode");
          SqlNameIndx.Add(23, "Calibration");

          // select top 1 IsPass,TestCase,Descr,ErrorCode,TestTime,t.ProductID,SerialNumber,DateManufactured,PublicKey
          //,MACAddress,t.IMEI,IMSI,PrivateKey,EventType,DeviceAttribute,Platform,t.ICCID as ICCID,p.Model,EAN
          return SqlNameIndx;
      }
    
    }

    static class HeaderIndex
    {
        static public int MsgNameIndex = 0;
        static public int ToolIndex = 1;
        static public int UserIndex = 2;
        static public int MBSNIndex = 3;
        static public int WCIndex = 4;
        static public int LineIndex = 5;
        static public int TimeStampIndex = 6;

    
        
    }

    static class CheckWCIndex
    {
        static public int MsgArrayLength =7;

        static public int MsgNameIndex = 0;
        static public int ToolIndex = 1;
        static public int UserIndex = 2;
        static public int MBSNIndex = 3;
        static public int WCIndex = 4;
        static public int LineIndex = 5;
        static public int TimeStampIndex = 6;
    }

    static class GetLabelLocationIndex
    {
        static public int MsgArrayLength = 7;

        static public int MsgNameIndex = 0;
        static public int ToolIndex = 1;
        static public int UserIndex = 2;
        static public int SNIndex = 3;
        static public int WCIndex = 4;
        static public int LineIndex = 5;
        static public int TimeStampIndex = 6;
    }


    static class DataCollectionIndex
    {
        static public int MsgArrayLength = 30;

        static public int MsgNameIndex = 0;
        static public int ToolIndex = 1;
        static public int UserIndex = 2;
        static public int MBSNIndex = 3;
        static public int WCIndex = 4;
        static public int LineIndex = 5;
        static public int TimeStampIndex = 6;
        static public int IsPassIndex = 7;
        static public int TestItemIndex =8;
        static public int DescriptionIndex = 9;
        static public int ErrorCodeIndex = 10;
        static public int TestTimeIndex = 11;
        static public int PIDIndex = 12;
        static public int SerialNumberIndex = 13;
        static public int ManufacturedDateIndex = 14;
        static public int PublicKeyIndex = 15;
        static public int MacIndex = 16;
        static public int IMEIIndex = 17;
        static public int IMSIIndex = 18;
        static public int PrivateKeyIndex = 19;
        static public int EventTypeIndex = 20;
        static public int DeviceAttributeIndex = 21;
        static public int PlatformIndex = 22;
        static public int ICCIDIndex = 23;
        static public int ModelNumberIndex = 24;
        static public int EANIndex = 25;
        static public int Backlight = 26;
        static public int BatteryType = 27;
        static public int OperationMode = 28;
        static public int Calibration = 29;
    
    }

    static class TestLogIndex
    {
        static public int LogPathIndex = 6;
        static public int TimeStampIndex = 7;
    }

    static class TestCompletedIndex
    {
        static public int MsgArrayLength = 10;

        static public int IsPassIndex = 7;
        static public int FailureCodeIndex = 8;
        static public int TestFileNameIndex = 9;

    }
    static class ProcessCompletedIndex
    {
        static public int MsgArrayLength =9;

        static public int IsPassIndex = 7;
        static public int FailureCodeIndex = 8;      

    }

    static class QueryMBInfoIndex
    {
        static public int MsgArrayLength = 6;
        static public int TimeStampIndex =5;
    }


    #endregion
}

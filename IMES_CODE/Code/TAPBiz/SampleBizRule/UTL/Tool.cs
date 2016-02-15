using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizRule.Msg;
using BizRule.Config;
namespace UTL
{
     public enum StationType{ SA, FA, NODEFINE };
     public enum MsgType { CheckWC, 
                                        DataCollectionIndex, 
                                        TestCompletedIndex, 
                                        QueryBoxInfoIndex, 
                                        QueryMBInfoIndex,
                                        Get_LabelLocation,
                                        Process_Completed};
     public class Tool
    {
         public static bool IsCorrectMsg(MsgType msg,string[] msgContent, out string msgErr,int IsPassIndex)
         {
             msgErr = "";
             int correctLen = GetMsgLength(msg);
             if (correctLen != msgContent.Length)
             {
                 msgErr = "The length or message array : " + msgContent.Length.ToString() + " is wrong, the correct length is : " + correctLen.ToString();
                 return true;
             }
             if (IsPassIndex != 0)
             {
                 string isPass = msgContent[IsPassIndex].Trim();
                 if (isPass != "0" && isPass != "-1")
                 {
                     msgErr = "The IsPass data is wrong, it should be 0 or -1";
                     return true;
                 }
             }
                return false;
          
         }

         private static int GetMsgLength(MsgType msg)
         {    int result=0;
              switch (msg)
              {
                  case MsgType.CheckWC:
                      result=CheckWCIndex.MsgArrayLength;
                      break;
                  case MsgType.DataCollectionIndex:
                      result=DataCollectionIndex.MsgArrayLength;
                      break;
                  case MsgType.TestCompletedIndex:
                      result=TestCompletedIndex.MsgArrayLength;
                      break;
                  case MsgType.QueryBoxInfoIndex:
                      result = QueryBoxInfoIndex.MsgArrayLength;
                      break;
                  case MsgType.QueryMBInfoIndex:
                      result = QueryMBInfoIndex.MsgArrayLength;
                      break;
                  case MsgType.Get_LabelLocation:
                      result = GetLabelLocationIndex.MsgArrayLength;
                      break;
                  case MsgType.Process_Completed:
                      result = ProcessCompletedIndex.MsgArrayLength;
                      break;  
              
              }
               return result;
         }

         public static StationType GetStation(string FAlst,string SAlst,string wc)
         {
              if (SAlst.IndexOf(wc) >= 0)
             { return StationType.SA; }
             if (FAlst.IndexOf(wc) >= 0)
             { return StationType.FA; }
             return StationType.NODEFINE;
         }
      
     }

}

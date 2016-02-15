/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: interface for ECR Version
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-04-27 Tong.Zhi-Yong         Create 
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ISmallBoardECR
    {
        IList<string> GetFamilyList(string customer);

        IList<MBType> GetMBTypeList();
        
        IList<SmallBoardECRInfo> GetSmallBoardECRInfo(string family);

        void SaveSmallBoardECRInfo(SmallBoardECRInfo info);

        void DeleteSmallBoardECRInfo(SmallBoardECRInfo info);
    }
    
    [Serializable]
    public class MBType
    {
        public string Descr;
        public string Value;
    }
}

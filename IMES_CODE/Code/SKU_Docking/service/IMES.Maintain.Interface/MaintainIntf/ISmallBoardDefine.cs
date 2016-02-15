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
    public interface ISmallBoardDefine
    {
        IList<string> GetFamilyList(string customer);

        IList<MBType> GetMBTypeList();

        IList<string> GetPartNoList();

        IList<SmallBoardDefineInfo> GetSmallBoardDefineInfo(string family);

        void SaveSmallBoardDefineInfo(SmallBoardDefineInfo info);

        void AddSmallBoardDefineInfo(SmallBoardDefineInfo info);

        void DeleteSmallBoardDefineInfo(SmallBoardDefineInfo info);
    }
}

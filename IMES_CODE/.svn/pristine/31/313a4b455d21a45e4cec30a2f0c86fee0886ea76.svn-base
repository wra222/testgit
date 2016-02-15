/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: FamilyInfo Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 
 * * issue:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IFamilyInfoEx : IFamilyInfo, IFamilyInfoNameEx
    {

        IList<FamilyInfoDef> GetFamilyInfoList(string strFamilyName);

        FamilyInfoDef GetFamilyInfo(string strFamilyInfoId);

        void DeleteFamilyInfo(FamilyInfoDef model);
    }
}

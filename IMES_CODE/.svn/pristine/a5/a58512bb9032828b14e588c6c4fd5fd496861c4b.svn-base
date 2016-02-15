/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-2-20   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ITPCBMaintain
    {
        IList<TPCBInfo> GetTpcbList();

        IList<TPCBInfo> GetTpcbList(string family);

        void SaveTPCBInfo(TPCBInfo item);

        int GetID(string faimly, string partNo);

        void DeleteTPCBInfo(string family, string partNo);

        IList<TPCBInfo> CheckHasList(string family, string partNo);

        IList<TPCBInfo> CheckSameList(TPCBInfo item);

        void UpdateTPCBInfo(TPCBInfo item);

        void InsertTPCBInfo(TPCBInfo item);

        int CheckExistsRecord(string family, string partNo);
    }
}

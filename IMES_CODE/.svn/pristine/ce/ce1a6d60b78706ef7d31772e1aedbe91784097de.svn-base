/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-6-23   itc210001        create
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
    public interface IFAStation
    {
        IList<string> GetLineList();

        IList<FaStationInfo> GetFaStationInfoList();

        IList<FaStationInfo> GetFaStationInfoList(string line);

        int CheckExistsRecord(string line, string station);

        int GetID(string line, string station);

        void UpdateFaStation(FaStationInfo item);

        void InsertFaStation(FaStationInfo item);

        void DeleteFaStationInfo(string line, string station);
    }
}

using System;
using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{

    public interface ICheckItemType
    {
        /// <summary>
        /// GetCheckItemTypeByCondition
        /// </summary>
        /// <returns></returns>
        IList<CheckItemTypeInfo> GetCheckItemTypeByCondition(CheckItemTypeInfo condition);

        /// <summary>
        /// AddCheckItemTypeInfo
        /// </summary>
        /// <param name="item"></param>
        void AddCheckItemTypeInfo(CheckItemTypeInfo item);

        /// <summary>
        /// UpdateCheckItemTypeInfo
        /// </summary>
        /// <param name="item"></param>
        void UpdateCheckItemTypeInfo(CheckItemTypeInfo item);

        /// <summary>
        /// DeleteCheckItemTypeInfo
        /// </summary>
        /// <param name="id"></param>
        void DeleteCheckItemTypeInfo(string name);
    }
}

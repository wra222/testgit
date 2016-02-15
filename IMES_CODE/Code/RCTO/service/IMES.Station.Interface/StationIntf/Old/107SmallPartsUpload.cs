/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Small Parts Upload Interface
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-05-07   LuycLiu     Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface ISmallPartsUpload
    {
        /// <summary>
        /// 查询smallparts upload信息
        /// </summary>
        /// <returns>smallparts upload信息列表</returns>
        IList<SMALLPartsUploadInfo> Query();

        /// <summary>
        /// 保存至数据库
        /// </summary>
        /// <param name="list">smallparts upload信息列表</param>
        void Save(IList<SMALLPartsUploadInfo> list);

    }
}



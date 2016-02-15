/*
 * INVENTEC corporation (c)2010 all rights reserved. 

 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.PCA.EcrVersion;
namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IFRUMBVersion
    {
        /// <summary>
        /// 獲得FRUMBVersion下拉列表數據
        /// </summary>
        /// <returns>list of FRUMBVersion</returns>
        IList<FruMBVerInfo> GetFruMBVer(string partNo);

        /// <summary>
        /// 獲得FRUMBVersion PartNo下拉列表數據
        /// </summary>
        /// <returns>list of FRUMBVersion PartNo</returns>

        IList<string> GetPartNoInFruMBVer();


        /// <summary>
        /// 新增FRUMBVersion數據
        /// </summary>
        /// <param name="info">EcrVersionInfo</param>

        void InsertFruMBVer(FruMBVerInfo item);


        /// <summary>
        /// 保存FRUMBVersion數據
        /// </summary>
        /// <param name="info">EcrVersionInfo</param>
        void UpdateFruMBVer(FruMBVerInfo item);

        /// <summary>
        /// 刪除FRUMBVersion數據
        /// </summary>
        /// <param name="info"></param>

        void RemoveFruMBVer(int id);

       
     
    }
}

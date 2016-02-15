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
    public interface IECRVersion
    {
        /// <summary>
        /// 獲得Family下拉列表數據
        /// </summary>
        /// <returns>list of FamilyInfo</returns>
        IList<FamilyInfo> GetFamilyInfoListForECRVersion();


        IList<FamilyInfo> GetFamilyInfoListForSA();

        /// <summary>
        /// 根據Family獲得ECRVersion相應數據
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<EcrVersionInfo> GetECRVersionInfoListByFamily(string family);

        /// <summary>
        /// 保存ECRVersion數據
        /// </summary>
        /// <param name="info">EcrVersionInfo</param>
        void SaveECRVersion(EcrVersionInfo info);

        /// <summary>
        /// 更新ECRVersion數據
        /// </summary>
        /// <param name="info"></param>
        void UpdateECRVersion(EcrVersionInfo info);

        /// <summary>
        /// 刪除ECRVersion數據
        /// </summary>
        /// <param name="info"></param>
        void DeleteECRVersion(EcrVersionInfo info);
    }
}

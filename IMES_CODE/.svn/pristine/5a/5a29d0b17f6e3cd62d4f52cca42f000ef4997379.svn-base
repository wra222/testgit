// created by itc207024 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPalletWeight
    {
        /// <summary>
        /// 获得PalletWeightMaintain列表
        /// </summary>
        /// <returns></returns>
        IList<PalletWeightMaintain> GetPalletWeightToleranceList(string family);

        /// <summary>
        /// 添加新纪录
        /// </summary>
        /// <returns></returns>
        void AddPalletWeight(PalletWeightMaintain palletWeight);

        /// <summary>
        /// 更新纪录
        /// </summary>
        /// <returns></returns>
        void UpdatePalletWeight(PalletWeightMaintain palletWeight);

        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <returns></returns>
        void DeletePalletWeight(int id);

        /// <summary>
        /// 判断Region是否已经存在
        /// </summary>
        /// <returns></returns>
        bool IFPalletWeightIsExists(string region, string family);
    }
}

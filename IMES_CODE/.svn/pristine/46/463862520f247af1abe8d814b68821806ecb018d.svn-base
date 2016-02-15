using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    [Serializable]
    public class PCBVersionInfo
    {
        public string Family;
        public string MBCode;
        public string PCBVer;
        public string CTVer;
        public string Supplier;
        public string Remark;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }
    public interface IPCBVersion
    {
        /// <summary>
        /// 根據Family獲得PCBVersion相應數據
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<PCBVersionInfo> GetPCBVersionInfoListByFamily(string family);

        /// <summary>
        /// 保存PCBVersion數據
        /// </summary>
        /// <param name="info">EcrVersionInfo</param>
        void SavePCBVersion(PCBVersionInfo info);

        /// <summary>
        /// 刪除PCBVersion數據
        /// </summary>
        /// <param name="info"></param>
        void DeletePCBVersion(PCBVersionInfo info);
    }
}

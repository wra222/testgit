using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;
using System.Data;
using System;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// PCAReceiveReturnMBInfo
    /// </summary>
    [Serializable]
    public class PCAReceiveReturnMBInfo
    {
        /// <summary>
        /// MBSN
        /// </summary>
        public string MBSN { get; set; }
        /// <summary>
        /// Line
        /// </summary>
        public string Line { get; set; }
        /// <summary>
        /// Defect
        /// </summary>
        public string Defcet { get; set; }
        /// <summary>
        /// DefcetDescr
        /// </summary>
        public string DefcetDescr { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// Editor
        /// </summary>
        public string Editor { get; set; }
        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt { get; set; }
    }

    /// <summary>
    /// MB信息获取与保存
    /// </summary>
    public interface IPCAReceiveReturnMB
    {
        /// <summary>
        /// 输入MBSno，卡站
        /// </summary>
        /// <param name="input">MBSno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        IList<PCAReceiveReturnMBInfo> InputMBSno(string input, string line, string editor, string station, string customer);

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="mb">MBSno</param>
        string Save(string mb,string IsBU,string Rework,string MBLookLike);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="mb">MBSno</param>
        void Cancel(string mb);  
    }
}

// INVENTEC corporation (c)2010 all rights reserved. 
// Description: CombineKPCT interface
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-06   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 记录客户CT号
    /// 目的：建立IEC CT和客户CT号码的对应关系，实现厂内规则统一
    /// </summary>
    public interface ICombineKPCT
    {
        /// <summary>
        /// 根据选择的KP type和线别得到已结合总数，进行显示
        /// (只有Customer是TSB时才需要调用本方法)
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="KPType"></param>
        /// <returns>已结合总数</returns>
        int GetCombinedQty(
            string pdLine,
            string KPType);

        /// <summary>
        /// 启动工作流，检查CT SN的合法性,检查通过后KPType不可更改
        /// 如果当前选择的KPType是Battery，通过后输入BatteryPN,调用InputBatteryPN
        /// 如果当前选择的KPType不是Battery，通过后输入Vendor CT SN ,调用InputVendorCTSN
        /// </summary>
        /// <param name="IECCTSN"></param>
        /// <param name="KPType"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="pdLine"></param>
        /// <param name="customer"></param>
        void InputIECCTSN(
            string IECCTSN,
            string KPType,
            string editor, string station, string pdLine,
            string customer,string pcode);


        /// <summary>
        /// 如果选择的KPType是Battery，调用该方法检查输入的BatteryPN是否和CTSN的PN相同
        /// 通过后调用InputVendorCTSN
        /// 未通过可再刷BatteryPN，再调用本方法
        /// </summary>
        /// <param name="IECCTSN"></param>
        /// <param name="batteryPN"></param>
        void InputBatteryPN(
            string IECCTSN,
            string batteryPN);

        /// <summary>
        /// 输入VendorSN，检查合法性，检查通过后更新PartSN表
        /// 未通过可再刷vendorCTSN，再调用本方法
        /// </summary>
        /// <param name="IECCTSN"></param>
        /// <param name="vendorCTSN"></param>
        void InputVendorCTSN(
            string IECCTSN,
            string vendorCTSN);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="IECCTSN"></param>
        void Cancel(string IECCTSN);
    }
}

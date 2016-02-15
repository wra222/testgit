// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-11-15   200038                       Create
// Known issues:
//

using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;

namespace IMES.CheckItemModule.BTCB.Filter
{
    [Export(typeof (ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.BTCB.Filter.dll")]
    internal class CheckModule : ICheckModule
    {
        private const String ServiceDoorTypeName = "ServiceDoor";
        private const String HDDDoorTypeName = "HDDDoor";
        private const String BTCBTypeName = "BTCB";
        /// <summary>
        /// 检查当前内存中是否已经刷入BTCB/ServiceDoor/HDDDoor料号，获取当前刷入料VC对应PartNo的Vendor属性，并做大写处理。
        /// 若Vendor属性不存在，则不做判断；若获取的Vendor属性值（不包括不存在的Vendor属性）的数量，大于2，则报错：“供应商不一致，该Part不能刷入”，取消该Part的刷入，并等待刷入下一个PartSn
        /// </summary>
        /// <param name="partUnit"></param>
        /// <param name="bomItem"></param>
        /// <param name="station"></param>
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit == null)
            {
                throw new FisException("CHK174", new[] { "IMES.CheckItemModule.BTCB.Filter.CheckModule.Check" });
            }

            String currentVendorCode = GetVendorCodeOfCheckedPart(((PartUnit)partUnit).Pn, (IFlatBOMItem)bomItem);
            if (string.IsNullOrEmpty(currentVendorCode))
            {
                return;
            }
            currentVendorCode = currentVendorCode.ToUpper();

            Session session = SessionManager.GetInstance.GetSession(((PartUnit)partUnit).ProductId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            if (string.IsNullOrEmpty(station))
            {
                throw new FisException("CHK174", new[] { "IMES.CheckItemModule.BTCB.Filter.CheckModule.Check" });
            }


            var bom = (IFlatBOM)session.GetValue(Session.SessionKeys.SessionBom);
            if (bom != null)
            {
                foreach (IFlatBOMItem item in bom.BomItems)
                {
                    if (string.Compare(ServiceDoorTypeName, item.CheckItemType) == 0
                        || string.Compare(HDDDoorTypeName, item.CheckItemType) == 0
                        || string.Compare(BTCBTypeName, item.CheckItemType) == 0)
                    {
                        foreach (PartUnit checkedPart in item.CheckedPart)
                        {
                            String checkedPartVendorCode = GetVendorCodeOfCheckedPart(checkedPart.Pn, item);
                            if (!string.IsNullOrEmpty(checkedPartVendorCode)
                                && (string.Compare(currentVendorCode, checkedPartVendorCode.ToUpper()) != 0))
                            {
                                throw new FisException("CHK965", new string[]{});
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据Pn获取FlatBOMItem中该Part的VendorCode
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        String GetVendorCodeOfCheckedPart(String pn, IFlatBOMItem item)
        {
            if (item.AlterParts != null)
            {
                foreach (IPart alterPart in item.AlterParts)
                {
                    if (string.Compare(alterPart.PN, pn) == 0)
                    {
                        return alterPart.GetAttribute("Vendor");
                    }
                }
            }
            return string.Empty;
        }
    }
}

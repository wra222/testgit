// INVENTEC corporation (c)2009 all rights reserved. 
// Description: VBStrategy
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-04   YangWeihua                 create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.BOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PartStrategy.CommonStrategy;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PartStrategy.VBStrategy
{
    /// <summary>
    /// VB类型Part的Match,Check,Bind策略的实现类
    /// </summary>
    public class VBStrategy : AbstractStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public VBStrategy(IList<PartCheck> checkSetting) : base(checkSetting)
        {
            
        }
        
        /// <summary>
        /// 1. 10码(去掉校验位)
        /// 2. VB SN#前两码=Part的VB属性
        /// </summary>
        /// <param name="target">目标串</param>
        /// <param name="part">part</param>
        /// <returns>是否匹配</returns>
//        public override bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit)
//        {
//
//            containCheckBit = false;
//
            //at Board Input station, VB111 pn is requried
//            if (station.CompareTo("40") == 0)
//            {
//                return  part.PN.Equals(target);
//            }
//
            //match by length
//            switch (target.Length)
//            {
//                case 11:
//                    containCheckBit = true;
//                    break;
//                case 10:
//                    containCheckBit = false;
//                    break;
//                default:
//                    return false;
//            }
//
//            string vbCode = ((IPart) part).GetAttribute("VB");
//            string svbCode = ((IPart)part).GetAttribute("SVB");
//            if ((target.Substring(0, 2) != vbCode) && (target.Substring(0, 2) != svbCode))
//            {
//                return false;
//            }
//
//            return true;
//        }

        /// <summary>
        /// VB类型使用默认方法进行绑定
        /// </summary>
        /// <param name="part"></param>
        /// <param name="owner"></param>
//        public override void BindTo(IProductPart part, IPartOwner owner)
//        {
//            owner.AddPart(part);
//        }

        /// <summary>
        /// VGASN 是否重复(是否已与其它MB/Product对象绑定)
        /// </summary>
// 注：增加ValueType后, PN，SN在不同站收集是通过PartCheckSetting数据设定，可以按通用规则处理
//        public override void Check(IBOMPart part, IPartOwner owner, string station)
//        {
            //at Board Input station, VB111 pn does not need any check
//            if (station.CompareTo("40") == 0)
//            {
//                return;
//            }
//            owner.PartUniqueCheck(part.PN, part.MatchedSn);
//        }
    }
}

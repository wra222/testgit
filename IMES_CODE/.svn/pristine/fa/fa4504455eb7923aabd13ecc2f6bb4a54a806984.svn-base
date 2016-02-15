// INVENTEC corporation (c)2009 all rights reserved. 
// Description: KPStrategy
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
using System.Threading;
using IMES.FisObject.Common.BOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PartSn;
using IMES.FisObject.PartStrategy.CommonStrategy;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PartStrategy.KPStrategy
{
    /// <summary>
    /// KP类型Part的Match,Check,Bind策略的实现类
    /// </summary>
    public class KPStrategy : AbstractStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public KPStrategy(IList<PartCheck> checkSetting) : base(checkSetting)
        {
            //if (checkSetting == null) throw new ArgumentNullException("checkSetting");
        }

        /// <summary>
        /// 使用PartCheckMatch中定义的规则match
        /// </summary>
        /// <param name="target">需要匹配的字符串</param>
        /// <param name="part">part</param>
        /// <param name="containCheckBit">是否包含校验位</param>
        /// <returns></returns>
        public override bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit)
        {
            containCheckBit = false;

            if (!valueType.Equals(("SN")))
            {
                return base.Match(target, part, station, valueType, out containCheckBit);
            }

            //match by length
            if (target.Length != 14)
            {
                    return false;
            }

            //match by assembly code
            bool assMatched = false;
            string prefix = target.Substring(0, 6);
            foreach (var assCode in part.AssemblyCode)
            {
                if (prefix.Equals(assCode))
                {
                    assMatched = true;
                }
            }

            //确定是否带校验位
            if (assMatched)
            {
                IPartSnRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
                PartSn psn = rep.Find(target);
                if (psn != null)
                {
                    //sn of new format: sn lenth = 14; no checkbit;
                    containCheckBit = false;
                    return true;
                }
                else
                {
                    psn = rep.Find(target.Substring(0, 13));
                    if (psn != null)
                    {
                        //sn of old format: sn lenth=13; with checkbit on 14th;
                        containCheckBit = true;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 唯一性检查:只能与一个ProId绑定
        /// </summary>
        /// <param name="part"></param>
        /// <param name="owner"></param>
        public override void Check(IBOMPart part, IPartOwner owner, string station)
        {
            //1.在PartSN存在
//            IPartSnRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
//            PartSn psn = rep.Find(part.MatchedSn);
//            if (psn == null)
//            {
//                List<string> erpara = new List<string>();
//                erpara.Add(part.MatchedSn);
//                var ex = new FisException("CHK047");
//                throw ex;
//            }

            //2.唯一性检查:只能与一个ProId绑定
            base.Check(part, owner, station);
        }

    }
}

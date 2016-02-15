// INVENTEC corporation (c)2009 all rights reserved. 
// Description: AbstractStrategy
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

namespace IMES.FisObject.PartStrategy.CommonStrategy
{
    /// <summary>
    /// 各种类型Part的Match,Check,Bind策略的公共实现部分
    /// </summary>
    public abstract class AbstractStrategy : IPartStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected AbstractStrategy(IList<PartCheck> checkSetting)
        {
            _checkSetting = checkSetting;
        }

        private IList<PartCheck> _checkSetting;

        /// <summary>
        /// 将part绑定到Product/MB的默认实现，即直接绑定至owner
        /// </summary>
        /// <param name="part">part</param>
        /// <param name="owner">owner</param>
        public virtual void BindTo(IProductPart part, IPartOwner owner)
        {
            PartCheck checkSetting = this.GetPartCheckSettingData(part.ValueType);
            if (checkSetting == null)
            {
                string msg = string.Format("PartCheck data of {0} type does not exist.", part.PartType);
                Exception ex = new Exception(msg);
                throw ex;
            }

            part = ProductPart.PartSpecialDeal(part);

            if (checkSetting.NeedSave == 1)
            {
                owner.AddPart(part);
            }
        }
        

        /// <summary>
        /// 使用PartCheckMatch中定义的规则match
        /// </summary>
        /// <param name="target">需要匹配的字符串</param>
        /// <param name="part">part</param>
        /// <param name="containCheckBit">是否包含校验位</param>
        /// <returns></returns>
        public virtual bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit)
        {
            PartCheck checkSetting = this.GetPartCheckSettingData(valueType);
            if (checkSetting == null)
            {
                string msg = string.Format("PartCheck data of {0} type does not exist.", part.Type);
                Exception ex = new Exception(msg);
                throw ex;
            }

            return checkSetting.MatchRule.Match(target, part, out containCheckBit);
        }

        /// <summary>
        /// 默认的PartCheck实现，只进行唯一性检查
        /// </summary>
        /// <param name="part">需要check的part</param>
        /// <param name="owner">PartOwner</param>
        public virtual void Check(IBOMPart part, IPartOwner owner, string station)
        {
            PartCheck checkSetting = this.GetPartCheckSettingData(part.ValueType);
            if (checkSetting == null)
            {
                string msg = string.Format("PartCheck data of {0} type does not exist.", part.Type);
                Exception ex = new Exception(msg);
                throw ex;
            }

            if (checkSetting.NeedCheck == 1)
            {
                string realSn = ProductPart.PartSpecialDeal(part.Type, part.ValueType, part.MatchedSn);
                owner.PartUniqueCheck(part.PN, realSn);
            }
        }


        /// <summary>
        /// Set PartCheckSetting data to the strategy object
        /// </summary>
        /// <param name="setting">PartCheckSetting data</param>
        public void SetPartCheckSetting(IList<PartCheck> setting)
        {
            _checkSetting = setting;
        }

        /// <summary>
        /// To check whether a bompart need partcheck according to its checksetting data
        /// </summary>
        /// <param name="bomPart">Bom part</param>
        /// <returns>Whether a check is required</returns>
        public bool IfNeedCheck(BOMPart bomPart)
        {
            PartCheck checkSetting = this.GetPartCheckSettingData(bomPart.ValueType);
            if (checkSetting == null)
            {
                string msg = string.Format("PartCheck data of {0} type does not exist.", bomPart.Type);
                Exception ex = new Exception(msg);
                throw ex;
            }
            return checkSetting.NeedCheck > 0;
        }

        /// <summary>
        /// 根据valueType获取唯一的PartCheck规则设定
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private PartCheck GetPartCheckSettingData(string valueType)
        {
            foreach (var item in _checkSetting)
            {
                if (item.ValueType.Equals(valueType))
                {
                    return item;
                }
            }
            return null;
        }

    }
}

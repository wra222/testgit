using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PartStrategy.CommonStrategy;

namespace IMES.FisObject.PartStrategy.MEStrategy
{
    public class MEStrategy: AbstractStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MEStrategy(PartCheck checkSetting) : base(checkSetting)
        {
        }

        /// <summary>
        /// Check Vendor CT是否有结合厂内CT：Table:TSB..KPDet_LCM  灯管 
        /// 唯一性检查:只能与一个ProId绑定
        /// </summary>
        /// <param name="part"></param>
        /// <param name="owner"></param>
        public override void Check(IMES.FisObject.Common.BOM.IBOMPart part, IPartOwner owner)
        {
            //Check Vendor CT是否有结合厂内CT：Table:TSB..KPDet_LCM  灯管 
            //todo: how to check
            int i; 

            //唯一性检查:只能与一个ProId绑定
            base.Check(part, owner);
        }
    }
}

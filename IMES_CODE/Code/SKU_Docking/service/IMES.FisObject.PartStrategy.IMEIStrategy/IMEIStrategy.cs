// INVENTEC corporation (c)2009 all rights reserved. 
// Description: IMEIStrategy
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
using IMES.FisObject.Common.Part;
using IMES.FisObject.PartStrategy.CommonStrategy;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.BOM;

namespace IMES.FisObject.PartStrategy.IMEIStrategy
{
    /// <summary>
    /// 一般类型Part的Match,Check,Bind策略实现类
    /// </summary>
    public class IMEIStrategy :AbstractStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public IMEIStrategy(IList<PartCheck> checkSetting)
            : base(checkSetting)
        {
        }
        public override void Check(IBOMPart part, IPartOwner owner, string station)
        {
            //1.在PartSN存在
            //2.唯一性检查:只能与一个ProId绑定
            base.Check(part, owner, station);
        }
        public override bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit)
        {
           containCheckBit = false;
           return   base.Match(target, part, station, valueType, out containCheckBit);
        }
        public override void BindTo(IProductPart part, IPartOwner owner)
        {
            var product = (IProduct)owner;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            var productRepoistory = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            product.IMEI = part.Value;
            productRepoistory.Update(product, session.UnitOfWork); 
        
        }
    }
}

// INVENTEC corporation (c)2009 all rights reserved. 
// Description: COAStrategy
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
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PartStrategy.CommonStrategy;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PartStrategy.COAStrategy
{
    /// <summary>
    /// COA类型Part的Match,Check,Bind策略的实现类
    /// </summary>
    public class COAStrategy : AbstractStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public COAStrategy(IList<PartCheck> checkSetting) : base(checkSetting)
        {
        }

        /// <summary>
        /// Bind part to owner
        /// 1.Update COAStatus, Status=A1
        ///2.记录COALOG
        ///3.Product.COAID=Code
        /// </summary>
        /// <param name="part">part</param>
        /// <param name="owner">owner</param>
        public override void BindTo(IProductPart part, IPartOwner owner)
        {
//            if (!part.ValueType.Equals(("SN")))
//            {
//                base.BindTo(part, owner);
//                return;
//            }

            var product = (IProduct)owner;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            var productRepoistory = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            COAStatus coa = coaRepository.Find(part.Value);

            //1
            coa.Status = COAStatus.COAStatusConst.A1;

            //2
            var log = new COALog
                          {
                              COASN = coa.COASN,
                              StationID = session.Station,
                              LineID = session.Line,
                              Editor = session.Editor,
                              Cdt = DateTime.Now
                          };
            coa.addCOALog(log);

            //3
            product.COAID = part.Value;

            coaRepository.Update(coa, session.UnitOfWork);
            productRepoistory.Update(product, session.UnitOfWork);
        }


        /// <summary>
        /// 使用PartCheckMatch中定义的规则match
        /// 1. 14码
        /// 2. Code=COAStatus.COASN; COAStatus.CustPN=BOM中的PartNo.
        /// </summary>
        /// <param name="target">需要匹配的字符串</param>
        /// <param name="part">part</param>
        /// <param name="containCheckBit">是否包含校验位</param>
        /// <returns></returns>
        public override bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit)
        {
            if (!valueType.Equals(("SN")))
            {
                return base.Match(target, part, station, valueType, out containCheckBit);
            }

            if (!base.Match(target, part, station, valueType, out containCheckBit))
            {
                return false;
            }

//            containCheckBit = false;
//            if (target.Length != 14)
//            {
//                return false;
//            }

            var coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            COAStatus coa = coaRepository.Find(target);
            if (coa == null)
            {
                return false;
            }

            if (!coa.IECPN.Equals(part.PN))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 1.COA是可用状态(COAStatus.Status = P1 or A2 or A3)
        ///  注：
        ///          最后的实现按宋杰在Unit Label Print UC中的描述完成：
        ///          COAStatus.COASN=code and COAStatus.Line=@PdLine and COAStatus.Status为P2或A2、A3
        /// </summary>
        /// <param name="part">需要check的part</param>
        /// <param name="owner">PartOwner</param>
        public override void Check(IBOMPart part, IPartOwner owner, string station)
        {
//            if (!part.ValueType.Equals(("SN")))
//            {
//                base.Check(part, owner, station);
//                return;
//            }

            Session session = SessionManager.GetInstance.GetSession(((IProduct)owner).ProId, Session.SessionType.Product);
            var coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            COAStatus coa = coaRepository.Find(part.MatchedSn);
            if (
                    (!coa.Status.Equals(COAStatus.COAStatusConst.A2)
                    && !coa.Status.Equals(COAStatus.COAStatusConst.A3)
                    && !coa.Status.Equals(COAStatus.COAStatusConst.P2)
                    )  
                    || !coa.LineID.Equals(session.Line)
                )
            {
                List<string> erpara = new List<string>();
                erpara.Add(part.MatchedSn);
                var ex = new FisException("CHK046", erpara);
                throw ex;
            }
        }
    }
}

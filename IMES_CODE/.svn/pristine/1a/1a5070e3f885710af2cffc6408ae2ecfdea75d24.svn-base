// INVENTEC corporation (c)2009 all rights reserved. 
// Description: MBStrategy
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
using IMES.FisObject.Common.Process;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PartStrategy.CommonStrategy;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PartStrategy.MBStrategy
{
    /// <summary>
    /// MB类型Part的Match,Check,Bind策略的实现类
    /// </summary>
    public class MBStrategy : AbstractStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MBStrategy(IList<PartCheck> checkSetting) : base(checkSetting)
        {
        }

        /// <summary>
        /// 1. 10码(去掉校验位)
        /// 2. VB SN#前两码=Part的MB属性
        /// </summary>
        /// <param name="target">目标串</param>
        /// <param name="part">part</param>
        /// <returns>是否匹配</returns>
        public override bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit)
        {
            if (!valueType.Equals(("SN")))
            {
                return base.Match(target, part, station, valueType, out containCheckBit);
            }

            //1.10码(去掉校验位)
            if (!base.Match(target, part, station, valueType, out containCheckBit))
            {
                return false;
            }

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
            
            //2.MB SN在PCB表中对应记录的Model与当前Pn一致
            string mbsn = target;
            if (containCheckBit)
            {
                mbsn = mbsn.Substring(0, mbsn.Length - 1);
            }

            IMBRepository rep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB mb = rep.Find(mbsn);

            if (mb == null)
            {
                return false;
            }

            if (!mb.Model.Equals(part.PN))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Bind pat to owner:
        /// A.Product.PCBID=mb sn#
        ///B.需要修改MB 的PCA..PCBStatus.Station = 32
        ///C.记录MB Log  Inert PCA..PCBLog
        ///D.将MB相关信息写入Product表:
        ///Product.PCBModel=PCA..PCB.PCBModelID
        ///Product. MAC =PCA..PCB. MAC
        ///Product. UUID =PCA..PCB. UUID
        ///Product. MBECR =PCA..PCB. ECR
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
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            var mb = mbRepository.Find(part.Value);

            string line = string.IsNullOrEmpty(session.Line) ? product.Status.Line : session.Line;

            //A
            product.PCBID = mb.Sn;

            //B
            var status = new MBStatus(mb.Key.ToString(), "32", MBStatusEnum.Pass , session.Editor, line, DateTime.Now, DateTime.Now);
            mb.MBStatus = status;

            //C
            var mbLog = new MBLog(
                0,
                mb.Sn,
                mb.Model,
                "32",
                1,
                line,
                session.Editor,
                DateTime.Now);

            mb.AddLog(mbLog);

            //D
            product.PCBModel = mb.Model;
            product.MAC = mb.MAC;
            product.UUID = mb.UUID;
            product.MBECR = mb.ECR;
            product.CVSN = mb.CVSN;

            mbRepository.Update(mb, session.UnitOfWork);
            productRepoistory.Update(product, session.UnitOfWork);
            
        }

        /// <summary>
        /// Check part:
        ///A.Code(3)  in   ‘0123456789’
        /// Code(4)  in   ‘0123456789ABC’
        /// Code(5)  in   A：AOI (A面)
        /// M：MB (B面)
        /// V ：VGA/B
        ///B.按照process定义来判断MB是否能与ProId进行绑定，可以进入32站
        ///C.MB SN#前两码=TC阶model的MB属性
        /// </summary>
        public override void Check(IBOMPart part, IPartOwner owner, string station)
        {
//            if (!part.ValueType.Equals(("SN")))
//            {
//                base.Check(part, owner, station);
//                return;
//            }

            IProduct product = (IProduct)owner;

            //A. Check MB Sn format
//            const string code3Range = "0123456789";
//            const string code4Range = "0123456789ABC";
//            const string code5Range = "AMV";
//
//            if (!code3Range.Contains(part.MatchedSn.Substring(2, 1))
//                || !code4Range.Contains(part.MatchedSn.Substring(3, 1))
//                || !code5Range.Contains(part.MatchedSn.Substring(4, 1)))
//            {
//                List<string> erpara = new List<string>();
//                erpara.Add(part.MatchedSn);
//                var ex = new FisException("CHK048", erpara);
//                throw ex;
//            }

            //B. Block station fo MB
            IProcessRepository processRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            processRepository.SFC(session.Line, session.Customer, "32", part.MatchedSn, "MB"); //对于MB，不论在任何站与Product绑定，该站的站号都是32

            //C.MB SN#前两码=TC阶model的MB属性
            string vbCode = ((IProduct)owner).ModelObj.GetAttribute("MB");
            // Vincent 2011-08-04 ModelInfo MB name have more than one MBCode value example MB=A7~A6
            //if (part.MatchedSn.Substring(0, 2) != vbCode)
            if (!vbCode.Contains(part.MatchedSn.Substring(0, 2)))
            {
                List<string> erpara = new List<string>();
                erpara.Add(part.MatchedSn);
                var ex = new FisException("CHK070", erpara);
                throw ex;
            }
            // Check the PCB has been used by other product
            IProductRepository productRepository =
                       RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<IProduct> productList = productRepository.GetProductListByPCBID(part.MatchedSn);
            if (productList.Count > 0)
            {
                List<string> erpara = new List<string>();
                erpara.Add("MB");
                erpara.Add(part.MatchedSn);
                erpara.Add("");
                var ex = new FisException("CHK009", erpara);
                throw ex;


            }
            // Check the PCB has been used by other product

        }
    }
}

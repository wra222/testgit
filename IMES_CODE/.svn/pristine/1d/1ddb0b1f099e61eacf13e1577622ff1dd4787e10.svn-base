using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Maintain.Implementation
{
    class WLBTDescrManager : MarshalByRefObject, IWLBTDescr
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        public IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        public IAssemblyCodeRepository assemblyCodeRepository = RepositoryFactory.GetInstance().GetRepository<IAssemblyCodeRepository, AssemblyCode>();

        //取得Part
        //SELECT PartType, Descr FROM IMES_GetData..Part
        //    WHERE PartNo = ? 
        //        AND Flag <> '0'
        public PartMaintainInfo GetPartByPartNo(string partNo)
        {
            IPart part = partRepository.GetPartByPartNo(partNo);
            if (part == null)
            {
                return null;
            }
            else
            {
                PartMaintainInfo partMaintainInfo = new PartMaintainInfo();
                partMaintainInfo.PartNo = part.PN;
                partMaintainInfo.Descr = part.Descr;
                partMaintainInfo.PartType = part.Type;
                partMaintainInfo.CustPartNo = part.CustPn;
                partMaintainInfo.AutoDL = part.AutoDL;
                partMaintainInfo.Remark = part.Remark;
                partMaintainInfo.Editor = part.Editor;
                partMaintainInfo.Cdt = part.Cdt;
                partMaintainInfo.Udt = part.Udt;
                return partMaintainInfo;
            }

        }

        //取得Part的Type Description
        //SELECT DISTINCT InfoValue as [Type Description]
        //FROM IMES_GetData..AssemblyCodeInfo
        //WHERE InfoType = ?
        //      AND ISNULL(InfoValue, '') <> ''
        //      AND AssemblyCode IN (SELECT AssemblyCode FROM IMES_GetData..AssemblyCode WHERE PartNo = ?)
        //ORDER BY [Type Description]  
        public IList<string> GetPartTypeDescr(string infoType, string partNo)
        {
            IList<string> descrList = assemblyCodeRepository.GetPartTypeDescr(infoType, partNo);
            return descrList;
        }

        //取得全部的WLBTDescr  
        //SELECT *
        //FROM WLBTDescr
        //ORDER BY Code, Site, Tp 
        public IList<WLBTDescr> GetAllWLBTDescr()
        {
            return productRepository.GetAllWLBTDescr();
        }


        //取得某一Part的WLBTDescr  
        //SELECT *
        //FROM WLBTDescr
        //WHERE Code = ?
        //ORDER BY Code, Site, Tp
        public IList<WLBTDescr> GetWLBTDescrListByPartNo(string partNo)
        {
            return productRepository.GetWLBTDescrListByPartNo(partNo);
        }

        //判断WLBTDescr是否已经存在  
        //SELECT * 
        //FROM [WLBTDescr] 
        //WHERE Code = ? AND Tp = ? AND Site = ? AND TpDescr = ? 
        public int IFWLBTDescrIsExists(WLBTDescr descr)
        {
            return productRepository.IFWLBTDescrIsExists(descr); 
        }


        //更新WLBTDescr
        //UPDATE [WLBTDescr] 
        //SET Descr = ?, Editor = ?, Udt = GETDATE()
        //WHERE Code = ? AND Tp = ? AND Site = ? AND TpDescr = ? 
        public void UpdateWLBTDescr(WLBTDescr descr)
        {
            productRepository.UpdateWLBTDescr(descr);
        }

        //新增WLBTDescr纪录, 返回ID    
        //INSERT INTO [WLBTDescr]([Code],[Tp],[TpDescr],[Descr],[Site],[Editor],[Cdt],[Udt])
        //		VALUES(?, ?, ?, ?, ?, ?, GETDATE(), GETDATE())  
        public int InsertWLBTDescr(WLBTDescr descr)
        {
            productRepository.InsertWLBTDescr(descr);
            return descr.Id;
        }

        //删除WLBTDescr纪录
        public void deleteWLBTDescr(string id)
        {
            productRepository.DeleteWLBTDescr(id);
        }
    }
}

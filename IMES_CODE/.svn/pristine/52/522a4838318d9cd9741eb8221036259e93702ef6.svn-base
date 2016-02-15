/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: FamilyInfo Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012/05/18   kaisheng           (Reference Ebook SourceCode) Create
 * * issue:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using log4net;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Maintain.Implementation
{
    public class FamilyInfoManagerEx : FamilyInfoManager, IFamilyInfoEx
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IFamilyInfoName

        public IList<FamilyInfoNameEx> GetFamilyInfoName()
        {
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            return itemRepositoryEx.GetFamilyInfoName();
        }


        public void AddFamilyInfoName(FamilyInfoNameEx item)
        {
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            itemRepositoryEx.AddFamilyInfoName(item);
        }

        public void UpdateSelectedFamilyInfoName(FamilyInfoNameEx item, string nameKey)
        {
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            itemRepositoryEx.UpdateFamilyInfoName(item, nameKey);
        }

        public void DeleteSelectedFamilyInfoName(string nameKey)
        {
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            itemRepositoryEx.DeleteFamilyInfoName(nameKey);
        }

        #endregion


        #region IFamilyInfoEx Members


        public IList<FamilyInfoDef> GetFamilyInfoList(string strFamilyName)
        {
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            return itemRepositoryEx.GetFamilyInfoDefList(strFamilyName);
        }

        public FamilyInfoDef GetFamilyInfo(string strFamilyInfoId)
        {
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            return itemRepositoryEx.GetFamilyInfoDef(strFamilyInfoId);
        }

        public void DeleteFamilyInfo(FamilyInfoDef model)
        {
            logger.Info("DeleteFamilyInfo ID="+model.id+", Name="+model.name+", Editor="+model.editor);
            IFamilyRepositoryEx itemRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IFamilyRepositoryEx>();
            itemRepositoryEx.DeleteFamilyInfo(model);
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.FisObject.Common.Model
{
    public interface IFamilyRepositoryEx : IFamilyRepository
    {
        #region for maintain
        /// <summary>
        /// 获取FamilyInfoName表记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<FamilyInfoNameEx> GetFamilyInfoName();

        /// <summary>
        /// 新增FamilyInfoName表记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        void AddFamilyInfoName(FamilyInfoNameEx item);


        /// <summary>
        /// 修改FamilyInfoName表记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        void UpdateFamilyInfoName(FamilyInfoNameEx item, string nameKey);


        /// <summary>
        /// 刪除FamilyInfoName表记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        void DeleteFamilyInfoName(string nameKey);

        /// <summary>
        /// 获取FamilyInfo表记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<FamilyInfoDef> GetFamilyInfoDefList(string strFamilyName);

        /// <summary>
        /// 获取FamilyInfo记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        FamilyInfoDef GetFamilyInfoDef(string strFamilyInfoId);

        void DeleteFamilyInfo(FamilyInfoDef model);
        void DeleteFamilyInfoDefered(IUnitOfWork uow,FamilyInfoDef model);

        IList<string> GetFamilyByCustomer(string customer);

        void DeleteFamilyEx(string family, string customer);
        void DeleteFamilyExDefered(IUnitOfWork uow,string family, string customer);

        #endregion


    }
}

using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IQCRatio
    {

        //        /////
        //取得某customer下QCRatioList 
        //SELECT [IMES_FA].[dbo].[QCRatio].[Family]
        //      ,[IMES_FA].[dbo].[QCRatio].[QCRatio]
        //      ,[IMES_FA].[dbo].[QCRatio].[EOQCRatio]
        //  FROM [IMES_FA].[dbo].[QCRatio] inner join 
        //[AT_IMES_GetData].[dbo].[Family] on [IMES_FA].[dbo].[QCRatio].[Family]=[AT_IMES_GetData].[dbo].[Family].[Family]
        //WHERE [AT_IMES_GetData].[dbo].[Family].[CustomerID]='customer' order by [Family]
        DataTable GetQCRatioList(string customer);

        //若Family为空，则将当前Customer保存到记录中的Family栏位
        string AddQCRatio(QCRatioDef item);

        //若Family为空，则将当前Customer保存到记录中的Family栏位
        string SaveQCRatio(QCRatioDef item, string oldId);

        /// <summary>
        /// 删除一条QCRatio
        /// </summary>
        /// <param name="item"></param>
        void DeleteQCRatio(QCRatioDef item);
        /// <summary>
        /// 取得下拉列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<SelectInfoDef> GetCustomerFamilyList(string customer);
        /// <summary>
        /// GetCustomerFamilyListAddline
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<SelectInfoDef> GetCustomerFamilyListAddline(string customer);
        /// <summary>
        /// 根据Family取得Model列表
        /// </summary>
        /// <param name="Family"></param>
        /// <returns></returns>
        IList<SelectInfoDef> GetModelListByFamily(string Family);


    }
}

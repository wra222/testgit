﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// SMTMO接口
    /// </summary>
    public interface ISelectData
    {

        //        SELECT     Station.Station 
        //FROM         CheckItem INNER JOIN
        //Station ON CheckItem.Mode = Station.OperationObject
        //WHERE CheckItem.ID=checkItemId order by Station
        //若新选项不是“Add a new Check Item”，则Station下拉列表中选项换为与新当前CheckItem对应Mode一致（Station.OperationObject）的Station。
        IList<SelectInfoDef> GetStationListByCheckedItemID(string checkItemId);


        IList<SelectInfoDef> GetCustomerFamilyList(String customer);

        //取得CheckItem下拉列表
        //SELECT [ID],[ItemName] FROM [IMES_GetData].[dbo].[CheckItem] WHERE Customer='customer' order by [ItemName]
        IList<SelectInfoDef> GetItemNameListByCustomer(string customer);


        //PartType列表，
        //SELECT [PartType]
        //       FROM [IMES_GetData].[dbo].[PartType] ORDER BY [PartType]
        //选项包括系统中所有的Part Type 
        //PartTypeAttribute
        IList<SelectInfoDef> GetPartTypeList();

        //Value Type列表

        //SELECT [Code]      
        //  FROM [IMES_GetData].[dbo].[PartTypeAttribute]
        //where PartType='partType' ORDER BY [Code]   

        //Change：1、ValueType下拉列表的选项全部换为与新选PartType相关的属性名称，其第一个选项被选取。
        //ValueType 选项包括PartNo、Descr、CustPartNo、FruNo、Vendor、IECVersion以及当前Part Type的所有Attribute
        IList<SelectInfoDef> GetValueTypeList(string partType);


        //取得Model列表, 取Family表和Model表关联？ CustomerID字长80？
        //问题：空项在前还是在后，第一个被选取, 是
        //Model 下拉列表的选项除空项外全部换为与新选Customer对应的Model，其第一个选项被选取；

        //SELECT distinct Model.Model
        //FROM  Family INNER JOIN
        //Model ON Family.Family = Model.Family WHERE CustomerID ='customer' 
        //order by Model
        IList<SelectInfoDef> GetCustomerModelList(string customer);

        //取得station的列表
        //选项包括所有的Station，按创建时间排序
        //SELECT [Station]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        //ORDER BY [Cdt]
        IList<SelectInfoDef> GetStationList();

        //取得当前客户相关的PartType
        // PartType下拉列表框中选项换为PartCheck表中当前Customer的所有相关Part Type。

        //SELECT [PartType] FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck] WHERE [Customer]='customer' order by [PartType]
        IList<SelectInfoDef> GetCustomerPartTypeList(String customer);

    }



    public interface ICustomer
    {

        /// <summary>
        /// 取CustomerInfo列表
        /// </summary>
        /// <returns></returns>
        IList<CustomerInfo> GetCustomerList();
        void AddCustomer(CustomerInfo customerInfo);
        void DeleteCustomer(CustomerInfo customerInfo);
        void UploadCustomer(CustomerInfo condition, CustomerInfo value);
    }

    public interface IFamily2
    {
        /// <summary>
        /// 取得Customer下的family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        IList<FamilyDef> GetFamilyInfoList(String customerId);

        /// <summary>
        /// 取得所有family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <returns></returns>
        IList<FamilyDef> GetFamilyInfoList();

        /// <summary>
        ///  取得一条family的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        FamilyDef GetFamily(String id);

        /// <summary>
        /// 保存一条family的记录数据(Add)
        /// </summary>
        /// <param name="Object"></param>
        void AddFamily(FamilyDef obj);


        /// <summary>
        /// 保存一条family的记录数据(update), 若Family名称为其他存在的Family的名称相同，则提示业务异常
        /// </summary>
        /// <param name="Object"></param>
        void UpdateFamily(FamilyDef obj, String oldFamily);

        /// <summary>
        /// "删除一条family的记录数据
        /// </summary>
        /// <param name="?"></param>
        void DeleteFamily(FamilyDef obj);

        /// <summary>
        /// 取得family对应的customer，若没有，返回空字串
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        string GetCustomerByFamily(string family);

    }

    public interface IWarranty
    {
        /// <summary>
        /// 取得Customer Id下的Warranty数据的list(按Type栏位排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        IList<WarrantyDef> GetWarrantyList(String customerId);

        /// <summary>
        /// 取得一条Warranty的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        WarrantyDef GetWarranty(String id);

        /// <summary>
        /// 保存一条Warranty的记录数据(Add)
        /// </summary>
        /// <param name="?"></param>
        string AddWarranty(WarrantyDef obj);

        /// <summary>
        ///  保存一条Warranty的记录数据 (update)
        /// </summary>
        /// <param name="Object"></param>
        void UpdateWarranty(WarrantyDef obj, String oldWarranty);
        /// <summary>
        /// 删除一条Warranty数据
        /// </summary>
        /// <param name="?"></param>
        void DeleteWarranty(String id);

    }

    public interface IMACRange
    {


        /// <summary>
        /// 取得MACRange List列表（按“Code”列的字母序排序）
        /// </summary>
        /// <returns></returns>
        IList<MACRangeDef> GetMACRangeList();

        /// <summary>
        /// 取得一条MACRang的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        MACRangeDef GetMACRange(String id);

        /// <summary>
        /// 保存一条MACRang的记录数据(Add)
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        string AddMACRange(MACRangeDef obj);

        /// <summary>
        /// 保存一条MACRang的记录数据(update)
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        void UpdateMACRange(MACRangeDef obj, String oldMACRange);

        /// <summary>
        /// 删除一条MACRang数据
        /// </summary>
        /// <param name="?"></param>
        void DeleteMACRange(String id);

        /// <summary>
        /// 取得MACRange的信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MACInfoDef GetMACInfo(string code);
		
		/// <summary>
        /// Bartender打印时,根据存储过程名字和参数执行存储过程,获取主Bartender Name/Value/DataType
        /// </summary>
        /// <param name="currentSPName">存储过程名字</param>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns>主bat名称</returns>
        IList<NameValueDataTypeInfo> GetBartenderNameValueList(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues);



    }
}

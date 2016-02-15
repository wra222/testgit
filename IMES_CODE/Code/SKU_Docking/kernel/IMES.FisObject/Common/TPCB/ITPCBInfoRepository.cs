// INVENTEC corporation (c)2010 all rights reserved. 
// Description: TPCB
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-15   Chen Xu (eb1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.TPCB
{
    
    /// <summary>
    /// TPCB 的Repository接口
    /// </summary>
    public interface ITPCBInfoRepository : IRepository<TPCB_Info>
    {
        /// <summary>
        /// DropDownList获得Family下拉框信息 (not 1397)
        /// SELECT '' as Family UNION SELECT DISTINCT CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family  
        /// ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END AS Family 
        /// FROM IMES_GetData..Model	
        /// WHERE LEFT(Model, 4) <> '1397'AND ISNULL(Family, '') <> ''	
        /// ORDER BY Family
        /// </summary>
        /// <returns>FamilyInfo</returns>
        IList<FamilyInfo> GetFamilyList();

        /// <summary>
        /// 根据下拉框选择的family，取得Type下拉框信息
        /// SELECT '' as [Type] UNION
        /// SELECT DISTINCT RTRIM(Type) as [Type]
        /// FROM TPCB WHERE Family = @Family AND PdLine = 'TPCB' ORDER BY [Type]
        /// </summary>
        /// <param name="family">family</param>
        /// <returns>Type</returns>
        IList<string> GetTypeList(string family);

        /// <summary>
        /// 根据下拉框选择的family和type, 取得PartNo下拉框信息
        /// SELECT '' as [Part No] UNION
        /// SELECT DISTINCT RTRIM(PartNo) as [Part No]
        /// FROM TPCB	WHERE Family = @Family AND Type = @Type AND PdLine = 'TPCB'	ORDER BY [Part No]
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="type">type</param>
        /// <returns>PartNo</returns>
        IList<string> GetPartNoList(string family, string type);

        /// <summary>
        /// 根据下拉框选择的family和partno, 取得PartNo下拉框信息
        /// SELECT RTRIM(Dcode) as [Date Code]
        /// FROM TPCB WHERE Family = @Family AND PartNo = @PartNo AND PdLine = 'TPCB'
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="partno">partno</param>
        /// <returns>DCode</returns>
        IList<string> GetDCode(string family, string partno);

        /// <summary>
        /// 根据下拉框选择的family和partno, 取得PartNo下拉框信息
        /// SELECT RTRIM(Vendor) as [Vendor SN]
        /// FROM TPCB WHERE Family = @Family AND PartNo = @PartNo AND PdLine = 'TPCB'
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="partno">partno</param>
        /// <returns>VendorSN</returns>
        IList<string> GetVendorSN(string family, string partno);

        /// <summary>
        /// 显示全部TPCBInfo数据相关信息
        /// SELECT RTRIM(Family) as Family, RTRIM(PdLine) as [PdLine], 
        /// RTRIM(Type) as [Type],RTRIM(PartNo) as [Part No], 
        /// RTRIM(Vendor) as Vendor, 
        /// RTRIM(Dcode) as [Date Code],
        /// RTRIM(Editor) as Editor, 
        /// Cdt as [Create Date]	
        /// FROM TPCB 
        /// WHERE Family = @Family AND PdLine = @PdLine ORDER BY Type, PartNo
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <returns>返回TPCBInfo信息</returns>
        IList<TPCBInfo> Query(string family, string pdline);

        /// <summary>
        /// 显示全部TPCBInfo数据相关信息
        /// SELECT RTRIM(Family) as Family, RTRIM(PdLine) as [PdLine], 
        /// RTRIM(Type) as [Type],RTRIM(PartNo) as [Part No], 
        /// RTRIM(Vendor) as Vendor, 
        /// RTRIM(Dcode) as [Date Code],
        /// RTRIM(Editor) as Editor, 
        /// Cdt as [Create Date]	
        /// FROM TPCB 
        /// WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo ORDER BY Type, PartNo
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <param name="partNo">partNo</param>
        /// <returns>返回TPCBInfo信息</returns>
        IList<TPCBInfo> Query(string family, string pdline, string partNo);

        /// <summary>
        /// 【保存】或【更新】TPCBInfo信息
        /// IIF EXISTS(SELECT * FROM TPCB WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo)
        /// UPDATE TPCB SET Dcode = @Dcode, Editor = @Editor, Udt = GETDATE()
        /// WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo
        /// ELSE
        /// INSERT INTO [TPCB]([Family],[PdLine],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
        /// VALUES (@Family, @PdLine, @Type, @PartNo, @Vendor, @Dcode, @Editor, GETDATE())
        /// </summary>
        /// <param name="value">value</param>
        void SaveTPCB(TPCBInfo value);
       
        /// <summary>
        /// 根据vcode号，删除vcode数据相关信息
        /// DELETE FROM TPCB WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <param name="partno">partno</param>
        void DeleteTPCB(string family, string pdline, string partno);

        /// <summary>
        /// 【保存】TPCBDet信息
        /// INSERT INTO [TPCBDet]([Code],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
        /// SELECT @TPCBCode, Type, PartNo, Vendor, Dcode, @Editor, GETDATE()
        /// FROM TPCB 
        /// WHERE Family = @Family AND PdLine = @PdLine
        /// </summary>
        /// <param name="tpcbCode">tpcbCode</param>
        /// <param name="family">family</param>
        /// <param name="pdline">pdline</param>
        /// <param name="editor">editor</param>
        void SaveTPCBDet(string tpcbCode, string family, string pdline, string editor);

        /// <summary>
        /// 在保存前，检查TPCBDet信息是否重复
        /// </summary>
        /// <param name="tpcbCode">tpcbCode</param>
        /// <param name="editor">editor</param>
        /// <returns>返回存在信息</returns>
        IList<TPCBDet> CheckTPCBDet(string tpcbCode,string editor);

        #region Defered

        void SaveTPCBDefered(IUnitOfWork uow, TPCBInfo value);

        void DeleteTPCBDefered(IUnitOfWork uow, string family, string pdline, string partno);

        void SaveTPCBDetDefered(IUnitOfWork uow, string tpcbCode, string family, string pdline, string editor);

        #endregion

        #region For Maintain

        /// <summary>
        /// SQL语句：
        /// SELECT ID,Family, Type as [Type], PartNo as [Part No], Vendor as [Vendor SN], Editor, Cdt as [Create Date], Udt
        ///          FROM IMES_FA..TPCB
        ///          WHERE PdLine = 'TPCB'
        ///          ORDER BY Family, Type, PartNo, Vendor
        /// </summary>
        /// <returns></returns>
        IList<TPCBInfo> GetTpcbList();

        /// <summary>
        /// 返回值和参数说明：
        /// 参数IList<TPCBInfo>即是第一个方法的参数，实际上此方法为第一个方法的重载方法;
        /// 参数为字符串类型，对应数据库表中的Family字段
        /// SQL语句：
        /// SELECT ID,Family, Type as [Type], PartNo as [Part No], Vendor as [Vendor SN], Editor, Cdt as [Create Date], Udt
        ///          FROM IMES_FA..TPCB
        ///          WHERE PdLine = 'TPCB'
        ///                    AND Family = @Family
        ///          ORDER BY Family, Type, PartNo, Vendor
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<TPCBInfo> GetTpcbList(string family);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值；
        /// 参数为之前提到的TPCBInfo结构类型。
        /// SQL语句：
        /// IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[TPCB]
        ///          WHERE PdLine = 'TPCB'
        ///                    AND Family = @Family
        ///                    AND PartNo = @PartNo)
        ///          UPDATE [IMES_FA].[dbo].[TPCB] 
        ///                    SET Vendor = @VendorSN,
        ///                             Editor = @Editor,
        ///                             Udt = GETDATE()
        ///                    WHERE PdLine = 'TPCB'
        ///                             AND Family = @Family
        ///                             AND PartNo = @PartNo
        /// ELSE
        ///          INSERT INTO [IMES_FA].[dbo].[TPCB]([Family],[PdLine],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
        ///                    VALUES (@Family, 'TPCB', @Type, @PartNo, @VendorSN, '', @Editor, GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void SaveTPCBInfo(TPCBInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为int类型，对应数据库TPCB表中的主键ID
        /// 参数为之前提到的TPCBInfo结构类型
        /// SQL语句：
        /// SELECT ID
        /// FROM [IMES_FA].[dbo].[TPCB]
        /// WHERE PdLine = 'TPCB'
        ///         AND Family = @Family
        ///         AND PartNo = @PartNo
        /// </summary>
        /// <param name="family"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        int GetID(string family, string partNo);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值；
        /// 参数为两个字符串类型，分别对应数据库中TPCB表中的Family和PartNo字段；
        /// SQL语句：
        /// DELETE FROM [IMES_FA].[dbo].[TPCB]
        ///          WHERE PdLine = 'TPCB'
        ///                    AND Family = @Family
        ///                    AND PartNo = @PartNo
        /// </summary>
        /// <param name="family"></param>
        /// <param name="partNo"></param>
        void DeleteTPCBInfo(string family, string partNo);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值仍然为以TPCBInfo结构类型为泛型参数的IList；
        /// 参数为两个字符串，分别对应数据库TPCB表中的Family和PartNo字段。
        /// SQL语句：
        /// SELECT * FROM [IMES_FA].[dbo].[TPCB]
        ///          WHERE PdLine = 'TPCB'
        ///                    AND Family = @Family
        ///                    AND PartNo = @PartNo
        /// </summary>
        /// <param name="family"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<TPCBInfo> CheckHasList(string family, string partNo);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值是以TPCBInfo结构类型为泛型参数的IList；
        /// 参数同样是TPCBInfo结构类型，在SQL语句中只用到其中的Family，Type和PartNo字段。
        /// SQL语句：
        /// SELECT * FROM [IMES_FA].[dbo].[TPCB]
        /// WHERE PdLine = 'TPCB'
        ///          AND Family = @Family
        ///          AND Type = @Type
        ///          AND PartNo = @PartNo
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IList<TPCBInfo> CheckSameList(TPCBInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值
        /// 参数为TPCBInfo结构类型
        /// SQL语句：
        /// IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[TPCB]
        ///          WHERE PdLine = 'TPCB'
        ///                    AND Family = @Family
        ///                    AND PartNo = @PartNo)
        ///         UPDATE [IMES_FA].[dbo].[TPCB] 
        ///                    SET Vendor = @VendorSN,
        ///                         Type=@Type,
        ///                         Editor = @Editor,
        ///                         Udt = GETDATE()
        ///                    WHERE PdLine = 'TPCB'
        ///                         AND Family = @Family
        ///                         AND PartNo = @PartNo
        /// </summary>
        /// <param name="item"></param>
        void UpdateTPCBInfo(TPCBInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值
        /// 参数为TPCBInfo结构类型
        /// SQL语句：
        /// INSERT INTO [IMES_FA].[dbo].[TPCB]([Family],[PdLine],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
        /// VALUES (@Family, 'TPCB', @Type, @PartNo, @VendorSN, '', @Editor, GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertTPCBInfo(TPCBInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为代表记录条数的Int值
        /// 参数为两个字符串类型的值，分别对应[IMES_FA].[dbo].[TPCB]表内的Family和PartNo字段。
        /// SQL语句：
        /// SELECT COUNT(*) FROM [IMES_FA].[dbo].[TPCB]
        /// WHERE  PdLine = 'TPCB' AND [Family]=@Family AND [PartNo]=@PartNo
        /// </summary>
        /// <param name="family"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        int CheckExistsRecord(string family, string partNo);

        #region Defered

        void SaveTPCBInfoDefered(IUnitOfWork uow, TPCBInfo item);

        void DeleteTPCBInfoDefered(IUnitOfWork uow, string family, string partNo);

        void UpdateTPCBInfoDefered(IUnitOfWork uow, TPCBInfo item);

        void InsertTPCBInfoDefered(IUnitOfWork uow, TPCBInfo item);

        #endregion

        #endregion
    }
}

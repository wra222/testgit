// INVENTEC corporation (c)2011 all rights reserved. 
// Description: CartonSSCC
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-29   Chen Xu (eb1-4)              create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;

namespace IMES.FisObject.PAK.CartonSSCC
{
    /// <summary>
    /// CartonSSCC的Repository接口
    /// </summary>
    public interface ICartonSSCCRepository : IRepository<CartonSSCC>
    {
        ///// <summary>
        ///// 保存生成的CartonSSCC
        ///// </summary>
        ///// <param name="currentCartonSSCC"></param>
        //void GenerateCartonSSCC(CartonSSCC currentCartonSSCC);

        /// <summary>
        /// 根据cartonSNList删除CartonSCC表的记录
        /// </summary>
        /// <param name="cartonSNList"></param>
        void DeleteCartonSCC(IList<string> cartonSNList);

        /// <summary>
        /// INSERT CartonStatus
        /// </summary>
        /// <param name="item"></param>
        void AddCartonStatusInfo(CartonStatusInfo item);

        /// <summary>
        /// INSERT CartonLog
        /// </summary>
        /// <param name="item"></param>
        void AddCartonLogInfo(CartonLogInfo item);

        /// <summary>
        /// SELECT * FROM CartonInfo WHERE CartonNo=@cn AND InfoType='BoxId'
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<CartonInfoInfo> GetCartonInfoInfo(CartonInfoInfo condition);

        /// <summary>
        /// DELETE CartonInfo WHERE artonNo=@cn AND InfoType='BoxId'
        /// </summary>
        /// <param name="condition"></param>
        void DeleteCartonInfo(CartonInfoInfo condition);

        /// <summary>
        /// UPDATE CartonInfo SET InfoValue = @ucc, Editor = @editor, Udt = GETDATE()
        /// WHERE CartonNo = @cn AND InfoType = 'UCC'
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateCartonInfo(CartonInfoInfo setValue, CartonInfoInfo condition);

        /// <summary>
        /// INSERT INTO CartonInfo([CartonNo],[InfoType],[InfoValue],[Editor],[Cdt],[Udt])
        /// VALUES(@cn, 'UCC', @ucc, @editor, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertCartonInfo(CartonInfoInfo item);

        /// <summary>
        /// 更新CartonStatus
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateCartonStatus(CartonStatusInfo setValue, CartonStatusInfo condition);

        /// <summary>
        /// [dbo].[op_TemplateCheck]
        /// 输入
        ///   @DN nvarchar(10), 
        ///   @doctpye nvarchar(100)
        /// 输出
        ///   select @templatename
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="doctype"></param>
        /// <returns></returns>
        string GetTemplateNameViaCallOpTemplateCheck(string dn, string doctype);

        IList<CartonStatusInfo> GetCartonStatusInfo(CartonStatusInfo condition);

        //IList<string> GetCartonIdsByInfoValueAndCartonId_OnTrans(string infoType, string infoValue);

        #region . Defered .

        void AddCartonStatusInfoDefered(IUnitOfWork uow, CartonStatusInfo item);

        void AddCartonLogInfoDefered(IUnitOfWork uow, CartonLogInfo item);

        void DeleteCartonInfoDefered(IUnitOfWork uow, CartonInfoInfo condition);

        void UpdateCartonInfoDefered(IUnitOfWork uow, CartonInfoInfo setValue, CartonInfoInfo condition);

        void InsertCartonInfoDefered(IUnitOfWork uow, CartonInfoInfo item);

        void UpdateCartonStatusDefered(IUnitOfWork uow, CartonStatusInfo setValue, CartonStatusInfo condition);

        InvokeBody CheckTheBoxIdAndInsertOrUpdateDefered(IUnitOfWork uow, CartonInfoInfo value, CartonInfoInfo cond, string key);

        #endregion
    }
}

// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Maintain TPCB and TP
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-22   Chen Xu (eb1-4)              create
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
    /// Maintain TPCB的Repository接口
    /// </summary>
    public interface ITPCBRepository : IRepository<TPCB>
    {
        /// <summary>
        /// 根据tpcb和tp号，取得VCode信息
        /// SELECT Vcode FROM GetData..TPCB_Maintain WHERE TPCB = @TPCB AND Tp = @Tp
        /// </summary>
        /// <param name="tpcb">tpcb</param>
        /// <param name="tp">tp</param>
        /// <returns>VCode Info</returns>
        IList<string> GetVCodeInfo(string tpcb, string tp);

        /// <summary>
        /// 查询vcode是否已绑定
        /// SELECT * FROM GetData..TPCB_Maintain WHERE Vcode = @Vcode
        /// </summary>
        /// <param name="vcode">vcode</param>  
        /// <returns>VCode Info</returns>
        IList<VCodeInfo> GetVCodeCombineInfo(string vcode);

        /// <summary>
        /// 更新或插入 tpcb,tp和vcode信息
        ///   IF EXISTS(SELECT * FROM GetData..TPCB_Maintain WHERE TPCB = @TPCB AND Tp = @Tp)
        ///      UPDATE [TPCB_Maintain] SET [Vcode] = @Vcode, [Editor] = @Editor, [Cdt] = GETDATE() WHERE TPCB = @TPCB AND Tp = @Tp)
        ///   ELSE
        ///      INSERT INTO [TPCB_Maintain]([TPCB],[Tp],[Vcode],[Editor],[Cdt])  VALUES(@TPCB, @Tp, @Vcode, @Editor, GETDATE())
        /// </summary>
        /// <param name="value">value</param>
        void SaveVCodeInfo(VCodeInfo value);
       
        /// <summary>
        /// 根据vcode号，删除vcode数据相关信息
        /// DELETE GetData..TPCB_Maintain WHERE Vcode = @Vcode
        /// </summary>
        /// <param name="vcode">vcode</param>
        void DeleteVcodeInfo(string vcode);

        /// <summary>
        /// 检查用户输入的[TPCB]/[T/P]/[VCode]，在数据库中是否存在
        /// IF EXISTS(SELECT * FROM GetData..TPCB_Maintain WHERE TPCB = @TPCB AND Tp = @Tp AND Vcode = @Vcode)
        ///     PRINT 'Exist'
        /// ELSE
        ///     PRINT 'Not Exist'
        /// </summary>
        /// <param name="tpcb">tpcb</param>
        /// <param name="tp">tp</param>
        /// <param name="vcode">vcode</param>
        /// <returns>成功执行返回 影响行数 >1,否则返回 0 </returns>
        int CheckVCode(string tpcb, string tp, string vcode);

        /// <summary>
        /// 显示全部vcode数据相关信息（全集）
        /// SELECT * FROM GetData..TPCB_Maintain ORDER BY TPCB,Tp,Vcode
        /// </summary>
        /// <returns>返回VCodeInfo信息 ：TPCB | T/P | Vcode | Editor | Create Date 信息</returns>
        IList<VCodeInfo> QueryAll();

        #region Defered

        void SaveVCodeInfoDefered(IUnitOfWork uow, string tpcb, string tp, string vcode);

        void DeleteVcodeInfoDefered(IUnitOfWork uow, string vcode);

        #endregion
    }
}

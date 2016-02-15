using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.PrintLog
{
    /// <summary>
    /// PrintLog对象Repository接口
    /// </summary>
    public interface IPrintLogRepository : IRepository<PrintLog>
    {
        IList<PrintLog> GetPrintLogListByDescr(string descr);

        /// <summary>
        /// 在PrintLog中查找
        /// ProdId在BegNo和EndNo中范围内，且MO满足1的条件
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        IList<PrintLog> GetPrintLogListByRange(string prodId);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PrintLog> GetPrintLogListByCondition(PrintLog condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="descr"></param>
        /// <returns></returns>
        bool CheckExistPrintLogByLabelNameAndDescr(string name, string descr);
        
        /// <summary>
        /// select * from IMES2012_GetData..PrintLog where @mbSno between BegNo and EndNo and Name=@Tp'
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<PrintLog> GetPrintLogListByRange(string mbSno, string name);

        /// <summary>
        /// 往PrintList表插入记录 
        /// </summary>
        /// <param name="item"></param>
        void InsertPrintListInfo(PrintListInfo item);

        #region . Defered .

        void InsertPrintListInfoDefered(IUnitOfWork uow, PrintListInfo item);

        #endregion


        #region  add check Print log by vincent
        /// <summary>
        /// SELECT top 1 ID  FROM PrintLog WHERE BegNo<=@mbSno AND EndNo>=@mbSno AND Name=@Name
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckPrintLogListByRange(string mbSno, string name);
        /// <summary>
        ///  SELECT top 1 ID  FROM PrintLog WHERE BegNo<=@mbSno AND EndNo>=@mbSno AND Name=@Name and LabelTemplate=@LabelTemplate
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="name"></param>
        /// <param name="labelTemplate"></param>
        /// <returns></returns>
        bool CheckPrintLogListByRange(string mbSno, string name, string labelTemplate);
        /// <summary>
        /// SELECT top 1 ID  FROM PrintLog WHERE BegNo=@begSno AND EndNo=@endSno AND Name=@Name and LabelTemplate=@LabelTemplate
        /// </summary>
        /// <param name="begSno"></param>
        /// <param name="endSno"></param>
        /// <param name="name"></param>
        /// <param name="labelTemplate"></param>
        /// <returns></returns>
        bool CheckPrintLogListByRange(string begSno,string endSno, string name, string labelTemplate);
        #endregion
    }
}

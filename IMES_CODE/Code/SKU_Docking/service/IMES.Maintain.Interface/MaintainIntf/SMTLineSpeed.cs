using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface SMTLineSpeed
    {
        /// <summary>
        /// 添加一条SMT Line记录.
        /// </summary>
        /// <param name="?"></param>
        void AddSMTLine(SmtctInfo item);
        /// <summary>
        /// 删除选中的SMT Line记录.
        /// </summary>
        /// <param name="item"></param>
        void RemoveSMTLine(SmtctInfo item);
        /// <summary>
        /// 得到所有的SMT Line记录.
        /// </summary>
        /// <returns></returns>
        IList<SmtctInfo> GetAllSMTLineItems(string line);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cond"></param>
        void UpdateLotSMTLine(SmtctInfo item, SmtctInfo cond);

         /// <summary>
        /// 
        /// </summary>
        /// <param name="stage"></param>
        DataTable GetLine(string stage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="family"></param>
        IList<string> GetFamily(string family);
    }
}
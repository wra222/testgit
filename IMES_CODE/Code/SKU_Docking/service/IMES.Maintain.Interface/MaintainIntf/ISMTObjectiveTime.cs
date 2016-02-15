/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for SMT Objective Time Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-7-11   Jessica Liu            Create
* Known issues:
* TODO：
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// ISMTObjectiveTime接口
    /// </summary>
    public interface ISMTObjectiveTime
    {
        /// <summary> 
        ///取得所有的SMTLine记录
        /// </summary>
        /// <returns>返回数据库中所有存在的SMTLine记录</returns>
        IList<SMTLineDef> GetAllSMTLineInfo();

        /// <summary>
        /// 删除选中的一条SMTLineDef记录
        /// </summary>
        /// <param name="item">被选中的SMTLineDef</param>
        void DeleteOneSMTLine(SMTLineDef item);

        /// <summary>
        /// 添加一条符合条件的SMTLineDef记录,
        /// 当所添加的记录中的assembly在其他记录中存在时,抛出异常
        /// </summary>
        /// <param name="item"></param>
        void AddOneSMTLine(SMTLineDef item);

        /// <summary>
        /// 更新选中记录,
        /// 当此记录中assembly在其他的记录中存在时,抛出异常
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="oldLine"></param>
        void UpdateOneSMTLine(SMTLineDef newItem, string oldLine);

        /// <summary>
        /// 获取当前的时间
        /// </summary>
        DateTime GetCurDate();

        /// <summary>
        /// 获取Line信息列表
        /// </summary>
        /// <returns>返回数据库中所有存在的Line相关记录</returns>
        DataTable GetLineList();

        /*
        /// <summary>
        /// 根据输入的assembly模糊查询(assembly%)
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>返回符合条件的SMTLine记录</returns>
        IList<SMTLineDef> GetSMTLineByAssembly(string assembly);
        */
    }
}

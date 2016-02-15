using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IDept
    {
        /// <summary>
        /// 取得Dept List
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeptList();

        /// <summary>
        /// 取得 Section data
        /// select distinct(Section) from Dept where Dept=[Dept]
        /// union 
        /// select 'SMT1A'
        /// union
        /// select 'SMT1B'
        /// </summary>
        /// <returns></returns>
        IList<string> GetSectionList(DeptInfo condition);

        /// <summary>
        /// 取得Line List
        /// select * from Dept 
        /// where Dept = [Dept]
        /// and Section like '[Section]%'
        /// order by Dept, Section, Line, FISLine
        /// </summary>
        /// <returns></returns>
        IList<DeptInfo> GetLineList(DeptInfo eqCondition, DeptInfo likeCondition);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        void DeleteDeptInfo(DeptInfo condition);

        /// <summary>
        /// 添加记录 
        /// </summary>
        /// <returns></returns>
        string AddDeptInfo(DeptInfo item);


        /// <summary>
        /// 更新记录
        /// </summary>
        /// <returns></returns>
        void UpdateDeptInfo(DeptInfo setValue, DeptInfo condition);

        /// <summary>
        /// 获取全部Line list
        /// </summary>
        /// <returns></returns>
        IList<DeptInfo> GetDeptInfoList(DeptInfo condition);


 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public interface IGrade
    {
        /// <summary>
        /// 查询所有的Grades
        /// </summary>
        /// <returns></returns>
        IList<GradeInfo> GetAllGrades();
       /// <summary>
        /// 查询所有GradeInfo 按照Family
       /// </summary>
       /// <param name="family"></param>
       /// <returns></returns>
        IList<GradeInfo> GetGradesByFamily(string family);
       /// <summary>
        /// 添加GradeInfo
        /// 当添加的记录中的Family,Series,Grade在其他存在的记录中重复时,抛出异常
       /// </summary>
       /// <param name="grade"></param>
       /// <returns>返回被添加数据的ID</returns>
        string AddSelectedGrade(GradeInfo grade);
        /// <summary>
        /// 删除选中的Grade
        /// </summary>
        /// <param name="id"></param>
        void DeleteSelectedGrade(int id);
        /// <summary>
        /// 更新Grade
        /// 当添加的记录中的Family,Series,Grade在其他存在的记录中重复时,抛出异常
        /// 当所要更新的记录在数据库中不存在的时,抛出异常
        /// </summary>
        /// <param name="newItem"></param>
        void UpdateSelectedGrade(GradeInfo newItem);
       /// <summary>
       /// 查询所有的Family
       /// </summary>
       /// <returns></returns>
        IList<string> GetAllFamilys();
       /// <summary>
       /// 得到所有的HP family...
       /// </summary>
       /// <param name="customerId"></param>
       /// <returns></returns>
        IList<string> GetHPFamilys();

    }
}

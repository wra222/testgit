using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ITestMB
    {

        /// <summary>
        /// 1.添加MB_Test一条数据
        /// </summary>
        /// <param name="mbTest"></param>
        void addMB_Test(MB_TestDef mbTest);

        /// <summary>
        /// 2.删除一条MB_Test数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="family"></param>
        /// <param name="type"></param>
        void deleteMB_Test(string code, string family, bool type);

        ///
        /// 3.根据family查询所有testMB数据
        IList<MB_TestDef> getMBTestbyFamily(string family);

        /// <summary>
        /// 5.根据code，family和type获取testMB数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="family"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<MB_TestDef> GetMBTestByCodeFamilyAndType(string code,string family,bool type);
    }
}

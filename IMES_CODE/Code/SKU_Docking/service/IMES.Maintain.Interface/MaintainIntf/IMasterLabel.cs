using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface
{
    public interface IMasterLabel
    {
        /// <summary>
        /// 查询所有的masterlabel的记录.
        /// </summary>
        /// <returns></returns>
        IList<MasterLabelDef> GetAllMasterLabels();
        /// <summary>
        /// 删除选中的masterLabel记录.
        /// </summary>
        /// <param name="id"></param>
        void RemoveMasterLabelItem(int id);
        /// <summary>
        /// 根据vc,family查找符合条件的记录
        /// </summary>
        /// <param name="vc">vc 字段</param>
        /// <param name="family">family 字段</param>
        /// <returns></returns>
        IList<MasterLabelDef> GetMasterLabelByVCAndCode(string vc, string family);
        /// <summary>
        /// 添加或更新一条masterlabel
        /// </summary>
        /// <param name="ml"></param>
        void SaveMasterLabelItem(MasterLabelDef ml);
        
    }
}

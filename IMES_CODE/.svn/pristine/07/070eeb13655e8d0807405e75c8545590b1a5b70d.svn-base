using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPODLabelPartMaintain
    {
        /// <summary>
        ///  取得PartNo相关的PODLabel数据的list(按Family列的字母序排序)
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<PODLabelPartDef> GetPODLabelPartListByPartNo(String PartNo);

        /// <summary>
        /// 取得某family和起始包含Partno字符串的PODLabelPart数据的list
        /// </summary>
        /// <param name="PartNo"></param>
        /// <param name="Family"></param>
        /// <returns></returns>
        IList<PODLabelPartDef> GetListByPartNoAndFamily(String PartNo, String Family);

        /// <summary>
        /// 取得所有PODLabel数据的list(按Family列的字母序排序)
        /// </summary>
        /// <returns></returns>
        IList<PODLabelPartDef> GetPODLabelPartList();

        /// <summary>
        /// 保存一条PODLabel的记录数据(update)
        /// </summary>
        /// <param name="Object"></param>
        void UpdatePODLabelPart(PODLabelPartDef obj, String PartNo, String Family);

        /// <summary>
        /// 保存一条PODLabel的记录数据(update)
        /// </summary>
        /// <param name="Object"></param>
        void UpdatePODLabelPart(PODLabelPartDef obj, String PartNo);

        /// <summary>
        /// 添加一条PODLabelPart记录
        /// </summary>
        /// <param name="obj"></param>
        void AddPODLabelPart(PODLabelPartDef obj);

        /// <summary>
        /// "删除一条PODLabel的记录数据
        /// </summary>
        /// <param name="?"></param>
        void DeletePODLabelPart(String PartNo);
    }
}

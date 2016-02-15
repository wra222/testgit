// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.FisObject.Common.MO;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// [Model] 实现如下功能：
    /// 使用此界面來维护Model资料.
    /// </summary>
    public interface IMOBOMManager
    {
        /// <summary>
        /// 选项包括Status栏位不是“C”（已出货）的所有MO
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns>MO List</returns>
        IList<MOMaintainInfo> GetNonCMOList();                


        /// <summary>
        /// 取得MOBOM的记录数据
        /// </summary>
        /// <param name="Object">Object</param>
        /// <returns>MOBOMMaintainInfo List</returns>
        IList<MOBOMMaintainInfo> GetMOBOMList(string MO);

        /// <summary>
        /// 根据MO,group，Devilation=1查询MOBOM表，取得MOBOMList
        /// </summary>
        /// <param name="MO">MO</param>
        /// <param name="Group">Group</param>
        /// <param name="Deviation">Deviation</param>
        IList<MOBOMMaintainInfo> GetMOBOMListByGroup(string MO, int Group, bool Deviation, int mobomId);             



        /// <summary>
        /// 取得一条MOBOM的记录数据
        /// </summary>
        /// <param name="id">id</param>
        MOBOMMaintainInfo GetMOBOM(string id);                 

        /// <summary>
        /// 新增一条MOBOM的记录数据,return id.检查待新增的part no在同一MO、Devilation下是否已存在相同的Part No。若是，返回true,否则，false 
        /// </summary>
        /// <param name="Object">Object</param>
        int AddMOBOM(MOBOMMaintainInfo Object);


        /// <summary>
        /// 保存一条MOBOM的记录数据,检查有无当前MO的Devilation等于0的记录存在。复制当前MO的所有记录并将它们的Devilation栏位设置为0
        /// </summary>
        /// <param name="Object">Object</param>
        void SaveMOBOM(MOBOMMaintainInfo Object, string oldMo, string oldPartNo);


        /// <summary>
        /// 删除一条MOBOM的记录数据, 将当前被选记录的对应Devilation等于0的记录的Action栏位设置为“DELETE”，同时删除Devilation等于1的当前记录。
        /// </summary>
        /// <param name="id">id</param>
        void DeleteMOBOM(string id);


        /// <summary>
        /// 从MO表取得一条MO的记录
        /// </summary>
        /// <param name="MO">MO</param>
        MOMaintainInfo GetMOInfo(string MO);
 
        /// <summary>
        /// 向MOBOM表保存mobomId对应记录的Group值
        /// </summary>
        /// <param name="MO">mobomId</param>
        /// <param name="MO">Group</param>
        void SaveGroupNo(int mobomId, string mo, int Group);

        /// <summary>
        /// 该Date是MO BOM中当前MO相关所有记录的Udt的最大值
        /// </summary>
        /// <param name="mo">mo</param>
        DateTime GetMaxUdt(string mo);

    }

}

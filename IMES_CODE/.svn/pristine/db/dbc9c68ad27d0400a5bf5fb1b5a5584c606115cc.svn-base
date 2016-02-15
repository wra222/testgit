using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IRework
    {
        /// <summary>
        ///根据productIDList取得product的详细信息
        /// </summary>
        /// <returns></returns>
        IList<ProductInfoMaintain> GetProductListByIDList(IList<string> productIDList);

        /// <summary>
        ///返回不合法的productID，逗号分割
        /// </summary>
        /// <returns></returns>
        string ReturnInvalidProductID(IList<string> productIDList);

        //检查输入的ProductID是否是已经存在的ProductID
        bool CheckExistedProdId(string strProdId);

        //取得Product的详细信息
        ProductInfoMaintain GetProductInfo(string strProdId);

        /// <summary>
        /// 创建Rework，检查Product List中所有Product是否可以做Rework，若不可以，则提示用户，放弃后续操作。
        /// </summary>
        /// <returns></returns>
        string CreateRework(IList<string> productIDList,string editor); 

        /// <summary>
        /// 选项包括Rework_ReleaseType表中的所有Process,按字符序排列 
        /// </summary>
        /// <returns></returns>
        IList<ProcessMaintainInfo> GetProcessList();

        /// <summary>
        /// 取得指定时间段内修改过的Rework记录
        /// </summary>
        /// <returns></returns>
        DataTable GetReworkList(string strDateFrom, string strDateTo);

        /// <summary>
        ///设置strReworkCode与strProcess相关联 
        /// </summary>
        /// <returns></returns>
        void SetProcess(string strProcess, string strReworkCode, string editor);

        /// <summary>
        /// 删除Rework记录 
        /// </summary>
        /// <returns></returns>
        void DeleteRework(string strReworkCode);

        /// <summary>
        /// 判断Rework是否可以Submit 
        /// </summary>
        /// <returns></returns>
        void CheckReworkSubmit(string reworkCode);

        /// <summary>
        /// 判断Rework是否有ReleaseType 
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllReleaseTypeByRework(string reworkCode);

        /// <summary>
        /// 将被选Rework的状态改为1(Submit)  
        /// </summary>
        /// <returns></returns>
       void SubmitRework(string strReworkCode, string editor);

        /// <summary>
       /// 将被选Rework的状态改为2(Confirm)  ,复制数据, 解绑
        /// </summary>
        /// <returns></returns>
       void ConfirmRework(string strReworkCode, string editor);

      /// <summary>
      /// 将被选Rework的状态改为2(Confirm)  ,复制数据, 解绑
      /// </summary>
      /// <returns></returns>
       DataTable GetProductListByReworkCode(string strReworkCode);

       //取得Rework下的Product，按Model分组
       IList<ModelAndCount> GetProductListByReworkGroupByModel(string reworkCode); 

       //取得Rework下的Product，按Station分组
       IList<StationAndCount> GetProductListByReworkGroupByStation(string reworkCode);

       //取得Rework下的unit的数量
       int GetUnitCountByRework(string reworkCode);
       
      //取得Rework的ProcessStation
       IList<ProcessStationInfo> GetProcessStationList(string reworkCode);
    }
}

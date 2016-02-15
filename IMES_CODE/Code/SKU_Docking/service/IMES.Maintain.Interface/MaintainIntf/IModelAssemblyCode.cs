using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public interface IModelAssemblyCode
    {
        /// <summary>
        /// 得到所有的ModelAssembly记录
        /// </summary>
        /// <returns></returns>
       IList<ModelAssemblyCodeInfo> GetAllModelAssemblyCodeInfo();


        /// <summary>
       /// 添加一条ModelAssembly
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
       void AddModelAssemblyCodeInfo(ModelAssemblyCodeInfo item);

        /// <summary>
       /// Delete ModelAssembly
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
       void DelModelAssemblyCodeInfo(string astType, string astCode);

        /// <summary>
       ///   update ModelAssembly
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
       void UpdateModelAssemblyCodeInfo(ModelAssemblyCodeInfo item, string astType, string astCode);
        void CheckDuplicateData(string astType, string astCode);
    }
}

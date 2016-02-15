// created by itc207024 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IModelWeightTolerance
    {
        /// <summary>
        /// 获得ModelWeightTolerance列表
        /// </summary>
        /// <returns></returns>
        IList<ModelWeightTolerance> GetModelWeightToleranceList(string customer,string family);

        /// <summary>
        /// 添加新纪录
        /// </summary>
        /// <returns></returns>
        void AddModelWeightTolerance(ModelWeightTolerance modelWeightTolerace);

        /// <summary>
        /// 更新纪录
        /// </summary>
        /// <returns></returns>
        void UpdateModelWeightTolerance(ModelWeightTolerance modelWeightTolerace,string model);

        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <returns></returns>
        void DeleteModelWeightTolerance(string model);

        /// <summary>
        /// 判断Model或者Customer是否已经存在
        /// </summary>
        /// <returns></returns>
        bool IFModelIsExists(string model);
    }
}

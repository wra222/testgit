/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: PAKCHN(TW)LabelLightNo Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012/05/18   kaisheng           (Reference Ebook SourceCode) Create
 * * issue:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public interface IPAKLabelLightNo
    {
        /// <summary>
        /// 查询所有的PakChnTwLightInfo
        /// </summary>
        /// <returns></returns>
        IList<PakChnTwLightInfo> GetAllPAKLabelLightNo();
        /// <summary>
        /// 添加PAKLabelLightNo
        /// 当添加的记录在其他存在的记录中重复时,抛出异常
        /// </summary>
        /// <param name="LightNoItem"></param>
        /// <returns>返回被添加数据的ID</returns>
        int AddSelectedPAKLabelLightNo(PakChnTwLightInfo LightNoItem);
        /// <summary>
        /// 删除选中的PAKLabelLightNo
        /// </summary>
        /// <param name="LightNoItem"></param>
        void DeleteSelectedPAKLabelLightNo(PakChnTwLightInfo LightNoItem);
        /// <summary>
        /// 更新PAKLabelLightNo
        /// 当添加的记录在其他存在的记录中重复时,抛出异常
        /// 当所要更新的记录在数据库中不存在的时,抛出异常
        /// </summary>
        /// <param name="LightNoItem"></param>
        void UpdateSelectedPAKLabelLightNo(PakChnTwLightInfo LightNoItem);
          

    }
}

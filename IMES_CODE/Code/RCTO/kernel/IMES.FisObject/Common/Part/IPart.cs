﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Part
{
    ///<summary>
    /// Part接口
    ///</summary>
    public interface IPart : IAggregateRoot
    {
        /// <summary>
        /// PartNumber, Part的唯一性标识
        /// </summary>
        string PN { get; set;}

        /// <summary>
        /// Same with DB Name
        /// </summary>
        string PartNo { get; }

        /// <summary>
        /// BomNode type
        /// </summary>
        string BOMNodeType { get; }

        /// <summary>
        /// Part具体类型: CPU, DIMM
        /// </summary>
        string Type { get; set;}

        /// <summary>
        /// same as Table Column Name
        /// </summary>
        string PartType { get; }

        /// <summary>
        /// 类型分组
        /// </summary>
        string TypeGroup { get;}

        /// <summary>
        /// Part的CustomerPn
        /// </summary>
        string CustPn { get; set;}

        /// <summary>
        /// same as table column name
        /// </summary>
        string CustPartNo { get; }

        /// <summary>
        /// Part描述信息
        /// </summary>
        string Descr { get; set;}

        /// <summary>
        /// Remark
        /// </summary>
        string Remark { get; set;}

        /// <summary>
        /// AutoDL
        /// </summary>
        string AutoDL { get; set;}

        /// <summary>
        /// 维护人员工号
        /// </summary>
        string Editor { get; set;}

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime Udt { get; set;}

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime Cdt { get; set;}

        /// <summary>
        /// Part描述信息 2
        /// </summary>
        string Descr2 { get; set; }

        /// <summary>
        /// Part属性
        /// </summary>
        IList<PartInfo> Attributes { get; }

        /// <summary>
        /// 增加Part属性
        /// </summary>
        /// <param name="attr"></param>
        void AddAttribute(PartInfo attr);

        /// <summary>
        /// 更新Part属性
        /// </summary>
        /// <param name="attr"></param>
        void ChangeAttribute(PartInfo attr);

        /// <summary>
        /// 删除Part属性
        /// </summary>
        /// <param name="attr"></param>
        void DeleteAttribute(PartInfo attr);

        /// <summary>
        /// 获取Part扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        string GetAttribute(string name);

        //根据属性名获取属性值， 属性包括基础属性和扩展属性，不能用于KP类型
        string GetProperty(string propertyName);

        /// <summary>
        /// 根据属性名获取属性值， 属性包括基础属性和扩展属性，仅用于KP类型
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="assemblyCode">属性值</param>
        /// <returns></returns>
        string GetProperty(string propertyName, string assemblyCode);

        /// <summary>
        /// 获取Part扩展属性，仅用于KP类型
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="assemblyCode">assCode</param>
        /// <returns>属性值</returns>
        string GetAttribute(string name, string assemblyCode);

        void SetKey(string key);
    }
}

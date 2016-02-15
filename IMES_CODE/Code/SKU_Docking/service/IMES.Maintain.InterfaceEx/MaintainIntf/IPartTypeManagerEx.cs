﻿// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// [Model] 实现如下功能：
    /// 使用此界面來维护PartType资料.
    /// </summary>
    public interface IPartTypeManagerEx
    {

        IList<string> GetBomNodeTypeList();
        IList<string> GetPartTypeList(string bomNodeType);
        void AddPartType(string partTypeName,string partTypeGroup,string editor);
        void SavePartType(PartTypeMaintainInfo Object, string strOldPartType);
        void SavePartType(PartTypeMaintainInfo Object);
        IList<PartTypeMaintainInfo> GetPartTypeList();
        void DeletePartType(string strOldPartType);
        void DeletePartType(string id, string strOldPartType);
    }
    // 摘要:
    //     Part类型
    
}

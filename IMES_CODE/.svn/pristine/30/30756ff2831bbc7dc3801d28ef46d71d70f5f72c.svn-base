// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 各种类型Part的Match,Check,Bind策略接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-04   YangWeihua                 create
// Known issues:

using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using System;

namespace IMES.FisObject.Common.Part.PartPolicy
{
    ///<summary>
    /// 各种类型Part的Match,Check,Bind策略接口
    ///</summary>
    public interface IPartPolicy
    {

        IFlatBOM FilterBOM(IHierarchicalBOM hierarchicalBOM, string station, object mainObject);

        /// <summary>
        /// Bind a part to an owner(Prodcut/MB/Pizza)
        /// </summary>
        /// <param name="part">Part to be bind</param>
        /// <param name="owner">Part owner</param>
        /// <param name="station">station</param>
        /// <param name="editor">Editor</param>
        /// <param name="key"></param>
        void BindTo(PartUnit part, IPartOwner owner, string station, string editor, string key);

        /// <summary>
        /// Bind a part to an owner(Prodcut/MB/Pizza)
        /// </summary>
        /// <param name="part">Part to be bind</param>
        /// <param name="owner">Part owner</param>
        /// <param name="station">station</param>
        /// <param name="editor">Editor</param>
        /// <param name="key"></param>
        /// <param name="needCommonSaveRule"></param>
        /// <param name="needSaveRule"></param>
        void BindTo(PartUnit part, IPartOwner owner, string station, string editor, string key, Nullable<bool> needCommonSaveRule, Nullable<bool> needSaveRule);

        /// <summary>
        /// Try to match an input to a specified BomPart
        /// </summary>
        /// <param name="subject">An input string to be matched</param>
        /// <param name="bomItem">bomItem</param>
        /// <param name="station">Station</param>
        /// <returns>Match result</returns>
        PartUnit Match(string subject, IFlatBOMItem bomItem, string station);

        /// <summary>
        /// Check a specified part; A FisExcption would be thrown out if check failed.
        /// </summary>
        /// <param name="pu">pu</param>
        /// <param name="item">Bom part</param>
        /// <param name="owner">Part owner</param>
        /// <param name="station">Station</param>
        /// <param name="bom">bom</param>
        void Check(PartUnit pu, IFlatBOMItem item, IPartOwner owner, string station, IFlatBOM bom);

        /// <summary>
        /// To check whether a PartCheckType need unique check according to its checksetting data
        /// </summary>
        bool NeedUniqueCheck { get; }

        /// <summary>
        /// To check whether a PartCheckType need forbid check according to its checksetting data
        /// </summary>
        bool NeedPartForbidCheck { get; }

        /// <summary>
        /// To check whether a PartCheckType need defect component check according to its checksetting data
        /// </summary>
        bool NeedDefectComponentCheck { get; }

        /// <summary>
        /// To repair part Type according to its checksetting data
        /// </summary>
        bool RepairPartType { get; }

        /// <summary>
        /// To check whether a PartCheckType need common save
        /// </summary>
        bool NeedCommonSave { get; }
    }
}

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

namespace IMES.FisObject.Common.Part
{
    ///<summary>
    /// 各种类型Part的Match,Check,Bind策略接口
    ///</summary>
    public interface IPartStrategy
    {
        /// <summary>
        /// Bind a part to an owner(Prodcut/MB/Pizza)
        /// </summary>
        /// <param name="part">Part to be bind</param>
        /// <param name="owner">Part owner</param>
        void BindTo(IProductPart part, IPartOwner owner);

        /// <summary>
        /// Try to match an input to a specified BomPart
        /// </summary>
        /// <param name="target">An input string to be matched</param>
        /// <param name="part">Bom part</param>
        /// <param name="station">Station</param>
        /// <param name="valueType">Value type</param>
        /// <param name="containCheckBit">Whether checkbit is contained in the target string</param>
        /// <returns>Match result</returns>
        bool Match(string target, IBOMPart part, string station, string valueType, out bool containCheckBit);

        /// <summary>
        /// Check a specified part; A FisExcption would be thrown out if check failed.
        /// </summary>
        /// <param name="part">Bom part</param>
        /// <param name="owner">Part owner</param>
        /// <param name="station">Station</param>
        void Check(IBOMPart part, IPartOwner owner, string station);

        /// <summary>
        /// Set PartCheckSetting data to the strategy object
        /// </summary>
        /// <param name="setting">PartCheckSetting data</param>
        void SetPartCheckSetting(IList<PartCheck> setting);

        /// <summary>
        /// To check whether a bompart need partcheck according to its checksetting data
        /// </summary>
        /// <param name="bomPart">Bom part</param>
        /// <returns>Whether a check is required</returns>
        bool IfNeedCheck(BOMPart bomPart);
    }
}

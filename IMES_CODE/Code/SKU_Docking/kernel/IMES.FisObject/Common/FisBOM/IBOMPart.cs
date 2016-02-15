using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.FisObject.Common.FisBOM
{
    ///<summary>
    /// BOM中Part的Interface
    ///</summary>
    public interface IBOMPart : IPart
    {
        /// <summary>
        /// Part
        /// </summary>
        IPart Part { get; }
        

        /// <summary>
        /// 将指定字符串与此Part进行Match
        /// </summary>
        /// <param name="target">指定字符串</param>
        /// <param name="station">当前Station</param>
        /// <param name="valueType">需要匹配的Part属性类别</param>
        /// <returns>是否匹配</returns>
        bool Match(string target, string station, string valueType);
        
        /// <summary>
        /// Part检查，通常包括唯一性检查等
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="station">station</param>
        void Check(IPartOwner owner, string station);
        
        /// <summary>
        /// 检查Part是否被Hold
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="model">model</param>
        void CheckHold(string family, string model);
    }
}

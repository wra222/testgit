using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.FisObjectBase
{
    /// <summary>
    /// FisObject必须实现此接口
    /// </summary>
    public interface IFisObject : IDirty
    {
        ///<summary>
        /// 对象标识, 在同类型的FisObject范围内唯一
        ///</summary>
        object Key { get; }

        ///<summary>
        /// 对象标识, 在各种类型的FisObject范围内唯一
        ///</summary>
        GlobalKey GKey { get; }
    }

    ///<summary>
    /// 对象标识, 在各种类型的FisObject范围内唯一
    ///</summary>
    public class GlobalKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public GlobalKey(object key, string type)
        {
            Key = key;
            Type = type;
        }

        ///<summary>
        /// 对象标识, 在同类型的FisObject范围内唯一
        ///</summary>
        public object Key { get; set; }

        ///<summary>
        /// 对象具体类型
        ///</summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取对象的HashCode
        /// </summary>
        /// <returns>对象的HashCode</returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ this.Type.GetHashCode();
        }

        /// <summary>
        /// 判断指定对象是否与此对象相等
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return obj != null
                && obj is GlobalKey
                && this == (GlobalKey)obj;
        }

        /// <summary>
        /// 比较两个实例是否相等.
        /// </summary>
        /// <param name="base1">第一个实例
        /// <see cref="FisObjectBase"/>.</param>
        /// <param name="base2">第二个实例
        /// <see cref="FisObjectBase"/>.</param>
        /// <returns>如果相等返回true.</returns>
        public static bool operator ==(GlobalKey base1,
            GlobalKey base2)
        {
            // check for both null (cast to object or recursive loop)
            if ((object)base1 == null && (object)base2 == null)
            {
                return true;
            }

            // check for either of them == to null
            if ((object)base1 == null || (object)base2 == null)
            {
                return false;
            }

            if ((base1.Key != base2.Key) || (base1.Type != base2.Type))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 测试两个实例不等.
        /// </summary>
        /// <param name="base1">第一个实例
        /// <see cref="FisObjectBase"/>.</param>
        /// <param name="base2">第二个实例
        /// <see cref="FisObjectBase"/>.</param>
        /// <returns>如果不相等,返回true</returns>
        public static bool operator !=(GlobalKey base1,
            GlobalKey base2)
        {
            return (!(base1 == base2));
        }

    }
}

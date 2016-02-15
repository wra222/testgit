﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Util;
using log4net;
using System.Reflection;

namespace IMES.Infrastructure.FisObjectBase
{
    /// <summary>
    /// FisObject的基类, 提供key属性, 比较方法
    /// </summary>
    public abstract class FisObjectBase: IFisObject
    {
        protected static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string MsgBegin = "BEGIN: {0}::{1}()";
        private static readonly string MsgEnd = "END: {0}::{1}()";
        private static readonly string MsgError = "ERROR: {0}::{1}()";

        protected void LoggingBegin(Type tp, MethodBase method)
        {
            logger.DebugFormat(MsgBegin, tp.Name, method.Name);
        }
        protected void LoggingEnd(Type tp, MethodBase method)
        {
            logger.DebugFormat(MsgEnd, tp.Name, method.Name);
        }
        protected void LoggingError(Type tp, MethodBase method)
        {
            logger.DebugFormat(MsgError, tp.Name, method.Name);
        }
        protected void LoggingInfoFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected FisObjectBase()
        {
        }

        //public static object NewKey()
        //{
        //    return Guid.NewGuid();
        //}

        # region . StateTracker .

        protected StateTracker _tracker = new StateTracker();
        /// <summary>
        /// StateTracker to track state change
        /// </summary>
        public StateTracker Tracker
        {
            get
            {
                return _tracker;
            }
            set
            {
                _tracker = value;
            }
        }
        # endregion

        #region Equality Tests

        ///// <summary>
        /////  测试指定实例是否和本实例相等
        ///// </summary>
        ///// <param name="obj">
        /////  需要与本实例比较的指定实例
        ///// </param>
        ///// <returns>
        /////  如果相等则返回true
        ///// </returns>
        //public override bool Equals(object obj)
        //{
        //    return obj != null
        //        && obj is FisObjectBase
        //        && this == (FisObjectBase)obj;
        //}

        ///// <summary>
        ///// 比较两个实例是否相等.
        ///// </summary>
        ///// <param name="base1">第一个实例
        ///// <see cref="FisObjectBase"/>.</param>
        ///// <param name="base2">第二个实例
        ///// <see cref="FisObjectBase"/>.</param>
        ///// <returns>如果相等返回true.</returns>
        //public static bool operator ==(FisObjectBase base1,
        //    FisObjectBase base2)
        //{
        //    // check for both null (cast to object or recursive loop)
        //    if ((object)base1 == null && (object)base2 == null)
        //    {
        //        return true;
        //    }

        //    // check for either of them == to null
        //    if ((object)base1 == null || (object)base2 == null)
        //    {
        //        return false;
        //    }

        //    if (base1.Key != base2.Key)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// 测试两个实例不等.
        ///// </summary>
        ///// <param name="base1">第一个实例
        ///// <see cref="FisObjectBase"/>.</param>
        ///// <param name="base2">第二个实例
        ///// <see cref="FisObjectBase"/>.</param>
        ///// <returns>如果不相等,返回true</returns>
        //public static bool operator !=(FisObjectBase base1,
        //    FisObjectBase base2)
        //{
        //    return (!(base1 == base2));
        //}

        ///// <summary>
        ///// 获取key的hash code
        ///// </summary>
        ///// <returns>
        ///// key的hash code
        ///// </returns>
        //public override int GetHashCode()
        //{
        //    if (this.Key != null)
        //    {
        //        return this.Key.GetHashCode();
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        #endregion

        #region Implementation of IFisObject

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public abstract object Key { get; }

        ///<summary>
        /// 对象标识, 在各种类型的FisObject范围内唯一
        ///</summary>
        public GlobalKey GKey
        {
            get { return new GlobalKey(Key, this.GetType().FullName); }
        }


        #endregion

        #region Implementation of IDirty

        ///<summary>
        /// 对象是否被修改
        ///</summary>
        public virtual bool IsDirty { get;  set; }

        /// <summary>
        /// 清除对象内部所有Dirty flag
        /// </summary>
        public virtual void Clean()
        {
            IsDirty = false;
        }

        #endregion
    }
}

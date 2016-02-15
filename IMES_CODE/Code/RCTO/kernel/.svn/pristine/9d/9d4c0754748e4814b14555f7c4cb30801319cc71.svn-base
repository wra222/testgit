using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.FisObjectBase
{
    ///<summary>
    /// 用于定义属性改变事件处理方式的类
    ///</summary>
    public class DirtyInterceptor
    {
        ///<summary>
        ///</summary>
        ///<param name="obj">IDirty对象</param>
        ///<param name="oldValue">属性的旧值</param>
        ///<param name="newValue">属性的新值</param>
        public static void OnPropertyChange(object obj, object oldValue, object newValue)
        {
            if (!oldValue.Equals(newValue))
            {
                var dirty = obj as IDirty;
                if (dirty != null)
                    dirty.IsDirty = true;
            }
        }
    }
}

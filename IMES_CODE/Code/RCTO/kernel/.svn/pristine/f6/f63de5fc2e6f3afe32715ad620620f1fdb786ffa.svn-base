// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-23   He Jiang                     Create for tracking object states in a unified way
// Known issues:

using System.Collections;
using System.Data;
using System.Text;
using System;
using System.Reflection;
using log4net;

namespace IMES.Infrastructure.Util
{
    /// <summary>
    /// 状态改变跟踪器
    /// </summary>
    public class StateTracker
    {
        protected static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Hashtable _map = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 将状态标记为Detached
        /// </summary>
        /// <typeparam name="T">被跟踪对象类型</typeparam>
        /// <param name="t">被跟踪对象</param>
        public void MarkAsDetached<T>(T t)
        {
            if (t != null)
                _map.Remove(t);
        }

        ///// <summary>
        ///// 将状态标记为Unchanged
        ///// </summary>
        ///// <typeparam name="T">被跟踪对象类型</typeparam>
        ///// <param name="t">被跟踪对象</param>
        //public void MarkAsUnchanged<T>(T t)
        //{
        //    MarkState(t, DataRowState.Unchanged);
        //}

        /// <summary>
        /// 将状态标记为Added
        /// </summary>
        /// <typeparam name="T">被跟踪对象类型</typeparam>
        /// <param name="t">被跟踪对象</param>
        public void MarkAsAdded<T>(T t)
        {
            MarkState(t, DataRowState.Added);
        }

        /// <summary>
        /// 将状态标记为Deleted
        /// </summary>
        /// <typeparam name="T">被跟踪对象类型</typeparam>
        /// <param name="t">被跟踪对象</param>
        public void MarkAsDeleted<T>(T t)
        {
            MarkState(t, DataRowState.Deleted);
        }

        /// <summary>
        /// 将状态标记为Modified
        /// </summary>
        /// <typeparam name="T">被跟踪对象类型</typeparam>
        /// <param name="t">被跟踪对象</param>
        public void MarkAsModified<T>(T t)
        {
            MarkState(t, DataRowState.Modified);
        }

        /// <summary>
        /// 取得对象状态
        /// </summary>
        /// <typeparam name="T">被跟踪对象类型</typeparam>
        /// <param name="t">被跟踪对象</param>
        /// <returns>对象状态</returns>
        public DataRowState GetState<T>(T t)
        {
            DataRowState state = DataRowState.Detached;
            if (t != null)
            {
                object o = _map[t];
                if (o != null)
                    state = (DataRowState)o;
            }
            return state;
        }

        /// <summary>
        /// 标记对象状态
        /// </summary>
        /// <typeparam name="T">被跟踪对象类型</typeparam>
        /// <param name="t">被跟踪对象</param>
        /// <param name="state">对象状态</param>
        private void MarkState<T>(T t, DataRowState state)
        {
            if (t != null)
            {
                lock (_map.SyncRoot)
                {
                    DataRowState newState = DataRowState.Unchanged;
                    if (!_map.ContainsKey(t))
                    {
                        newState = state;
                    }
                    else
                    {
                        DataRowState drs = (DataRowState)_map[t];
                        switch (drs)
                        {
                            case DataRowState.Added:
                                switch (state)
                                {
                                    case DataRowState.Added:
                                    case DataRowState.Modified:
                                        newState = DataRowState.Added;
                                        break;
                                    case DataRowState.Deleted:
                                        MarkAsDetached(t);
                                        return;
                                }
                                break;
                            case DataRowState.Modified:
                                switch (state)
                                {
                                    case DataRowState.Added:
                                        return;
                                    case DataRowState.Modified:
                                    case DataRowState.Deleted:
                                        newState = state;
                                        break;
                                }
                                break;
                            case DataRowState.Deleted:
                                switch (state)
                                {
                                    case DataRowState.Added:
                                    case DataRowState.Modified:
                                        newState = DataRowState.Modified;
                                        break;
                                    case DataRowState.Deleted:
                                        newState = state;
                                        break;
                                }
                                break;
                        }
                    }
                    _map[t] = newState;
                }
            }
        }

        /// <summary>
        /// 清除所有跟踪的状态
        /// </summary>
        public void Clear()
        {
            lock (_map.SyncRoot)
            {
                //logger.InfoFormat("StateTracker[{0}]; Caller:[{1}].", this.GetHashCode().ToString(), new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);
                //logger.InfoFormat("StateTracker[{0}] content:{1} ", this.GetHashCode().ToString(), this.ToString());
                _map.Clear();
                //logger.InfoFormat("StateTracker[{0}] cleared! ", this.GetHashCode().ToString());
            }
        }

        public StateTracker Merge(StateTracker st)
        {
            if (st != null)
            {
                lock (_map.SyncRoot)
                {
                    IDictionaryEnumerator ide = st._map.GetEnumerator();
                    while (ide.MoveNext())
                    {
                        this.MarkState(ide.Key, (DataRowState)ide.Value);
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// 标记所有的状态为同一个
        /// </summary>
        /// <param name="state">对象状态</param>
        public void MarkAllAs(DataRowState state)
        {
            lock (_map.SyncRoot)
            {
                foreach (object o in _map.Keys)
                    _map[o] = state;
            }
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            lock (_map.SyncRoot)
            {
                foreach (object o in _map.Keys)
                {
                    object obj = _map[o];
                    Type objType = o.GetType();
                    string value = string.Empty;
                    PropertyInfo propi = objType.GetProperty("Key");
                    if (propi != null)
                    {
                        object oVal = propi.GetValue(o, null);//ITC-1122-0130
                        value = oVal == null ? "<NULL>" : oVal.ToString();
                    }
                    else
                    {
                        value = o.ToString();
                    }
                    ret.AppendFormat("Key:{0}, Type:{1}, Value:{2}, State:{3};  ", o.GetHashCode().ToString(), objType.Name, value, Enum.GetName(typeof(DataRowState),((DataRowState)obj)));
                }
            }
            return ret.ToString();
        }
    }
}

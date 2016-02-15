// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 用于确保同一个Product序号不可以在一个service上的多个站同时刷入
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-05   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;

namespace IMES.Infrastructure
{
    /// <summary>
    /// 用于控制并发争用的工具类
    /// </summary>
    public static class BindCacheManager
    {
        /// <summary>
        /// 返回false表示已包含改key
        /// 返回true表示add成功
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Add(string key, string value)
        {
            lock (_syncObj)
            {
                if (bindCacheMap.ContainsKey(key))
                {
                    return false;
                }
                bindCacheMap.Add(key, value);
                return true;
            }
        }

        /// <summary>
        /// 工作流结束时，根据workflowID清空该站缓存的所有Key
        /// </summary>
        /// <param name="value"></param>
        public static void RemoveByValue(string value)
        {
            if (bindCacheMap.ContainsValue(value))
            {
                List<string> keyList = new List<string>();
                foreach ( KeyValuePair<string,string> temp in bindCacheMap)
                {
                    if (temp.Value == value)
                    {
                        keyList.Add(temp.Key);
                    }
                }

                lock (_syncObj)
                {
                    for (int i = 0; i < keyList.Count; i++)
                    {
                        bindCacheMap.Remove(keyList[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 清除指定Key
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            lock (_syncObj)
            {
                if (bindCacheMap.ContainsKey(key))
                {
                    bindCacheMap.Remove(key);
                }
            }
        }

        /// <summary>
        /// 清楚所有缓存
        /// </summary>
        public static void Clear()
        {
            lock (_syncObj)
            {
                bindCacheMap.Clear();
            }
        }

        /// <summary>
        /// 根据指定Key获取对应Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            lock (_syncObj)
            {
                if (bindCacheMap.ContainsKey(key))
                {
                    return bindCacheMap[key];
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据指定Value获取所有对应Key列表
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> GetKeysByValue(string value)
        {
            lock (_syncObj)
            {
                List<string> keyList = new List<string>();
                foreach (KeyValuePair<string, string> temp in bindCacheMap)
                {
                    if (temp.Value == value)
                    {
                        keyList.Add(temp.Key);
                    }
                }

                return keyList;
            }
        }


        private static Dictionary<string, string> bindCacheMap = new Dictionary<string, string>();

        private static object _syncObj = new Object();
    }
}

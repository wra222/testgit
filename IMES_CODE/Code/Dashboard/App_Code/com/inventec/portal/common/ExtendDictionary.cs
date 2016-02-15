/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: a special dictionary that ignore add duplicate key error
 *              or get element ignore key not found error
 * 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-5-12   ZhangXueMin     Create 
 * Known issues:Any restrictions about this file 
 *              1 xxxxxxxx
 *              2 xxxxxxxx
 */

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

//避免重复加入报错

namespace com.inventec.portal.databaseutil
{
    public class ExtendDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public void Add(TKey key, TValue value)
        {
            if (!base.ContainsKey(key))
            {
                try
                {
                    base.Add(key, value);
                }
                catch
                {

                }
            }
        }

        public Object Get(TKey key)
        {
            if (base.ContainsKey(key))
            {
                return base[key];
            }
            return null;
        }

    }
}

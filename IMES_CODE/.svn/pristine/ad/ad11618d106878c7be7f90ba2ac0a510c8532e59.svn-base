using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.ExpressionScript
{
    /// <summary>
    /// not find key return default value
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
    {
        TValue _default;
        /// <summary>
        /// assign Default Value
        /// </summary>
        public TValue DefaultValue
        {
            get { return _default; }
            set { _default = value; }
        }
        /// <summary>
        /// inherit base class
        /// </summary>
        public DictionaryWithDefault() : base() { }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="defaultValue"></param>
        public DictionaryWithDefault(TValue defaultValue)
            : base()
        {
            _default = defaultValue;
        }
        /// <summary>
        /// no data return default value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new TValue this[TKey key]
        {
            get
            {
                TValue t;
                return base.TryGetValue(key, out t) ? t : _default;
            }
            set { base[key] = value; }
        }
    }
}

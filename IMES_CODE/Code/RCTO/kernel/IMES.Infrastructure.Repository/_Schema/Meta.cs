using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Infrastructure.Repository._Schema
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class DBFieldAttribute : Attribute
    {
        public DBFieldAttribute(SqlDbType fieldType, object min, object max, bool canNull, bool isPK, string remark)
        {
            this.fieldType = fieldType;
            this.range = new Range(min, max);
            this.canNull = canNull;
            this.isPK = isPK;
            this.remark = remark;
        }

        public SqlDbType fieldType { get; set; }
        public Range range { get; set; }
        public bool canNull { get; set; }
        public bool isPK { get; set; }
        public string remark { get; set; }
    }

    internal class Range
    {
        private object _max;
        private object _min;

        public Range(object min, object max)
        {
            _min = min;
            _max = max;
        }
        public object Max
        {
            get{return _max;}
            set{_max = value;}
        }
        public object Min
        {
            get{return _min;}
            set{_min = value;}
        }
    }

    //public class DB
    //{
    //    public string dbName { get; set; }
    //    public IDictionary<string,DBField> fields { get; set; }
    //}
}

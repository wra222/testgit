using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Infrastructure.Repository._Metas
{
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
            get { return _max; }
            set { _max = value; }
        }
        public object Min
        {
            get { return _min; }
            set { _min = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class DBFieldAttribute : Attribute
    {
        public DBFieldAttribute(SqlDbType columnType, object min, object max, bool canNull, bool isPK, string remark)
        {
            this.columnType = columnType;
            this.range = new Range(min, max);
            this.canNull = canNull;
            this.isPK = isPK;
            this.remark = remark;
        }

        public SqlDbType columnType { get; set; }
        public Range range { get; set; }
        public bool canNull { get; set; }
        public bool isPK { get; set; }
        public string remark { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public string tableName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class ORMappingAttribute : Attribute
    {
        public ORMappingAttribute(string column)
        {
            this.column = column;
            isDecColumnOrTable = true;
        }

        public ORMappingAttribute(Type table)
        {
            this.table = table;
            isDecColumnOrTable = false;
        }

        public string column { get; set; }
        public bool isDecColumnOrTable { get; set; }
        public Type table { get; set; }
    }
}

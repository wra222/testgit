using System;
using System.Collections.Generic;
using System.Text;

namespace Inventec.HPEDITS.XmlCreator.Database
{
    public interface ITableMappingLookup
    {        
        TableLookupResult.DocumentClass DocClass
        {
            get;
            set;
        }
        /// <summary>
        /// Check if the XML table is actually a table in DB or a field
        /// </summary>
        /// <returns>If entity doesn't exist, then TableLookupResult is null,
        /// if table is DBtable, then will return TableName, if is a field, then will return
        /// field name.</returns>
        TableLookupResult LookupXMLTable(string xmlTableName);
    }
}

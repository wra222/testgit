using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Inventec.HPEDITS.XmlCreator.Database
{
    public sealed class TableLookupResult
    {
        public enum DocumentClass
        {
            Generic,
            PackingList,
            PalletLabelA,
            PalletLabelB,
            BoxLabel
        }
        private bool isResultDBTable;
        private string tableName;
        private string fieldName;
        private string foreignKeyFieldName;
        public static string SAMEASPARENTKEY = "{SAME}";
        public static string IDFIELDNAME = "ID";
        private bool fieldsExactMatch=true;
        private Dictionary<string, string> fieldsMap = new Dictionary<string, string>();
        private bool isGeneric;
        private DocumentClass docClass;

        public DocumentClass DocClass
        {
            get { return docClass; }
            set { docClass = value; }
        }

        public bool IsGeneric
        {
            get { return isGeneric; }
            set { isGeneric = value; }
        }

        public bool FieldsExactMatch
        {
            get { return fieldsExactMatch; }
            set { fieldsExactMatch = value; }
        }

        public string ForeignKeyFieldName
        {
            get { return foreignKeyFieldName; }
            set { foreignKeyFieldName = value; }
        }
	
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }		
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }	
        public bool IsResultDBTable
        {
            get { return isResultDBTable; }
            set { isResultDBTable = value; }
        }        
        private TableLookupResult()
        {
        }
        public static TableLookupResult ConstructResult(
            bool isResultDBTable, string tableName, string fieldName,string foreignKeyFieldName)
        {
            return ConstructResult(isResultDBTable, tableName, fieldName, foreignKeyFieldName, true,null);
        }
        public static TableLookupResult ConstructResult(
            bool isResultDBTable, string tableName, string fieldName, string foreignKeyFieldName,
            bool fieldsExactMatch,Dictionary<string,string> fieldsMap)
        {
            return ConstructResult(isResultDBTable, tableName, fieldName, foreignKeyFieldName, true,null,true,DocumentClass.Generic);
        }
        public static TableLookupResult ConstructResult(
            bool isResultDBTable, string tableName, string fieldName, string foreignKeyFieldName,
            bool fieldsExactMatch, Dictionary<string, string> fieldsMap, bool isGeneric, DocumentClass documentClass)
        {
            TableLookupResult newResult = new TableLookupResult();
            newResult.isResultDBTable = isResultDBTable;
            newResult.tableName = tableName;
            newResult.fieldName = fieldName;
            newResult.foreignKeyFieldName = foreignKeyFieldName;
            newResult.fieldsExactMatch = fieldsExactMatch;
            if (!fieldsExactMatch)
                newResult.fieldsMap = fieldsMap;
            newResult.isGeneric = isGeneric;
            if (!isGeneric)
                newResult.docClass = documentClass;
            return newResult;
        }
        
    }
}

/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: deal sql type switch, make to 
 *              .net data type, and project data input type
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
using System.Collections.Generic;
using System.Text;
using com.inventec.system;
using com.inventec.system.exception;

namespace com.inventec.portal.databaseutil
{
    public class TypeSwitch
    {
        //存放SQL Server数据库类型与.net数据类型的对应
        private static ExtendDictionary<String, Object> sqlTypeDic;
        //存放SQL Server数据库类型与数据条件设置分类的对应
        private static ExtendDictionary<String, String> sqlDealTypeDic;
        //存放与System.Data.DbType的类型
        private static ExtendDictionary<String, Object> DbTypeDic;
        private static Boolean isFillValues = false;

        /*
         * 填充数据类型对应字典
         * param:
         * Return:
         * 
         * Remark: 
         * Example: 
         * Author: ZhangXueMin
         * Date:
         */
        private static void InitSqlTypeDic()
        {
            if (isFillValues != false)
            {
                return;
            }

            sqlTypeDic = new ExtendDictionary<String, Object>();
            sqlDealTypeDic = new ExtendDictionary<String, String>();
            DbTypeDic = new ExtendDictionary<String, Object>();
                
            sqlTypeDic.Add("bigint", SqlDbType.BigInt);
            sqlTypeDic.Add("binary", SqlDbType.VarBinary);
            sqlTypeDic.Add("bit", SqlDbType.Bit);
            sqlTypeDic.Add("char", SqlDbType.Char);
            //for only need support sql server 2005
            //sqlTypeDic.Add("date", SqlDbType.Date);
            sqlTypeDic.Add("datetime", SqlDbType.DateTime);
            //sqlTypeDic.Add("datetime2", SqlDbType.DateTime2);
            //sqlTypeDic.Add("datetimeoffset", SqlDbType.DateTimeOffset);
            sqlTypeDic.Add("decimal", SqlDbType.Decimal);
            sqlTypeDic.Add("float", SqlDbType.Float);
            sqlTypeDic.Add("image", SqlDbType.Binary);
            sqlTypeDic.Add("int", SqlDbType.Int);
            sqlTypeDic.Add("money", SqlDbType.Money);
            sqlTypeDic.Add("nchar", SqlDbType.NChar);
            //sqlTypeDic.Add("ntext", SqlDbType.NText);
            sqlTypeDic.Add("numeric", SqlDbType.Decimal);
            sqlTypeDic.Add("nvarchar", SqlDbType.NVarChar);
            sqlTypeDic.Add("real", SqlDbType.Real);
            //sqlTypeDic.Add("rowversion", SqlDbType.Timestamp);
            sqlTypeDic.Add("smalldatetime", SqlDbType.DateTime);
            sqlTypeDic.Add("smallint", SqlDbType.SmallInt);
            sqlTypeDic.Add("smallmoney", SqlDbType.SmallMoney);
            sqlTypeDic.Add("sql_variant", SqlDbType.Variant);
            //sqlTypeDic.Add("text", SqlDbType.Text);
            //sqlTypeDic.Add("time", SqlDbType.Time);
            sqlTypeDic.Add("timestamp", SqlDbType.Timestamp);
            sqlTypeDic.Add("tinyint", SqlDbType.TinyInt);
            sqlTypeDic.Add("uniqueidentifier", SqlDbType.UniqueIdentifier);
            sqlTypeDic.Add("varbinary", SqlDbType.VarBinary);
            sqlTypeDic.Add("varchar", SqlDbType.VarChar);
            sqlTypeDic.Add("xml", SqlDbType.Xml);

            /////////////////
            DbTypeDic.Add("bigint", DbType.Int64);
            DbTypeDic.Add("binary", DbType.Binary);
            DbTypeDic.Add("bit", DbType.Boolean);
            DbTypeDic.Add("char", DbType.String);
            //for only need support sql server 2005
            //sqlTypeDic.Add("date", SqlDbType.Date);
            DbTypeDic.Add("datetime", DbType.DateTime);
            //sqlTypeDic.Add("datetime2", SqlDbType.DateTime2);
            //sqlTypeDic.Add("datetimeoffset", SqlDbType.DateTimeOffset);
            DbTypeDic.Add("decimal", DbType.Decimal);
            DbTypeDic.Add("float", DbType.Double);
            DbTypeDic.Add("image", DbType.Binary);
            DbTypeDic.Add("int", DbType.Int32);
            DbTypeDic.Add("money", DbType.Decimal);
            DbTypeDic.Add("nchar", DbType.String);
            //sqlTypeDic.Add("ntext", SqlDbType.NText);
            DbTypeDic.Add("numeric", DbType.Decimal);
            DbTypeDic.Add("nvarchar", DbType.String);
            DbTypeDic.Add("real", DbType.Single);
            //sqlTypeDic.Add("rowversion", SqlDbType.Timestamp);
            DbTypeDic.Add("smalldatetime", DbType.DateTime);
            DbTypeDic.Add("smallint", DbType.Int16);
            DbTypeDic.Add("smallmoney", DbType.Decimal);
            DbTypeDic.Add("sql_variant", DbType.Object);
            //sqlTypeDic.Add("text", SqlDbType.Text);
            //sqlTypeDic.Add("time", SqlDbType.Time);
            DbTypeDic.Add("timestamp", DbType.Binary);
            DbTypeDic.Add("tinyint", DbType.Byte);
            DbTypeDic.Add("uniqueidentifier", DbType.Guid);
            DbTypeDic.Add("varbinary", DbType.Binary);
            DbTypeDic.Add("varchar", DbType.String);
            DbTypeDic.Add("xml", DbType.String);

            //////////////////////
            //int decimal float money numeric real
            sqlDealTypeDic.Add("bigint", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("int", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("smallint", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("tinyint", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("decimal", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("float", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("money", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("numeric", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("real", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);
            sqlDealTypeDic.Add("smallmoney", Constants.EDIT_CONDITION_FIELDTYPE_NUMERIC);


            sqlDealTypeDic.Add("bit", Constants.EDIT_CONDITION_FIELDTYPE_BOOLEAN);

            //char text
            sqlDealTypeDic.Add("char", Constants.EDIT_CONDITION_FIELDTYPE_CHAR);
            sqlDealTypeDic.Add("nchar", Constants.EDIT_CONDITION_FIELDTYPE_CHAR);
            //sqlDealTypeDic.Add("ntext", Constants.EDIT_CONDITION_FIELDTYPE_CHAR);
            sqlDealTypeDic.Add("nvarchar", Constants.EDIT_CONDITION_FIELDTYPE_CHAR);
            //sqlDealTypeDic.Add("text", Constants.EDIT_CONDITION_FIELDTYPE_CHAR);
            sqlDealTypeDic.Add("varchar", Constants.EDIT_CONDITION_FIELDTYPE_CHAR);

            //date
            //sqlDealTypeDic.Add("date", Constants.EDIT_CONDITION_FIELDTYPE_DATE);
            sqlDealTypeDic.Add("datetime", Constants.EDIT_CONDITION_FIELDTYPE_DATE);
            //sqlDealTypeDic.Add("datetime2", Constants.EDIT_CONDITION_FIELDTYPE_DATE);
            //sqlDealTypeDic.Add("datetimeoffset", Constants.EDIT_CONDITION_FIELDTYPE_DATE);
            sqlDealTypeDic.Add("smalldatetime", Constants.EDIT_CONDITION_FIELDTYPE_DATE);


            //sqlDealTypeDic.Add("binary", null);
            //sqlDealTypeDic.Add("image", null);
            //sqlDealTypeDic.Add("rowversion", null);
            //sqlDealTypeDic.Add("sql_variant", null);
            //sqlDealTypeDic.Add("time", null);
            //sqlDealTypeDic.Add("timestamp", null);
            //sqlDealTypeDic.Add("uniqueidentifier", null);
            //sqlDealTypeDic.Add("varbinary", null);
            //sqlDealTypeDic.Add("xml", null);  
            
            isFillValues = true;
       
        }


        /*
         * 取得对应的SqlDbType的类型
         * param:_type,原始数据类型._sqlType,对应的SqlDbType的类型.
         * Return:成功，true.失败,false.
         * 
         * Remark: 
         * Example: 
         * Author: ZhangXueMin
         * Date:
         */
        public static Boolean GetDataType(String _type, ref SqlDbType _sqlType)
        {

            InitSqlTypeDic();

            try
            {
                _sqlType = (SqlDbType)sqlTypeDic[_type];
            }
            catch
            {
                //need del
                //ExceptionManager.Throw("Condition data type are not right.");
                return false;
            }
            return true;

        }


        /*
          * 取得对应的SqlDbType数据类型
          * param:_type,原始数据类型.
          * Return:返回对应的SqlDbType数据类型.
          * 
          * Remark: 
          * Example: 
          * Author: ZhangXueMin
          * Date:
          */
        public static SqlDbType GetSqlDbType(String _type)
        {
            InitSqlTypeDic();

            SqlDbType type = SqlDbType.VarChar;
            try
            {
                String itemDataType = "";
                String itemDataTypeLength = "";
                ParseType(_type, ref itemDataType, ref itemDataTypeLength);
                type = (SqlDbType)sqlTypeDic[itemDataType];

            }
            catch
            {
                ExceptionManager.Throw(ExceptionMsg.SQL_GENERATE_MSG_CONDITION_DATA_TYPE_NOT_CORRECT);
            }
            return type;
        }


        public static DbType GetDbType(String _type)
        {
            InitSqlTypeDic();

            DbType type = DbType.String;
            try
            {
                String itemDataType = "";
                String itemDataTypeLength = "";
                ParseType(_type, ref itemDataType, ref itemDataTypeLength);
                type = (DbType)DbTypeDic[itemDataType];

            }
            catch
            {
                ExceptionManager.Throw(ExceptionMsg.SQL_GENERATE_MSG_CONDITION_DATA_TYPE_NOT_CORRECT);
            }
            return type;
        }

        /*
         * 取得几种处理方式的类型
         * param:_type,原始数据类型.
         * Return:处理分类的类型.
         * 
         * Remark: 
         * Example: 
         * Author: ZhangXueMin
         * Date:
         */
        public static String GetDataDealType(String _type)
        {
            InitSqlTypeDic();

            String dealType = "";
            try
            {
                String itemDataType = "";
                String itemDataTypeLength = "";
                ParseType(_type, ref itemDataType, ref itemDataTypeLength);
                dealType = sqlDealTypeDic[itemDataType];
            }
            catch
            {
                //need del
                ExceptionManager.Throw(ExceptionMsg.SQL_GENERATE_MSG_CONDITION_DATA_TYPE_NOT_CORRECT);
            }
            return dealType;
        }

        /*
          * 检查是否是设置参数的合法类型，目前要求，image等数据类型不能作为参数
          * param:_type,原始数据类型.
          * Return:合法true,不合法false.
          * 
          * Remark: 
          * Example: 
          * Author: ZhangXueMin
          * Date:
          */
        public static Boolean isCanSetParamType(String _type)
        {
            InitSqlTypeDic();

            try
            {
                String itemDataType = "";
                String itemDataTypeLength = "";
                ParseType(_type, ref itemDataType, ref itemDataTypeLength);
                SqlDbType type = (SqlDbType)sqlTypeDic[itemDataType];

            }
            catch
            {
                return false;
            }
            return true;
        }

        /*
         * 将数据类型分解
         * param:_initType,原始类型._dataType,分解后的类型._dataTypeLength,类型长度.
         * Return:
         * 
         * Remark: 
         * Example: 
         * Author: ZhangXueMin
         * Date:
         */
        public static void ParseType(String _initType, ref String _dataType, ref String _dataTypeLength)
        {
            String[] types = _initType.ToLower().Split(Constants.SQL_DATA_TYPE_LENGTH_SEPERATE_LEFT.ToCharArray());
            _dataType = types[0];
        }
        
        public TypeSwitch()
        {

        }

    }

}

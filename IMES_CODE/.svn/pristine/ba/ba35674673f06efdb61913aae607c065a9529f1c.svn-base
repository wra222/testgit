/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: include general utility function
 *              
 * 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-5-12   ZhangXueMin     Create 
 * Known issues:Any restrictions about this file 
 *              1 xxxxxxxx
 *              2 xxxxxxxx
 *    2009-11-04  ZhangXueMin       modify bug ITC-1031-0175 (过滤字串)
 */

using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using com.inventec.system;
using com.inventec.system.exception;


namespace com.inventec.portal.databaseutil
{
    
    public class GeneralUtil
    {
        //public const int FIELD_ITEM_TYPE_TABLE = 0;
        //public const int FIELD_ITEM_TYPE_FIELD = 1; 

        ////复制
        //public static object DeepClone(object source)
        //{
        //    if (source == null)
        //    {
        //        return null;
        //    }
        //    Object objectReturn = null;
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        try
        //        {
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            formatter.Serialize(stream, source);
        //            stream.Position = 0;
        //            objectReturn = formatter.Deserialize(stream);
        //        }
        //        catch (Exception e)
        //        {
        //            //log记录
        //        }
        //    }
        //    return objectReturn;
        //}

        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        ////拷贝字符串list
        //public static List<string> CloneStringList(List<string> srcList)
        //{
        //    return  CloneStringList(srcList, true);
        //}

        ////拷贝字符串list
        //public static ArrayList CloneStringList(ArrayList srcList)
        //{
        //    return CloneStringList(srcList, true);
        //}


        ////拷贝字符串list
        //public static List<string> CloneStringList(List<string> srcList,Boolean isNull2String)
        //{
        //    List<string> desList = new List<string>();
        //    for (int i = 0; i < srcList.Count; i++)
        //    {
        //        if (isNull2String == true)
        //        {
        //            desList.Add(Null2String(srcList[i]));
        //        }
        //        else
        //        {
        //            desList.Add(srcList[i]);
        //        }
        //    }
        //    return desList;
        //}

        ////拷贝字符串list
        //public static ArrayList CloneStringList(ArrayList srcList, Boolean isNull2String)
        //{
        //    ArrayList desList = new ArrayList();
        //    for (int i = 0; i < srcList.Count; i++)
        //    {
        //        if (isNull2String == true)
        //        {
        //            desList.Add(Null2String(srcList[i]));
        //        }
        //        else
        //        {
        //            desList.Add(srcList[i]);
        //        }
        //    }
        //    return desList;
        //}

        ////ArrayList转为字符串
        //public static String ListToString(ArrayList srcList)
        //{
        //    String desString = "";
        //    for (int i = 0; i < srcList.Count; i++)
        //    {
        //        desString = desString + srcList[i].ToString() + "|";

        //    }
        //    return desString;
        //}

        ////list转为字符串
        //public static String ListToString(List<String> srcList)
        //{
        //    String desString = "";
        //    for (int i = 0; i < srcList.Count; i++)
        //    {
        //        desString = desString + srcList[i].ToString() + "|";

        //    }
        //    return desString;
        //}


        //取得超时时间
        //by xmzhang
        public static int GetSqlTimeOut()
        {
            String timeOutString = "";
            int timeOut = 0;
            try
            {
                timeOutString = ConfigurationManager.AppSettings["SqlTimeOut"].ToString();
                timeOut = Int32.Parse(timeOutString);
            }
            catch
            {
                return -1;
                //ExceptionManager.Throw(ExceptionMsg.SQL_GENERATE_MSG_TIME_OUT_PARAM_ERROR);
            }
            return timeOut;
        }

        ////取得超时时间
        ////by xmzhang
        //public static int GetDBLinkTimeOut()
        //{
        //    String timeOutString = "";
        //    int timeOut = 0;
        //    try
        //    {
        //        timeOutString = ConfigurationManager.AppSettings["DBLinkTimeOut"].ToString();
        //        timeOut = Int32.Parse(timeOutString);
        //    }
        //    catch
        //    {
        //        return -1;
        //        //ExceptionManager.Throw(ExceptionMsg.SQL_GENERATE_MSG_TIME_OUT_PARAM_ERROR);
        //    }
        //    return timeOut;
        //}

        ////转换SQL的字符串
        //public static String SQLStringChange(String _param)
        //{
        //    String param = _param;
        //    return param.Replace("'", "''");
        //}

        ////数据库名称转换SQL的字符串//need check!!!
        //public static String SQLDatabaseNameChange(String _param)
        //{
        //    String param = _param;
        //    param = param.Replace("]", "]]");
        //    String result = "[" + param + "]";
        //    return result;
        //}

        ////存储过程名称转换SQL的字符串，需要检查规律的正确性//need check!!!
        //public static String SQLProcedureNameChange(String _param)
        //{
        //    String param = _param;
        //    param = param.Replace("]", "]]");
        //    String result = "[" + param + "]";
        //    return result;

        //}

        //public static String SQLLikeStringChange(String strSource)
        //{
        //    String strTemp = strSource;
        //    // strTemp = strTemp.Replace("\\'", "''");
        //    strTemp = strTemp.Replace("[", "[[]");
        //    strTemp = strTemp.Replace("%", "[%]");
        //    strTemp = strTemp.Replace("_", "[_]");
        //    //        strTemp = strTemp.replaceAll("\\^" ,"[^]") ;
        //    strTemp = strTemp.Replace("?", "[?]");
        //    return strTemp;
        //}

        //public static String GetBooleanString(String param)
        //{
        //    String paramString= Null2String(param);
        //    if (paramString.ToLower() == "true" || paramString == "1")
        //    {
        //        return "true";
        //    }
        //    else if (paramString.ToLower() == "false" || paramString == "0")
        //    {
        //        return "false";
        //    }
        //    else
        //    {
        //        String errmsg = ExceptionMsg.SQL_GENERATE_MSG_BIT_PARAM_NOT_RIGHT;
        //        ExceptionManager.Throw(errmsg);
        //    }
        //    return "true";
        //}

        ////将用户定义的字串，特殊字符转换
        //public static void ChangeForSQLUserField(ref String _param)
        //{
        //    Regex rx = new Regex(@"^\'.*\'$");
        //    //符合正则表达式以'开头以'结尾
        //    if (rx.IsMatch(_param))
        //    {
        //        String sub = _param.Substring(1, _param.Length - 2);
        //        _param = "'" + sub.Replace("'", "''") + "'";
        //    }
        //}

        ////将用户定义的字串中的真实字符串取出，去掉两边的单引号
        //public static String GetStringFromUserField(String _param)
        //{
        //    if (_param == null)
        //    {
        //        return null;
        //    }
        //    Regex rx = new Regex(@"^\'.*\'$");
        //    //符合正则表达式以'开头以'结尾
        //    if (rx.IsMatch(_param))
        //    {
        //        String sub = _param.Substring(1, _param.Length - 2);
        //        return sub;
        //    }
        //    return _param;
        //}


        ////判断一个字段元素，如果是字段元素，取得其表名称
        ////返回，表名称, 如果不是表名称字段，返回空字串
        ////参数：字段元素
        //public static String GetTableNameFromFieldItem(String _param)
        //{
        //    Regex rx = new Regex(@"^\[.*\]$");
        //    //符合正则表达式以[开头以]结尾
        //    if (rx.IsMatch(_param))
        //    {
        //        String item = _param;
        //        if (ParseNameFromExpression(ref item, FIELD_ITEM_TYPE_TABLE) == true)
        //        {
        //            return item;
        //        }
        //    }
        //    return Constants.EMPTY_STRING;

        //}


        ////判断是否一个由表名、字段名组成的字段
        ////返回，是否由表名、字段名组成的字段
        ////参数：字段元素
        //public static Boolean isTableFieldItem(String _param)
        //{
        //    Regex rx = new Regex(@"^\[.*\]$");
        //    //符合正则表达式以[开头以]结尾
        //    if (rx.IsMatch(_param))
        //    {
        //        return true;

        //    }
        //    return false;

        //}

        ////取得字段元素的字段名称
        ////返回，字段名称
        ////参数：字段元素
        //public static String GetFieldNameFromFieldItem(String _param)
        //{
        //    String item = _param;
        //    if (ParseNameFromExpression(ref item, FIELD_ITEM_TYPE_FIELD) == true)
        //    {
        //        return item;
        //    }
        //    return Constants.EMPTY_STRING;
        //}

        ////从字段中提取表名称或者字段名称
        //public static Boolean ParseNameFromExpression(ref String _param, int type)
        //{
        //    String[] seperate=new String[1];
        //    seperate[0]= Constants.EDIT_TABLE_FIELD_SEPERATE_EXT;
        //    String[] fields = _param.Split(seperate,StringSplitOptions.None);

        //    if (fields.Length > 1)
        //    {
        //        switch (type)
        //        {
        //            case FIELD_ITEM_TYPE_TABLE:
        //                //将表达式中的表名称加入
        //                _param = fields[0]+"]";
        //                break;

        //            case FIELD_ITEM_TYPE_FIELD:
        //                //将表达式中的字段名称加入
        //                _param = "["+fields[1];
        //                break;
        //        }
        //        return true;
        //    }
        //    return false;

        //}

        ////生成一个与传入的名称都不相重复的名称
        ////生成表的别名
        ////参数：1）baseName，用以生成的基础名称
        ////      2）allExistNames，所有当前存在的名称
        ////返回值：生成的名称
        ////操作：如果baseName在allExistNames中不存在，直接返回baseName;
        ////      否则加上序号直到成功为止
        ////生成的别名外面总有[]
        //public static String GetDifferenceTableName(String _baseName, ExtendDictionary<String, String> allExistNames)
        //{
        //    String baseName="";
        //    //Boolean isNeedDeal = false;

        //    //Regex rx = new Regex(@"^\[.*\]$");
        //    ////符合正则表达式以[开头以]结尾
        //    //if (rx.IsMatch(_baseName))
        //    //{
        //    //    baseName = _baseName.Substring(1, _baseName.Length - 2);
        //    //    //isNeedDeal = true;
        //    //}

        //    baseName=_baseName.Replace("[", "").Trim();
        //    _baseName.Replace("]", "").Trim();

        //    //过滤特殊符号，注意过滤空了需要补充名称
        //    if (baseName == "")
        //    {
        //        baseName = "UserTable";
        //    }

        //    int index = 1;
        //    String aliasName = GetAliasName(baseName,ref index,true);
        //    //if (isNeedDeal == true)
        //    {
        //        aliasName = "[" + aliasName + "]";
        //    }
        //    Boolean success = IsOnlyName(aliasName, allExistNames);
        //    while (success == false)
        //    {
        //        aliasName = GetAliasName(baseName, ref index,true);
        //        //if (isNeedDeal == true)
        //        {
        //            aliasName = "[" + aliasName + "]";
        //        }
        //        success = IsOnlyName(aliasName, allExistNames);
        //    }

        //    return aliasName;

        //}

        ////生成一个与传入的名称都不相重复的名称
        ////生成表的别名
        ////参数：1）baseName，用以生成的基础名称
        ////      2）allExistNames，所有当前存在的名称
        ////返回值：生成的名称
        ////操作：如果baseName在allExistNames中不存在，直接返回baseName;
        ////      否则加上序号直到成功为止
        //public static String GetDifferenceName(String baseName, ExtendDictionary<String, String> allExistNames)
        //{

        //    int index = 1;
        //    String aliasName = GetAliasName(baseName,ref index,false);
        //    Boolean success = IsOnlyName(aliasName, allExistNames);
        //    while (success == false)
        //    {
        //        aliasName = GetAliasName(baseName, ref index,false);
        //        success = IsOnlyName(aliasName, allExistNames);
        //    }

        //    return aliasName;

        //}

        //private static Boolean IsOnlyName(String name,ExtendDictionary<String, String> allExistNames)
        //{
        //    if(allExistNames.ContainsKey(name))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        ////初始基础值1，每次调用基础值加1
        ////isNameStartWithNoNumber是true 时，自动起的第一个名字没有后缀
        //private static String GetAliasName(String baseName,ref int index,Boolean isNameStartWithNoNumber)
        //{
        //    String result = baseName;
        //    if (isNameStartWithNoNumber == true)
        //    {
        //        if (index > 1)
        //        {
        //            result = result + index.ToString();
        //        }
        //    }
        //    else
        //    {
        //        result = result + index.ToString();
        //    }
        //    index=index +1;
        //    return result;
        //}

        ////dataset中的sqlstring 转换，回车换行变为加空格
        //public static String DataSetSqlStringChange(String _param)
        //{
        //    String param = _param;
        //    return param.Replace(System.Environment.NewLine, " " + System.Environment.NewLine);
        //}

       


    }
}

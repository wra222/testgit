using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;

/// <summary>
/// Method 的摘要描述
/// </summary>
public class Method
{
	public Method()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public static String DataSet_Parse(DataSet ds)
    {
        return ds.GetXml();
    }
    //s must xml string
    public static DataSet DataSet_Parse(string s)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(new System.Xml.XmlTextReader(new System.IO.StringReader(s)));

        return ds;
    }
    public static object h_set_Value(Hashtable h, String _key, String _value)
    {

        h.Remove(_key);
        h.Add(_key, _value);
        return h;
    }

    public static object h_get_Value(Hashtable h, String Key)
    {

        IDictionaryEnumerator c = h.GetEnumerator();

        while (c.MoveNext())
        {
            while ((c.Key.ToString()) == Key) return c.Value;
        }
        return "nodata";
    }

    public static string BuildXML(string obj_value, string obj_name)
    {
        StringBuilder myValue = new StringBuilder();
        myValue.Append("<");
        myValue.Append(obj_name);
        myValue.Append(">");
        myValue.Append(obj_value);
        myValue.Append("</");
        myValue.Append(obj_name);
        myValue.Append(">");
        return myValue.ToString();
    }

    public static string GrabXml(string str, string skey)
    {
        if (str == null)
        {
            return "";
        }
        int startlen = str.IndexOf("<" + skey + ">") + ("<" + skey + ">").Length;
        int len = str.IndexOf("</" + skey + ">") - startlen;

        string s_value = "";
        if (len > 0) { s_value = str.Substring(startlen, len); }

        return s_value;

    }

    public static string ChgXml(string str, string skey, string s_newvalue)
    {
        StringBuilder getStr = new StringBuilder();
        StringBuilder oldValue = new StringBuilder();
        StringBuilder newValue = new StringBuilder();
        getStr.Append(str);
        int exists = getStr.ToString().IndexOf(skey);
        if (exists < 0)
        {
            //str = str + BuildXML(s_value, skey);
            str = getStr.Append(BuildXML(s_newvalue, skey)).ToString();
        }
        else
        {
            //str = str.Replace("<" + skey + ">" + GrabXml(str, skey) + "</" + skey + ">", "<" + skey + ">" + s_value + "</" + skey + ">");
            oldValue.Append(BuildXML(GrabXml(str, skey),skey));
            newValue.Append(BuildXML(s_newvalue, skey));

            str = getStr.Replace(oldValue.ToString(), newValue.ToString()).ToString();
        }
        return str;
    }

    public static string GetSqlCmd(string s_proc, string vchCmd, string vchObjectName, string vchSet)
    {
        StringBuilder sqlCmd = new StringBuilder("EXEC ");
        sqlCmd.Append(s_proc);
        sqlCmd.Append(Constant.S_SPACE);
        sqlCmd.Append(" @vchCmd = N'");
        sqlCmd.Append(vchCmd);
        sqlCmd.Append("',");
        sqlCmd.Append(" @vchObjectName = N'");
        sqlCmd.Append(vchObjectName);
        sqlCmd.Append("',");
        sqlCmd.Append(" @vchSet = N'");
        sqlCmd.Append(vchSet);
        sqlCmd.Append("'");

        return sqlCmd.ToString();
    }
}

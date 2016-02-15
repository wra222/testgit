/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: property reader class
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-2-5    itc98047     Create 
 
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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using log4net;

/// <summary>
/// Summary description for PropertyReader
/// </summary>
public class PropertyReader
{
    private static readonly ILog log = LogManager.GetLogger(typeof(PropertyReader));
    private StreamReader sr = null;

    public PropertyReader(string strFilePath)
    {
        sr = new StreamReader(strFilePath);
    }

    /// <summary>
    /// 关闭文件流
    /// </summary>
    public void Close()
    {
        sr.Close();
        sr = null;
    }

    /// <summary>
    /// 根据键获得值字符串
    /// </summary>
    /// <param name="strKey">键</param>
    /// <returns>值</returns>
    public string GetPropertiesText(string strKey)
    {
        string strResult = string.Empty;
        string str = string.Empty;

        sr.BaseStream.Seek(0, SeekOrigin.End);
        sr.BaseStream.Seek(0, SeekOrigin.Begin);

        while ((str = sr.ReadLine()) != null)
        {
            if (str.Trim().Length > 0)
            {
                if (!str.TrimStart().Substring(0, 1).Equals("#")
                    && str.Substring(0, str.IndexOf('=')).Equals(strKey))
                {
                    strResult = str.Substring(str.IndexOf('=') + 1);
                    break;
                }
            }
        }
        return strResult;
    }

    /// <summary>
    /// 根据键获得值数组
    /// </summary>
    /// <param name="strKey">键</param>
    /// <returns>值数组</returns>
    //public string[] GetPropertiesArray(string strKey)
    //{
    //    string strResult = string.Empty;
    //    string str = string.Empty;
    //    sr.BaseStream.Seek(0, SeekOrigin.End);
    //    sr.BaseStream.Seek(0, SeekOrigin.Begin);
    //    while ((str = sr.ReadLine()) != null)
    //    {
    //        if (str.Substring(0, str.IndexOf('=')).Equals(strKey))
    //        {
    //            strResult = str.Substring(str.IndexOf('=')+1);
    //            break;
    //        }
    //    }
    //    return strResult.Split(',');
    //}

    public static String readProperty(String baseName, String propertyName)
    {
        String propertyValue = null;
        String filePath = AppDomain.CurrentDomain.BaseDirectory + "properties\\" + baseName + ".properties";
        PropertyReader pr = new PropertyReader(filePath);
        
        propertyValue = pr.GetPropertiesText(propertyName);
        pr.Close();

        return propertyValue;

    }

}


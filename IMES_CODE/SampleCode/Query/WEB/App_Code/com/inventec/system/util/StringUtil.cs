using System;
using System.Text;

/// <summary>
/// StringUtil 
/// </summary>
namespace com.inventec.system.util {
    public class StringUtil {

        /// <summary>
        /// Convert null to string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Null2String(object obj) {
            if (obj == null) {
                return "";
            }
            return obj.ToString();
        }

        /// <summary>
        /// Code the string to htmlcode.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string htmEncode(string s) {
            if (s == null || s.Equals("")) {
                return "";
            }

            StringBuilder stringbuffer = new StringBuilder();
            int j = s.Length;
            for (int i = 0; i < j; i++) {
                char c = s[i];
                switch (c) {
                    case (char) 60:
                        stringbuffer.Append("&lt;");
                        break;
                    case (char)62:
                        stringbuffer.Append("&gt;");
                        break;
                    case (char)38:
                        stringbuffer.Append("&amp;");
                        break;
                    case (char)34:
                        stringbuffer.Append("&quot;");
                        break;
                    /*case 169: stringbuffer.append("&copy;"); break;
                    case 174: stringbuffer.append("&reg;"); break;
                    case 165: stringbuffer.append("&yen;"); break;
                    case 8364: stringbuffer.append("&euro;"); break;
                    case 8482: stringbuffer.append("&#153;"); break;*/
                    case (char)13:
                        if (i < j - 1 && s[i + 1] == 10) {
                            stringbuffer.Append("<br>");
                            i++;
                        }
                        break;
                    //case 32: stringbuffer.append("&nbsp;"); break;
                    default:
                        stringbuffer.Append(c);
                        break;
                }
            }

            return stringbuffer.ToString();
        }

		public static String convertCharForSql(String source)
		{
			if (source == null || source.Equals(""))
			{
				return "";
			}

			source = source.Replace("'", "''");
			source = source.Replace("\\[", "[[]");
			source = source.Replace("%", "[%]");
			source = source.Replace("_", "[_]");

			return source;
		}

        /*
         * 为chart图标转换字符串
         * Parameters: 
         *      String: 原字符串
         * 
         * Return Value: 
         *      String： 转换后字符串
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc205106 zhao,qingrong 
         * Date:
         *      2009-2-24
         */
        public static String convertCharForChart(String source)
        {
            if (source == null || source.Equals(""))
            {
                return "";
            }

            source = source.Replace("\\", "\\\\");
            source = source.Replace("\"", "\\\"");
            source = source.Replace(",", " ");

            return source;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringUtil() {
        }

        /// <summary>
        /// 用于URL编码后的字串还原
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        /// //add by xmzh 
        public static String decode_URL(String strSource)
        {
            if (strSource == null)
            {
                return "";
            }
            String strMarkString = "#%&+', ";
            String strChangeString = "" + (char)21 + (char)22 + (char)23 + (char)24 + (char)25 + (char)26 + (char)27;
            String strTemp = strSource;
            String strMark;
            String strChange;
            for (int index = 0; index < strMarkString.Length; index++)
            {
                strMark = strMarkString.Substring(index, 1);
                strChange = strChangeString.Substring(index, 1);
                strTemp = strTemp.Replace(strChange, strMark);
            }
            return strTemp;
        }
        

    }
}
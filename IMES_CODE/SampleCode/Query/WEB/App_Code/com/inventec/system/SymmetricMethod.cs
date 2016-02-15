/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-17   zhaoqingrong     Create 
 *
 * Known issues:Any restrictions about this file 
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
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace com.inventec.system
{
    /*
     * 对称加密算法类   
     * Parameters: 
     * 
     * Return Value: 
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc205106 zhao,qingrong 
     * Date:
     *      2008-12-13
     */
    public class SymmetricMethod
    {

        private SymmetricAlgorithm mobjCryptoService;
        private string Key;

        /*
         * 对称加密类的构造函数   
         * Parameters: 
         * 
         * Return Value: 
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc205106 zhao,qingrong 
         * Date:
         *      2008-12-13
         */
        public SymmetricMethod()
        {
            mobjCryptoService = new RijndaelManaged();
            Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%)HilJ$lhj!y6&(*jkP87jH7";
        }

        /*
         * 获得密钥      
         * Parameters: 
         * 
         * Return Value: 
         *      密钥
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc205106 zhao,qingrong 
         * Date:
         *      2008-12-13
         */
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;

            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength, ' ');
            }
            
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /*
         * 获得初始向量IV      
         * Parameters: 
         * 
         * Return Value: 
         *      初试向量IV
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc205106 zhao,qingrong 
         * Date:
         *      2008-12-13
         */
        private byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh)u%g6HJ($jhWk7&!hg4ui%$hjk";
            
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength, ' ');
            }
            
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /*
         * 加密方法      
         * Parameters: 
         *      待加密的串
         * 
         * Return Value: 
         *      经过加密的串
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc205106 zhao,qingrong 
         * Date:
         *      2008-12-13
         */
        [AjaxPro.AjaxMethod]
        public string Encrypto(string Source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            
            byte[] bytOut = ms.ToArray();

            return Convert.ToBase64String(bytOut);
        }

        /*
         * 解密方法     
         * Parameters: 
         *      待解密的串
         * 
         * Return Value: 
         *      经过解密的串
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc205106 zhao,qingrong 
         * Date:
         *      2008-12-13
         */
        [AjaxPro.AjaxMethod]
        public string Decrypto(string Source)
        {
            byte[] bytIn = Convert.FromBase64String(Source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();

            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}

/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  iMES web constant 
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * Known issues:
 */
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace IMES.Infrastructure.Extend.Utility
{


    /// <summary>
    /// Summary description for iMESConstant
    /// </summary>
    public partial class ServiceConstant
    {
        public ServiceConstant()
        {
        }
        

        #region SAP  objectUri
        public const string SAPService = "IMESService.ISAPService";
       

        #endregion

        #region other
        public const string CHANNELNAME = "IMESChanelClient";
        #endregion



      
    }
}

﻿/*
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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace com.inventec.iMESWEB
{
    ///// <summary>
    ///// 区分是SA/FA/PAK/Common
    ///// </summary>
    //[AttributeUsage(AttributeTargets.Field)]
    //public class PhaseAttribute : Attribute
    //{
    //    public PhaseAttribute(Phase phs)
    //    {
    //        _phs = phs;
    //    }
    //    private Phase _phs = default(Phase);
    //    public Phase Phase 
    //    {
    //        get
    //        {
    //            return _phs;
    //        }
    //    }
    //}

    //public enum Phase
    //{
    //    Common = 0,
    //    SA,
    //    FA,
    //    PAK
    //}

    /// <summary>
    /// Summary description for iMESConstant
    /// </summary>
    public partial class WebConstant
    {
        public WebConstant()
        {
        }
        #region language configuration key
        public const string LANGUAGE = "language";
        # endregion


        public const string SUCCESSRET = "SUCCESSRET";

        //print.cab version
        public static string version = "1,0,0,12";


        #region other
        public const string CHANNELNAME = "IMESChanelClient";
        #endregion


        # region "session type"
        public const string SessionMB = "1";
        public const string SessionProduct = "2";
        public const string SessionCommon = "0";
        # endregion

        #region  UPH
        public const string UPHIUPHALL = "IUPH_ALL";
        public const string DINNER = "IDinner";
        public const string UPHAlarm = "Alarm";
        public const string IUPH_Family = "IUPH_Family";
        public const string IAlarmMailAddress = "IAlarmMailAddress";

        #endregion
    }
}
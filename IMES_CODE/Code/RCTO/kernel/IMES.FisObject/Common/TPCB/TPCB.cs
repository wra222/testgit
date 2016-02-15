// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Maintain TPCB and TP
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-22   Chen Xu (eb1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.TPCB
{
    /// <summary>
    /// Maintain TPCB and TP数据维护
    /// </summary>
    public class TPCB : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Maintain TPCB Class
        /// </summary>
        public TPCB()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Maintain TPCB数据属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tpcb"></param>
        /// <param name="tp"></param>
        /// <param name="vcode"></param>
        /// <param name="editor"></param>
        /// <param name="cdt"></param>
        public TPCB(int id, string tpcb, string tp, string vcode, string editor, DateTime cdt)
        {
            this._id = id;
            this._tpcb = tpcb;
            this._tp = tp;
            this._vcode = vcode;
            this._editor = editor;
            this._cdt = cdt;
            this._tracker.MarkAsAdded(this);
        }

         #region . Essential Fields .

         private int _id; 
         private string _tpcb;
         private string _tp;
         private string _vcode;
         private string _editor;
         private DateTime _cdt;
         
        /// <summary>
         /// 自增ID序号
         /// </summary>
         public int ID
         {
             get
             {
                 return this._id;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._id = value;
             }
         }

         // <summary>
         /// TPCB
         /// </summary>
        public string Tpcb
         {
             get
             {
                 return this._tpcb;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._tpcb = value;
             }
         }

         /// <summary>
         /// TouchPad
         /// </summary>
         public string Tp
         {
             get
             {
                 return this._tp;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._tp = value;
             }
         }

         /// <summary>
         /// Vcode
         /// </summary>
         public string Vcode
         {
             get
             {
                 return this._vcode;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._vcode = value;
             }
         }

       

         /// <summary>
         /// Editor
         /// </summary>
         public string Editor
         {
             get
             {
                 return this._editor;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._editor = value;
             }
         }

         /// <summary>
         /// 创建时间
         /// </summary>
         public DateTime Cdt
         {
             get
             {
                 return this._cdt;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._cdt = value;
             }
         }

         #endregion

         #region Overrides of FisObjectBase

         /// <summary>
         /// 对象标示key, 在同类型FisObject范围内唯一
         /// </summary>
         public override object Key
         {
             get { return this._id; }
         }

         #endregion
    }
}

// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  TPCB
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-15   Chen Xu (eb1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.TPCB
{
    /// <summary>
    /// Product生产订单类
    /// </summary>
    public class TPCB_Info : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// TPCB Class
        /// </summary>
        public TPCB_Info()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// TPCB数据属性
        /// </summary>
        /// <param name="Family"></param>
        /// <param name="PdLine"></param>
        /// <param name="Type"></param>
        /// <param name="PartNo"></param>
        /// <param name="Vendor"></param>
        /// <param name="DCode"></param>
        /// <param name="editor"></param>
        /// <param name="cdt"></param>
        public TPCB_Info(string Family, string PdLine, string Type, string PartNo, string Vendor, string DCode, string editor, DateTime cdt, DateTime udt)
        {
            this._family = Family;
            this._pdLine = PdLine;
            this._type = Type;
            this._partNo = PartNo;
            this._vendor = Vendor;
            this._dcode = Dcode;
            this._editor = Editor;
            this._cdt = cdt;
            this._udt = udt;
            this._tracker.MarkAsAdded(this);
        }

         #region . Essential Fields .

         private int _id;
         private string _family;
         private string _pdLine;
         private string _type;
         private string _partNo;
         private string _vendor;
         private string _dcode;
         private string _editor;
         private DateTime _cdt;
         private DateTime _udt;

         /// <summary>
         /// ID
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

         /// <summary>
         /// Family
         /// </summary>
         public string Family
         {
             get
             {
                 return this._family;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._family = value;
             }
         }

         /// <summary>
         /// PdLine
         /// </summary>
         public string PdLine
         {
             get
             {
                 return this._pdLine;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._pdLine = value;
             }
         }

         /// <summary>
         /// Type
         /// </summary>
         public string Type
         {
             get
             {
                 return this._type;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._type = value;
             }
         }


         /// <summary>
         /// PartNo
         /// </summary>
         public string PartNo
         {
             get
             {
                 return this._partNo;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._partNo = value;
             }
         }


         /// <summary>
         /// Vendor
         /// </summary>
         public string Vendor
         {
             get
             {
                 return this._vendor;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._vendor = value;
             }
         }

         /// <summary>
         /// Vcode
         /// </summary>
         public string Dcode
         {
             get
             {
                 return this._dcode;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._dcode = value;
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

         /// <summary>
         /// 修改时间
         /// </summary>
         public DateTime Udt
         {
             get
             {
                 return this._udt;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._udt = value;
             }
         }

         #endregion

         #region Overrides of FisObjectBase

         /// <summary>
         /// 对象标示key, 在同类型FisObject范围内唯一
         /// </summary>
         public override object Key
         {
             get { return _id; }
         }

         #endregion
    }
}

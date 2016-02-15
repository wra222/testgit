// INVENTEC corporation (c)2011 all rights reserved. 
// Description: CartonSSCC
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-29   Chen Xu (eb1-4)              create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.FisObject.PAK.CartonSSCC
{
    /// <summary>
    /// CartonSSCC数据
    /// </summary>
    public class CartonSSCC : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// CartonSSCC数据
        /// </summary>
        public CartonSSCC()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// CartonSSCC数据
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <param name="sscc"></param>
        /// <param name="editor"></param>
        /// <param name="cdt"></param>
        public CartonSSCC(string cartonSN, string sscc,string editor, DateTime cdt)
        {
            this._cartonSN = cartonSN;
            this._sscc = sscc;
            this._editor = editor;
            this._cdt = cdt;
            this._tracker.MarkAsAdded(this);
        }

         #region . Essential Fields .

         private string _cartonSN;
         private string _sscc;
         private string _editor;
         private DateTime _cdt;
         
  
         // <summary>
         /// CartonSN 唯一标识
         /// </summary>
        public string CartonSN
         {
             get
             {
                 return this._cartonSN;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._cartonSN = value;
             }
         }

         /// <summary>
         /// 生成的SSCC号码
         /// </summary>
        public string SSCC
         {
             get
             {
                 return this._sscc;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._sscc = value;
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
             get { return _cartonSN; }
         }

         #endregion
    }
}

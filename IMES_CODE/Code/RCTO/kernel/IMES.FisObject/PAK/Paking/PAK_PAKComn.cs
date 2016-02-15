// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PAK_PAKComn类对应PAK_PAKComn表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-1    210003                       create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PAK.Paking
{
    public class PAK_PAKComn : FisObjectBase, IAggregateRoot
    {

        #region . Essential Fields .
        private string _id;
        /// <summary>
        /// Pallet序号
        /// </summary>
        public string id
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
        #endregion
        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return id; }
        }

        #endregion
    }
}

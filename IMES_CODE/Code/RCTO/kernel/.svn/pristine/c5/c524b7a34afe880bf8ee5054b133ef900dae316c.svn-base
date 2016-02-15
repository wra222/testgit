using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;
using IMES.FisObject.Common.Process;
using IMES.DataModel;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Hold
{
    public class Hold : FisObjectBase, IAggregateRoot
    {
         /// <summary>
        /// Constructor
        /// </summary>
        public Hold()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region IFisObject Members
        public override object Key
        {
            get { return string.Empty; }
        }
        #endregion
    }
}

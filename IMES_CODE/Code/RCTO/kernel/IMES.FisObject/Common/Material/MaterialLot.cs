using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.FisObject.Common.Process;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Material
{
    [ORMapping(typeof(mtns.MaterialLot))]
    public class MaterialLot : FisObjectBase, IAggregateRoot
    {
         /// <summary>
        /// Constructor
        /// </summary>
        public MaterialLot()
        {
            this._tracker.MarkAsAdded(this);
        }

        private static IProcessRepository _procRepository;
        private static IProcessRepository ProcessRepository
        {
            get
            {
                if (_procRepository == null)
                    _procRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository>();
                return _procRepository;
            }
        }

        private static IMaterialBoxRepository _materialBoxRepository;
        private static IMaterialBoxRepository MaterialBoxRepository
        {
            get
            {
                if (_materialBoxRepository == null)
                    _materialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
                return _materialBoxRepository;
            }
        }

        #region . Essential Fields .
        [ORMapping(mtns.MaterialLot.fn_lotNo)]
        private string _LotNo = null;
        [ORMapping(mtns.MaterialLot.fn_materialType)]
        private string _MaterialType = null;
        [ORMapping(mtns.MaterialLot.fn_specNo)]
        private string _SpecNo = null;
        [ORMapping(mtns.MaterialLot.fn_qty)]
        private int _Qty = int.MinValue;
        [ORMapping(mtns.MaterialLot.fn_status)]
        private string _Status = null;
        [ORMapping(mtns.MaterialLot.fn_editor)]
        private string _Editor = null;
        [ORMapping(mtns.MaterialLot.fn_cdt)]
        private DateTime _Cdt = DateTime.MinValue;
        [ORMapping(mtns.MaterialLot.fn_udt)]
        private DateTime _Udt = DateTime.MinValue;

        public string LotNo
        {
            get
            {
                return this._LotNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._LotNo = value;
            }
        }      
        public string MaterialType
        {
            get
            {
                return this._MaterialType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MaterialType = value;
            }
        }        
        public string SpecNo
        {
            get
            {
                return this._SpecNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._SpecNo = value;
            }
        }
        public int Qty
        {
            get
            {
                return this._Qty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Qty = value;
            }
        }  
        public string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Status = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._Editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._Cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Cdt = value;
            }
        }

        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Udt = value;
            }
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _LotNo; }
        }
        #endregion

        #region external object
        private object _syncmaterialProcessObj = new object();
        private MaterialProcess _materialProcess = null;
        public MaterialProcess GetMaterialProcess
        {
            get
            {
                lock (_syncmaterialProcessObj)
                {
                    if (_materialProcess == null)
                    {
                        _materialProcess = ProcessRepository.GetMaterialProcessByType(_MaterialType);
                    }
                    return _materialProcess;
                }
            }
        }

        private object _syncmaterialBoxObj = new object();
        private IList<MaterialBox> _materialBoxList = null;
        public IList<MaterialBox> GetMaterialBoxs
        {
            get
            {
                lock (_syncmaterialBoxObj)
                {
                    if (_materialBoxList == null)
                    {
                        _materialBoxList = MaterialBoxRepository.GetMaterialBoxByLot(_MaterialType,_LotNo);
                    }
                    return _materialBoxList;
                }
            }
        }

        #endregion
    }
}

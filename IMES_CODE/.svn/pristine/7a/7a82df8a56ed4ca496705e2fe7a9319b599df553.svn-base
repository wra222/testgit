using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;

namespace IMES.FisObject.PAK.BSam
{

    /// <summary>
    /// this table is read only data structure
    /// </summary>
    public class BSamLocation : FisObjectBase, IAggregateRoot
    {

        private static IBSamRepository _bsamRepository = null;
        private static IBSamRepository BSamRepository
        {
            get
            {
                if (_bsamRepository == null)
                    _bsamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository, BSamLocation>();
                return _bsamRepository;
            }
        }

        public BSamLocation()
        {
            //this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _LocationId = "";
        private string _Model = "";
        private int _Qty = 0;
        private int _RemainQty = 0;
        private int _FullQty = 0;
        private int _FullCartonQty = 0;
        //private int _FullQtyPerCarton = 0;

        private string _HoldInput= "N";
        private string _HoldOutput = "N";
        
           
        private string _Editor = "";
        private DateTime _Cdt = DateTime.MinValue;
        private DateTime _Udt = DateTime.MinValue;

        public string LocationId
        {
            get
            {
                return this._LocationId;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._locationId= value;
            //}
        }

        public string Model
        {
            get
            {
                return this._Model;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._model = value;
            //}
        }

        public int Qty
        {
            get
            {
                return this._Qty;
            }
            //set
            //{
            //    //this._tracker.MarkAsModified(this);
            //    this._qty = value;
            //}
        }

        public int RemainQty
        {
            get
            {
                return this._RemainQty;
            }
            //set
            //{
            //    //this._tracker.MarkAsModified(this);
            //    this._remainQty = value;
            //}
        }

        public int FullQty
        {
            get
            {
                return this._FullQty;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._fullQty = value;
            //}
        }


        public int FullCartonQty
        {
            get
            {
                return this._FullCartonQty;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._fullCartonQty = value;
            //}
        }

        public string HoldInput
        {
            get
            {
                return this._HoldInput;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._holdInput = value;
            //}
        }

        public string HoldOutput
        {
            get
            {
                return this._HoldOutput;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._holdOutput = value;
            //}
        }

        public int FullQtyPerCarton
        {
            get {  return _FullCartonQty!=0? (int)( _FullQty/_FullCartonQty): 0;  }
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
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._editor = value;
            //}
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
            //set
            //{
            //    //this._tracker.MarkAsModified(this);
            //    this._cdt = value;
            //}
        }

        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            //set
            //{
            //    //this._tracker.MarkAsModified(this);
            //    this._udt = value;
            //}
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _LocationId; }
        }

        #endregion

        #region link external object
        private IList<string> _productIDList = null;
        private object _syncObj__productIDList = new object();

        public IList<string> ProductIDs
        {
            get {
                lock (_syncObj__productIDList)
                {
                    if (_productIDList == null)
                    {
                        BSamRepository.FillProductIDInLoc(this);
                    }
                    if (_productIDList != null)
                    {
                        return new ReadOnlyCollection<string>(_productIDList);
                    }
                    else
                        return null;
                }
            }

        }

        #endregion

        #region handle internal data
        //private int _movingCount = 0;
        

        //public void MoveInLoc(string model, int fullQtyPerCarton)
        //{
        //    if (_holdInput == "N" && 
        //          (_model ==model ||string.IsNullOrEmpty(_model) ))
        //    {
        //        _fullQtyPerCarton = fullQtyPerCarton;
        //        _model = model;
        //        _movingCount++;
        //        this._tracker.MarkAsModified(this);
        //    }
        //    else
        //    {
        //        throw new Exception("not allow moveIn Location");
        //    }
        //}

        //public void MoveOutLoc(string model)
        //{
        //    if (_holdOutput == "N" && _model == model)
        //    {
        //        _movingCount--;
        //        this._tracker.MarkAsModified(this);
        //    }
        //    else
        //    {
        //        throw new Exception("not allow MoveOut Location");
        //    }
        //}

        //public int MovingCount
        //{
        //    get  { return _movingCount;}
            
        //}

        #endregion

    }
}

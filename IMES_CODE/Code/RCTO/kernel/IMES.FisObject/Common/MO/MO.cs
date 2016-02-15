using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.MO
{
    /// <summary>
    /// Product生产订单类
    /// </summary>
    [ORMapping(typeof(Mo))]
    public class MO : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MO()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(Mo.fn_mo)]
        private string _mo = null;
        [ORMapping(Mo.fn_plant)]
        private string _plant = null;
        [ORMapping(Mo.fn_model)]
        private string _model = null;
        [ORMapping(Mo.fn_createDate)]
        private DateTime _createDate = DateTime.MinValue;//default(DateTime);
        [ORMapping(Mo.fn_startDate)]
        private DateTime _startDate = DateTime.MinValue;//default(DateTime);
        [ORMapping(Mo.fn_qty)]
        private int _qty = int.MinValue;//default(int);//private short _qty = default(short);
        [ORMapping(Mo.fn_sapstatus)]
        private string _sapstatus = null;
        [ORMapping(Mo.fn_sapqty)]
        private int _sapqty = int.MinValue;//default(int);//private short _sapqty = default(short);
        [ORMapping(Mo.fn_print_Qty)]
        private int _print_qty = int.MinValue;//default(int);//private short _print_qty = default(short);
        [ORMapping(Mo.fn_transfer_Qty)]
        private int _transfer_qty = int.MinValue;//default(int);//private short _transfer_qty = default(short);
        [ORMapping(Mo.fn_status)]
        private string _status = null;
        [ORMapping(Mo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;//default(DateTime);
        [ORMapping(Mo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;//default(DateTime);
        [ORMapping(Mo.fn_customerSN_Qty)]
        private int _customerSN_Qty = int.MinValue;

        [ORMapping(Mo.fn_poNo)]
        private string _poNo = null;

        //private object _syncObj_print_qty = new object();
        //private object _syncObj_qty = new object();

        ///<summary>
        /// MONO
        ///</summary>
        public string MONO
        {
            get { return this._mo; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._mo = value; 
            }
        }

        /// <summary>
        /// Plant
        /// </summary>
        public string Plant
        {
            get { return this._plant; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._plant = value; 
            }
        }
        /// <summary>
        /// MO所属机型
        /// </summary>
        public string Model
        {
            get { return this._model; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._model = value; 
            }
        }

        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate
        {
            get { return this._createDate; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._createDate = value; 
            }
        }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate
        {
            get { return this._startDate; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._startDate = value; 
            }
        }
        /// <summary>
        /// MO包含的Product数量
        /// </summary>
        public int Qty
        {
            get { return this._qty; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._qty = value; 
            }
        }

        /// <summary>
        /// Sap status
        /// </summary>
        public string SAPStatus
        {
            get { return this._sapstatus; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._sapstatus = value; 
            }
        }

        /// <summary>
        /// Sap Qty
        /// </summary>
        public int SAPQty
        {
            get { return this._sapqty; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._sapqty = value; 
            }
        }
        /// <summary>
        /// MO中已产生的ProductID数量
        /// </summary>
        public int PrtQty
        {
            get { return this._print_qty; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._print_qty = value; 
            }
        }
        /// <summary>
        /// MO中已产生的ProductID数量
        /// </summary>
        public int TransferQty
        {
            get { return this._transfer_qty; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._transfer_qty = value;
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public string Status
        {
            get { return this._status; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._status = value; 
            }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return this._cdt; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value; 
            }
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return this._udt; }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
                this._tracker.MarkAsModified(this);
            }
        }

        public int CustomerSN_Qty
        {
            get { return this._customerSN_Qty; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._customerSN_Qty = value;
            }
        }

        /// <summary>
        /// PoNo 
        /// </summary>
        public string PoNo
        {
            get { return this._poNo; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._poNo = value;
            }
        }

        #endregion

        /// <summary>
        /// MO中已与CustomerSn绑定的Product数量
        /// </summary>
        public int SnoQty { get; set; }

        /// <summary>
        /// 增加已产生ProductID数量
        /// </summary>
        /// <param name="byQty">此次增加的数量</param>
        /// <returns>增加后的数量</returns>
        public int IncreasePrintedQty(short byQty)
        {
            //lock (_syncObj_print_qty)
            //{
            return this._print_qty += byQty;
            //}
            //return 0;
        }

        /// <summary>
        /// 减少已产生的ProductID数量
        /// </summary>
        /// <param name="byQty">此次减少的数量</param>
        /// <returns>减少后的数量</returns>
        public int DecreasePrintedQty(short byQty)
        {
            return this._print_qty -= byQty;
        }

        /// <summary>
        /// 增加已与CustomerSn绑定的Product数量
        /// </summary>
        /// <param name="byQty">此次增加的数量</param>
        /// <returns>增加后的数量</returns>
        public int IncreaseSnoQty(int byQty)
        {
            return 0;
        }

        /// <summary>
        /// 减少已与CustomerSn绑定的Product数量
        /// </summary>
        /// <param name="byQty">此次减少的数量</param>
        /// <returns>减少后的数量</returns>
        public int DecreaseSnoQty(short byQty)
        {
            return 0;
        }

        /// <summary>
        /// 产生指定数量的ProductId
        /// </summary>
        /// <param name="qty">产生的数量</param>
        public void GenerateProId(short qty)
        {
            
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _mo; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PCA.MBMO
{
    /// <summary>
    /// 主板制造订单类。一个制造订单确定生产多少块某一型号的主板。
    /// </summary>
    public class MBMO : FisObjectBase, IMBMO
    {
        public MBMO(string mono, string family, string model, int qty, int printedqty, string remark, string status, string editor, DateTime cdt, DateTime udt)
        {
             _mono = mono;
             _family = family;
             _model = model;
             _qty = qty;
             _printedqty = printedqty;
             _remark = remark;
             _status = status;
             _editor = editor;
             _cdt = cdt;
             _udt = udt;

             this.Tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MBMO()
        {
            this.Tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _mono = string.Empty;
        private string _family = string.Empty;
        private string _model = string.Empty;
        private int _qty = 0;
        private int _printedqty = 0;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private string _editor = string.Empty;
        private DateTime _cdt = default(DateTime);
        private DateTime _udt = default(DateTime);

        ///<summary>
        /// MONo
        ///</summary>
        public string MONo 
        {
            get { return _mono; }
            set 
            {
                this._tracker.MarkAsModified(this);
                _mono = value;
            } 
        }

        ///<summary>
        /// MB Family
        ///</summary>
        public string Family
        {
            get { return _family; }
            set
            {
                this._tracker.MarkAsModified(this);
                _family = value;
            }
        }

        ///<summary>
        /// Model
        ///</summary>
        public string Model
        {
            get { return _model; }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        ///<summary>
        /// Mo包含的Mb总数
        ///</summary>
        public int Qty
        {
            get { return _qty; }
            set
            {
                this._tracker.MarkAsModified(this);
                _qty = value;
            }
        }

        ///<summary>
        /// 此MO中已产生MB序号的数量
        ///</summary>
        public int PrintedQty
        {
            get { return _printedqty; }
            set
            {
                this._tracker.MarkAsModified(this);
                _printedqty = value;
            }
        }

        ///<summary>
        /// Remark
        ///</summary>
        public string Remark
        {
            get { return _remark; }
            set
            {
                this._tracker.MarkAsModified(this);
                _remark = value;
            }
        }

        ///<summary>
        /// Status
        ///</summary>
        public string Status
        {
            get { return _status; }
            set
            {
                this._tracker.MarkAsModified(this);
                _status = value;
            }
        }

        ///<summary>
        /// Editor
        ///</summary>
        public string Editor
        {
            get { return _editor; }
            set
            {
                this._tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        ///<summary>
        /// Cdt
        ///</summary>
        public DateTime Cdt
        {
            get { return _cdt; }
        }

        ///<summary>
        /// Udt
        ///</summary>
        public DateTime Udt
        {
            get { return _udt; }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _mono; }
        }

        #endregion

        #region . IMBMO .

        /// <summary>
        /// 增加打印主板标签数量
        /// </summary>
        /// <param name="byQty">新增数量</param>
        /// <returns>实际增加数量</returns>
        public int IncreasePrintedQty(int byQty)
        {
            return 0;
        }

        /// <summary>
        /// 减少打印主板标签数量
        /// </summary>
        /// <param name="byQty">减少数量</param>
        /// <returns>实际减少数量</returns>
        public int DecreasePrintedQty(int byQty)
        {
            return 0;
        }

        /// <summary>
        /// 回收已组装的主板。以便将来重新生产。
        /// </summary>
        /// <param name="startMBSn">起始主板序号</param>
        /// <param name="endMBSn">结束主板序号</param>
        public void DismantleMB(string startMBSn, string endMBSn)
        {

        }

        /// <summary>
        /// 产生主板信息。
        /// </summary>
        /// <param name="qty">待产生数量</param>
        /// <returns>产生的主板序号集合</returns>
        public IList<string> GenerateMB(int qty)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// 添加主板回收日志。
        /// </summary>
        private void AddDismantleLog()
        {

        }
    }
}
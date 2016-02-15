using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.FisObject.Common.Misc;
using IMES.DataModel;
using System.Reflection;

namespace IMES.FisObject.Common.UPS
{
    [ORMapping(typeof(mtns.UPSCombinePO))]
    public class UPSCombinePO:FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UPSCombinePO()
        {
            //this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.UPSCombinePO.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(mtns.UPSCombinePO.fn_hppo)]
        public String HPPO = null;

        [ORMapping(mtns.UPSCombinePO.fn_iecpo)]
        public String IECPO = null;

        [ORMapping(mtns.UPSCombinePO.fn_iecpoitem)]
        public String IECPOItem = null;

        [ORMapping(mtns.UPSCombinePO.fn_model)]
        public String Model = null;

        [ORMapping(mtns.UPSCombinePO.fn_receiveDate)]
        public DateTime ReceiveDate = DateTime.MinValue;

        [ORMapping(mtns.UPSCombinePO.fn_productID)]
        public String ProductID = null;

        [ORMapping(mtns.UPSCombinePO.fn_custsn)]
        public String CUSTSN = null;

        [ORMapping(mtns.UPSCombinePO.fn_station)]
        public String Station = null;

        [ORMapping(mtns.UPSCombinePO.fn_isShipPO)]
        public String IsShipPO = null;

        [ORMapping(mtns.UPSCombinePO.fn_status)]
        public String Status = null;

        [ORMapping(mtns.UPSCombinePO.fn_remark)]
        public String Remark = null;


        [ORMapping(mtns.UPSCombinePO.fn_editor)]
        public string Editor = null;

        [ORMapping(mtns.UPSCombinePO.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(mtns.UPSCombinePO.fn_udt)]
        public DateTime Udt = DateTime.MinValue;

        public object GetProperty(string name)
        {
            FieldInfo prop = GetType().GetField(name);
            if (prop != null)
            {
                return prop.GetValue(this);
            }
            return null;
        }

        #region IFisObject Members
        public override object Key
        {
            get { return ProductID; }
        }

        #endregion

        #region Link To Other
        private static IUPSRepository _upsRep = null;
        private static IUPSRepository UPSRep
        {
            get
            {
                if (_upsRep == null)
                    _upsRep = RepositoryFactory.GetInstance().GetRepository<IUPSRepository>();
                return _upsRep;
            }
        }
        #endregion

        private IList<UPSPOAVPartInfo> _avPartList;
        private object _syncObj_avPartObj = new object();

        private UPSHPPOInfo  _hpPo;
        private object _syncObj_PoObj = new object();

        #region  Get UPS another object function
        public IList<UPSPOAVPartInfo> UPSAVPart
        {
            get
            {
                lock (_syncObj_avPartObj)
                {
                    if (_avPartList == null )
                    {
                        if (!string.IsNullOrEmpty(this.HPPO))
                        {
                            _avPartList = UPSRep.GetAVPartByHPPO(this.HPPO);
                        }
                    }
                    return _avPartList;
                }
            }
        }

        public UPSHPPOInfo UPSHPPO
        {
            get
            {
                lock (_syncObj_PoObj)
                {
                    if (_hpPo == null)
                    {
                        if (!string.IsNullOrEmpty(this.HPPO))
                        {
                            _hpPo = UPSRep.GetHPPO(this.HPPO);
                        }
                    }
                    return _hpPo;
                }
            }
        }

        #endregion
    }

    public enum EnumUPSCombinePOStatus
    {
        Free=1,
        Used,
        Release
    }

    public enum EnumUPSModelPOStatus
    {
        Enable = 1,
        Disable
    }
}

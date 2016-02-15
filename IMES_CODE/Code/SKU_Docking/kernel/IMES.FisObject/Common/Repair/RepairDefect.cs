using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Repair
{
    /// <summary>
    /// 维修记录中的Defect
    /// </summary>
    [ORMapping(typeof(KeyPartRepair_DefectInfo))]
    public class RepairDefect : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RepairDefect()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RepairDefect(int id, int repairid, string type, string defectcodeid, string cause, string obligation, string component, string site, string location, string majorpart, string remark, string vendorCT, string partType, string oldpart, string oldpartsno, string newpart, string newpartsno, string newpartdatecode, bool ismanual, string manufacture, string versionA, string versionB, string returnsign, string side, string mark, string subdefect, string piastation, string distribution, string _4m, string responsibility, string action, string cover, string uncover, string trackingstatus, string mtaid, string returnStation, string editor, DateTime cdt, DateTime udt)
        {
            this._id = id;
            _repairid = repairid;
            _type = type;
            _defectcodeid = defectcodeid;
            _cause = cause;
            _obligation = obligation;
            _component = component;
            _site = site;
            _location = location;
            _majorpart = majorpart;
            _remark = remark;
            _vendorCT = vendorCT;
            _partType = partType;
            _oldpart = oldpart;
            _oldpartsno = oldpartsno;
            _newpart = newpart;
            _newpartsno = newpartsno;
            _newpartdatecode = newpartdatecode;
            _ismanual = ismanual;
            _manufacture = manufacture;
            _versionA = versionA;
            _versionB = versionB;
            _returnsign = returnsign;
            _side = side;
            _mark = mark;
            _subdefect = subdefect;
            _piastation = piastation;
            _distribution = distribution;
            __4m = _4m;
            _responsibility = responsibility;
            _action = action;
            _cover = cover;
            _uncover = uncover;
            _trackingstatus = trackingstatus;
            _mtaid = mtaid;
            _returnStation = returnStation;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(KeyPartRepair_DefectInfo.fn_id)]
        int _id = int.MinValue;
        [ORMapping(KeyPartRepair_DefectInfo.fn_keyPartRepairID)]
        int _repairid = int.MinValue;
        [ORMapping(KeyPartRepair_DefectInfo.fn_type)]
        string _type;
        [ORMapping(KeyPartRepair_DefectInfo.fn_defectCode)]
        string _defectcodeid;
        [ORMapping(KeyPartRepair_DefectInfo.fn_cause)]
        string _cause;
        [ORMapping(KeyPartRepair_DefectInfo.fn_obligation)]
        string _obligation;
        [ORMapping(KeyPartRepair_DefectInfo.fn_component)]
        string _component;
        [ORMapping(KeyPartRepair_DefectInfo.fn_site)]
        string _site;
        [ORMapping(KeyPartRepair_DefectInfo.fn_location)]
        string _location;
        [ORMapping(KeyPartRepair_DefectInfo.fn_majorPart)]
        string _majorpart;
        [ORMapping(KeyPartRepair_DefectInfo.fn_remark)]
        string _remark;
        [ORMapping(KeyPartRepair_DefectInfo.fn_vendorCT)]
        string _vendorCT;
        [ORMapping(KeyPartRepair_DefectInfo.fn_partType)]
        string _partType;
        [ORMapping(KeyPartRepair_DefectInfo.fn_oldPart)]
        string _oldpart;
        [ORMapping(KeyPartRepair_DefectInfo.fn_oldPartSno)]
        string _oldpartsno;
        [ORMapping(KeyPartRepair_DefectInfo.fn_newPart)]
        string _newpart;
        [ORMapping(KeyPartRepair_DefectInfo.fn_newPartSno)]
        string _newpartsno;

        string _newpartdatecode;
        //[ORMapping(KeyPartRepair_DefectInfo.fn_isManual)]
        bool _ismanual;
        [ORMapping(KeyPartRepair_DefectInfo.fn_manufacture)]
        string _manufacture;
        [ORMapping(KeyPartRepair_DefectInfo.fn_versionA)]
        string _versionA;
        [ORMapping(KeyPartRepair_DefectInfo.fn_versionB)]
        string _versionB;
        [ORMapping(KeyPartRepair_DefectInfo.fn_returnSign)]
        string _returnsign;

        string _side;
        [ORMapping(KeyPartRepair_DefectInfo.fn_mark)]
        string _mark;
        [ORMapping(KeyPartRepair_DefectInfo.fn_subDefect)]
        string _subdefect;
        [ORMapping(KeyPartRepair_DefectInfo.fn_piastation)]
        string _piastation;
        [ORMapping(KeyPartRepair_DefectInfo.fn_distribution)]
        string _distribution;
        [ORMapping(KeyPartRepair_DefectInfo.fn__4M_)]
        string __4m;
        [ORMapping(KeyPartRepair_DefectInfo.fn_responsibility)]
        string _responsibility;
        [ORMapping(KeyPartRepair_DefectInfo.fn_action)]
        string _action;
        [ORMapping(KeyPartRepair_DefectInfo.fn_cover)]
        string _cover;
        [ORMapping(KeyPartRepair_DefectInfo.fn_uncover)]
        string _uncover;
        [ORMapping(KeyPartRepair_DefectInfo.fn_trackingStatus)]
        string _trackingstatus;
        [ORMapping(KeyPartRepair_DefectInfo.fn_mtaid)]
        string _mtaid;
        [ORMapping(KeyPartRepair_DefectInfo.fn_returnStn)]
        string _returnStation;
        [ORMapping(KeyPartRepair_DefectInfo.fn_editor)]
        string _editor;
        [ORMapping(KeyPartRepair_DefectInfo.fn_cdt)]
        DateTime _cdt = DateTime.MinValue;
        [ORMapping(KeyPartRepair_DefectInfo.fn_udt)]
        DateTime _udt = DateTime.MinValue;



        /// <summary>
        /// Id
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
        /// Repiar Id
        /// </summary>
        public int RepairID 
        {
            get
            {
                return this._repairid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._repairid = value;
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
        /// Defect Code
        /// </summary>
        public string DefectCodeID 
        {
            get
            {
                return this._defectcodeid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._defectcodeid = value;
            }
        }

        /// <summary>
        /// Cause
        /// </summary>
        public string Cause 
        {
            get
            {
                return this._cause;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cause = value;
            } 
        }

        /// <summary>
        /// Obligation
        /// </summary>
        public string Obligation 
        {
            get
            {
                return this._obligation;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._obligation = value;
            } 
        }

        /// <summary>
        /// Component
        /// </summary>
        public string Component 
        {
            get
            {
                return this._component;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._component = value;
            } 
        }

        /// <summary>
        /// Site
        /// </summary>
        public string Site 
        {
            get
            {
                return this._site;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._site = value;
            } 
        }

        /// <summary>
        /// Location
        /// </summary>
        public string Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._location = value;
            }
        }

        /// <summary>
        /// MajorPart
        /// </summary>
        public string MajorPart 
        {
            get
            {
                return this._majorpart;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._majorpart = value;
            }
        }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark 
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._remark = value;
            }
        }

        /// <summary>
        /// VendorCT
        /// </summary>
        public string VendorCT 
        {
            get
            {
                return this._vendorCT;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._vendorCT = value;
            }
        }

        /// <summary>
        /// PartType
        /// </summary>
        public string PartType 
        {
            get
            {
                return this._partType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._partType = value;
            } 
        }

        /// <summary>
        /// OldPart
        /// </summary>
        public string OldPart 
        {
            get
            {
                return this._oldpart;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._oldpart = value;
            } 
        }

        /// <summary>
        /// OldPartSno
        /// </summary>
        public string OldPartSno 
        {
            get
            {
                return this._oldpartsno;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._oldpartsno = value;
            }
        }

        /// <summary>
        /// NewPart
        /// </summary>
        public string NewPart 
        {
            get
            {
                return this._newpart;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._newpart = value;
            }
        }

        /// <summary>
        /// NewPartSno
        /// </summary>
        public string NewPartSno 
        {
            get
            {
                return this._newpartsno;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._newpartsno = value;
            } 
        }

        /// <summary>
        /// NewPartDateCode
        /// </summary>
        public string NewPartDateCode 
        {
            get
            {
                return this._newpartdatecode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._newpartdatecode = value;
            } 
        }

        /// <summary>
        /// IsManual
        /// </summary>
        public bool IsManual 
        {
            get
            {
                return this._ismanual;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._ismanual = value;
            }
        }

        /// <summary>
        /// Manufacture
        /// </summary>
        public string Manufacture 
        {
            get
            {
                return this._manufacture;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._manufacture = value;
            }
        }

        /// <summary>
        /// VersionA
        /// </summary>
        public string VersionA 
        {
            get
            {
                return this._versionA;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._versionA = value;
            }
        }

        /// <summary>
        /// VersionB
        /// </summary>
        public string VersionB 
        {
            get
            {
                return this._versionB;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._versionB = value;
            }
        }

        /// <summary>
        /// ReturnSign
        /// </summary>
        public string ReturnSign 
        {
            get
            {
                return this._returnsign;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._returnsign = value;
            }
        }

        /// <summary>
        /// Side
        /// </summary>
        public string Side 
        {
            get
            {
                return this._side;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._side = value;
            } 
        }

        /// <summary>
        /// Mark
        /// </summary>
        public string Mark 
        {
            get
            {
                return this._mark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._mark = value;
            } 
        }

        /// <summary>
        /// SubDefect
        /// </summary>
        public string SubDefect 
        {
            get
            {
                return this._subdefect;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._subdefect = value;
            }
        }

        /// <summary>
        /// PIAStation
        /// </summary>
        public string PIAStation 
        {
            get
            {
                return this._piastation;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._piastation = value;
            } 
        }

        /// <summary>
        /// Distribution
        /// </summary>
        public string Distribution 
        {
            get
            {
                return this._distribution;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._distribution = value;
            }
        }

        /// <summary>
        /// 4M
        /// </summary>
        public string _4M 
        {
            get
            {
                return this.__4m;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this.__4m = value;
            } 
        }

        /// <summary>
        /// Responsibility
        /// </summary>
        public string Responsibility 
        {
            get
            {
                return this._responsibility;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._responsibility = value;
            } 
        }

        /// <summary>
        /// Action
        /// </summary>
        public string Action 
        {
            get
            {
                return this._action;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._action = value;
            } 
        }

        /// <summary>
        /// Cover
        /// </summary>
        public string Cover 
        {
            get
            {
                return this._cover;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cover = value;
            } 
        }

        /// <summary>
        /// Uncover
        /// </summary>
        public string Uncover 
        {
            get
            {
                return this._uncover;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._uncover = value;
            }
        }

        /// <summary>
        /// TrackingStatus
        /// </summary>
        public string TrackingStatus 
        {
            get
            {
                return this._trackingstatus;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._trackingstatus = value;
            } 
        }

        /// <summary>
        /// MTAID
        /// </summary>
        public string MTAID 
        {
            get
            {
                return this._mtaid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._mtaid = value;
            } 
        }

        public string ReturnStation
        {
            get { return _returnStation; }
            set { _returnStation = value; }
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
        /// Cdt
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
        /// Udt
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

        public MtaMarkInfo MTAMark { get; set; }

        private int setter_RepairId
        {
            set { _repairid = value; }
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

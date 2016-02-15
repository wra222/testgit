using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PCA.MBModel
{
    /// <summary>
    /// 主板型号类。
    /// </summary>
    public class MBModel : FisObjectBase, IMBModel
    {
        private string _pn;
        private string _mbcode;
        private string _mdl;
        private string _type; //MB/SB/VB
        private string _family;

        private MBCode _mbCode = null; // multi qty of MBCode
        private object _syncObj_mbcode = new object();

        private IMES.FisObject.Common.Model.Family _familyObj = null;
        private object _syncObj_family = new object();

        private IPart _part = null;
        private object _syncObj_part = new object();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MBModel(string pn, string mbcode, string mdl, string type, string family)
        {
            _pn = pn;
            _mbcode = mbcode;
            _mdl = mdl;
            _type = type;
            _family = family;
            _tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 主板型号零件号。
        /// </summary>
        public string Pn
        {
            get { return _pn; }
        }

        /// <summary>
        /// 主板型号代码。
        /// </summary>
        public string Mbcode
        {
            get { return _mbcode; }
        }

        /// <summary>
        /// ???
        /// </summary>
        public string Mdl
        {
            get { return _mdl; }
        }

        /// <summary>
        /// 主板类型：MB/SB/VB
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// 是否为连扳
        /// </summary>
        public bool IsCompositeBoard
        {
            get { return false; }
        }

        public bool IsVB
        {
            get { return this.Type.CompareTo(MB.MB.MBType.VB) == 0; }
        }

        /// <summary>
        /// MB的Family
        /// </summary>
        public string Family
        {
            get { return _family; }
        }

        public IMES.FisObject.Common.Model.Family FamilyObj
        {
            get
            {
                lock (_syncObj_family)
                {
                    if (_family != null && _familyObj == null)
                    {
                        MbModelRepository.FillFamilyObj(this);
                    }
                    return _familyObj;
                }
            }
        }

        public int MultiQty
        {
            get { return MbCodeObj.MultQty; }
        }

        public MBCode MbCodeObj
        {
            get
            {
                lock (_syncObj_mbcode)
                {
                    if (_mbcode != null && _mbCode == null)
                    {
                        MbModelRepository.FillMBCodeObj(this);
                    }
                    return _mbCode;
                }
            }
        }

        public IPart PartObj
        {
            get
            {
                lock (_syncObj_part)
                {
                    if (_part == null )
                    {
                        _part = PartRepository.Find(_pn);
                    }
                    return _part;
                }
            }
        }

        private static IMBModelRepository _mbModelRepository;
        private static IMBModelRepository MbModelRepository
        {
            get
            {
                if (_mbModelRepository == null)
                    _mbModelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                return _mbModelRepository;
            }
        }

        private static IPartRepository _partRepository;
        private static IPartRepository PartRepository
        {
            get
            {
                if (_partRepository == null)
                    _partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                return _partRepository;
            }
        }
        #region Overrides of FisObjectBase
        /// <summary>
        /// 对象标示key
        /// </summary>
        public override object Key
        {
            get { return _pn; }
        }
        #endregion
    }
}
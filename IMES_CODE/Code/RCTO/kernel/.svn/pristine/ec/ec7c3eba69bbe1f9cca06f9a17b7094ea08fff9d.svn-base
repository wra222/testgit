using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Region))]
    [Serializable]
    public class RegionInfo
    {
        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_customer)]
        private string _customer = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_region)]
        private string _region = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_regionCode)]
        private string _regionCode = null;

        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_description)]
        private string _description = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_editor)]
        private string _editor = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Region.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        public string Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public string RegionCode
        {
            get { return _regionCode; }
            set { _regionCode = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public DateTime Cdt
        {
            get { return _cdt; }
            set { _cdt = value; }
        }


        public DateTime Udt
        {
            get { return _udt; }
            set { _udt = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(RunInTimeControl))]
    public class RunInTimeControlInfoMaintain
    {
        [ORMapping(RunInTimeControl.fn_id)]
        private int id = int.MinValue;
        [ORMapping(RunInTimeControl.fn_code)]
        private string code = null;
        [ORMapping(RunInTimeControl.fn_type)]
        private string type = null;
        [ORMapping(RunInTimeControl.fn_hour)]
        private string hour = null;
        [ORMapping(RunInTimeControl.fn_remark)]
        private string remark = null;
        [ORMapping(RunInTimeControl.fn_editor)]
        private string editor = null;
        [ORMapping(RunInTimeControl.fn_cdt)]
        private DateTime cdt = new DateTime();
        [ORMapping(RunInTimeControl.fn_udt)]
        private DateTime udt = new DateTime();
        [ORMapping(RunInTimeControl.fn_controlType)]
        private bool controlType = false;
        [ORMapping(RunInTimeControl.fn_testStation)]
        private string testStation = null;

        public bool ControlType
        {
            get { return controlType; }
            set { this.controlType = value; }
        }

        public string TestStation
        {
            get { return testStation; }
            set
            {
                this.testStation = value;
            }
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }

        public string Code
        {
            get { return code; }
            set { this.code = value; }
        }

        public string Type
        {
            get { return type; }
            set { this.type = value; }
        }

        public string Hour
        {
            get { return hour; }
            set { this.hour = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { this.remark = value; }
        }

        public string Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        public DateTime Cdt
        {
            get { return cdt; }
            set { cdt = value; }
        }

        public DateTime Udt
        {
            get { return udt; }
            set { udt = value; }
        }
    }
}

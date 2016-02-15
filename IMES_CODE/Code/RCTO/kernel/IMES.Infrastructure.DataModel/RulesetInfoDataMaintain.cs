using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class RulesetInfoDataMaintain
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int priority;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        private string condition1="";

        public string Condition1
        {
            get { return condition1; }
            set { condition1 = value; }
        }

        private string condition2 = "";

        public string Condition2
        {
            get { return condition2; }
            set { condition2 = value; }
        }

        private string condition3 = "";

        public string Condition3
        {
            get { return condition3; }
            set { condition3 = value; }
        }

        private string condition4 = "";

        public string Condition4
        {
            get { return condition4; }
            set { condition4 = value; }
        }

        private string condition5 = "";

        public string Condition5
        {
            get { return condition5; }
            set { condition5 = value; }
        }

        private string condition6 = "";

        public string Condition6
        {
            get { return condition6; }
            set { condition6 = value; }
        }

        private string _editor;

        public string Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                 _editor = value;
            }
        }

        private DateTime _udt;

        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                _udt = value;
            }
        }

        private DateTime _cdt;

        public DateTime Cdt
        {
            get
            {
                return _cdt;
            }
            set
            {
                _cdt = value;
            }
        }
    }
}

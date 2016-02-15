using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ModelWeightTolerance
    {
        private string model = "";

        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        private string unitTolerance = "";

        public string UnitTolerance
        {
            get { return unitTolerance; }
            set { unitTolerance = value; }
        }
        private string cartonTolerance = "";

        public string CartonTolerance
        {
            get { return cartonTolerance; }
            set { cartonTolerance = value; }
        }
        private string editor = "";

        public string Editor
        {
            get { return editor; }
            set { editor = value; }
        }
        private DateTime cdt = new DateTime();

        public DateTime Cdt
        {
            get { return cdt; }
            set { cdt = value; }
        }
        private DateTime udt = new DateTime();

        public DateTime Udt
        {
            get { return udt; }
            set { udt = value; }
        } 
    }
}

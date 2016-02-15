namespace IMES.FisObject.Common.FisBOM
{
    /// <summary>
    /// Part的一个属性， 该类的每个实例，表示PartCheck里收集到的一个Part属性值(SN, PN, GCode...)
    /// </summary>
    public class PartUnit
    {
        /// <summary>
        /// Matched FlatBomItem GUID
        /// </summary>      
        private string _flatBomItemGuid = null;
        /// <summary>
        /// For TestKP case
        /// </summary>
        private bool _isTestKP = false;
        /// <summary>
        /// attached object
        /// </summary>
        private object _tag = null;

        /// <summary>
        ///pn=PartID,sn=PartSN,type=BomNodeType,ValueType=PartType,itemType=CheckItemType
        /// </summary>
        public PartUnit(string pn, string sn, string type, string valueType, string iecPn, string custPn, string itemType)
        {
            Pn = pn;//part.pn
            Sn = sn;//string.empty
            Type = type;//part.BOMNodeType
            ValueType = valueType;//part.Type
            IECPn = iecPn;//empty
            CustPn = custPn;//part.CustPn
            ItemType = itemType;//bom.CheckItemType
        }

        /// <summary>
        ///pn=PartID,sn=PartSN,type=BomNodeType,ValueType=PartType,itemType=CheckItemType
        ///isPreCheckedPart:whether is PreChecked PartUnit default value is false 
        /// </summary>
        public PartUnit(string pn, string sn, string type, string valueType, string iecPn, string custPn, string itemType,bool isPreCheckedPart)
        {
            Pn = pn;//part.pn
            Sn = sn;//string.empty
            Type = type;//part.BOMNodeType
            ValueType = valueType;//part.Type
            IECPn = iecPn;//empty
            CustPn = custPn;//part.CustPn
            ItemType = itemType;//bom.CheckItemType
            _isPrecheckedPart = isPreCheckedPart;
        }
        /// <summary>
        /// Product_Part.PartID
        /// </summary>
        public string Pn { get; private set; }

        /// <summary>
        /// Product_Part.PartSN
        /// </summary>
        public string Sn { get; private set; }

        /// <summary>
        /// Product_Part.BomNodeType
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Product_Part.PartType
        /// </summary>
        public string ValueType { get; private set; }

        /// <summary>
        /// Product_Part.CheckItemType
        /// </summary>
        public string ItemType { get; private set; }

        /// <summary>
        /// Product_Part.IECPn
        /// </summary>
        public string IECPn { get; private set; }

        /// <summary>
        /// Product_Part.CustomerPn
        /// </summary>
        public string CustPn { get; private set; }

        /// <summary>
        /// Product_Part.ProductId
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        ///  set CurrentSession object
        /// </summary>
        public object CurrentSession { get; set; }

        private bool _isPrecheckedPart = false;
        /// <summary>
        /// Pre-Cheched Part
        /// </summary>
        public bool IsPrecheckedPart 
        {
            get { return _isPrecheckedPart; }         
        }
        /// <summary>
        /// Matched FlatBomItem GUID
        /// </summary>
        public string FlatBomItemGuid
        {
            get { return _flatBomItemGuid; }
        }
        /// <summary>
        /// Is TestKP Case
        /// </summary>
        public bool IsTestKP 
        {
            get { return _isTestKP;}
            set {_isTestKP=value;} 
        }

        /// <summary>
        /// attached object
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
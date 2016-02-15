using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ModelMaintainInfo
    {

        /// <summary>
        /// DN序号
        /// </summary>
        public string Family;
        /// <summary>
        /// DN序号
        /// </summary>
        public string Model;

        /// <summary>
        /// Shipment序号
        /// </summary>
        public string CustPN;

        /// <summary>
        /// 
        /// </summary>
        public string Region;

        /// <summary>
        /// 
        /// </summary>
        public string ShipType;

        /// <summary>
        /// 出货日期
        /// </summary>
        public string Status;

        /// <summary>
        /// OSCode
        /// </summary>
        public string OsCode;

        /// <summary>
        /// 状态
        /// </summary>
        public string OSDesc;

        /// <summary>
        /// Price
        /// </summary>
        public string Price;
        /// <summary>
        /// 状态
        /// </summary>
        public DateTime BomApproveDate;

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt;


        public DateTime Udt;

    }


    [Serializable]
    public class ModelInfoMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id;
        /// <summary>
        /// Model
        /// </summary>
        public string Model;

        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Value
        /// </summary>
        public string Value;

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;


        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt;

    }

    [Serializable]
    public class ModelInfoNameMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// Model
        /// </summary>
        public string Region;

        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Descr
        /// </summary>
        public string Descr;

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;


        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt;

    }

    [Serializable]
    public class ModelInfoNameAndModelInfoValueMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id;
        /// <summary>
        /// Model
        /// </summary>
        public string Model;

        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Value
        /// </summary>
        public string Value;

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor;

        /// <summary>
        /// Description
        /// </summary>
        public string Description;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;


        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt;

    }

    [Serializable]
    public class ShipTypeMaintainInfo
    {


        /// <summary>
        /// ShipType
        /// </summary>
        public string ShipType;

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor;

        /// <summary>
        /// Description
        /// </summary>
        public string Description;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;


        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt;

    }

}

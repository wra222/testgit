using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class MOMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string MO;
        /// <summary>
        /// 
        /// </summary>
        public string Plant;

        /// <summary>
        /// 
        /// </summary>
        public string Model;

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate;

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate;

        /// <summary>
        /// 
        /// </summary>
        public int Qty;

        /// <summary>
        /// 
        /// </summary>
        public int SAPQty;

        /// <summary>
        /// 
        /// </summary>
        public string SAPStatus;

        /// <summary>
        /// 
        /// </summary>
        public int Print_Qty;

        /// <summary>
        /// 
        /// </summary>
        public string Status;

        /// <summary>
        /// 
        /// </summary>
        public DateTime Cdt;


        public DateTime Udt;

    }


    [Serializable]
    public class MOBOMMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int ID;
        /// <summary>
        /// 
        /// </summary>
        public string MO;

        /// <summary>
        /// 
        /// </summary>
        public string PartNo;

        /// <summary>
        /// 
        /// </summary>
        public string AssemblyCode;

        /// <summary>
        /// 
        /// </summary>
        public int Qty;

        /// <summary>
        /// 
        /// </summary>
        public int Group;

        /// <summary>
        /// 
        /// </summary>
        public bool Deviation;

        /// <summary>
        /// 
        /// </summary>
        public string Action;

        /// <summary>
        /// 
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
}

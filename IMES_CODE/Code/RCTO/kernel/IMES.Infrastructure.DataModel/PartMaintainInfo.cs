﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class PartMaintainInfo
    {


        /// <summary>
        /// 
        /// </summary>
        public string PartNo;
        /// <summary>
        /// 
        /// </summary>
        public string Descr;

        /// <summary>
        /// 
        /// </summary>
        public string PartType;

        /// <summary>
        /// 
        /// </summary>
        public string CustPartNo;

        /// <summary>
        /// 
        /// </summary>
        public int Flag;


        /// <summary>
        /// 
        /// </summary>
        public string AutoDL;

        /// <summary>
        /// 
        /// </summary>
        public string Remark;

        /// <summary>
        /// 
        /// </summary>
        public string Descr2;


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
    public class PartInfoMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// PartNo
        /// </summary>
        public string PartNo;

        /// <summary>
        /// Name
        /// </summary>
        public string InfoType;

        /// <summary>
        /// Value
        /// </summary>
        public string InfoValue;

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
    public class AssemblyCodeMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 
        /// </summary>
        public string PartNo;

        /// <summary>
        /// 
        /// </summary>
        public string Family;

        /// <summary>
        /// 
        /// </summary>
        public string Region;

        /// <summary>
        /// 
        /// </summary>
        public string Model;

        /// <summary>
        /// 
        /// </summary>
        public string AssemblyCode;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;

        /// <summary>
        /// 
        /// </summary>
        public string Editor;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Udt;

    }

    [Serializable]
    public class AssemblyCodeInfoMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 
        /// </summary>
        public string AssemblyCode;

        /// <summary>
        /// 
        /// </summary>
        public string InfoType;

        /// <summary>
        /// 
        /// </summary>
        public string InfoValue;

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

    [Serializable]
    public class PartTypeAttributeAndPartInfoValueMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// PartNo
        /// </summary>
        public string MainTableKey;

        /// <summary>
        /// Name
        /// </summary>
        public string Item;

        /// <summary>
        /// Value
        /// </summary>
        public string Content;

        /// <summary>
        /// Description
        /// </summary>
        public string Description;

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
    public class PartTypeMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public int ID;

        /// <summary>
        /// 
        /// </summary>
        public string PartType;

        /// <summary>
        /// 
        /// </summary>
        public string PartTypeGroup;


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
    public class PartTypeDescMaintainInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID;

        /// <summary>
        /// 
        /// </summary>
        public string PartType;

        /// <summary>
        /// 
        /// </summary>
        public string Description;


    }

    [Serializable]
    public class PartTypeAttributeMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string PartType;

        /// <summary>
        /// 
        /// </summary>
        public string Code;

        /// <summary>
        /// 
        /// </summary>
        public string Description;

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
    public class PartTypeMappingMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public int ID;

        /// <summary>
        /// 
        /// </summary>
        public string SAPType;

        /// <summary>
        /// 
        /// </summary>
        public string FISType;


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
    public class RegionMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string Region;

        /// <summary>
        /// 
        /// </summary>
        public string Description;

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

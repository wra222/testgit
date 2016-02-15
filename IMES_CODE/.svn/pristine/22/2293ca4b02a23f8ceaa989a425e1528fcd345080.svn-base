using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(mtns.Process))]
    public class ProcessMaintainInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [ORMapping(mtns.Process.fn_type)]
        public string Type = null;
        
        /// <summary>
        /// 
        /// </summary>
        [ORMapping(mtns.Process.fn_process)]
        public string Process = null;

        /// <summary>
        /// 
        /// </summary>
        [ORMapping(mtns.Process.fn_descr)]
        public string Description = null;

        /// <summary>
        /// Editor
        /// </summary>
        [ORMapping(mtns.Process.fn_editor)]
        public string Editor = null;

        /// <summary>
        /// 创建时间
        /// </summary>
        [ORMapping(mtns.Process.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        [ORMapping(mtns.Process.fn_udt)]
        public DateTime Udt = DateTime.MinValue;
    }


    [Serializable]
    public class ProcessStationMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// Model
        /// </summary>
        public string Process;

        /// <summary>
        /// Name
        /// </summary>
        public string PreStation;

        /// <summary>
        /// Value
        /// </summary>
        public string Station;


        /// <summary>
        /// Editor
        /// </summary>
        public int Status;

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
    public class ReworkReleaseTypeMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 
        /// </summary>
        public string Process;

        /// <summary>
        /// 
        /// </summary>
        public string ReleaseType;


        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;

    }

    [Serializable]
    public class ReworkProcessMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string ReworkCode;

        /// <summary>
        /// 
        /// </summary>
        public string Process;

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
    public class PalletProcessMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string Customer;

        /// <summary>
        /// 
        /// </summary>
        public string Process;

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
    public class PartProcessMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string MBFamily;

        /// <summary>
        /// 
        /// </summary>
        public string Process;

        /// <summary>
        /// 
        /// </summary>
        public string PilotRun;

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
    public class ModelProcessMaintainInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 
        /// </summary>
        public string RuleType;
        /// <summary>
        /// 
        /// </summary>
        public string Model;

        /// <summary>
        /// 
        /// </summary>
        public string Process;

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
    public class StationMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string Station;
        /// <summary>
        /// 
        /// </summary>
        public string StationType;
        /// <summary>
        /// 
        /// </summary>
        public int OperationObject;

        /// <summary>
        /// 
        /// </summary>
        public string Descr;

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
    public class ReworkMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string ReworkCode;
        /// <summary>
        /// 
        /// </summary>
        public string Status;


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
    public class CustomerMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string Customer;
        /// <summary>
        /// 
        /// </summary>
        public string Code;


        /// <summary>
        /// 
        /// </summary>
        public string Description;

    }

    [Serializable]
    public class MBFamilyMaintainInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public string Customer;
        /// <summary>
        /// 
        /// </summary>
        public string Code;


        /// <summary>
        /// 
        /// </summary>
        public string Description;

    }



    [Serializable]
    public class  ProcessInfoDef
    {
        public ProcessMaintainInfo ProcessInfo;
        public IList<ProcessStationMaintainInfo> ProcessStationList;

        public ProcessInfoDef()
        {
            ProcessInfo = new ProcessMaintainInfo();
            ProcessStationList = new List<ProcessStationMaintainInfo>();
        }
    }

}

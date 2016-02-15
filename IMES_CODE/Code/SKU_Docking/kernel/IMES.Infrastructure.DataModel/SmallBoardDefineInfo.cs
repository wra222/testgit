﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(SmallBoardDefine))]
    public class SmallBoardDefineInfo
    {
        [ORMapping(SmallBoardDefine.fn_id)]
        public long ID = long.MinValue;
        [ORMapping(SmallBoardDefine.fn_family)]
        public string Family = null;
        [ORMapping(SmallBoardDefine.fn_mbtype)]
        public string MBType = null;
        [ORMapping(SmallBoardDefine.fn_partNo)]
        public string PartNo = null;
        [ORMapping(SmallBoardDefine.fn_qty)]
        public int Qty = int.MinValue;
        [ORMapping(SmallBoardDefine.fn_maxQty)]
        public int MaxQty = int.MinValue;
        [ORMapping(SmallBoardDefine.fn_priority)]
        public int Priority = int.MinValue;
       
        [ORMapping(SmallBoardDefine.fn_ecr)]
        public string ECR = null;

        [ORMapping(SmallBoardDefine.fn_iecver)]
        public string IECVer = null;

        [ORMapping(SmallBoardDefine.fn_remark)]
        public string Remark = null;
        [ORMapping(SmallBoardDefine.fn_editor)]
        public string Editor = null;
        [ORMapping(SmallBoardDefine.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;
        [ORMapping(SmallBoardDefine.fn_udt)]
        public DateTime Udt = DateTime.MinValue;
    }
}

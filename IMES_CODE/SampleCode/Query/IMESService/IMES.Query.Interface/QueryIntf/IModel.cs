﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IModel
    {
        DataTable GetModel(string Family);
    }
}

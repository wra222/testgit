using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using UTL.MetaData;
using System.Data.SqlClient;

namespace UPS.UTL.SQL
{
    public class UPSDatabase:DataContext
    {
        public Table<UPSPOAVPart> UPSPOAVPartEntity;
        public Table<UPSHPPO> UPSHPPOEntity;
        public Table<UPSIECPO> UPSIECPOEntity;
        public Table<UPSCombinePO> UPSCombinePOEntity;
        public Table<UPSModel> UPSModelEntity;
        
        public UPSDatabase(string connection)
            : base(connection)
        {
        }

        public UPSDatabase(SqlConnection connection)
            : base(connection)
        {
        }
    }
}

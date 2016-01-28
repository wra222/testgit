namespace HOUSE_WAYBILLS_001_20130124 {
    
    
    /// <summary>
    ///Represents a strongly typed in-memory cache of data.
    ///</summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [global::System.Serializable()]
    [global::System.ComponentModel.DesignerCategoryAttribute("code")]
    [global::System.ComponentModel.ToolboxItem(true)]
    [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
    [global::System.Xml.Serialization.XmlRootAttribute("HOUSE_WAYBILLS")]
    [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
    public partial class HOUSE_WAYBILLS : global::System.Data.DataSet {
        
        private HOUSE_WAYBILLDataTable tableHOUSE_WAYBILL;
        
        private PACK_ID_LINE_ITEMDataTable tablePACK_ID_LINE_ITEM;
        
        private global::System.Data.DataRelation relationHOUSE_WAYBILL_PACK_ID_LINE_ITEM;
        
        private global::System.Data.SchemaSerializationMode _schemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public HOUSE_WAYBILLS() {
            this.BeginInit();
            this.InitClass();
            global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            base.Relations.CollectionChanged += schemaChangedHandler;
            this.EndInit();
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected HOUSE_WAYBILLS(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                base(info, context, false) {
            if ((this.IsBinarySerialized(info, context) == true)) {
                this.InitVars(false);
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                this.Tables.CollectionChanged += schemaChangedHandler1;
                this.Relations.CollectionChanged += schemaChangedHandler1;
                return;
            }
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((this.DetermineSchemaSerializationMode(info, context) == global::System.Data.SchemaSerializationMode.IncludeSchema)) {
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                ds.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                if ((ds.Tables["HOUSE_WAYBILL"] != null)) {
                    base.Tables.Add(new HOUSE_WAYBILLDataTable(ds.Tables["HOUSE_WAYBILL"]));
                }
                if ((ds.Tables["PACK_ID_LINE_ITEM"] != null)) {
                    base.Tables.Add(new PACK_ID_LINE_ITEMDataTable(ds.Tables["PACK_ID_LINE_ITEM"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
            }
            this.GetSerializationData(info, context);
            global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public HOUSE_WAYBILLDataTable HOUSE_WAYBILL {
            get {
                return this.tableHOUSE_WAYBILL;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public PACK_ID_LINE_ITEMDataTable PACK_ID_LINE_ITEM {
            get {
                return this.tablePACK_ID_LINE_ITEM;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.BrowsableAttribute(true)]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public override global::System.Data.SchemaSerializationMode SchemaSerializationMode {
            get {
                return this._schemaSerializationMode;
            }
            set {
                this._schemaSerializationMode = value;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new global::System.Data.DataTableCollection Tables {
            get {
                return base.Tables;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new global::System.Data.DataRelationCollection Relations {
            get {
                return base.Relations;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void InitializeDerivedDataSet() {
            this.BeginInit();
            this.InitClass();
            this.EndInit();
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override global::System.Data.DataSet Clone() {
            HOUSE_WAYBILLS cln = ((HOUSE_WAYBILLS)(base.Clone()));
            cln.InitVars();
            cln.SchemaSerializationMode = this.SchemaSerializationMode;
            return cln;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void ReadXmlSerializable(global::System.Xml.XmlReader reader) {
            if ((this.DetermineSchemaSerializationMode(reader) == global::System.Data.SchemaSerializationMode.IncludeSchema)) {
                this.Reset();
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                ds.ReadXml(reader);
                if ((ds.Tables["HOUSE_WAYBILL"] != null)) {
                    base.Tables.Add(new HOUSE_WAYBILLDataTable(ds.Tables["HOUSE_WAYBILL"]));
                }
                if ((ds.Tables["PACK_ID_LINE_ITEM"] != null)) {
                    base.Tables.Add(new PACK_ID_LINE_ITEMDataTable(ds.Tables["PACK_ID_LINE_ITEM"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXml(reader);
                this.InitVars();
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override global::System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream();
            this.WriteXmlSchema(new global::System.Xml.XmlTextWriter(stream, null));
            stream.Position = 0;
            return global::System.Xml.Schema.XmlSchema.Read(new global::System.Xml.XmlTextReader(stream), null);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars() {
            this.InitVars(true);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars(bool initTable) {
            this.tableHOUSE_WAYBILL = ((HOUSE_WAYBILLDataTable)(base.Tables["HOUSE_WAYBILL"]));
            if ((initTable == true)) {
                if ((this.tableHOUSE_WAYBILL != null)) {
                    this.tableHOUSE_WAYBILL.InitVars();
                }
            }
            this.tablePACK_ID_LINE_ITEM = ((PACK_ID_LINE_ITEMDataTable)(base.Tables["PACK_ID_LINE_ITEM"]));
            if ((initTable == true)) {
                if ((this.tablePACK_ID_LINE_ITEM != null)) {
                    this.tablePACK_ID_LINE_ITEM.InitVars();
                }
            }
            this.relationHOUSE_WAYBILL_PACK_ID_LINE_ITEM = this.Relations["HOUSE_WAYBILL_PACK_ID_LINE_ITEM"];
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitClass() {
            this.DataSetName = "HOUSE_WAYBILLS";
            this.Prefix = "";
            this.EnforceConstraints = true;
            this.SchemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
            this.tableHOUSE_WAYBILL = new HOUSE_WAYBILLDataTable();
            base.Tables.Add(this.tableHOUSE_WAYBILL);
            this.tablePACK_ID_LINE_ITEM = new PACK_ID_LINE_ITEMDataTable();
            base.Tables.Add(this.tablePACK_ID_LINE_ITEM);
            global::System.Data.ForeignKeyConstraint fkc;
            fkc = new global::System.Data.ForeignKeyConstraint("HOUSE_WAYBILL_PACK_ID_LINE_ITEM", new global::System.Data.DataColumn[] {
                        this.tableHOUSE_WAYBILL.HOUSE_WAYBILL_IdColumn}, new global::System.Data.DataColumn[] {
                        this.tablePACK_ID_LINE_ITEM.HOUSE_WAYBILL_IdColumn});
            this.tablePACK_ID_LINE_ITEM.Constraints.Add(fkc);
            fkc.AcceptRejectRule = global::System.Data.AcceptRejectRule.None;
            fkc.DeleteRule = global::System.Data.Rule.Cascade;
            fkc.UpdateRule = global::System.Data.Rule.Cascade;
            this.relationHOUSE_WAYBILL_PACK_ID_LINE_ITEM = new global::System.Data.DataRelation("HOUSE_WAYBILL_PACK_ID_LINE_ITEM", new global::System.Data.DataColumn[] {
                        this.tableHOUSE_WAYBILL.HOUSE_WAYBILL_IdColumn}, new global::System.Data.DataColumn[] {
                        this.tablePACK_ID_LINE_ITEM.HOUSE_WAYBILL_IdColumn}, false);
            this.relationHOUSE_WAYBILL_PACK_ID_LINE_ITEM.Nested = true;
            this.Relations.Add(this.relationHOUSE_WAYBILL_PACK_ID_LINE_ITEM);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerializeHOUSE_WAYBILL() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerializePACK_ID_LINE_ITEM() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void SchemaChanged(object sender, global::System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == global::System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
            HOUSE_WAYBILLS ds = new HOUSE_WAYBILLS();
            global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
            global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
            global::System.Xml.Schema.XmlSchemaAny any = new global::System.Xml.Schema.XmlSchemaAny();
            any.Namespace = ds.Namespace;
            sequence.Items.Add(any);
            type.Particle = sequence;
            global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
            if (xs.Contains(dsSchema.TargetNamespace)) {
                global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                try {
                    global::System.Xml.Schema.XmlSchema schema = null;
                    dsSchema.Write(s1);
                    for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                        schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                        s2.SetLength(0);
                        schema.Write(s2);
                        if ((s1.Length == s2.Length)) {
                            s1.Position = 0;
                            s2.Position = 0;
                            for (; ((s1.Position != s1.Length) 
                                        && (s1.ReadByte() == s2.ReadByte())); ) {
                                ;
                            }
                            if ((s1.Position == s1.Length)) {
                                return type;
                            }
                        }
                    }
                }
                finally {
                    if ((s1 != null)) {
                        s1.Close();
                    }
                    if ((s2 != null)) {
                        s2.Close();
                    }
                }
            }
            xs.Add(dsSchema);
            return type;
        }
        
        public delegate void HOUSE_WAYBILLRowChangeEventHandler(object sender, HOUSE_WAYBILLRowChangeEvent e);
        
        public delegate void PACK_ID_LINE_ITEMRowChangeEventHandler(object sender, PACK_ID_LINE_ITEMRowChangeEvent e);
        
        /// <summary>
        ///Represents the strongly named DataTable class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [global::System.Serializable()]
        [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class HOUSE_WAYBILLDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable {
            
            private global::System.Data.DataColumn columnWAYBILL_NUMBER;
            
            private global::System.Data.DataColumn columnSHIP_MODE;
            
            private global::System.Data.DataColumn columnINTL_CARRIER;
            
            private global::System.Data.DataColumn columnCURRENT_DATE;
            
            private global::System.Data.DataColumn columnACTUAL_SHIPDATE;
            
            private global::System.Data.DataColumn columnHAWB_PALLET_QTY;
            
            private global::System.Data.DataColumn columnHAWB_BOX_QTY;
            
            private global::System.Data.DataColumn columnHAWB_UNIT_QTY;
            
            private global::System.Data.DataColumn columnCONSOL_INVOICE;
            
            private global::System.Data.DataColumn columnDUTY_CODE;
            
            private global::System.Data.DataColumn columnPREF_GATEWAY;
            
            private global::System.Data.DataColumn columnTRANS_SERV_LEVEL;
            
            private global::System.Data.DataColumn columnHAWB_ACT_WEIGHT;
            
            private global::System.Data.DataColumn columnHAWB_GROSS_WEIGHT;
            
            private global::System.Data.DataColumn columnSHIP_FROM_NAME;
            
            private global::System.Data.DataColumn columnSHIP_FROM_NAME_2;
            
            private global::System.Data.DataColumn columnSHIP_FROM_NAME_3;
            
            private global::System.Data.DataColumn columnSHIP_FROM_STREET;
            
            private global::System.Data.DataColumn columnSHIP_FROM_STREET_2;
            
            private global::System.Data.DataColumn columnSHIP_FROM_CITY;
            
            private global::System.Data.DataColumn columnSHIP_FROM_STATE;
            
            private global::System.Data.DataColumn columnSHIP_FROM_ZIP;
            
            private global::System.Data.DataColumn columnSHIP_FROM_COUNTRY_NAME;
            
            private global::System.Data.DataColumn columnSHIP_FROM_COUNTRY_CODE;
            
            private global::System.Data.DataColumn columnSHIP_FROM_CONTACT;
            
            private global::System.Data.DataColumn columnSHIP_FROM_TELEPHONE;
            
            private global::System.Data.DataColumn columnSHIP_TO_ID;
            
            private global::System.Data.DataColumn columnSHIP_TO_NAME;
            
            private global::System.Data.DataColumn columnSHIP_TO_NAME_2;
            
            private global::System.Data.DataColumn columnSHIP_TO_NAME_3;
            
            private global::System.Data.DataColumn columnSHIP_TO_STREET;
            
            private global::System.Data.DataColumn columnSHIP_TO_STREET_2;
            
            private global::System.Data.DataColumn columnSHIP_TO_CITY;
            
            private global::System.Data.DataColumn columnSHIP_TO_STATE;
            
            private global::System.Data.DataColumn columnSHIP_TO_ZIP;
            
            private global::System.Data.DataColumn columnSHIP_TO_COUNTRY_CODE;
            
            private global::System.Data.DataColumn columnSHIP_TO_COUNTRY_NAME;
            
            private global::System.Data.DataColumn columnHOUSE_WAYBILL_Id;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLDataTable() {
                this.TableName = "HOUSE_WAYBILL";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal HOUSE_WAYBILLDataTable(global::System.Data.DataTable table) {
                this.TableName = table.TableName;
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected HOUSE_WAYBILLDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn WAYBILL_NUMBERColumn {
                get {
                    return this.columnWAYBILL_NUMBER;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_MODEColumn {
                get {
                    return this.columnSHIP_MODE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn INTL_CARRIERColumn {
                get {
                    return this.columnINTL_CARRIER;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn CURRENT_DATEColumn {
                get {
                    return this.columnCURRENT_DATE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn ACTUAL_SHIPDATEColumn {
                get {
                    return this.columnACTUAL_SHIPDATE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HAWB_PALLET_QTYColumn {
                get {
                    return this.columnHAWB_PALLET_QTY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HAWB_BOX_QTYColumn {
                get {
                    return this.columnHAWB_BOX_QTY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HAWB_UNIT_QTYColumn {
                get {
                    return this.columnHAWB_UNIT_QTY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn CONSOL_INVOICEColumn {
                get {
                    return this.columnCONSOL_INVOICE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn DUTY_CODEColumn {
                get {
                    return this.columnDUTY_CODE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn PREF_GATEWAYColumn {
                get {
                    return this.columnPREF_GATEWAY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn TRANS_SERV_LEVELColumn {
                get {
                    return this.columnTRANS_SERV_LEVEL;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HAWB_ACT_WEIGHTColumn {
                get {
                    return this.columnHAWB_ACT_WEIGHT;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HAWB_GROSS_WEIGHTColumn {
                get {
                    return this.columnHAWB_GROSS_WEIGHT;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_NAMEColumn {
                get {
                    return this.columnSHIP_FROM_NAME;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_NAME_2Column {
                get {
                    return this.columnSHIP_FROM_NAME_2;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_NAME_3Column {
                get {
                    return this.columnSHIP_FROM_NAME_3;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_STREETColumn {
                get {
                    return this.columnSHIP_FROM_STREET;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_STREET_2Column {
                get {
                    return this.columnSHIP_FROM_STREET_2;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_CITYColumn {
                get {
                    return this.columnSHIP_FROM_CITY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_STATEColumn {
                get {
                    return this.columnSHIP_FROM_STATE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_ZIPColumn {
                get {
                    return this.columnSHIP_FROM_ZIP;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_COUNTRY_NAMEColumn {
                get {
                    return this.columnSHIP_FROM_COUNTRY_NAME;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_COUNTRY_CODEColumn {
                get {
                    return this.columnSHIP_FROM_COUNTRY_CODE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_CONTACTColumn {
                get {
                    return this.columnSHIP_FROM_CONTACT;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_FROM_TELEPHONEColumn {
                get {
                    return this.columnSHIP_FROM_TELEPHONE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_IDColumn {
                get {
                    return this.columnSHIP_TO_ID;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_NAMEColumn {
                get {
                    return this.columnSHIP_TO_NAME;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_NAME_2Column {
                get {
                    return this.columnSHIP_TO_NAME_2;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_NAME_3Column {
                get {
                    return this.columnSHIP_TO_NAME_3;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_STREETColumn {
                get {
                    return this.columnSHIP_TO_STREET;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_STREET_2Column {
                get {
                    return this.columnSHIP_TO_STREET_2;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_CITYColumn {
                get {
                    return this.columnSHIP_TO_CITY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_STATEColumn {
                get {
                    return this.columnSHIP_TO_STATE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_ZIPColumn {
                get {
                    return this.columnSHIP_TO_ZIP;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_COUNTRY_CODEColumn {
                get {
                    return this.columnSHIP_TO_COUNTRY_CODE;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn SHIP_TO_COUNTRY_NAMEColumn {
                get {
                    return this.columnSHIP_TO_COUNTRY_NAME;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HOUSE_WAYBILL_IdColumn {
                get {
                    return this.columnHOUSE_WAYBILL_Id;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLRow this[int index] {
                get {
                    return ((HOUSE_WAYBILLRow)(this.Rows[index]));
                }
            }
            
            public event HOUSE_WAYBILLRowChangeEventHandler HOUSE_WAYBILLRowChanging;
            
            public event HOUSE_WAYBILLRowChangeEventHandler HOUSE_WAYBILLRowChanged;
            
            public event HOUSE_WAYBILLRowChangeEventHandler HOUSE_WAYBILLRowDeleting;
            
            public event HOUSE_WAYBILLRowChangeEventHandler HOUSE_WAYBILLRowDeleted;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AddHOUSE_WAYBILLRow(HOUSE_WAYBILLRow row) {
                this.Rows.Add(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLRow AddHOUSE_WAYBILLRow(
                        string WAYBILL_NUMBER, 
                        string SHIP_MODE, 
                        string INTL_CARRIER, 
                        string CURRENT_DATE, 
                        string ACTUAL_SHIPDATE, 
                        double HAWB_PALLET_QTY, 
                        double HAWB_BOX_QTY, 
                        double HAWB_UNIT_QTY, 
                        string CONSOL_INVOICE, 
                        string DUTY_CODE, 
                        string PREF_GATEWAY, 
                        string TRANS_SERV_LEVEL, 
                        double HAWB_ACT_WEIGHT, 
                        double HAWB_GROSS_WEIGHT, 
                        string SHIP_FROM_NAME, 
                        string SHIP_FROM_NAME_2, 
                        string SHIP_FROM_NAME_3, 
                        string SHIP_FROM_STREET, 
                        string SHIP_FROM_STREET_2, 
                        string SHIP_FROM_CITY, 
                        string SHIP_FROM_STATE, 
                        string SHIP_FROM_ZIP, 
                        string SHIP_FROM_COUNTRY_NAME, 
                        string SHIP_FROM_COUNTRY_CODE, 
                        string SHIP_FROM_CONTACT, 
                        string SHIP_FROM_TELEPHONE, 
                        string SHIP_TO_ID, 
                        string SHIP_TO_NAME, 
                        string SHIP_TO_NAME_2, 
                        string SHIP_TO_NAME_3, 
                        string SHIP_TO_STREET, 
                        string SHIP_TO_STREET_2, 
                        string SHIP_TO_CITY, 
                        string SHIP_TO_STATE, 
                        string SHIP_TO_ZIP, 
                        string SHIP_TO_COUNTRY_CODE, 
                        string SHIP_TO_COUNTRY_NAME) {
                HOUSE_WAYBILLRow rowHOUSE_WAYBILLRow = ((HOUSE_WAYBILLRow)(this.NewRow()));
                object[] columnValuesArray = new object[] {
                        WAYBILL_NUMBER,
                        SHIP_MODE,
                        INTL_CARRIER,
                        CURRENT_DATE,
                        ACTUAL_SHIPDATE,
                        HAWB_PALLET_QTY,
                        HAWB_BOX_QTY,
                        HAWB_UNIT_QTY,
                        CONSOL_INVOICE,
                        DUTY_CODE,
                        PREF_GATEWAY,
                        TRANS_SERV_LEVEL,
                        HAWB_ACT_WEIGHT,
                        HAWB_GROSS_WEIGHT,
                        SHIP_FROM_NAME,
                        SHIP_FROM_NAME_2,
                        SHIP_FROM_NAME_3,
                        SHIP_FROM_STREET,
                        SHIP_FROM_STREET_2,
                        SHIP_FROM_CITY,
                        SHIP_FROM_STATE,
                        SHIP_FROM_ZIP,
                        SHIP_FROM_COUNTRY_NAME,
                        SHIP_FROM_COUNTRY_CODE,
                        SHIP_FROM_CONTACT,
                        SHIP_FROM_TELEPHONE,
                        SHIP_TO_ID,
                        SHIP_TO_NAME,
                        SHIP_TO_NAME_2,
                        SHIP_TO_NAME_3,
                        SHIP_TO_STREET,
                        SHIP_TO_STREET_2,
                        SHIP_TO_CITY,
                        SHIP_TO_STATE,
                        SHIP_TO_ZIP,
                        SHIP_TO_COUNTRY_CODE,
                        SHIP_TO_COUNTRY_NAME,
                        null};
                rowHOUSE_WAYBILLRow.ItemArray = columnValuesArray;
                this.Rows.Add(rowHOUSE_WAYBILLRow);
                return rowHOUSE_WAYBILLRow;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public virtual global::System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override global::System.Data.DataTable Clone() {
                HOUSE_WAYBILLDataTable cln = ((HOUSE_WAYBILLDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataTable CreateInstance() {
                return new HOUSE_WAYBILLDataTable();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnWAYBILL_NUMBER = base.Columns["WAYBILL_NUMBER"];
                this.columnSHIP_MODE = base.Columns["SHIP_MODE"];
                this.columnINTL_CARRIER = base.Columns["INTL_CARRIER"];
                this.columnCURRENT_DATE = base.Columns["CURRENT_DATE"];
                this.columnACTUAL_SHIPDATE = base.Columns["ACTUAL_SHIPDATE"];
                this.columnHAWB_PALLET_QTY = base.Columns["HAWB_PALLET_QTY"];
                this.columnHAWB_BOX_QTY = base.Columns["HAWB_BOX_QTY"];
                this.columnHAWB_UNIT_QTY = base.Columns["HAWB_UNIT_QTY"];
                this.columnCONSOL_INVOICE = base.Columns["CONSOL_INVOICE"];
                this.columnDUTY_CODE = base.Columns["DUTY_CODE"];
                this.columnPREF_GATEWAY = base.Columns["PREF_GATEWAY"];
                this.columnTRANS_SERV_LEVEL = base.Columns["TRANS_SERV_LEVEL"];
                this.columnHAWB_ACT_WEIGHT = base.Columns["HAWB_ACT_WEIGHT"];
                this.columnHAWB_GROSS_WEIGHT = base.Columns["HAWB_GROSS_WEIGHT"];
                this.columnSHIP_FROM_NAME = base.Columns["SHIP_FROM_NAME"];
                this.columnSHIP_FROM_NAME_2 = base.Columns["SHIP_FROM_NAME_2"];
                this.columnSHIP_FROM_NAME_3 = base.Columns["SHIP_FROM_NAME_3"];
                this.columnSHIP_FROM_STREET = base.Columns["SHIP_FROM_STREET"];
                this.columnSHIP_FROM_STREET_2 = base.Columns["SHIP_FROM_STREET_2"];
                this.columnSHIP_FROM_CITY = base.Columns["SHIP_FROM_CITY"];
                this.columnSHIP_FROM_STATE = base.Columns["SHIP_FROM_STATE"];
                this.columnSHIP_FROM_ZIP = base.Columns["SHIP_FROM_ZIP"];
                this.columnSHIP_FROM_COUNTRY_NAME = base.Columns["SHIP_FROM_COUNTRY_NAME"];
                this.columnSHIP_FROM_COUNTRY_CODE = base.Columns["SHIP_FROM_COUNTRY_CODE"];
                this.columnSHIP_FROM_CONTACT = base.Columns["SHIP_FROM_CONTACT"];
                this.columnSHIP_FROM_TELEPHONE = base.Columns["SHIP_FROM_TELEPHONE"];
                this.columnSHIP_TO_ID = base.Columns["SHIP_TO_ID"];
                this.columnSHIP_TO_NAME = base.Columns["SHIP_TO_NAME"];
                this.columnSHIP_TO_NAME_2 = base.Columns["SHIP_TO_NAME_2"];
                this.columnSHIP_TO_NAME_3 = base.Columns["SHIP_TO_NAME_3"];
                this.columnSHIP_TO_STREET = base.Columns["SHIP_TO_STREET"];
                this.columnSHIP_TO_STREET_2 = base.Columns["SHIP_TO_STREET_2"];
                this.columnSHIP_TO_CITY = base.Columns["SHIP_TO_CITY"];
                this.columnSHIP_TO_STATE = base.Columns["SHIP_TO_STATE"];
                this.columnSHIP_TO_ZIP = base.Columns["SHIP_TO_ZIP"];
                this.columnSHIP_TO_COUNTRY_CODE = base.Columns["SHIP_TO_COUNTRY_CODE"];
                this.columnSHIP_TO_COUNTRY_NAME = base.Columns["SHIP_TO_COUNTRY_NAME"];
                this.columnHOUSE_WAYBILL_Id = base.Columns["HOUSE_WAYBILL_Id"];
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnWAYBILL_NUMBER = new global::System.Data.DataColumn("WAYBILL_NUMBER", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnWAYBILL_NUMBER);
                this.columnSHIP_MODE = new global::System.Data.DataColumn("SHIP_MODE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_MODE);
                this.columnINTL_CARRIER = new global::System.Data.DataColumn("INTL_CARRIER", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnINTL_CARRIER);
                this.columnCURRENT_DATE = new global::System.Data.DataColumn("CURRENT_DATE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnCURRENT_DATE);
                this.columnACTUAL_SHIPDATE = new global::System.Data.DataColumn("ACTUAL_SHIPDATE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnACTUAL_SHIPDATE);
                this.columnHAWB_PALLET_QTY = new global::System.Data.DataColumn("HAWB_PALLET_QTY", typeof(double), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnHAWB_PALLET_QTY);
                this.columnHAWB_BOX_QTY = new global::System.Data.DataColumn("HAWB_BOX_QTY", typeof(double), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnHAWB_BOX_QTY);
                this.columnHAWB_UNIT_QTY = new global::System.Data.DataColumn("HAWB_UNIT_QTY", typeof(double), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnHAWB_UNIT_QTY);
                this.columnCONSOL_INVOICE = new global::System.Data.DataColumn("CONSOL_INVOICE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnCONSOL_INVOICE);
                this.columnDUTY_CODE = new global::System.Data.DataColumn("DUTY_CODE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnDUTY_CODE);
                this.columnPREF_GATEWAY = new global::System.Data.DataColumn("PREF_GATEWAY", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnPREF_GATEWAY);
                this.columnTRANS_SERV_LEVEL = new global::System.Data.DataColumn("TRANS_SERV_LEVEL", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnTRANS_SERV_LEVEL);
                this.columnHAWB_ACT_WEIGHT = new global::System.Data.DataColumn("HAWB_ACT_WEIGHT", typeof(double), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnHAWB_ACT_WEIGHT);
                this.columnHAWB_GROSS_WEIGHT = new global::System.Data.DataColumn("HAWB_GROSS_WEIGHT", typeof(double), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnHAWB_GROSS_WEIGHT);
                this.columnSHIP_FROM_NAME = new global::System.Data.DataColumn("SHIP_FROM_NAME", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_NAME);
                this.columnSHIP_FROM_NAME_2 = new global::System.Data.DataColumn("SHIP_FROM_NAME_2", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_NAME_2);
                this.columnSHIP_FROM_NAME_3 = new global::System.Data.DataColumn("SHIP_FROM_NAME_3", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_NAME_3);
                this.columnSHIP_FROM_STREET = new global::System.Data.DataColumn("SHIP_FROM_STREET", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_STREET);
                this.columnSHIP_FROM_STREET_2 = new global::System.Data.DataColumn("SHIP_FROM_STREET_2", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_STREET_2);
                this.columnSHIP_FROM_CITY = new global::System.Data.DataColumn("SHIP_FROM_CITY", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_CITY);
                this.columnSHIP_FROM_STATE = new global::System.Data.DataColumn("SHIP_FROM_STATE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_STATE);
                this.columnSHIP_FROM_ZIP = new global::System.Data.DataColumn("SHIP_FROM_ZIP", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_ZIP);
                this.columnSHIP_FROM_COUNTRY_NAME = new global::System.Data.DataColumn("SHIP_FROM_COUNTRY_NAME", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_COUNTRY_NAME);
                this.columnSHIP_FROM_COUNTRY_CODE = new global::System.Data.DataColumn("SHIP_FROM_COUNTRY_CODE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_COUNTRY_CODE);
                this.columnSHIP_FROM_CONTACT = new global::System.Data.DataColumn("SHIP_FROM_CONTACT", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_CONTACT);
                this.columnSHIP_FROM_TELEPHONE = new global::System.Data.DataColumn("SHIP_FROM_TELEPHONE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_FROM_TELEPHONE);
                this.columnSHIP_TO_ID = new global::System.Data.DataColumn("SHIP_TO_ID", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_ID);
                this.columnSHIP_TO_NAME = new global::System.Data.DataColumn("SHIP_TO_NAME", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_NAME);
                this.columnSHIP_TO_NAME_2 = new global::System.Data.DataColumn("SHIP_TO_NAME_2", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_NAME_2);
                this.columnSHIP_TO_NAME_3 = new global::System.Data.DataColumn("SHIP_TO_NAME_3", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_NAME_3);
                this.columnSHIP_TO_STREET = new global::System.Data.DataColumn("SHIP_TO_STREET", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_STREET);
                this.columnSHIP_TO_STREET_2 = new global::System.Data.DataColumn("SHIP_TO_STREET_2", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_STREET_2);
                this.columnSHIP_TO_CITY = new global::System.Data.DataColumn("SHIP_TO_CITY", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_CITY);
                this.columnSHIP_TO_STATE = new global::System.Data.DataColumn("SHIP_TO_STATE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_STATE);
                this.columnSHIP_TO_ZIP = new global::System.Data.DataColumn("SHIP_TO_ZIP", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_ZIP);
                this.columnSHIP_TO_COUNTRY_CODE = new global::System.Data.DataColumn("SHIP_TO_COUNTRY_CODE", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_COUNTRY_CODE);
                this.columnSHIP_TO_COUNTRY_NAME = new global::System.Data.DataColumn("SHIP_TO_COUNTRY_NAME", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnSHIP_TO_COUNTRY_NAME);
                this.columnHOUSE_WAYBILL_Id = new global::System.Data.DataColumn("HOUSE_WAYBILL_Id", typeof(int), null, global::System.Data.MappingType.Hidden);
                base.Columns.Add(this.columnHOUSE_WAYBILL_Id);
                this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnHOUSE_WAYBILL_Id}, true));
                this.columnWAYBILL_NUMBER.AllowDBNull = false;
                this.columnSHIP_MODE.AllowDBNull = false;
                this.columnINTL_CARRIER.AllowDBNull = false;
                this.columnCURRENT_DATE.AllowDBNull = false;
                this.columnACTUAL_SHIPDATE.AllowDBNull = false;
                this.columnHAWB_PALLET_QTY.AllowDBNull = false;
                this.columnHAWB_BOX_QTY.AllowDBNull = false;
                this.columnHAWB_UNIT_QTY.AllowDBNull = false;
                this.columnCONSOL_INVOICE.AllowDBNull = false;
                this.columnDUTY_CODE.AllowDBNull = false;
                this.columnPREF_GATEWAY.AllowDBNull = false;
                this.columnTRANS_SERV_LEVEL.AllowDBNull = false;
                this.columnHAWB_ACT_WEIGHT.AllowDBNull = false;
                this.columnHAWB_GROSS_WEIGHT.AllowDBNull = false;
                this.columnSHIP_FROM_NAME.AllowDBNull = false;
                this.columnSHIP_FROM_NAME_2.AllowDBNull = false;
                this.columnSHIP_FROM_NAME_3.AllowDBNull = false;
                this.columnSHIP_FROM_STREET.AllowDBNull = false;
                this.columnSHIP_FROM_STREET_2.AllowDBNull = false;
                this.columnSHIP_FROM_CITY.AllowDBNull = false;
                this.columnSHIP_FROM_STATE.AllowDBNull = false;
                this.columnSHIP_FROM_ZIP.AllowDBNull = false;
                this.columnSHIP_FROM_COUNTRY_NAME.AllowDBNull = false;
                this.columnSHIP_FROM_COUNTRY_CODE.AllowDBNull = false;
                this.columnSHIP_FROM_CONTACT.AllowDBNull = false;
                this.columnSHIP_FROM_TELEPHONE.AllowDBNull = false;
                this.columnSHIP_TO_ID.AllowDBNull = false;
                this.columnSHIP_TO_NAME.AllowDBNull = false;
                this.columnSHIP_TO_NAME_2.AllowDBNull = false;
                this.columnSHIP_TO_NAME_3.AllowDBNull = false;
                this.columnSHIP_TO_STREET.AllowDBNull = false;
                this.columnSHIP_TO_STREET_2.AllowDBNull = false;
                this.columnSHIP_TO_CITY.AllowDBNull = false;
                this.columnSHIP_TO_STATE.AllowDBNull = false;
                this.columnSHIP_TO_ZIP.AllowDBNull = false;
                this.columnSHIP_TO_COUNTRY_CODE.AllowDBNull = false;
                this.columnSHIP_TO_COUNTRY_NAME.AllowDBNull = false;
                this.columnHOUSE_WAYBILL_Id.AutoIncrement = true;
                this.columnHOUSE_WAYBILL_Id.AllowDBNull = false;
                this.columnHOUSE_WAYBILL_Id.Unique = true;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLRow NewHOUSE_WAYBILLRow() {
                return ((HOUSE_WAYBILLRow)(this.NewRow()));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder) {
                return new HOUSE_WAYBILLRow(builder);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Type GetRowType() {
                return typeof(HOUSE_WAYBILLRow);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.HOUSE_WAYBILLRowChanged != null)) {
                    this.HOUSE_WAYBILLRowChanged(this, new HOUSE_WAYBILLRowChangeEvent(((HOUSE_WAYBILLRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.HOUSE_WAYBILLRowChanging != null)) {
                    this.HOUSE_WAYBILLRowChanging(this, new HOUSE_WAYBILLRowChangeEvent(((HOUSE_WAYBILLRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.HOUSE_WAYBILLRowDeleted != null)) {
                    this.HOUSE_WAYBILLRowDeleted(this, new HOUSE_WAYBILLRowChangeEvent(((HOUSE_WAYBILLRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.HOUSE_WAYBILLRowDeleting != null)) {
                    this.HOUSE_WAYBILLRowDeleting(this, new HOUSE_WAYBILLRowChangeEvent(((HOUSE_WAYBILLRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemoveHOUSE_WAYBILLRow(HOUSE_WAYBILLRow row) {
                this.Rows.Remove(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
                global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                HOUSE_WAYBILLS ds = new HOUSE_WAYBILLS();
                global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "HOUSE_WAYBILLDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                if (xs.Contains(dsSchema.TargetNamespace)) {
                    global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                    global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                    try {
                        global::System.Xml.Schema.XmlSchema schema = null;
                        dsSchema.Write(s1);
                        for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                            schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                            s2.SetLength(0);
                            schema.Write(s2);
                            if ((s1.Length == s2.Length)) {
                                s1.Position = 0;
                                s2.Position = 0;
                                for (; ((s1.Position != s1.Length) 
                                            && (s1.ReadByte() == s2.ReadByte())); ) {
                                    ;
                                }
                                if ((s1.Position == s1.Length)) {
                                    return type;
                                }
                            }
                        }
                    }
                    finally {
                        if ((s1 != null)) {
                            s1.Close();
                        }
                        if ((s2 != null)) {
                            s2.Close();
                        }
                    }
                }
                xs.Add(dsSchema);
                return type;
            }
        }
        
        /// <summary>
        ///Represents the strongly named DataTable class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [global::System.Serializable()]
        [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class PACK_ID_LINE_ITEMDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable {
            
            private global::System.Data.DataColumn columnPACK_ID;
            
            private global::System.Data.DataColumn columnHP_SO;
            
            private global::System.Data.DataColumn columnPACK_ID_LINE_ITEM_UNIT_QTY;
            
            private global::System.Data.DataColumn columnHOUSE_WAYBILL_Id;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMDataTable() {
                this.TableName = "PACK_ID_LINE_ITEM";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal PACK_ID_LINE_ITEMDataTable(global::System.Data.DataTable table) {
                this.TableName = table.TableName;
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected PACK_ID_LINE_ITEMDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn PACK_IDColumn {
                get {
                    return this.columnPACK_ID;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HP_SOColumn {
                get {
                    return this.columnHP_SO;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn PACK_ID_LINE_ITEM_UNIT_QTYColumn {
                get {
                    return this.columnPACK_ID_LINE_ITEM_UNIT_QTY;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn HOUSE_WAYBILL_IdColumn {
                get {
                    return this.columnHOUSE_WAYBILL_Id;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMRow this[int index] {
                get {
                    return ((PACK_ID_LINE_ITEMRow)(this.Rows[index]));
                }
            }
            
            public event PACK_ID_LINE_ITEMRowChangeEventHandler PACK_ID_LINE_ITEMRowChanging;
            
            public event PACK_ID_LINE_ITEMRowChangeEventHandler PACK_ID_LINE_ITEMRowChanged;
            
            public event PACK_ID_LINE_ITEMRowChangeEventHandler PACK_ID_LINE_ITEMRowDeleting;
            
            public event PACK_ID_LINE_ITEMRowChangeEventHandler PACK_ID_LINE_ITEMRowDeleted;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AddPACK_ID_LINE_ITEMRow(PACK_ID_LINE_ITEMRow row) {
                this.Rows.Add(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMRow AddPACK_ID_LINE_ITEMRow(string PACK_ID, string HP_SO, double PACK_ID_LINE_ITEM_UNIT_QTY, HOUSE_WAYBILLRow parentHOUSE_WAYBILLRowByHOUSE_WAYBILL_PACK_ID_LINE_ITEM) {
                PACK_ID_LINE_ITEMRow rowPACK_ID_LINE_ITEMRow = ((PACK_ID_LINE_ITEMRow)(this.NewRow()));
                object[] columnValuesArray = new object[] {
                        PACK_ID,
                        HP_SO,
                        PACK_ID_LINE_ITEM_UNIT_QTY,
                        null};
                if ((parentHOUSE_WAYBILLRowByHOUSE_WAYBILL_PACK_ID_LINE_ITEM != null)) {
                    columnValuesArray[3] = parentHOUSE_WAYBILLRowByHOUSE_WAYBILL_PACK_ID_LINE_ITEM[37];
                }
                rowPACK_ID_LINE_ITEMRow.ItemArray = columnValuesArray;
                this.Rows.Add(rowPACK_ID_LINE_ITEMRow);
                return rowPACK_ID_LINE_ITEMRow;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public virtual global::System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override global::System.Data.DataTable Clone() {
                PACK_ID_LINE_ITEMDataTable cln = ((PACK_ID_LINE_ITEMDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataTable CreateInstance() {
                return new PACK_ID_LINE_ITEMDataTable();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnPACK_ID = base.Columns["PACK_ID"];
                this.columnHP_SO = base.Columns["HP_SO"];
                this.columnPACK_ID_LINE_ITEM_UNIT_QTY = base.Columns["PACK_ID_LINE_ITEM_UNIT_QTY"];
                this.columnHOUSE_WAYBILL_Id = base.Columns["HOUSE_WAYBILL_Id"];
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnPACK_ID = new global::System.Data.DataColumn("PACK_ID", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnPACK_ID);
                this.columnHP_SO = new global::System.Data.DataColumn("HP_SO", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnHP_SO);
                this.columnPACK_ID_LINE_ITEM_UNIT_QTY = new global::System.Data.DataColumn("PACK_ID_LINE_ITEM_UNIT_QTY", typeof(double), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnPACK_ID_LINE_ITEM_UNIT_QTY);
                this.columnHOUSE_WAYBILL_Id = new global::System.Data.DataColumn("HOUSE_WAYBILL_Id", typeof(int), null, global::System.Data.MappingType.Hidden);
                base.Columns.Add(this.columnHOUSE_WAYBILL_Id);
                this.columnPACK_ID.AllowDBNull = false;
                this.columnHP_SO.AllowDBNull = false;
                this.columnPACK_ID_LINE_ITEM_UNIT_QTY.AllowDBNull = false;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMRow NewPACK_ID_LINE_ITEMRow() {
                return ((PACK_ID_LINE_ITEMRow)(this.NewRow()));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder) {
                return new PACK_ID_LINE_ITEMRow(builder);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Type GetRowType() {
                return typeof(PACK_ID_LINE_ITEMRow);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PACK_ID_LINE_ITEMRowChanged != null)) {
                    this.PACK_ID_LINE_ITEMRowChanged(this, new PACK_ID_LINE_ITEMRowChangeEvent(((PACK_ID_LINE_ITEMRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PACK_ID_LINE_ITEMRowChanging != null)) {
                    this.PACK_ID_LINE_ITEMRowChanging(this, new PACK_ID_LINE_ITEMRowChangeEvent(((PACK_ID_LINE_ITEMRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PACK_ID_LINE_ITEMRowDeleted != null)) {
                    this.PACK_ID_LINE_ITEMRowDeleted(this, new PACK_ID_LINE_ITEMRowChangeEvent(((PACK_ID_LINE_ITEMRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PACK_ID_LINE_ITEMRowDeleting != null)) {
                    this.PACK_ID_LINE_ITEMRowDeleting(this, new PACK_ID_LINE_ITEMRowChangeEvent(((PACK_ID_LINE_ITEMRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemovePACK_ID_LINE_ITEMRow(PACK_ID_LINE_ITEMRow row) {
                this.Rows.Remove(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
                global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                HOUSE_WAYBILLS ds = new HOUSE_WAYBILLS();
                global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "PACK_ID_LINE_ITEMDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                if (xs.Contains(dsSchema.TargetNamespace)) {
                    global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                    global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                    try {
                        global::System.Xml.Schema.XmlSchema schema = null;
                        dsSchema.Write(s1);
                        for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                            schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                            s2.SetLength(0);
                            schema.Write(s2);
                            if ((s1.Length == s2.Length)) {
                                s1.Position = 0;
                                s2.Position = 0;
                                for (; ((s1.Position != s1.Length) 
                                            && (s1.ReadByte() == s2.ReadByte())); ) {
                                    ;
                                }
                                if ((s1.Position == s1.Length)) {
                                    return type;
                                }
                            }
                        }
                    }
                    finally {
                        if ((s1 != null)) {
                            s1.Close();
                        }
                        if ((s2 != null)) {
                            s2.Close();
                        }
                    }
                }
                xs.Add(dsSchema);
                return type;
            }
        }
        
        /// <summary>
        ///Represents strongly named DataRow class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class HOUSE_WAYBILLRow : global::System.Data.DataRow {
            
            private HOUSE_WAYBILLDataTable tableHOUSE_WAYBILL;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal HOUSE_WAYBILLRow(global::System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tableHOUSE_WAYBILL = ((HOUSE_WAYBILLDataTable)(this.Table));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string WAYBILL_NUMBER {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.WAYBILL_NUMBERColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.WAYBILL_NUMBERColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_MODE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_MODEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_MODEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string INTL_CARRIER {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.INTL_CARRIERColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.INTL_CARRIERColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string CURRENT_DATE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.CURRENT_DATEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.CURRENT_DATEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string ACTUAL_SHIPDATE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.ACTUAL_SHIPDATEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.ACTUAL_SHIPDATEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public double HAWB_PALLET_QTY {
                get {
                    return ((double)(this[this.tableHOUSE_WAYBILL.HAWB_PALLET_QTYColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.HAWB_PALLET_QTYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public double HAWB_BOX_QTY {
                get {
                    return ((double)(this[this.tableHOUSE_WAYBILL.HAWB_BOX_QTYColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.HAWB_BOX_QTYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public double HAWB_UNIT_QTY {
                get {
                    return ((double)(this[this.tableHOUSE_WAYBILL.HAWB_UNIT_QTYColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.HAWB_UNIT_QTYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string CONSOL_INVOICE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.CONSOL_INVOICEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.CONSOL_INVOICEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string DUTY_CODE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.DUTY_CODEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.DUTY_CODEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string PREF_GATEWAY {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.PREF_GATEWAYColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.PREF_GATEWAYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string TRANS_SERV_LEVEL {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.TRANS_SERV_LEVELColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.TRANS_SERV_LEVELColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public double HAWB_ACT_WEIGHT {
                get {
                    return ((double)(this[this.tableHOUSE_WAYBILL.HAWB_ACT_WEIGHTColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.HAWB_ACT_WEIGHTColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public double HAWB_GROSS_WEIGHT {
                get {
                    return ((double)(this[this.tableHOUSE_WAYBILL.HAWB_GROSS_WEIGHTColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.HAWB_GROSS_WEIGHTColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_NAME {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_NAMEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_NAMEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_NAME_2 {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_NAME_2Column]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_NAME_2Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_NAME_3 {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_NAME_3Column]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_NAME_3Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_STREET {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_STREETColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_STREETColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_STREET_2 {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_STREET_2Column]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_STREET_2Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_CITY {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_CITYColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_CITYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_STATE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_STATEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_STATEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_ZIP {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_ZIPColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_ZIPColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_COUNTRY_NAME {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_COUNTRY_NAMEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_COUNTRY_NAMEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_COUNTRY_CODE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_COUNTRY_CODEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_COUNTRY_CODEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_CONTACT {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_CONTACTColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_CONTACTColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_FROM_TELEPHONE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_FROM_TELEPHONEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_FROM_TELEPHONEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_ID {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_IDColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_IDColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_NAME {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_NAMEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_NAMEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_NAME_2 {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_NAME_2Column]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_NAME_2Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_NAME_3 {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_NAME_3Column]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_NAME_3Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_STREET {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_STREETColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_STREETColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_STREET_2 {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_STREET_2Column]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_STREET_2Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_CITY {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_CITYColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_CITYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_STATE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_STATEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_STATEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_ZIP {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_ZIPColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_ZIPColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_COUNTRY_CODE {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_COUNTRY_CODEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_COUNTRY_CODEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string SHIP_TO_COUNTRY_NAME {
                get {
                    return ((string)(this[this.tableHOUSE_WAYBILL.SHIP_TO_COUNTRY_NAMEColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.SHIP_TO_COUNTRY_NAMEColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int HOUSE_WAYBILL_Id {
                get {
                    return ((int)(this[this.tableHOUSE_WAYBILL.HOUSE_WAYBILL_IdColumn]));
                }
                set {
                    this[this.tableHOUSE_WAYBILL.HOUSE_WAYBILL_IdColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMRow[] GetPACK_ID_LINE_ITEMRows() {
                if ((this.Table.ChildRelations["HOUSE_WAYBILL_PACK_ID_LINE_ITEM"] == null)) {
                    return new PACK_ID_LINE_ITEMRow[0];
                }
                else {
                    return ((PACK_ID_LINE_ITEMRow[])(base.GetChildRows(this.Table.ChildRelations["HOUSE_WAYBILL_PACK_ID_LINE_ITEM"])));
                }
            }
        }
        
        /// <summary>
        ///Represents strongly named DataRow class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class PACK_ID_LINE_ITEMRow : global::System.Data.DataRow {
            
            private PACK_ID_LINE_ITEMDataTable tablePACK_ID_LINE_ITEM;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal PACK_ID_LINE_ITEMRow(global::System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tablePACK_ID_LINE_ITEM = ((PACK_ID_LINE_ITEMDataTable)(this.Table));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string PACK_ID {
                get {
                    return ((string)(this[this.tablePACK_ID_LINE_ITEM.PACK_IDColumn]));
                }
                set {
                    this[this.tablePACK_ID_LINE_ITEM.PACK_IDColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string HP_SO {
                get {
                    return ((string)(this[this.tablePACK_ID_LINE_ITEM.HP_SOColumn]));
                }
                set {
                    this[this.tablePACK_ID_LINE_ITEM.HP_SOColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public double PACK_ID_LINE_ITEM_UNIT_QTY {
                get {
                    return ((double)(this[this.tablePACK_ID_LINE_ITEM.PACK_ID_LINE_ITEM_UNIT_QTYColumn]));
                }
                set {
                    this[this.tablePACK_ID_LINE_ITEM.PACK_ID_LINE_ITEM_UNIT_QTYColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int HOUSE_WAYBILL_Id {
                get {
                    try {
                        return ((int)(this[this.tablePACK_ID_LINE_ITEM.HOUSE_WAYBILL_IdColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("資料表 \'PACK_ID_LINE_ITEM\' 中資料行 \'HOUSE_WAYBILL_Id\' 的值是 DBNull。", e);
                    }
                }
                set {
                    this[this.tablePACK_ID_LINE_ITEM.HOUSE_WAYBILL_IdColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLRow HOUSE_WAYBILLRow {
                get {
                    return ((HOUSE_WAYBILLRow)(this.GetParentRow(this.Table.ParentRelations["HOUSE_WAYBILL_PACK_ID_LINE_ITEM"])));
                }
                set {
                    this.SetParentRow(value, this.Table.ParentRelations["HOUSE_WAYBILL_PACK_ID_LINE_ITEM"]);
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsHOUSE_WAYBILL_IdNull() {
                return this.IsNull(this.tablePACK_ID_LINE_ITEM.HOUSE_WAYBILL_IdColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetHOUSE_WAYBILL_IdNull() {
                this[this.tablePACK_ID_LINE_ITEM.HOUSE_WAYBILL_IdColumn] = global::System.Convert.DBNull;
            }
        }
        
        /// <summary>
        ///Row event argument class
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class HOUSE_WAYBILLRowChangeEvent : global::System.EventArgs {
            
            private HOUSE_WAYBILLRow eventRow;
            
            private global::System.Data.DataRowAction eventAction;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLRowChangeEvent(HOUSE_WAYBILLRow row, global::System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public HOUSE_WAYBILLRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        /// <summary>
        ///Row event argument class
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class PACK_ID_LINE_ITEMRowChangeEvent : global::System.EventArgs {
            
            private PACK_ID_LINE_ITEMRow eventRow;
            
            private global::System.Data.DataRowAction eventAction;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMRowChangeEvent(PACK_ID_LINE_ITEMRow row, global::System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PACK_ID_LINE_ITEMRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}

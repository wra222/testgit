using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// AttributeNames 的摘要说明
/// </summary>
namespace com.inventec.system {
    public class AttributeNames {
        public const string REPORT_XMLFORMAT = "xmlFormatReport";

        // add by zhao, qing-rong
        public const string TREE_NODE_ID = "tree_node_id";
        public const string TREE_NODE_TYPE = "tree_node_type";

        public const string USER_ID = "user_id";

        public const string DATA_SERVER_ID = "data_server_id";
        public const string DATA_SERVER_NAME = "data_server_name";
        public const string DATA_SERVER_ALIAS = "data_server_alias";
        public const string DATA_SERVER_USERNAME = "data_server_user_name";
        public const string DATA_SERVER_PWD = "data_server_pwd";

        public const string DATA_BASE_ID = "data_base_id";
        public const string DATA_BASE_NAME = "data_base_name";
        public const string DATA_BASE_IS_CHECKED = "data_base_is_checked";
        public const string DATA_BASE_B_NORMALLY = "data_base_is_normally";

        public const string DATA_TABLE_NAME = "data_table_name";

        public const string PUBLISH_FILE_ID = "publish_file_id";
        public const string PUBLISH_FILE_TYPE = "publish_file_type";

        public const string SERIES_NAME_KEY = "series_key_name";
        public const string SERIES_STRING_KEY = "series_string_key";
        public const string SERIES_CHART_TYPE_KEY = "series_chart_type_key";
        public const string SERIES_DIMENSION_KEY = "series_dimension_key";
        public const string SERIES_DATATABLE_KEY = "series_datatable_key";
        public const string SERIES_IS_STATISTIC_KEY = "series_is_statistic_key";
        public const string SERIES_STATISTIC_CHART_TYPE_KEY = "series_statistic_chart_typ_key";

        public const string CHART_FILE_NAME = "chart_file_name";
        public const string CHART_FILE_DATA = "chart_file_data";
        public const string CHART_FILE_DATA_STR = "chart_file_data_str";
        public const string NUMBER_FORMAT_KEY = "number_format_key";

        //----add end by zhao qingrong

        // add by hou, man-li
        public const string DATA_FILE_NAME = "data_file_name";
        public const string DATA_PROPERTY_COMMENT = "data_property_comment";
        public const string DATA_REPORT_FOLDER_ID = "data_report_folder_id";
        public const string DATA_REPORT_ID = "data_report_id";
        public const string DATA_REPORT_NEW_OR_EDIT = "data_report_new_or_edit";

        public const string DATA_AREA_HEAD = "data_area_head";
        public const string DATA_AREA_VERTICAL = "data_area_vertical";
        public const string DATA_AREA_HORIZON = "data_area_horizon";
        public const string DATA_AREA_PADDING = "data_area_padding";
        public const string DATA_AREA_TAIL = "data_area_tail";
        public const string TREE_PARENT_NODE_ID = "tree_parent_node_id";// for the main frame tree
        public const string DATA_REPORT_FOLDER_TYPE = "data_report_folder_type";
        public const string DATA_EDITOR_ID = "data_editor_id";
        public const string DATA_FOLDER_CREATE_TIME = "data_folder_create_time";
        public const string DATA_FOLDER_UPDATE_TIME = "data_folder_update_time";
        public const string XML_STRUCTURE_DATA = "xml_structure_data";


        // add by kan, dao-ming for preview
        public const string XML_FORMAT_STRING = "xmlFormatString";
        public const string REPORT_INFO = "report";
        public const string REPORT_PROGRESS_ID = "report_progress_id";
        public const string CHART_INFO = "chart";
        public const string IMAGE_PATH = "image_path";
        public const string DATA_EXCED_LIMIT = "data_exced_limit";
        public const string DATA_RECORD_COUNT_0 = "data_record_count_0";

		//added by liu zhao
		public const string ROLE_ID = "rbpc_role_id";
		public const string ROLE_APP = "rbpc_role_application";
		public const string ROLE_TYPE = "rbpc_role_type";
		public const string ROLE_NAME = "rbpc_role_name";
		public const string ROLE_COMMENT = "rbpc_role_description";
		public const string ROLE_EDITOR = "rbpc_role_editor";

        public const string STRUCTURE_XML = "STRUCTURE_XML";

        //by xmzh
        public const string TABLE_NAME = "tableName";
        public const string FIELD_NAME = "fieldName";
        public const string PRE_CONDITION_SET = "preConditionSetting";




        // add by itc98079
        public const string USER_NAME = "userName";
        public const string USER_CODE = "code";
        public const string E_MAIL = "email";
        public const string DEPARTMENT = "department";
        public const string MANAGER = "manager";
        public const string DOMAIN = "domain";
        public const string COMPANY = "company";
        public const string AUTHOR_OBJ = "authority_obj";
        public const string TOKEN = "token";

        public const string PERMISSION_INFO = "permission_info";
        
        
        public AttributeNames() {
            
        }
    }
}
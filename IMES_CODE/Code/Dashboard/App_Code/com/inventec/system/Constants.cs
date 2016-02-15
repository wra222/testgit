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
/// Summary description for Constants
/// </summary>
namespace com.inventec.system
{
    public class Constants
    {
	    public Constants()
	    {
	    }


        public const int FA_STAGE = 1;
        public const int SA_STAGE = 2;
        public const int SMT_STAGE = 3;

        public const string COL_DELIM = "\u001C";        
        public const string ROW_DELIM = "\u001B";
        public const string TEXT_QUALIFIER = "\u001A";
        public const string TREE_EXPAND_PATH = "tree_expand_path";
        public const string WT_DELIM = "\u200E";
        public const string STR_VALUE_SEPARATOR = ";&)";
        /**
         * 0:data folder;
         * 1:report folder;
         * 2:chart folder;
         * 3:excels folder;
         * 4.dashboard folder;
         * 5.publish folder;
         * 6.Account folder;
         * 7:dataserver;
         * 8:database;
         * 9:report file;
         * 10:chart file;
         * 11:excels file;
         * 12.dashboard file;
         * 13:publish report file;
         * 14:publish chart file;
         * 15:publish excels file;
         * 16.publish dashboard file;
         */
        public const int NODE_TYPE_FOLDER_DATA = 0;
        public const int NODE_TYPE_FOLDER_REPORT = 1;
        public const int NODE_TYPE_FOLDER_CHART = 2;
        public const int NODE_TYPE_FOLDER_EXCEL = 3;
        public const int NODE_TYPE_FOLDER_DASHBOARD = 4;
        public const int NODE_TYPE_FOLDER_PUBLISH = 5;
        public const int NODE_TYPE_FOLDER_ACCOUNT = 6;
        public const int NODE_TYPE_DATASERVER = 7;
        public const int NODE_TYPE_DATABASE = 8;
        public const int NODE_TYPE_REPORT = 9;
        public const int NODE_TYPE_CHART = 10;
        public const int NODE_TYPE_EXCEL = 11;
        public const int NODE_TYPE_DASHBOARD = 12;
        public const int NODE_TYPE_REPORT_PUBLISH = 13;
        public const int NODE_TYPE_CHART_PUBLISH = 14;
        public const int NODE_TYPE_EXCEL_PUBLISH = 15;
        public const int NODE_TYPE_DASHBOARD_PUBLISH = 16;


        //统计查询 --如果不做统计，则NO.
        //statistic value enum 
        public const string EDIT_FIELD_STATISTIC_AVG = "AVG";
        public const string EDIT_FIELD_STATISTIC_COUNT = "COUNT";
        public const string EDIT_FIELD_STATISTIC_COUNTDISTINCT = "COUNT DISTINCT";
        public const string EDIT_FIELD_STATISTIC_MAX = "MAX";
        public const string EDIT_FIELD_STATISTIC_MIN = "MIN";
        public const string EDIT_FIELD_STATISTIC_SUM = "SUM";
        public const string EDIT_FIELD_STATISTIC_NO = "";

        //public const string EDIT_COLUMN_STATISTIC_SUBTOTAL = "SUBTOTAL";
        //public const string EDIT_COLUMN_STATISTIC_AVERAGE = "AVERAGE";
        //public const string EDIT_COLUMN_STATISTIC_SUBTOTAL_AVERAGE = "SUBTOTAL_AVERAGE";
        //public const string EDIT_COLUMN_STATISTIC_AVERAGE_SUBTOTAL = "AVERAGE_SUBTOTAL";

        //小计字段
        //subtotal value enum;
        public const string EDIT_FIELD_SUBTOTAL_NO = "";
        public const string EDIT_FIELD_SUBTOTAL_SEPARATE = "SEPARATE";
        public const string EDIT_FIELD_SUBTOTAL_SUBTOTAL = "SUBTOTAL";
        public const string EDIT_FIELD_SUBTOTAL_AVERAGE = "AVERAGE";
        public const string EDIT_FIELD_SUBTOTAL_SUBTOTAL_AVERAGE = "SUBTOTAL_AVERAGE";
        public const string EDIT_FIELD_SUBTOTAL_AVERAGE_SUBTOTAL = "AVERAGE_SUBTOTAL";


        //排序 – 排序类型（NO，不排，ASC，升序，DESC，降序）
        //sort value enum;
        public const string EDIT_FIELD_SORT_NO = "";
        public const string EDIT_FIELD_SORT_ASC = "ASC";
        public const string EDIT_FIELD_SORT_DESC = "DESC";

       
        //表达式类型
        //type value enum;
        public const string EDIT_FIELD_TYPE_STRING = "String";
        public const string EDIT_FIELD_TYPE_NUMBER = "Number";

        //条件设定，运算符的常量
        //operate value enum;
        public const string EDIT_CONDITION_OPERATE_EQUAL = "=";
        public const string EDIT_CONDITION_OPERATE_LESS_THAN= "<";
        public const string EDIT_CONDITION_OPERATE_GREATER_THAN = ">";
        public const string EDIT_CONDITION_OPERATE_UNEQUAL = "<>";
        public const string EDIT_CONDITION_OPERATE_GREATER_OR_EQUAL = ">=";
        public const string EDIT_CONDITION_OPERATE_LESS_OR_EQUAL = "<=";
        public const string EDIT_CONDITION_OPERATE_BETWEEN = "BETWEEN";
        public const string EDIT_CONDITION_OPERATE_LIKE = "LIKE";
        public const string EDIT_CONDITION_OPERATE_IN = "IN";

        //表达式类型设定，运算符的常量
        //expressionType value enum;
        public const string EDIT_FIELD_EXPRESSION_TYPE_STANDARD = "STANDARD";
        public const string EDIT_FIELD_EXPRESSION_TYPE_EXTEND = "EXTEND";
        public const string EDIT_FIELD_EXPRESSION_TYPE_ALIAS = "ALIAS";


        //条件设定数据类型
        public const string EDIT_CONDITION_FIELDTYPE_CHAR = "FIELD_TYPE_CHAR";
        public const string EDIT_CONDITION_FIELDTYPE_DATE = "FIELD_TYPE_DATE";
        public const string EDIT_CONDITION_FIELDTYPE_NUMERIC = "FIELD_TYPE_NUMERIC";
        public const string EDIT_CONDITION_FIELDTYPE_BOOLEAN = "FIELD_TYPE_BOOLEAN"; 


        //表名字段名之间的分割符
        public const string EDIT_TABLE_FIELD_SEPERATE = "."; 

        //数据类型数据之间加的()符号
        public const string SQL_DATA_TYPE_LENGTH_SEPERATE_LEFT = "(";
        public const string SQL_DATA_TYPE_LENGTH_SEPERATE_RIGHT = ")";

        //Multiple Y Axis
        public const string Y_AXIS_NONE = "None";
        public const string Y_AXIS_SECONDARY_AXIS = "Secondary Axis";
        public const string Y_AXIS_MORE_AXES = "More Axes";
        public const string Y_AXIS_1 = "Y-Axis-1";
        public const string Y_AXIS_2 = "Y-Axis-2";

		//char type add itc205106
        public const string CHART_TYPE_COLUMN = "Column";
        public const string CHART_TYPE_LINE = "Line";
        public const string CHART_TYPE_PIE = "Pie";
        public const string CHART_TYPE_LINE_COLUMN = "Line-Column";
        public const string CHART_TYPE_PARETO = "Pareto";

        //char subtype add itc205106
        public const string CHART_SUBTYPE_CLUSTERED_COLUMN = "Clustered Column";
        public const string CHART_SUBTYPE_STACKED_COLUMN = "Stacked Column";
        public const string CHART_SUBTYPE_NORMAL_PIE = "Normal Pie";
        public const string CHART_SUBTYPE_EXPLODED_PIE = "Exploded Pie";

        //char dimension add itc205106
        public const string CHART_DIMENSION_2D = "2D";
        public const string CHART_DIMENSION_2D_DEPTH = "2D with Depth";

        //series type add itc205106
        public const string CHART_SERIES_TYPE_CLUSTERED_COLUMN = "Clustered Column";
        public const string CHART_SERIES_TYPE_STACKED_COLUMN = "Stacked Column";
        public const string CHART_SERIES_TYPE_LINE = "Line";
        public const string CHART_SERIES_TYPE_NORMAL_PIE = "Normal Pie";
        public const string CHART_SERIES_TYPE_EXPLODED_PIE = "Exploded Pie";

		//added by itc204011
		public const string RBPC_APPLICATION = "FISREPORT10";
		public const string PARAMETER_TYPE_DOMAIN = "domain";
		public const string PARAMETER_TYPE_COMPANY = "company";
		public const string PARAMETER_TYPE_DEPARTMENT = "department";
		public const string ADD_SINGLE_USER_ROLE = "add_single_user_role";
		public const string ADD_USERS_TO_ROLE = "add_users_to_role";
		public const string FLAG_SUCCESS = "flag_success";
		public const string RECORD_SUM = "record_sum";
		public const string PLEASE_ENTER_A_VALID = "Please enter a valid ";

		public const string RBPC_PERMISSION_SYS_ACCOUNT_AUTHORITY = "00000000000000000000000000000001";
		public const string RBPC_PERMISSION_SYS_DATA_SOURCE_SETTING = "00000000000000000000000000000002";
		public const string RBPC_PERMISSION_SYS_PUBLISH = "00000000000000000000000000000003";
		public const string RBPC_PERMISSION_REPORT_CREATE_EDIT = "00000000000000000000000000000004";
		public const string RBPC_PERMISSION_REPORT_DELETE = "00000000000000000000000000000005";
		public const string RBPC_PERMISSION_CHART_CREATE_EDIT = "00000000000000000000000000000006";
		public const string RBPC_PERMISSION_CHART_DELETE = "00000000000000000000000000000007";

        //numberformat add by itc205106
        public const string NUMBER_FORMAT_INTEGER = "#,###";
        public const string NUMBER_FORMAT_DECIMAL = "#,###.###";
        public const string NUMBER_FORMAT_PERCENT_INTEGER = "#,###%";
        public const string NUMBER_FORMAT_PERCENT = "#,###.###%";


        //add by lzy
        public const string CHART_OR_REPORT_STRUCT = "key_in_session_chart_report ";
        public const string REPORT_STYLEXML = "key_in_session_stylexml";
        public const string DELIMITER_STRUCT_STYLE_OF_REPORT = "split_point_struct_style";
        public const string CR_EDIT_ANCESTOR_UPLOAD = "UPLOAD";

        //数据传输 如chart自己的ID 205106 for tree
        public const string NODE_TYPE = "type";
        public const string TREE_UUID = "uuid";
        public const string NODE_TEXT = "text";
        public const string NODE_UUID = "nodeuuid";
        public const string PARENT_TREE_ID = "parentTreeId";
        public const string PARENT_NODE_ID = "parentNodeuuid";


        //by xmzh，为读week的偏移量定义的函数名称，偏移量各站不一样，此常量大家一般不用，不要修改
        public const string RUN_TIME_WEEK_NUMBER_OF_MONTH_FUNCTION = "dimDealWeekNumberOfMonth";
        public const string RUN_TIME_WEEK_NUMBER_OF_YEAR_FUNCTION = "dimDealWeekNumberOfYear";

        public const string ENTER_TYPE = "entertype";

        //add by zqr 205106 for y 轴 类型
        public const string VALUE_AXIS_TYPE_FIELD_VALUE = "set series from field value";
        public const string VALUE_AXIS_TYPE_FIELD = "set series from field";

        //add by zqr来自内网，或者外网
        public const string COME_FROM_KEY = "comefromkey";
        public const string COME_FROM_INNER = "inner";
        public const string COME_FROM_OUTER = "outer";
        public const string COME_FROM_OUTER_ROOT = "outer_root";

        //add by zqr
        public const string CHART_PARETO_SERIES_NAME = "Accum Rate";
        public const string CHART_PARETO_NON_SERIES_NAME = "Defect Rate";

        //for import user, add by kandaoming
        public const string ADMIN_USER = "admin_user";
        public const string ADMIN_PWD = "admin_pwd";
        public const string DOMAIN_LDAP_ADDRESS = "domain_ldap_address";
        public const string DOMAIN_SEARCH_FILTER = "domain_search_filter";
        public const string DOMAIN_NAME = "domain_name";


        public const int DIM_DATETIME_INPUT_CONTROL_TYPE_DATE = 0;
        public const int DIM_DATETIME_INPUT_CONTROL_TYPE_TIME = 1;
        public const int DIM_DATETIME_INPUT_CONTROL_TYPE_CAN_NOT_SELECT = 2;

        public const string DATE_TIME_SEPARATING_DELIMITER = ":";

        public const string DIM_DATETIME_MARK_RUNTIME_DATE = "RunTimeDate";//+-365
        public const string DIM_DATETIME_MARK_RUNTIME_WEEKNUMBER_OF_MONTH = "RunTimeWeekNumberOfMonth";//+-5
        public const string DIM_DATETIME_MARK_RUNTIME_WEEKNUMBER_OF_YEAR = "RunTimeWeekNumberOfYear";//+-52
        public const string DIM_DATETIME_MARK_RUNTIME_MONTH = "RunTimeMonth";//+-12
        public const string DIM_DATETIME_MARK_RUNTIME_QUARTER = "RunTimeQuarter";//+-4
        public const string DIM_DATETIME_MARK_RUNTIME_YEAR = "RunTimeYear";//+-5
        public const string DIM_DATETIME_MARK_RUNTIME_HOUR = "RunTimeHour";//+-23
        public const string DIM_DATETIME_MARK_RUNTIME_SIMPLEHOUR = "RunTimeSimpleHour";//+-23
        public const string DIM_DATETIME_MARK_OTHER = "CanNotSelect";


        public const string DIM_DATETIME_FUNCTION_RUNTIME_DATE = "dimDealDate";//+-365
        public const string DIM_DATETIME_FUNCTION_RUNTIME_WEEKNUMBER_OF_MONTH = "dimDealWeekNumberOfMonth";//+-5
        public const string DIM_DATETIME_FUNCTION_RUNTIME_WEEKNUMBER_OF_YEAR = "dimDealWeekNumberOfYear";//+-52
        public const string DIM_DATETIME_FUNCTION_RUNTIME_MONTH = "dimDealMonth";//+-12
        public const string DIM_DATETIME_FUNCTION_RUNTIME_QUARTER = "dimDealQuarter";//+-4
        public const string DIM_DATETIME_FUNCTION_RUNTIME_YEAR = "dimDealYear";//+-5
        public const string DIM_DATETIME_FUNCTION_RUNTIME_HOUR = "dimDealHour";//+-23
        public const string DIM_DATETIME_FUNCTION_RUNTIME_SIMPLEHOUR = "dimDealSimpleHour";//+-23

        public const int RUNTIME_DATE_UP_LIMIT = 365;
        public const int RUNTIME_DATE_DOWN_LIMIT = -365;
        public const int RUNTIME_WEEKNUMBER_OF_MONTH_UP_LIMIT = 5;
        public const int RUNTIME_WEEKNUMBER_OF_MONTH_DOWN_LIMIT = -5;
        public const int RUNTIME_WEEKNUMBER_OF_YEAR_UP_LIMIT = 52;
        public const int RUNTIME_WEEKNUMBER_OF_YEAR_DOWN_LIMIT = -52;
        public const int RUNTIME_MONTH_UP_LIMIT = 12;
        public const int RUNTIME_MONTH_DOWN_LIMIT = -12;
        public const int RUNTIME_QUARTER_UP_LIMIT = 4;
        public const int RUNTIME_QUARTER_DOWN_LIMIT = -4;
        public const int RUNTIME_YEAR_UP_LIMIT = 5;
        public const int RUNTIME_YEAR_DOWN_LIMIT = -5;
        public const int RUNTIME_HOUR_UP_LIMIT = 23;
        public const int RUNTIME_HOUR_DOWN_LIMIT = -23;
        public const int RUNTIME_SIMPLEHOUR_UP_LIMIT = 23;
        public const int RUNTIME_SIMPLEHOUR_DOWN_LIMIT = -23;

        public const int SQL_TOP_NUMBER = 10000;

        public const string COOKIE_LOGIN = "portal_cookie_login";
        public const string COOKIE_LOGIN_NAME = "portal_cookie_login_name";
        public const string COOKIE_LOGIN_PWD = "portal_cookie_login_pwd";
        public const string COOKIE_LOGIN_REMEMBER = "portal_cookie_login_remember";
        public const string COOKIE_EXTERNAL_LOGIN = "cookie_external_login";
        public const string COOKIE_EXTERNAL_LOGIN_NAME = "cookie_external_login_name";
        public const string COOKIE_EXTERNAL_LOGIN_PWD = "cookie_external_login_pwd";
        public const string COOKIE_EXTERNAL_LOGIN_REMEMBER = "cookie_external_login_remember";

        public const int SELECT_VALUE_LIST_TOP_NUMBER = 3000;
        public const String LOGIN_ENTRY = "LOGIN_ENTRY";


        //added by itc98079
        public const string APPLICATION_ALL = "all";
        public const string RBPC_PERMISSION_TYPE_PRIMARY = "/permission";
        public const string DOMAIN_SELECT_ITEM_LOCAL = "local";

        public const string TABLE_COLUMN_LOCAL = "local";
        public const string TABLE_COLUMN_DOMAIN = "domain";
        public const string TABLE_COLUMN_COMPANY = "company";
        public const string TABLE_COLUMN_DEPARTMENT = "department";

        public const string TABLE_COLUMN_USER_EMAIL = "eMail";
        public const string TABLE_COLUMN_USER_LOGIN = "Login";
        public const string TABLE_COLUMN_USER_NAME = "Name";
        public const string TABLE_COLUMN_USER_DEPARTMENT = "Department";
        public const string TABLE_COLUMN_USER_COMPANY = "Company";
        public const string TABLE_COLUMN_USER_DESCRIPTION = "Description";
        public const string TABLE_COLUMN_USER_GROUP = "Group";
        public const string TABLE_COLUMN_USER_ID = "id";

        public const string TABLE_COLUMN_GROUP_SYMBOL = " ";
        public const string TABLE_COLUMN_GROUP_NAME = "User Group";
        public const string TABLE_COLUMN_GROUP_CDT = "Create Time";
        public const string TABLE_COLUMN_GROUP_UDT = "Update Time";
        public const string TABLE_COLUMN_GROUP_AUTHOR = "Author";
        public const string TABLE_COLUMN_GROUP_COMMENT = "Comment";
        public const string TABLE_COLUMN_GROUP_AUTHOR_ID = "AuthorId";
        public const string TABLE_COLUMN_GROUP_ID = "id";
        public const string TABLE_COLUMN_GROUP_TYPE = "type";

        public const string TABLE_COLUMN_ACCOUNT_NAME = "name";
        public const string TABLE_COLUMN_ACCOUNT_CDT = "cdt";
        public const string TABLE_COLUMN_ACCOUNT_UDT = "udt";
        public const string TABLE_COLUMN_ACCOUNT_EDITORID = "editorId";
        public const string TABLE_COLUMN_ACCOUNT_DESCR = "descr";
        public const string TABLE_COLUMN_ACCOUNT_LOGIN = "login";
        public const string TABLE_COLUMN_ACCOUNT_ID = "id";
        public const string TABLE_COLUMN_ACCOUNT_TYPE = "type";



        public const string TABLE_COLUMN_PERMISSION_NAME = "Name";
        public const string TABLE_COLUMN_PERMISSION_ID = "Id";

        public const string RBPC_ACCOUNT_TYPE_GROUP = "0";
        public const string RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP = "1";

        public const string RBPC_ACCOUNT_USER_TYPE = "userType";
        public const string RBPC_ACCOUNT_USER = "accountuser";
        public const string RBPC_ACCOUNT_DEPARTMENT = "accountdepartment";
        public const string RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR = ";";

        public const string CHECKBOX_ITEM_DELIM = ",";

        public const string ALL_OPTION = "All";

        public const string SUBSYSTEM_OPEN_MODE_IN = "0";    //sub system的打开方式，以内嵌方式打开
        public const string SUBSYSTEM_OPEN_MODE_OUT = "1";    //sub system的打开方式，以外部方式打开

        public const string SINGLE_USER_NAME = "single user";
        public const string GROUP_NAME = "group";
        public const string DOMAIN_USER_NAME = "domain user";
        public const string LOCAL_USER_NAME = "local user";
        public const string DEPARTMENT_NAME = "department";  

        //by xmzh//////////
        public const string DATASOURCE_TYPE_REAL = "0";
        public const string DATASOURCE_TYPE_UNREAL = "1";


        public const string DASHBOARD_STAGE_DATA = "Dashboard_Stage_Data";
        public const string DASHBOARD_LINE_DATA = "Dashboard_Line_Data";
        public const string DASHBOARD_STATION_DATA = "Dashboard_Station_Data";
        public const string DASHBOARD_FAMILY_DATA = "Dashboard_Family_Data";

        public const string DASHBOARD_STAGE_DATA_UR = "Dashboard_Stage_Data_UR";
        public const string DASHBOARD_LINE_DATA_UR = "Dashboard_Line_Data_UR";
        public const string DASHBOARD_STATION_DATA_UR = "Dashboard_Station_Data_UR";
        public const string DASHBOARD_FAMILY_DATA_UR = "Dashboard_Family_Data_UR";


    }
}

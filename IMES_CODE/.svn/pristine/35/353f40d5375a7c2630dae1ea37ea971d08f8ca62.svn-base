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
/// Summary description for ExceptionMsg
/// </summary>
namespace com.inventec.system
{
    public class ExceptionMsg
    {

        public ExceptionMsg()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //session ³¬Ê±
        public const string SESSION_TIMEOUT = "Sorry, your session has timed out. Please logon again!";
        public const string ERROR_PARAM_NULL = "param is null";
        public const string DB_OBJECT_NULL = "Database object is null!";

        public const string ERROR_CONNECT_DATA_SERVER = "System can't connect to the database server.";
        public const string ERROR_GET_DATA_BASE_LIST = "No database selected.";
        public const string ERROR_SELECT_DATA_BASE_LIST = "All of the databases you selected can't be connected, please select again.";
        public const string ERROR_DELETE_DATA_BASE = "Some detabase selected cannot be deleted. Including ";

        public const string ERROR_DELETE_DATA_SERVER = "This Database server is being used. You can't delete it.";

        public const string ERROR_DISPALY_CHCAT_COLUMN_NAME = "Sorry, the column name selected is wrong!";
        public const string ERROR_DISPALY_CHCAT_COLUMN_DATA = "Sorry, the column data selected is wrong!";

        public const string ERROR_PUBLISH_GET_PARAM = "Sorry, the param is null!";
        public const string ERROR_PUBLISH_SAVE_PARAM = "Sorry, errors occurred during the file is saved!";

        public const string ERROR_CHART_PIC_NOT_EXIST = "Sorry, the chart don't exist!";
        public const string ERROR_CHART_FILE_SAVE = "Sorry, errors occurred during the file is exported!";

        //add by zqr for pareto chart
        public const string ERROR_CHART_PARETO_TWO_AXIS = "The pareto chart must has two Y Axis!";
        public const string ERROR_CHART_PARETO_Y1_AXIS_SERIES = "The pareto chart must has only one series!";//add by zqr
        public const string ERROR_CHART_PARETO_COUNT = "The data amounts of the pareto chart must be more than ten!";//add by zqr
        public const string ERROR_CHART_PARETO = "The chart is not a pareto chart!";//add by zqr

		public const string ERROR_AUTHORITY_NULL_PARAM = "System cannot get the information! Please refresh main window. Sorry for the inconvenience.";//added by itc204011
		public const string ERROR_SAME_GROUP_EXISTED = "This group name already exists. Please enter a new name.";//added by itc204011
		public const string ERROR_GROUP_NAMES_HAVE_BEEN_USED = "  This group name already exists. You can rename the existing group name.";
		public const string ERROR_SAME_ACCOUNT_EXISTED = "A same account has already existed";//added by itc204011
		public const string ERROR_PLEASE_ENTER_A_VALID_GROUP_NAME = "Please enter a valid group name.";//added by itc204011
		public const string ERROR_SELECT_DEPARTMENT = "Please select at least one department.";//added by itc204011
        
        public const string ERROR_IMOUTPORT_CANT_FIND_STRUCT = "Can't get struct from Session!";//added by lzy
        public const string ERROR_IMOUTPORT_WRONG_FORMAT = "Invalid file format!";//added by lzy
        public const string ERROR_IMOUTPORT_FILE_TOO_LARGE = "The file is too large!";//added by lzy
        public const string ERROR_IMOUTPORT_WRONG_FILE_TYPE = "This file type can't be uploaded!";//added by lzy
        public const string ERROR_IMOUTPORT_WRONG_UUID = "Can't find this file!";//added by lzy
        
        //added by hml
        public const string ERROR_REPORT_FOLDER_NAME = "This folder name is already used. Please enter a new name.";

        public const string DOMAINCONFIG_PROPERTIES_ERROR = "Error in domainconfig properties!";

        public const string REPORT_TEMPLATE_SHEET = "There can be only one template sheet!";
        public const string REPORT_SHEET_COUNT_0 = "No data to be shown!";
        public const string REPORT_SHEET_COUNT = "Must not be more than 255 sheets!";
        public const string REPORT_TEMPLATE_ERROR = "The report template is error!";
        public const string REPORT_ROWS_COUNT = "Excel can not be more than 65536 lines!";
        public const string REPORT_COLUMNS_COUNT = "Excel can not be more than 256 columns!";
		public const string REPORT_AREA_ERROR = "Sorry, excel can not be more than 65536 lines, 256 columns!";
        public const string REPORT_NAME_DUPLICATE = "Sorry, the file name has already exists in database, please reset it again!";
		public const string DIVISOR_0 = "Sorry, Divisor can not be 0!";
		public const string EXPRESSION_TYPE_ERROR = "Sorry, expression's type is error!";
        public const string CHART_COUNT_0 = "No data to be shown!";
        public const string ERROR_DISPALY_CHCAT = CHART_COUNT_0;
        public const string CHART_NAME_DUPLICATE = "Sorry, the name has already existed!";
        public const string ALIAS_DUPLICATE = "Sorry, the alias has already existed!";
        public const string CHART_DELETED = "Sorry, the chart is deleted!";

        //by xmzh
        public const string SQL_GENERATE_MSG_EXPRESSION_TYPE_NOT_CORRECT = "ExpressionType in fieldInfo is not correct.";
        public const string SQL_GENERATE_MSG_FIELD_ALIAS_NOT_CORRECT = "FieldAlias in fieldInfo is not correct.";
        public const string SQL_GENERATE_MSG_STATISTIC_NOT_CORRECT = "Statistic in fieldInfo is not correct.";
        public const string SQL_GENERATE_MSG_CONDITION_FILENAME_NOT_CORRECT = "FieldName in condition is not correct.";
        public const string SQL_GENERATE_MSG_CONDITION_DIMDATETIME_NOT_CORRECT = "DimDateTimeData in condition is not correct.";
        public const string SQL_GENERATE_MSG_RELATIONSHIP_NOT_CORRECT = "Relationship data is not correct.";
        public const string SQL_GENERATE_MSG_FIELD_INFO_EXPRESSION_NOT_CORRECT = "Expression in fieldInfo is not correct.";
        public const string SQL_GENERATE_MSG_FIELD_INFO_EMPTY = "Definition in field info is empty.";
        public const string SQL_GENERATE_MSG_UNKNOWN_STATIC = "Unknown static function.";
        public const string SQL_GENERATE_MSG_FIELD_DEFINE_NOT_CORRECT = "Field define is not correct.";
        public const string SQL_GENERATE_MSG_RELATIONSHIP_SETTING_NOT_CORRECT = "Table's relationship setting is not correct.";
        public const string SQL_GENERATE_MSG_RELATIONSHIP_NO_RELATION_PROMPT1 = "Not all of table have relations in your relationship setting.";
        public const string SQL_GENERATE_MSG_TIME_OUT_PARAM_ERROR = "Sql time out value is not setted correctly in web.config file.";

        //1. A,B,F
        //2. C,D,
        public const string SQL_GENERATE_MSG_RELATIONSHIP_NO_RELATION_PROMPT2 = "Continue to do anyway?";
        public const string SQL_GENERATE_MSG_CONDITION_DATA_NOT_CORRECT = "Condition data is not right, empty value.";
        public const string SQL_GENERATE_MSG_CONDITION_DATA_TYPE_NOT_CORRECT = "Data type condition is not correct.";
        public const string SQL_GENERATE_MSG_NEED_SORT_DATA = "Sort data is necessary in topinfo structure if you have defined other data in topInfo.";
        public const string SQL_GENERATE_MSG_RUNTIME_PARAMETER_NOT_CORRECT = "Runtime parameter in condition set is not correct.";
        public const string SQL_GENERATE_MSG_OPERATION_NOT_SUPPORT = "Operation in condition  is not to be supported.";
        public const string SQL_GENERATE_MSG_NO_FOUND_TIME_DATA = "Can not find data in the database, Or may be selected date is out of range.";
        public const string SQL_GENERATE_MSG_DATABASE_ID_NOT_CORRECT = "The database id in parameter is not correct.";
        public const string SQL_GENERATE_MSG_DATABASE_REMOVED = "The specified database has been removed.";
        public const string SQL_GENERATE_MSG_KEY_NOT_EXIST = "The key is not exist in dictionary parameter.";
        public const string SQL_GENERATE_MSG_CONDITION_PARAMETER_NOT_CORRECT = "Data parameter in condition set is not correct.";

        public const string SQL_RESULT_EXCED_LIMIT = "Record number of data set is larger than 10,000. System will intercept 10,000.";

        public const string SQL_GENERATE_MSG_SQLSERVER_ERROR_PROMPT = "Sql server error:";

        public const string SELECT_VALUE_LIST_PRECONDITION_EMPTY_ERROR = "Parameter in prior condition is not correct, empty value.";

        public const string SELECT_VALUE_LIST_PRECONDITION_ERROR = "Parameter in prior condition is not correct.";

        public const string SELECT_VALUE_LIST_INVALID = "Invalid datatable.";

        //add by xmzh
        public const string SQL_GENERATE_MSG_BIT_PARAM_NOT_RIGHT = "Parameter value is not in correct.";

        //////////////////

    }
}

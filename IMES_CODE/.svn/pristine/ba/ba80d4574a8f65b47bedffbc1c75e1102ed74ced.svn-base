var IMESUti = IMESUti || {};

IMESUti.Url=( function() {
 
    //private
    var _station = $.url.param("Station")||'';
    var _pcode=$.url.param("PCode")||'';
    var _userid=$.url.param("UserId")||'';
    var _customer=$.url.param("Customer")||'';
    var _username=$.url.param("UserName")||'';
    var _accountid=$.url.param("AccountId")||'';
    var _login=$.url.param("Login")||'';
    var _sessionid=$.url.param("SessionId")||'';
    var _uObj = {
         Station: _station,
         UserId : _userid,
         UserName :_username,
         Customer :_customer,
         AccountId:_accountid,
         Login:_login,
         PCode:_pcode,
         SessionId:_sessionid
    };

    return  {
        GetUserInfo: function() {
            return _uObj;
        },
        GetParamByName:function(name,defValue) {
          var _d=defValue||''
          return $.url.param(name)||_d;
        }
    }
}
)();

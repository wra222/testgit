var ExecSPWebService=function() {
ExecSPWebService.initializeBase(this);
this._timeout = 0;
this._userContext = null;
this._succeeded = null;
this._failed = null;
}
ExecSPWebService.prototype={
_get_path:function() {
 var p = this.get_path();
 if (p) return p;
 else return ExecSPWebService._staticInstance.get_path();},
GetSPResult:function(editor,db,sp,parameterNameArray,parameterValueArray,succeededCallback, failedCallback, userContext) {
return this._invoke(this._get_path(), 'GetSPResult',false,{editor:editor,db:db,sp:sp,parameterNameArray:parameterNameArray,parameterValueArray:parameterValueArray},succeededCallback,failedCallback,userContext); }}
ExecSPWebService.registerClass('ExecSPWebService',Sys.Net.WebServiceProxy);
ExecSPWebService._staticInstance = new ExecSPWebService();
ExecSPWebService.set_path = function(value) { ExecSPWebService._staticInstance.set_path(value); }
ExecSPWebService.get_path = function() { return ExecSPWebService._staticInstance.get_path(); }
ExecSPWebService.set_timeout = function(value) { ExecSPWebService._staticInstance.set_timeout(value); }
ExecSPWebService.get_timeout = function() { return ExecSPWebService._staticInstance.get_timeout(); }
ExecSPWebService.set_defaultUserContext = function(value) { ExecSPWebService._staticInstance.set_defaultUserContext(value); }
ExecSPWebService.get_defaultUserContext = function() { return ExecSPWebService._staticInstance.get_defaultUserContext(); }
ExecSPWebService.set_defaultSucceededCallback = function(value) { ExecSPWebService._staticInstance.set_defaultSucceededCallback(value); }
ExecSPWebService.get_defaultSucceededCallback = function() { return ExecSPWebService._staticInstance.get_defaultSucceededCallback(); }
ExecSPWebService.set_defaultFailedCallback = function(value) { ExecSPWebService._staticInstance.set_defaultFailedCallback(value); }
ExecSPWebService.get_defaultFailedCallback = function() { return ExecSPWebService._staticInstance.get_defaultFailedCallback(); }
ExecSPWebService.set_path("/IMES2012_Maintain/CommonFunction/Service/ExecSPWebService.asmx");
ExecSPWebService.GetSPResult= function(editor,db,sp,parameterNameArray,parameterValueArray,onSuccess,onFailed,userContext) {ExecSPWebService._staticInstance.GetSPResult(editor,db,sp,parameterNameArray,parameterValueArray,onSuccess,onFailed,userContext); }

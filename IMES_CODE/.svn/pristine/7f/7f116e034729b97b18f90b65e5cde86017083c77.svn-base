// author: He Jiang (mailto:hejiang@tju.edu.cn)

var args = WScript.Arguments.Named;

var lf = args('listfile');
var pa = args('parameters');

if (!lf) {
	WScript.Echo('Usage: CScript.exe PrintPDF.js /listfile:pdf-list-file.txt /parameters:addtional-parameters');
	WScript.Quit(1);
}

if (!pa || !isNaN(pa)) {
	pa = '-print-to-default -print-settings "noscale"';
}else {
    pa = '-print-to ' + '"'+pa+'"' +' -print-settings "noscale"';
}

var fso = WScript.CreateObject("Scripting.FileSystemObject");
var lf_file = fso.OpenTextFile(lf, 1/*ForReading*/, false/*Dont Create*/, 0/*TristateUseDefault*/);
if (!lf_file) {
	WScript.Echo('Open file failed.');
	WScript.Quit(2);
}


var exe_path = '"' + fso.GetFile(WScript.ScriptFullName).ParentFolder.Path + '\\SumatraPDF.exe"';
var lf_txt = '';
var cmd_line ='';
var recordCount=0;
while (!lf_file.AtEndOfStream) {
	var l = lf_file.ReadLine().replace(/\s*(\S+)\s*/, '$1');
	if (l)
	{  
		lf_txt += '"' + l + '" ';
	    if (recordCount>24)
		{			
			cmd_line = exe_path + ' ' + pa + ' ' + lf_txt;
			WScript.Echo(cmd_line);			
			WScript.CreateObject("WScript.Shell").Run(cmd_line, 0,true);			
			lf_txt='';
			recordCount=0;				
		}
		else
		{
			recordCount++;
		}
	}
}

if(lf_txt!='')
{	
	cmd_line = exe_path + ' ' + pa + ' ' + lf_txt;
	WScript.Echo(cmd_line);	
	WScript.CreateObject("WScript.Shell").Run(cmd_line, 0, false);
}



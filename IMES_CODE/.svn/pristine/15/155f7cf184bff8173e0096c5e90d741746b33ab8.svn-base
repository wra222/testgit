@ECHO OFF
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM Install windows service of IMES...
%FrameworkDIR%\v2.0.50727\installUtil /u IMES.Query.Service.exe
if %errorlevel% neq 0 goto END
ECHO unInstall IMES Query service success!
GOTO END

:END
@ECHO ON

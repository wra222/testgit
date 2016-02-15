@ECHO OFF
set CWD=%~dp0
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM Install windows service of IMES...
%FrameworkDIR%\v2.0.50727\installUtil /u %CWD%TAPService.exe
if %errorlevel% neq 0 goto END
ECHO install TAP service success!
GOTO END

:END
@ECHO ON
PAUSE

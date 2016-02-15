@ECHO OFF
set path=%~dp0bin;%path%>> %SystemRoot%\System32\autoexec.nt

if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM Install windows service of IMESService...
%FrameworkDIR%\v2.0.50727\InstallUtil IMES.Service.PizzaKitting.exe
if %errorlevel% neq 0 goto END
ECHO install IMES PizzaKitting Service success!
GOTO END

:END
@ECHO ON

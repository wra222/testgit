@ECHO OFF
REM Prepare directory
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM step1 Delete output;create new output, copy workflow+bat+exe to output service folder
rd output /s/q
mkdir "output\IMESQueryService"
mkdir "output\IMESQueryWeb"
mkdir "output\IMESQueryWeb\TmpExcel"
mkdir "output\IMESQueryWeb\tmp"

REM step2 clear old bin; build service solution
%FrameworkDIR%\v3.5\MSBuild.exe IMESService\IMESQueryService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
%FrameworkDIR%\v3.5\MSBuild.exe IMESService\IMESQueryService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildService


ECHO -------------------------------------          
ECHO *************************************
echo *** IMES Query service build succeeded! *** 
ECHO *************************************    
ECHO -------------------------------------

REM step3 copy builded bin files to service and  del pdb file
del bin\*.pdb /s/f/q
xcopy bin\*.bat "output\IMESQueryService" /y/s
xcopy bin\*.dll "output\IMESQueryService" /y/s
xcopy bin\*.exe "output\IMESQueryService" /y/s
xcopy bin\*.config "output\IMESQueryService" /y/s
REM del output\IMESQueryService\uninstallIMESMaintainService.bat /s/f/q
REM del output\IMESQueryService\installIMESMaintainService.bat /s/f/q

REM step4 build IMESWeb
REM %FrameworkDIR%\v2.0.50727\aspnet_compiler -v "WEB" -p "WEB" -fixednames "output\IMESQueryWeb" -c -u
%FrameworkDIR%\v2.0.50727\aspnet_compiler -v "WEB" -p "WEB" -c 
if %errorlevel% neq 0 goto ErrBuildUI
REM del WEB\Bin\IMES.DataModel.dll
REM del WEB\Bin\IMES.Infrastructure.FisException.dll
REM del WEB\Bin\IMES.Station.Interface.dll
REM del WEB\Bin\IMES.Maintain.Interface.dll

xcopy WEB "output\IMESQueryWeb" /y/s



ECHO -----------------------------------          
ECHO ***********************************
ECHO *** IMES IMESQueryWeb build succeeded! ***
ECHO ***********************************
ECHO -----------------------------------  
GOTO ClearAll


:ErrBuildService
rd output /s/q 
ECHO ---------------------------------
ECHO !!! IMES service build failed !!!
ECHO ---------------------------------
GOTO ClearAll

:ErrBuildUI
ECHO --------------------------------
ECHO !!! IMES IMESQueryWeb build failed !!!
ECHO --------------------------------
GOTO ClearAll

:ClearAll
rem del bin\*.dll
del bin\*.exe 
del bin\*.config

GOTO END

:END
pause
@ECHO ON



@ECHO OFF
REM Prepare directory
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework
CALL BuildIMESKernel.bat

@ECHO OFF
REM step1 Delete output;create new output, copy workflow+bat+exe to output service folder
rd output /s/q
mkdir "output\IMESWeb"
mkdir "output\IMESService"
mkdir "output\IMESService\rules"
mkdir "output\IMESService\PartPolicyModule"
mkdir "output\IMESMaintainService"
mkdir "output\SQL"

xcopy Service\IMES.Station.Workflow\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\*.rules  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\BIRCH\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\BIRCH\*.rules  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\BSam\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\BSam\*.rules  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\DefectComponent\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\DefectComponent\*.rules  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\FAI\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\FAI\*.rules  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\TRIS\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\TRIS\*.rules  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\CPU\*.xoml  output\IMESService\ /y/Q
xcopy Service\IMES.Station.Workflow\CPU\*.rules  output\IMESService\ /y/Q


xcopy SQL output\SQL\ /y/s/Q
REM step2 clear old bin; build service solution
ECHO [0;1;34m
ECHO -------------------------------------          
ECHO *************************************
ECHO ***   IMES service is building !  *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0m
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo 
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q /nologo /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildService

ECHO [0;1;32m
ECHO -------------------------------------          
ECHO *************************************
ECHO ***  IMES service build succeed!  *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0;1;34m
ECHO -------------------------------------          
ECHO *************************************
ECHO *** Docking service is building ! *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0m
REM step3 clear old bin; build Docking service solution
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESDockingService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo 
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESDockingService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q /nologo /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildDockingService

ECHO [0;1;32m
ECHO -------------------------------------          
ECHO *************************************
ECHO *** Docking service build succeed!*** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0m

REM step3 copy builded bin files to service and  del pdb file
xcopy bin\rules "output\IMESService\rules" /Y/S/Q
xcopy bin\*.bat "output\IMESService" /Y/S/Q
xcopy bin\*.dll "output\IMESService" /Y/S/Q
xcopy bin\IMES.CheckItemModule*.dll  "output\IMESService\PartPolicyModule" /Y/Q
xcopy bin\IMES.CheckItemModule*.pdb  "output\IMESService\PartPolicyModule" /Y
xcopy bin\*.exe "output\IMESService" /Y/S/Q
xcopy bin\*.config "output\IMESService" /Y/S/Q
xcopy bin\*.pdb    "output\IMESService" /y/s

del bin\*.pdb /s/f/q
del output\IMESService\uninstallIMESMaintainService.bat /f/q
del output\IMESService\installIMESMaintainService.bat /f/q
xcopy bin\IMES.DataModel.dll  WEB\Bin\ /Y/Q
xcopy bin\IMES.DataModelEx.dll  WEB\Bin\ /Y/Q
xcopy bin\IMES.Infrastructure.Repository.Metas.dll  WEB\Bin\ /Y/Q
xcopy bin\IMES.Infrastructure.FisException.dll  WEB\Bin\ /Y/Q
xcopy bin\IMES.Station.Interface.dll  WEB\Bin\ /Y/Q
xcopy bin\IMES.Docking.Interface.dll  WEB\Bin\ /Y/Q
REM step3.5 build RemoteControl
ECHO [0;1;32m
ECHO -------------------------------------          
ECHO *************************************
ECHO ***   Copy Service dll succeed!   *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0m

REM step4 clear old bin; build Maintainservice solution
ECHO [0;1;34m
ECHO ------------------------------------------          
ECHO ******************************************
ECHO *** IMESMaintain service is building ! *** 
ECHO ******************************************    
ECHO ------------------------------------------
ECHO [0m

%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo 
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q /nologo /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildMaintainService

ECHO [0;1;32m
ECHO ---------------------------------------------          
ECHO *********************************************
ECHO ***  IMESMaintain service build succeed!  *** 
ECHO *********************************************    
ECHO ---------------------------------------------
ECHO [0m

REM step5 copy builded bin files to Maintainservice and  del pdb file
xcopy bin\uninstallIMESMaintainService.bat "output\IMESMaintainService" /y/s/Q
xcopy bin\installIMESMaintainService.bat "output\IMESMaintainService" /y/s/Q
xcopy bin\*.dll "output\IMESMaintainService" /y/s/Q
xcopy bin\*.exe "output\IMESMaintainService" /y/s/Q
xcopy bin\*.config "output\IMESMaintainService" /y/s/Q
xcopy bin\*.pdb    "output\IMESMaintainService" /y/s
xcopy bin\IMES.Maintain.Interface.dll  WEB\Bin\ /Y/Q
xcopy bin\IMES.Maintain.InterfaceEx.dll  WEB\Bin\ /Y/Q
del bin\*.pdb /s/f/q

ECHO [0;1;32m
ECHO -------------------------------------          
ECHO *************************************
ECHO ***  Copy Maintain dll succeed!   *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0m

REM step6 build IMESWeb
ECHO [0;1;34m
ECHO -----------------------------          
ECHO *****************************
ECHO *** IMESWeb is building ! *** 
ECHO *****************************    
ECHO -----------------------------
ECHO [0m
%FrameworkDIR%\v2.0.50727\aspnet_compiler -v "WEB" -p "WEB" -c -errorstack -nologo > IMESWEBBuild.log
if %errorlevel% neq 0 goto ErrBuildUI

ECHO [0;1;32m
ECHO -----------------------------------          
ECHO ***********************************
ECHO ***    build IMESWeb succeed!   ***
ECHO ***********************************
ECHO -----------------------------------
ECHO [0m
REM step7 Copy IMESWeb file
xcopy WEB "output\IMESWeb" /y/s/Q
del WEB\Bin\IMES.DataModel.dll /f/q
del WEB\Bin\IMES.Infrastructure.FisException.dll /f/q
del WEB\Bin\IMES.Station.Interface.dll /f/q
del WEB\Bin\IMES.Maintain.Interface.dll /f/q
del WEB\Bin\IMES.Maintain.InterfaceEx.dll /f/q
del WEB\Bin\IMES.Docking.Interface.dll /f/q
del WEB\Bin\IMES.DataModelEx.dll /f/q
del WEB\Bin\IMES.Infrastructure.Repository.Metas.dll /f/q

ECHO [0;1;32m
ECHO -----------------------------------          
ECHO ***********************************
ECHO *** Copy IMESWeb files succeed! ***
ECHO ***********************************
ECHO -----------------------------------
  
GOTO ClearAll

:ErrBuildService
rd output /s/q 
ECHO [0;1;31m
ECHO ---------------------------------
ECHO !!! IMES service build failed !!!
ECHO ---------------------------------
ECHO [0m
pause
GOTO ClearAll

:ErrBuildDockingService
rd output /s/q
ECHO [0;1;31m 
ECHO ------------------------------------
ECHO !!! Docking service build failed !!!
ECHO ------------------------------------
ECHO [0m
pause
GOTO ClearAll

:ErrBuildMaintainService
rd output /s/q
ECHO [0;1;31m
ECHO ---------------------------------
ECHO !!! IMES Maintain service build failed !!!
ECHO ---------------------------------
ECHO [0m
pause
GOTO ClearAll

:ErrBuildUI
ECHO [0;1;31m
ECHO ----------------------------
ECHO !!! IMESWeb build failed !!!
ECHO ----------------------------
ECHO [0m
pause
del WEB\Bin\IMES.DataModel.dll /f/q
del WEB\Bin\IMES.Infrastructure.FisException.dll /f/q
del WEB\Bin\IMES.Station.Interface.dll /f/q
del WEB\Bin\IMES.Maintain.Interface.dll /f/q
del WEB\Bin\IMES.Maintain.InterfaceEx.dll /f/q
del WEB\Bin\IMES.Docking.Interface.dll /f/q
del WEB\Bin\IMES.DataModelEx.dll /f/q
del WEB\Bin\IMES.Infrastructure.Repository.Metas.dll /f/q
GOTO ClearAll

:ClearAll
ECHO [0m
del bin\*.dll /f/q
del bin\*.exe /f/q
del bin\*.config /f/q

%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESDockingService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo


GOTO END

:END
pause
@ECHO ON


@ECHO OFF
REM Prepare directory
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework
CALL BuildIMESKernel.bat

REM step1 Delete output;create new output, copy workflow+bat+exe to output service folder
rd output /s/q
mkdir "output\IMESRCTOWeb"
mkdir "output\IMESRCTOService"
mkdir "output\IMESRCTOService\rules"
mkdir "output\IMESRCTOService\PartPolicyModule"
REM MA# mkdir "output\IMESMaintainService"
mkdir "output\SQL"

copy Service\IMES.Station.Workflow\*.xoml  output\IMESRCTOService\ /y
copy Service\IMES.Station.Workflow\*.rules  output\IMESRCTOService\ /y
xcopy SQL output\SQL\ /y/s
REM step2 clear old bin; build service solution
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildService


ECHO -------------------------------------          
ECHO *************************************
echo *** IMES service build succeeded! *** 
ECHO *************************************    
ECHO -------------------------------------

REM step3 copy builded bin files to service and  del pdb file

xcopy bin\rules "output\IMESRCTOService\rules" /y/s
xcopy bin\*.bat "output\IMESRCTOService" /y/s
xcopy bin\*.dll "output\IMESRCTOService" /y/s
xcopy bin\IMES.CheckItemModule*.dll  "output\IMESRCTOService\PartPolicyModule" /Y
xcopy bin\*.pdb    "output\IMESMaintainService" /y/s
xcopy bin\*.exe "output\IMESRCTOService" /y/s
xcopy bin\*.config "output\IMESRCTOService" /y/s
REM MA# del output\IMESRCTOService\uninstallIMESMaintainService.bat /s/f/q
REM MA# del output\IMESRCTOService\installIMESMaintainService.bat /s/f/q
copy bin\IMES.DataModel.dll  WEB\Bin\ /Y
copy bin\IMES.Infrastructure.Repository.Metas.dll  WEB\Bin\ /Y
copy bin\IMES.Infrastructure.FisException.dll  WEB\Bin\ /Y
copy bin\IMES.Station.Interface.dll  WEB\Bin\ /Y

del bin\*.pdb /s/f/q
REM step3.5 build RemoteControl

REM step4 clear old bin; build Maintainservice solution
REM MA# %FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
REM MA# %FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESMaintainServiceBuild.log;Verbosity=m;Encoding=UTF-8
REM MA# if %errorlevel% neq 0 goto ErrBuildMaintainService

REM MA# ECHO ---------------------------------------------          
REM MA# ECHO *********************************************
REM MA# echo *** IMESMaintain service build succeeded! *** 
REM MA# ECHO *********************************************    
REM MA# ECHO ---------------------------------------------

REM step5 copy builded bin files to Maintainservice and  del pdb file
REM MA# del bin\*.pdb /s/f/q
REM MA# xcopy bin\uninstallIMESMaintainService.bat "output\IMESMaintainService" /y/s
REM MA# xcopy bin\installIMESMaintainService.bat "output\IMESMaintainService" /y/s
REM MA# xcopy bin\*.dll "output\IMESMaintainService" /y/s
REM MA# xcopy bin\*.exe "output\IMESMaintainService" /y/s
REM MA# xcopy bin\*.config "output\IMESMaintainService" /y/s
REM MA# copy bin\IMES.Maintain.Interface.dll  WEB\Bin\/Y

REM step6 build IMESWeb
xcopy WEB "output\IMESRCTOWeb" /y/s
del WEB\Bin\IMES.DataModel.dll
del WEB\Bin\IMES.Infrastructure.FisException.dll
del WEB\Bin\IMES.Station.Interface.dll
REM MA# del WEB\Bin\IMES.Maintain.Interface.dll

ECHO -----------------------------------          
ECHO ***********************************
ECHO *** IMES IMESWeb Copy succeeded! ***
ECHO ***********************************
ECHO -----------------------------------  
GOTO ClearAll

:ErrBuildService
rd output /s/q 
ECHO ---------------------------------
ECHO !!! IMES service build failed !!!
ECHO ---------------------------------
GOTO ClearAll

:ErrBuildMaintainService
rd output /s/q
ECHO ---------------------------------
ECHO !!! IMES Maintain service build failed !!!
ECHO ---------------------------------
pause
GOTO ClearAll

:ErrBuildUI
del WEB\Bin\IMES.DataModel.dll
del WEB\Bin\IMES.Infrastructure.FisException.dll
del WEB\Bin\IMES.Station.Interface.dll
REM MA# del WEB\Bin\IMES.Maintain.Interface.dll
ECHO --------------------------------
ECHO !!! IMES IMESWeb build failed !!!
ECHO --------------------------------
GOTO ClearAll

:ClearAll
del bin\*.dll
del bin\*.exe 
del bin\*.config

%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q 
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q
rd Kernel\IMES.Infrastructure.DataModel\obj /s/q
rd Kernel\IMES.FisObject\obj /s/q
rd Kernel\IMES.FisObject.PartStrategy.COAStrategy\obj /s/q
rd Kernel\IMES.FisObject.PartStrategy.CommonStrategy\obj /s/q
rd Kernel\IMES.FisObject.PartStrategy.KPStrategy\obj /s/q
rd Kernel\IMES.FisObject.PartStrategy.MBStrategy\obj /s/q
rd Kernel\IMES.FisObject.PartStrategy.VBStrategy\obj /s/q
rd Kernel\IMES.Infrastructure\obj /s/q
rd Kernel\IMES.Infrastructure.FisException\obj /s/q
rd Kernel\IMES.Infrastructure.Repository\obj /s/q
rd Kernel\IMES.Infrastructure.Session\obj /s/q
rd Kernel\IMES.Infrastructure.Utility\obj /s/q
rd Kernel\IMES.Infrastructure.WorkflowRuntime\obj /s/q
rd Kernel\IMES.Interface.RouteManagement\obj /s/q
rd Kernel\IMES.Interface.RouteManagement\bin /s/q
rd Service\IMES.Activity\obj /s/q
rd Service\IMES.ConsoleHost\obj /s/q
rd Service\IMES.Infrastructure.Extend\obj /s/q
rd Service\IMES.Service.SA\obj /s/q
rd Service\IMES.Service.FA\obj /s/q
rd Service\IMES.Service.PAK\obj /s/q
rd Service\IMES.Station.Implementation\obj /s/q
rd Service\IMES.Station.Interface\obj /s/q
REM MA# rd Service\IMES.Maintain.ConsoleHost\obj /s/q
REM MA# rd Service\IMES.Maintain.Implementation\obj /s/q
REM MA# rd Service\IMES.Maintain.Interface\obj /s/q
REM MA# rd Service\IMES.Maintain.Service\obj /s/q

rem add by chensong
rd kernel\IMES.CheckItemModule.Utility\obj /s/q
rd kernel\IMES.Infrastructure.Repository.Metas\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.ATSN1.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.ATSN3.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.Battery.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.BTCB.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.BTDL.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.C5.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.CN.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.COA_OOA.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.CPU.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.CT.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.CTNonBattery\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.DDR.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.DockingMB.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.DockingPN.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.DockingSN.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.FirstPizzaID.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.HDD.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.HomeCard.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.Interface\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.Inverter.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.KB.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.KP.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.LCM.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.MAC.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.MB.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.NylonCaseXX.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.ODD.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.OOA.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PizzaPart.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PL_BoxID.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PosterCard.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PosterCardXX.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PP.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PrivacyFilter.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PXCT.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.PXNCT.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.RomeoBattery.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.RoyaltyPaper.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.SecondPizzaID.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.Thermal.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.TouchScreen.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.TPCB2.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.TPCB.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.TPDL.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.V2.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.VGA.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.VirtualTPCB.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.WarrantyCard.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.WL.Filter\obj /s/q
rd kernel\IMES.CheckItemModule\IMES.CheckItemModule.WWAN.Filter\obj /s/q
del web\Bin\IMES.Infrastructure.Repository.Metas.dll


GOTO END

:END
pause
@ECHO ON



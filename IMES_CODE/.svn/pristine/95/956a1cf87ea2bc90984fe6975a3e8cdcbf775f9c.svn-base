@ECHO OFF
REM Prepare directory
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework
CALL BuildIMESKernel.bat

REM step1 Delete output;create new output, copy workflow+bat+exe to output service folder
rd output /s/q
mkdir "output\IMESDockingWeb"
mkdir "output\IMESDockingService"
mkdir "output\IMESDockingService\rules"
mkdir "output\IMESDockingService\PartPolicyModule"
mkdir "output\SQL"
mkdir "output\IMESDockingMaintainService"

copy Service\IMES.Docking.Workflow\*.xoml  output\IMESDockingService\ /y
copy Service\IMES.Docking.Workflow\*.rules  output\IMESDockingService\ /y
copy Service\IMES.Station.Workflow\ESOPandAoiKbTest.xoml  output\IMESDockingService\ /y
copy Service\IMES.Station.Workflow\ESOPandAoiKbTest.rules  output\IMESDockingService\ /y
copy Service\IMES.Station.Workflow\PackingList.xoml  output\IMESDockingService\ /y
copy Service\IMES.Station.Workflow\PackingList.rules  output\IMESDockingService\ /y
copy Service\IMES.Station.Workflow\PackingListForPL.xoml  output\IMESDockingService\ /y
xcopy SQL output\SQL\ /y/s
REM step2 clear old bin; build service solution
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESDockingService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESDockingService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildService


ECHO -------------------------------------          
ECHO *************************************
echo *** IMES service build succeeded! *** 
ECHO *************************************    
ECHO -------------------------------------

REM step3 copy builded bin files to service and  del pdb file
del bin\*.pdb /s/f/q
xcopy bin\rules "output\IMESDockingService\rules" /y/s
xcopy bin\*.bat "output\IMESDockingService" /y/s
xcopy bin\*.dll "output\IMESDockingService" /y/s
xcopy bin\IMES.CheckItemModule*.dll  "output\IMESDockingService\PartPolicyModule" /Y
xcopy bin\*.exe "output\IMESDockingService" /y/s
xcopy bin\*.config "output\IMESDockingService" /y/s
del output\IMESDockingService\uninstallIMESMaintainService.bat /s/f/q
del output\IMESDockingService\installIMESMaintainService.bat /s/f/q
del output\IMESDockingService\uninstallIMESDockingMaintainService.bat /s/f/q
del output\IMESDockingService\installIMESDockingMaintainService.bat /s/f/q
copy bin\IMES.DataModel.dll  WEB\Bin\ /Y
copy bin\IMES.Infrastructure.Repository.Metas.dll  WEB\Bin\ /Y
copy bin\IMES.Infrastructure.FisException.dll  WEB\Bin\ /Y
copy bin\IMES.Station.Interface.dll  WEB\Bin\ /Y
copy bin\IMES.Docking.Interface.dll  WEB\Bin\ /Y
copy bin\IMES.DataModelEx.dll  WEB\Bin\ /Y
copy bin\IMES.FisObjectEx.dll  WEB\Bin\ /Y

REM step4 clear old bin; build Maintainservice solution
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
%FrameworkDIR%\v3.5\MSBuild.exe Service\IMESMaintainService.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESMaintainServiceBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildMaintainService

ECHO ---------------------------------------------          
ECHO *********************************************
echo *** IMESMaintain service build succeeded! *** 
ECHO *********************************************    
ECHO ---------------------------------------------

REM step5 copy builded bin files to Maintainservice and  del pdb file
del bin\*.pdb /s/f/q
xcopy bin\uninstallIMESDockingMaintainService.bat "output\IMESDockingMaintainService" /y/s
xcopy bin\installIMESDockingMaintainService.bat "output\IMESDockingMaintainService" /y/s
xcopy bin\*.dll "output\IMESDockingMaintainService" /y/s
xcopy bin\*.exe "output\IMESDockingMaintainService" /y/s
xcopy bin\*.config "output\IMESDockingMaintainService" /y/s
copy bin\IMES.Maintain.Interface.dll  WEB\Bin\/Y
copy bin\IMES.Maintain.InterfaceEx.dll  WEB\Bin\ /Y

REM step6 build IMESDockingWeb
xcopy WEB "output\IMESDockingWeb" /y/s
del WEB\Bin\IMES.DataModel.dll
del WEB\Bin\IMES.Infrastructure.FisException.dll
del WEB\Bin\IMES.Station.Interface.dll
del WEB\Bin\IMES.Docking.Interface.dll
del WEB\Bin\IMES.Maintain.Interface.dll
del IMES.DataModelEx.dll
del IMES.FisObjectEx.dll
del IMES.Maintain.InterfaceEx.dll


ECHO -----------------------------------          
ECHO ***********************************
ECHO *** IMES IMESDockingWeb Copy succeeded! ***
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
del WEB\Bin\IMES.Maintain.Interface.dll
del WEB\Bin\IMES.Docking.Interface.dll
ECHO --------------------------------
ECHO !!! IMES IMESDockingWeb build failed !!!
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
rd Service\IMES.Docking.Service\obj /s/q
rd Service\IMES.Station.Implementation\obj /s/q
rd Service\IMES.Station.Interface\obj /s/q
rd Service\IMES.Docking.Implementation\obj /s/q
rd Service\IMES.Docking.Interface\obj /s/q
rd Service\IMES.Maintain.ConsoleHost\obj /s/q
rd Service\IMES.Maintain.Implementation\obj /s/q
rd Service\IMES.Maintain.Interface\obj /s/q
rd Service\IMES.Maintain.Service\obj /s/q
rd Service\IMES.DockingMaintain.Service\obj /s/q

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
rd service\IMES.Docking.ConsoleHost\obj /s/q







GOTO END

:END
pause
@ECHO ON



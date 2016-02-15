@ECHO OFF
REM Prepare directory
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM step2 clear old bin; build Kernel solution
%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q  
%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q  /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildKernel


ECHO -------------------------------------          
ECHO *************************************
echo *** IMES Kernel build succeeded! *** 
ECHO *************************************    
ECHO -------------------------------------

REM step3 copy builded bin files to Kernel and  del pdb file
del bin\*.pdb /s/f/q
GOTO END

:ErrBuildKernel
rd output /s/q 
ECHO ---------------------------------
ECHO !!! IMES Kernel build failed !!!
ECHO ---------------------------------
pause
GOTO ClearAll

:ClearAll
del bin\*.dll
del bin\*.exe 
del bin\*.config

%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q 
FOR /R Kernel %%I IN (.) DO IF "%%~nxI"=="obj" rmdir /s /q "%%~fI"

GOTO END

:END
@ECHO ON



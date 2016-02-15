@ECHO OFF
REM Prepare directory
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM step2 clear old bin; build Kernel solution
ECHO [0;1;34m
ECHO -------------------------------------          
ECHO *************************************
ECHO ***   IMES Kernel is building !   *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [1m 
%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q /nologo
%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Build /p:WarningLevel=0;configuration=Release /verbosity:q /nologo /l:FileLogger,Microsoft.Build.Engine;logfile=IMESBuild.log;Verbosity=m;Encoding=UTF-8
if %errorlevel% neq 0 goto ErrBuildKernel

ECHO [0;1;32m
ECHO -------------------------------------          
ECHO *************************************
ECHO *** IMES Kernel build succeeded!  *** 
ECHO *************************************    
ECHO -------------------------------------
ECHO [0m
REM step3 copy builded bin files to Kernel and  del pdb file
del bin\*.pdb /f/q
GOTO END

:ErrBuildKernel
rd output /s/q 
ECHO [0;1;31m
ECHO ---------------------------------
ECHO !!! IMES Kernel build failed !!!
ECHO ---------------------------------
ECHO [0m
pause
GOTO ClearAll

:ClearAll
del bin\*.dll /f/q
del bin\*.exe /f/q
del bin\*.config /f/q

%FrameworkDIR%\v3.5\MSBuild.exe Kernel\IMESKernel.sln /t:Clean /p:WarningLevel=0;configuration=Release /verbosity:q 
FOR /R Kernel %%I IN (.) DO IF "%%~nxI"=="obj" rmdir /s/q "%%~fI"

GOTO END

:END
@ECHO ON



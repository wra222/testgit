@ECHO OFF
REM step1 Delete output;create new output
rd output /s/q
mkdir "..\output"

REM step2 Copy web
xcopy "..\dashboard" "..\output\" /y/s

@ECHO ON



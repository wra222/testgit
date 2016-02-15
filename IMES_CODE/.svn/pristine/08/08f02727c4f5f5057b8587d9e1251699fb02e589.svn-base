
set server=10.99.183.65
set user=imes
set pwd=imes
set db=HPIMES

for %%a in (".\sql\*.sql") do (
echo. >> .\output.txt
echo %%a >> .\output.txt
Sqlcmd -S %server% -U %user% -P %pwd% -d %db% -i %%a >> .\output.txt
)

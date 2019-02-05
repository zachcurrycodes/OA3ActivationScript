@ECHO OFF
Set /p YN=Is this an Gigabyte mainboard?(Y/N)
If /i "%YN%"=="Y" call run_gb1.cmd
If /i "%YN%"=="N" call run_else.cmd
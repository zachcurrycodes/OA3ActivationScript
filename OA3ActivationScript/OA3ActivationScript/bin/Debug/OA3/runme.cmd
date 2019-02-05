@ECHO OFF
Set /p YN=Is this an ASUS mainboard?(Y/N)
If /i "%YN%"=="Y" call run_ASUS.cmd
If /i "%YN%"=="N" call run_giga.cmd
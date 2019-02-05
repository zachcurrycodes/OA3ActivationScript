@ECHO OFF
cscript slmgr.vbs /ipk nf6hc-qh89w-f8wyv-wwxv4-wfg6p
del output\*.* /q
ECHO Obtaining a key
Oa3tool.exe /assemble /configfile=oa3.cfg
ECHO injecting key
WinOA30_64.exe /f output\oa3.bin
ECHO Validating Key Injection
Oa3tool.exe /validate
ECHO Done!
ECHO Please reboot and run Final.cmd
Pause
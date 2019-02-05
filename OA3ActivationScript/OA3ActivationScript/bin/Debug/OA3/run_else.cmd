@ECHO OFF
cscript slmgr.vbs /ipk nf6hc-qh89w-f8wyv-wwxv4-wfg6p
del output\*.* /q
ECHO Obtaining a key
Oa3tool.exe /assemble /configfile=oa3.cfg
ECHO injecting key
afuwinx64 /aoutput\oa3.bin
ECHO Validating Key Injection
Oa3tool.exe /validate
ECHO Reporting
Oa3tool.exe /report /configfile=oa3.cfg
ECHO Done!
Pause

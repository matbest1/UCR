echo off
echo 	888     888  .d8888b.   .d8888b.  
echo 	888     888 d88P  Y88b d88P  Y88b 
echo 	888     888 888    888 Y88b.      
echo 	888     888 888         ""Y888b.   
echo 	888     888 888            ""Y88b. 
echo 	888     888 888    888       ""888 
echo 	Y88b. .d88P Y88b  d88P Y88b  d88P 
echo         "Y88888P"   "Y8888P"   "Y8888P"
echo
color 0B   
echo UCS-Restart-Op v0.1 by Aidid   
echo.   
echo Your UCS is being restarted, Please wait. . .   
echo Killing process ucs.exe. . .
timeout /t 300
taskkill /f /im ucs.exe   
cls   
echo Success!   
echo.   
echo Your UCS is now starting. . .   
timeout /t 15
start ucs.exe   
exit 

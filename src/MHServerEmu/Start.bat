@echo off
set APACHE_SERVER_ROOT=%cd%\Apache
start %APACHE_SERVER_ROOT%\bin\httpd.exe
start "MHServerEmu" MHServerEmu.exe
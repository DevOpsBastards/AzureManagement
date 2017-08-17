
SET BASEPATH=D:\OneDrive\SourceCode\Play\
SET PATHEXE=%BASEPATH%\AzureManagement\AzureManagement\bin\Debug\AzureManagement.exe
SET APPTYPE="Web"
SET ENVTYPE="Test"
SET APPNAME="HUDSONTOWN"

SET AZURERSGCMD="%PATHEXE%" --Action "RSG" --apptype %APPTYPE% --envtype %ENVTYPE% --appname %APPNAME% 
Echo "Processing ResourceGrp: %AZURERSGCMD%" 
%AZURERSGCMD%

SET AZUREAPPPLANCMD="%PATHEXE%" --Action "APPPLAN" --apptype %APPTYPE% --envtype %ENVTYPE% --appname %APPNAME%
Echo "Processing App Plan: %AZUREAPPPLANCMD%" 
%AZUREAPPPLANCMD%

SET AZUREWEBAPPCMD="%PATHEXE%" --Action "WEBAPP" --apptype %APPTYPE% --envtype %ENVTYPE% --appname %APPNAME%
Echo "Processing WebApp: %AZUREWEBAPPCMD%" 
%AZUREWEBAPPCMD%


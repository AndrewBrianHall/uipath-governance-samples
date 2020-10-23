@echo off

set _EXE=UiPath.Studio.exe
FOR /F %%x IN ('tasklist /NH /FI "IMAGENAME eq %_EXE%"') DO IF %%x == %_EXE% goto PROCESSRUNNING
goto DISABLEPOLICY

:PROCESSRUNNING
echo Close all open Studio instances before disabling policy
goto :exit

:DISABLEPOLICY
reg query HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource >nul 
if %errorlevel% == 0 (
    reg  delete  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /f >nul
)

if exist "%appdata%\UiPath\uipath.policies.config.cache" (
    del "%appdata%\UiPath\uipath.policies.config.cache"
)

if exist "%appdata%\NuGet\Nuget.Config.original" (
    del "%appdata%\NuGet\Nuget.Config"
    ren "%appdata%\NuGet\Nuget.Config.original" "Nuget.Config"
)

FOR /D %%G in ("%localappdata%\UiPath\app-*") DO set COMMUNITY_INSTALL_FOLDER=%%G
if exist "%COMMUNITY_INSTALL_FOLDER%\Rules\" (
    SET CUSTOM_RULE_FILE="%COMMUNITY_INSTALL_FOLDER%\Rules"
    goto copyrules
) 
if exist "%programfiles(x86)%\UiPath\Studio\Rules\" (
    SET CUSTOM_RULE_FILE="%programfiles(x86)%\UiPath\Studio\Rules"
    goto copyrules
) else (
    echo "No Studio installation found"
    goto OkExit
)

:copyrules
set SAMPLE_BINARY_DIR=%CD%\..\sample-rule-binaries
for /f %%f in ('dir /b "%SAMPLE_BINARY_DIR%"') DO if exist %CUSTOM_RULE_FILE%\%%f ( del %CUSTOM_RULE_FILE%\%%f )
echo Custom rules deleted successfully

SET RULES_CONFIG_FOLDER=%localappdata%\UiPath\Rules
if exist "%RULES_CONFIG_FOLDER%\RuleConfig.json" (
    del "%RULES_CONFIG_FOLDER%\RuleConfig.json" 
    @REM ren "%RULES_CONFIG_FOLDER%\RuleConfig.json.original" "RuleConfig.json"
)

goto OkExit


:OkExit
echo Policy successfully disabled

:exit

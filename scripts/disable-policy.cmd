@echo off

reg query HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource >nul 
if %errorlevel% == 0 (
    reg  delete  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /f >nul
)

if exist "%appdata%\UiPath\uipath.policies.config.cache" (
    del "%appdata%\UiPath\uipath.policies.config.cache"
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
    goto commonexit
)

:copyrules
set SAMPLE_BINARY_DIR=%CD%\..\sample-rule-binaries
for /f %%f in ('dir /b "%SAMPLE_BINARY_DIR%"') DO if exist %CUSTOM_RULE_FILE%\%%f ( del %CUSTOM_RULE_FILE%\%%f )
echo Custom rules deleted successfully
goto commonexit


:commonexit
echo Policy successfully disabled

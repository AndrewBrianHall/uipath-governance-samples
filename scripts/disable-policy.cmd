reg  delete  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /f

if exist "%appdata%\UiPath\uipath.policies.config.cache" (
    del "%appdata%\UiPath\uipath.policies.config.cache"
)

REM echo off
REM look for community folder
FOR /D %%G in ("%localappdata%\UiPath\app-*") DO set COMMUNITY_INSTALL_FOLDER=%%G
if exist "%COMMUNITY_INSTALL_FOLDER%\Rules\" (
    SET RULES_FOLDER=%COMMUNITY_INSTALL_FOLDER%\Rules\
    goto deleterules
) 
if exist "%programfiles(x86)%\UiPath\Studio\Rules\" (
    SET RULES_FOLDER=%programfiles(x86)%\UiPath\Studio\Rules
    goto deleterules
)

:deleterules
set SAMPLE_BINARY_DIR=%CD%\..\sample-rule-binaries
if exist "%RULES_FOLDER%" (
    echo on
    for /f %%f in ('dir /b "%SAMPLE_BINARY_DIR%"') DO if exist "%RULES_FOLDER%\%%f" (del "%RULES_FOLDER%\%%f")
    echo off
    goto commonexit
)

:commonexit

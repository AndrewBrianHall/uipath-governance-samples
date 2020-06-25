@echo off
if /I "%1" == "local" (
    SET POLICY_FILE="%CD%\..\sample-policy\uipath.policies.config"
) else (
    SET POLICY_FILE="https://raw.githubusercontent.com/AndrewBrianHall/uipath-governance-samples/master/sample-policy/uipath.policies.config"
)

FOR /D %%G in ("%localappdata%\UiPath\app-*") DO set COMMUNITY_INSTALL_FOLDER=%%G
if exist "%COMMUNITY_INSTALL_FOLDER%\Rules\" (
    SET RULES_FOLDER="%COMMUNITY_INSTALL_FOLDER%\Rules\"
    goto enablepolicy
) 
if exist "%programfiles(x86)%\UiPath\Studio\Rules\" (
    SET RULES_FOLDER="%programfiles(x86)%\UiPath\Studio\Rules"
    goto enablepolicy
) else (
    echo "No Studio installation found"
    goto commonexit
)

:enablepolicy

reg  add  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /t REG_SZ /f /d %POLICY_FILE% >nul

copy "%appdata%\NuGet\Nuget.Config" "%appdata%\NuGet\Nuget.Config.original"

SET RULES_CONFIG_FOLDER=%localappdata%\UiPath\Rules
if exist "%RULES_CONFIG_FOLDER%\RuleConfig.json" (
    copy "%RULES_CONFIG_FOLDER%\RuleConfig.json" "%RULES_CONFIG_FOLDER%\RuleConfig.json.original"
)

set SAMPLE_BINARY_DIR=%CD%\..\sample-rule-binaries
if exist %RULES_FOLDER% (
    for /f %%f in ('dir /b "%SAMPLE_BINARY_DIR%"') DO copy /Y "%SAMPLE_BINARY_DIR%\%%f" %RULES_FOLDER% >nul
    echo Custom rules deployed successfully
)

echo Policy successfully enabled

:commonexit
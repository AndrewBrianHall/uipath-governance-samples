@echo off

set _EXE=UiPath.Studio.exe
FOR /F %%x IN ('tasklist /NH /FI "IMAGENAME eq %_EXE%"') DO IF %%x == %_EXE% goto PROCESSRUNNING
goto ENABLEPOLICY

:PROCESSRUNNING
echo Close all open instances of Studio to enable
goto :exit

:ENABLEPOLICY
SET _LOCAL_FILE=0
IF /I "%1" == "local" SET _LOCAL_FILE=1
IF /I "%2" == "local" SET _LOCAL_FILE=1
IF %_LOCAL_FILE% EQU 1 (
    CALL make-config.cmd
    SET POLICY_FILE="%CD%\..\sample-policy\uipath.policies.config"
) else (
    SET POLICY_FILE="https://raw.githubusercontent.com/AndrewBrianHall/uipath-governance-samples/master/sample-policy/uipath.policies.config"
)

reg  add  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /t REG_SZ /f /d %POLICY_FILE% >nul

FOR /D %%G in ("%localappdata%\UiPath\app-*") DO set COMMUNITY_INSTALL_FOLDER=%%G
SET RULES_FOLDER=
if exist "%COMMUNITY_INSTALL_FOLDER%\Rules\" (
    SET RULES_FOLDER="%COMMUNITY_INSTALL_FOLDER%\Rules\"
) 
if exist "%programfiles(x86)%\UiPath\Studio\Rules\" (
    SET RULES_FOLDER="%programfiles(x86)%\UiPath\Studio\Rules"
) 


copy "%appdata%\NuGet\Nuget.Config" "%appdata%\NuGet\Nuget.Config.original"

SET RULES_CONFIG_FOLDER=%localappdata%\UiPath\Rules
if exist "%RULES_CONFIG_FOLDER%\RuleConfig.json" (
    copy "%RULES_CONFIG_FOLDER%\RuleConfig.json" "%RULES_CONFIG_FOLDER%\RuleConfig.json.original"
)

SET _DEPLOY_RULES=1
IF exist %RULES_FOLDER% SET _DEPLOY_RULES=1
IF /I "%1" == "norules" SET _DEPLOY_RULES=0
IF /I "%2" == "norules" SET _DEPLOY_RULES=0
SET SAMPLE_BINARY_DIR=%CD%\..\sample-rules\binaries
IF %_DEPLOY_RULES% EQU 1 (
    
    for /f %%f in ('dir /b "%SAMPLE_BINARY_DIR%"') DO copy /Y "%SAMPLE_BINARY_DIR%\%%f" %RULES_FOLDER% >nul
    echo Custom rules deployed successfully
)

echo Policy successfully enabled

:exit
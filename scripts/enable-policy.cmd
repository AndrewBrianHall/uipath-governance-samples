echo off
if /I "%1" == "local" (
    SET POLICY_FILE="%CD%\..\sample-policy\uipath.policies.config"
) else (
    SET POLICY_FILE="https://raw.githubusercontent.com/AndrewBrianHall/uipath-governance-samples/master/sample-policy/uipath.policies.config"
)

echo on

reg  add  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /t REG_SZ /f /d %POLICY_FILE%
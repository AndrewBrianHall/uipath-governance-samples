reg  delete  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /f

if exist "%appdata%\UiPath\uipath.policies.config.cache" (
    del "%appdata%\UiPath\uipath.policies.config.cache"
)
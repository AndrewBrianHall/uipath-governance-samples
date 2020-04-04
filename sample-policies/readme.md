
## Enable the sample policy on machine
- Open a Windows Command Prompt as an Administrator
- Copy and paste the following command and run it
```console
reg  add  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /t REG_SZ /f /d "https://raw.githubusercontent.com/AndrewBrianHall/uipath-governance-samples/master/sample-policies/uipath.policies.config"
```

## Disable the sample policy on the machine
- Open a Windows Command Prompt as an Administrator
- Copy and paste the following command and run it
```console
reg  delete  HKEY_CURRENT_USER\Software\UiPath /v GovernanceSource /f
```

## Enable the sample policy on machine
- Open a Windows Command Prompt
- Run the following command

```console
scripts\enable-policy.cmd [local] [norules]
```

### Options
- local - This uses the file on the local machine from this directory. This is useful if you want to test changes to the policy
- norules - Skips deploying the custom governance rules into the Studio installation directory.

## Disable the sample policy on the machine
- Open a Windows Command Prompt
- Run the following command
```console
scripts\disable-policy.cmd
```

## See it Action
Watch the official [Governance made easy for the StudioX user](https://www.youtube.com/watch?v=A1ElmiD_YIU) video

### Custom Rule Source
The source code for the custom rules is available in the [uipath-sample-wfa-rules repository](https://github.com/AndrewBrianHall/uipath-sample-wfa-rules/)

echo off
REM look for community folder
FOR /D %%G in ("%localappdata%\UiPath\app-*") DO set COMMUNITY_INSTALL_FOLDER=%%G
if exist "%COMMUNITY_INSTALL_FOLDER%\Rules\" (
    SET RULES_FOLDER="%COMMUNITY_INSTALL_FOLDER%\Rules\"
    goto copyrules
) 
if exist "%programfiles(x86)%\UiPath\Studio\Rules\" (
    SET RULES_FOLDER="%programfiles(x86)%\UiPath\Studio\Rules"
    goto copyrules
) else (
    echo "No Studio installation found"
    goto commonexit
)

:copyrules
if exist %RULES_FOLDER% (
    echo on
    set SAMPLE_BINARY_DIR=%CD%\..\sample-rule-binaries
    for /f %%f in ('dir /b %SAMPLE_BINARY_DIR%') DO copy /Y %SAMPLE_BINARY_DIR%\%%f %RULES_FOLDER%
    echo off
    goto commonexit
)

:commonexit

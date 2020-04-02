  
set PROJECT_NAME=CustomPropertyRule
set PROJECT_DIR=..\%PROJECT_NAME%
set PROJECT_FILE=%PROJECT_DIR%\%PROJECT_NAME%.csproj
set PROJECT_OUTPUT_DIR=%PROJECT_DIR%\bin
set NUGET_OUTPUT_DIR=%PROJECT_OUTPUT_DIR%\bin\NuGet
set LOCAL_NUGET_CACHE=%userprofile%\.nuget\packages
set LOCAL_NUGET_FEED=C:\NuGet
set OUTPUT_BINARY=%PROJECT_NAME%.dll
set NUGET_CACHE_BINARY_LOCATION=%LOCAL_NUGET_CACHE%\samplegovernancerules\1.1.0\lib\net461

msbuild %PROJECT_FILE% -p:Configuration=Debug
copy /Y "%PROJECT_OUTPUT_DIR%\Debug\%OUTPUT_BINARY%" "%NUGET_CACHE_BINARY_LOCATION%"
nuget pack %PROJECT_FILE% -OutputDirectory %NUGET_OUTPUT_DIR%  -Properties Configuration=Debug
for /f "usebackq delims=|" %%f in (`dir /b %NUGET_OUTPUT_DIR%`) do copy /Y %NUGET_OUTPUT_DIR%\%%f %LOCAL_NUGET_FEED%

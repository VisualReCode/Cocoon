FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 as build

# Copy files
RUN md c:\build
WORKDIR c:/build
COPY . c:/build

# Build the application
RUN nuget restore samples\wingtip-toys
RUN ["C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\BuildTools\\MSBuild\\Current\\Bin\\msbuild.exe","samples/wingtip-toys/wingtiptoys.sln", "/t:Rebuild" ,"/p:outdir=c:\\build\\outproject\\", "/p:Configuration=Release"]

## Switch to a runtime image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019 as runtime
WORKDIR C:/inetpub/wwwroot

## Install localdb
RUN Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
RUN choco install sqllocaldb -y
RUN sqllocaldb s MSSQLLocalDB
RUN mkdir App_Data

COPY --from=build c:/build/outproject/_publishedwebsites/wingtiptoys . 
COPY --from=build c:/build/samples/wingtip-toys/iis.ps1 .     

RUN "Powershell ./iis.ps1"

## Change the acls to allow localdb creation.
RUN ["icacls","C:\\inetpub\\wwwroot", "/grant", "everyone:(OI)(CI)F","/T"]


FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

# Copy files
RUN md c:\build
WORKDIR c:/build
COPY . c:/build

# Build the application
RUN dotnet publish -c release samples/MvcCocoon/src/MvcCocoon/MvcCocoon.csproj -o c:/build/app -r win10-x64

## Switch to a runtime image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019 as runtime
WORKDIR C:/inetpub/wwwroot

COPY --from=build c:/build/app/ . 
RUN Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

#RUN choco install dotnet-5.0-sdk -y
RUN choco install dotnet-5.0-windowshosting -y
#RUN choco install urlrewrite -y

## Change the acls to allow localdb creation.
#RUN ["icacls","C:\\inetpub\\wwwroot", "/grant", "everyone:(OI)(CI)F","/T"]


# mqpi [![Build Status](https://travis-ci.org/Isantipov/mqpi.svg?branch=master)](https://travis-ci.org/Isantipov/mqpi)
simple http mock server: developed on .net core, runs on linux (see [.net core docs](https://www.microsoft.com/net/core/preview#linuxubuntu) for list of supported OS versions). No runtime installation required (self-contained deployement)

mqpi is distributed under MIT license

## How to
### Develop
1. Install .net core SDK 2.0.0-preview2-006497 or higher - see [install guide](https://www.microsoft.com/net/core/preview#windowscmd) 
2. Install Visual Studio update 3 or higher
3. Build solution from command-line: `dotnet build`
4. Build solution in Visual Studion : as usual (`ctrl+shift+b`)
5. Debug: Visual Studio can run mqpi both on IIS express and self-hosted - use mqpi->Properties->Debug->Launch dropdown)

*same instructions apply for any other OS supporting .netcore 2.0 preview2 however only command-line building is usually available.

### Publish and Deploy
ATM only linux version of publish scripts is available.

The easiest way to run publish/deploy on Windows is to use WSL. See [this guide](https://msdn.microsoft.com/en-us/commandline/wsl/install_guide) to install and configure WSL.

Deployment uses ansible. Follow [this guide](https://www.jeffgeerling.com/blog/2017/using-ansible-through-windows-10s-subsystem-linux) to install ansible on WSL.

Alternatively you can just use a linux box with .net core 2 preview 2 installed

1. Publish for centOs: run `$ ./publishCentOs.sh`. Deployment artifact will be stored at mqpi/bin/Release/netcoreapp2.0/centos.7-x64/publish.zip 
2. Deploy on CentOs: run `$ ./deploy.sh` (target machine is specified in `deploy/hosts file`)

*publish and deploy for other OSes can be easily added by forking the publish and deployment scripts

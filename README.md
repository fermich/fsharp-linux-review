#### Install .NET Core:
```
$ wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
$ sudo dpkg -i packages-microsoft-prod.deb

$ sudo apt-get update
$ sudo apt-get install apt-transport-https
$ sudo apt-get update
$ sudo apt-get install dotnet-sdk-3.1
```

#### Install VSCodium for Linux:
- Download from: https://github.com/VSCodium/vscodium/releases
- Install **Ionide-fsharp** extension

#### Create F# project:
```
$ export DOTNET_CLI_TELEMETRY_OPTOUT=1
$ dotnet new console -lang "F#" -o FSharpReview
```

#### Run codium:
```
$ cd FSharpReview
$ codium .
```

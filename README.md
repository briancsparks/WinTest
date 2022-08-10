
# WinTest

A VS Test project that uses "Appium"-ish to drive HP Easy Start and test if the discovery and
setup is successful.

This is used within the E2EC's team test chamber (RF "fuzzing").

## Installing - TL;DR

In an elevated prompt:

```sh
WinAppDriver
```

In a normal prompt:

```sh
cd source
git clone https://github.com/briancsparks/WinTest.git
cd WinTest

nuget restore WinTest.sln
dotnet build

dotnet vstest ./EasyStartE2eTest/bin/Debug/EasyStartE2eTest.dll
```

Of course, that didn't work, right? Continue on to learn how to install
the tools.


## Installing

1. Prep the PC
2. Install build tools
3. Install runtimes


#### Prep the PC

TL;DR: Enable Developer Mode, and install git

* Windows Start Icon
  * Type: "developer settings"
  * Choose "Install apps from any source..."
  * Scroll down and click all the Apply buttons
* Install git
  * Git-2.37.1-64-bit.exe

#### Install Build Tools

TL;DR: Install dotnet, nuget

* Install dotnet
  * dotnet-sdk-6.0.302-win-x64.exe
  * Add `DOTNET_CLI_TELEMETRY_OPTOUT=1` to the environment.
* Install nuget
  * Put `nuget.exe` into `~/.dotnet/tools`

#### Install Runtimes

TL;DR: Install .NET SDK, WinAppDriver

* Install .NET SDK
  * NDP452-KB2901951-x86-x64-DevPack.exe
* Install WinAppDriver
  * WindowsApplicationDriver-1.2.99-win-x64.exe
  * Put it into the PATH. (C:\Program Files\Windows Application Driver)


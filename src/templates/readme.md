## PeachPie Blazor Templates

This directory contains various `dotnet` project templates to be used for creating PeachPie-Blazor compiled applications.

## Using templates

0. Install peachpie templates via: `dotnet new -i "PathTo\Peachpie.Blazor.Templates.1.0.0.nupkg"` 
1. Create a project based on chosen template e.g.: `dotnet new console -lang PHP`
2. Restore packages for the newly created project: `dotnet restore`
3. Run the project: `dotnet run`

## Prerequisites

- .NET 5.0 SDK
- PowerShell (only for building)

### Building

- Invoke `.\build.ps1` which calls a [build](https://github.com/peachpiecompiler/peachpie-templates/tree/master/build/build.ps1) (1) script
  - 1: Packages all templates into a nuget package (see `/out/Peachpie.Templates.x.y.z.nupkg`)
- After invoking the build script your template should be listed among `dotnet new -all`


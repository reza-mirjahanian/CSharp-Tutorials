

# .NET CLI + CMD Cheat Sheet

---

# 1) What these are

- **`dotnet`**: Command-line tool for creating, building, running, testing, packaging, and publishing .NET apps.

---

# 2) Basic `dotnet` commands

## Check installed SDK/runtime
```cmd
dotnet --info
```
Shows installed SDKs, runtimes, OS info, and environment details.

```cmd
dotnet --version
```
Shows current SDK version.

```cmd
dotnet --list-sdks
```
Lists installed SDK versions.

```cmd
dotnet --list-runtimes
```
Lists installed runtimes.

---

# 3) Create projects

## Create a console app
```cmd
dotnet new console -n MyApp
```
Creates a new console project named `MyApp`.

## Create a web app
```cmd
dotnet new web -n MyWebApp
```
Creates a minimal ASP.NET Core web app.

## Create a web API
```cmd
dotnet new webapi -n MyApi
```
Creates a Web API project.

## Create a class library
```cmd
dotnet new classlib -n MyLibrary
```
Creates a reusable library.

## Create a test project
```cmd
dotnet new xunit -n MyTests
```
Creates an xUnit test project.

## List templates
```cmd
dotnet new list
```
Shows available project templates.

## Get template details
```cmd
dotnet new console --help
```
Shows options for a specific template.

---

# 4) Solution and project management

## Create a solution
```cmd
dotnet new sln -n MySolution
```
Creates a solution file.

## Add project to solution
```cmd
dotnet sln add MyApp\MyApp.csproj
```
Adds a project to the solution.

## Remove project from solution
```cmd
dotnet sln remove MyApp\MyApp.csproj
```
Removes a project from the solution.

## List projects in solution
```cmd
dotnet sln list
```
Shows all projects in the solution.

---

# 5) Build and run

## Build project
```cmd
dotnet build
```
Builds current project or solution.

## Build release version
```cmd
dotnet build -c Release
```
Builds in Release configuration.

## Run app
```cmd
dotnet run
```
Builds and runs the app.

## Run without rebuilding
```cmd
dotnet run --no-build
```
Runs using existing build output.

## Run with arguments
```cmd
dotnet run -- arg1 arg2
```
Passes arguments to your app.

---

# 6) Restore dependencies

## Restore NuGet packages
```cmd
dotnet restore
```
Downloads project dependencies.

## Restore specific project
```cmd
dotnet restore MyApp\MyApp.csproj
```

---

# 7) Clean output

## Clean build files
```cmd
dotnet clean
```
Deletes intermediate build outputs.

## Clean Release build
```cmd
dotnet clean -c Release
```

---

# 8) Test commands

## Run tests
```cmd
dotnet test
```
Builds and runs tests.

## Run tests without rebuilding
```cmd
dotnet test --no-build
```

## Run tests with detailed output
```cmd
dotnet test -v normal
```

## Collect code coverage
```cmd
dotnet test --collect:"XPlat Code Coverage"
```
Runs tests and collects coverage if supported.

---

# 9) Add and remove packages

## Add NuGet package
```cmd
dotnet add package Newtonsoft.Json
```
Adds a package reference to the current project.

## Add specific version
```cmd
dotnet add package Newtonsoft.Json --version 13.0.3
```

## Remove package
```cmd
dotnet remove package Newtonsoft.Json
```

## Add project reference
```cmd
dotnet add reference ..\MyLibrary\MyLibrary.csproj
```
Links another project.

## Remove project reference
```cmd
dotnet remove reference ..\MyLibrary\MyLibrary.csproj
```

## List package references
```cmd
dotnet list package
```

## Check outdated packages
```cmd
dotnet list package --outdated
```

## List vulnerable packages
```cmd
dotnet list package --vulnerable
```

---

# 10) Publish apps

## Publish default
```cmd
dotnet publish
```
Creates files for deployment.

## Publish Release
```cmd
dotnet publish -c Release
```

## Publish to folder
```cmd
dotnet publish -c Release -o .\publish
```

## Publish self-contained
```cmd
dotnet publish -c Release -r win-x64 --self-contained true
```
Includes runtime with the app.

## Publish framework-dependent
```cmd
dotnet publish -c Release -r win-x64 --self-contained false
```
Requires target machine to have runtime installed.

---

# 11) Pack and NuGet

## Create NuGet package
```cmd
dotnet pack
```
Packages library into `.nupkg`.

## Pack Release
```cmd
dotnet pack -c Release
```

## Push package
```cmd
dotnet nuget push .\bin\Release\MyLib.1.0.0.nupkg --source "MyFeed"
```
Pushes package to a NuGet feed.

## Add NuGet source
```cmd
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget
```

## List NuGet sources
```cmd
dotnet nuget list source
```

---

# 12) Tool management

## Install global tool
```cmd
dotnet tool install -g dotnetsay
```

## Update global tool
```cmd
dotnet tool update -g dotnetsay
```

## Uninstall global tool
```cmd
dotnet tool uninstall -g dotnetsay
```

## List global tools
```cmd
dotnet tool list -g
```

## Create local tool manifest
```cmd
dotnet new tool-manifest
```

## Install local tool
```cmd
dotnet tool install dotnetsay
```

## Run local tool
```cmd
dotnet tool run dotnetsay
```

---

# 13) Managing SDK versions

## Create `global.json`
```cmd
dotnet new globaljson
```
Pins SDK version for the folder/project.

## Create with specific version
```cmd
dotnet new globaljson --sdk-version 8.0.100
```

---

# 14) Useful `dotnet` options

## Help
```cmd
dotnet --help
dotnet build --help
```

## Verbosity
```cmd
dotnet build -v minimal
dotnet build -v normal
dotnet build -v detailed
```
Controls log detail.

## No restore
```cmd
dotnet build --no-restore
```
Skips restore step.

## No dependencies
```cmd
dotnet build --no-dependencies
```
Builds current project only.

## Specify framework
```cmd
dotnet build -f net8.0
```

## Specify runtime
```cmd
dotnet publish -r linux-x64
```

---

# 15) Common `dotnet` project workflow

## Create app
```cmd
dotnet new console -n DemoApp
cd DemoApp
```

## Add package
```cmd
dotnet add package Dapper
```

## Run app
```cmd
dotnet run
```

## Build release
```cmd
dotnet build -c Release
```

## Publish
```cmd
dotnet publish -c Release -o .\publish
```

---

# 16) Windows CMD basics

## Show current directory
```cmd
cd
```

## Change directory
```cmd
cd foldername
cd ..
cd \
```

- `cd foldername` → enter folder
- `cd ..` → go up one level
- `cd \` → go to drive root

## Change drive
```cmd
D:
```
Switches to another drive.

## Change drive and folder together
```cmd
cd /d D:\Projects\MyApp
```

---

# 17) List files and folders

## List contents
```cmd
dir
```

## Wide view
```cmd
dir /w
```

## Show hidden files too
```cmd
dir /a
```

## Sort by date
```cmd
dir /o:d
```

## Sort by name
```cmd
dir /o:n
```

---

# 18) Create and remove folders/files

## Create folder
```cmd
mkdir MyFolder
```
or
```cmd
md MyFolder
```

## Remove empty folder
```cmd
rmdir MyFolder
```

## Remove folder and contents
```cmd
rmdir /s /q MyFolder
```

## Create empty file
```cmd
type nul > file.txt
```

## Delete file
```cmd
del file.txt
```

## Delete all `.log` files
```cmd
del *.log
```

---

# 19) Copy, move, rename

## Copy file
```cmd
copy file.txt D:\Backup\
```

## Copy all files
```cmd
copy *.* D:\Backup\
```

## Move file
```cmd
move file.txt D:\Docs\
```

## Rename file
```cmd
ren old.txt new.txt
```

## Rename folder
```cmd
ren OldFolder NewFolder
```

---

# 20) View file content

## Show file content
```cmd
type file.txt
```

## Pause output page by page
```cmd
more file.txt
```

## Combine with pipe
```cmd
type file.txt | more
```

---

# 21) Clear screen and command history

## Clear screen
```cmd
cls
```

## Show command history in current session
```cmd
doskey /history
```

---

# 22) Environment variables

## Show all variables
```cmd
set
```

## Show one variable
```cmd
echo %PATH%
```

## Set variable for current session
```cmd
set NAME=John
```

## Use variable
```cmd
echo %NAME%
```

## Permanently set variable
```cmd
setx NAME John
```
Note: `setx` affects future sessions, not current one.

---

# 23) Redirect output

## Write output to file
```cmd
dir > files.txt
```

## Append output
```cmd
dir >> files.txt
```

## Redirect errors
```cmd
somecommand 2> errors.txt
```

## Redirect all output
```cmd
somecommand > all.txt 2>&1
```

---

# 24) Pipes

## Send output of one command into another
```cmd
dir | more
```

## Search in output
```cmd
dir | find "txt"
```

---

# 25) Search text

## Find text in output
```cmd
find "hello" file.txt
```

## Case-insensitive search
```cmd
find /i "hello" file.txt
```

## Search recursively in files
```cmd
findstr /s /i "hello" *.txt
```

## Search with line numbers
```cmd
findstr /n "hello" file.txt
```

---

# 26) File attributes

## Show attributes
```cmd
attrib file.txt
```

## Make file read-only
```cmd
attrib +r file.txt
```

## Remove read-only
```cmd
attrib -r file.txt
```

## Show hidden/system files
```cmd
attrib
```

---

# 27) Process and system commands

## Show running tasks
```cmd
tasklist
```

## Kill process by image name
```cmd
taskkill /im notepad.exe /f
```

## Kill process by PID
```cmd
taskkill /pid 1234 /f
```

## Show system info
```cmd
systeminfo
```

## Show hostname
```cmd
hostname
```

---

# 28) Network commands

## Ping host
```cmd
ping google.com
```

## Trace route
```cmd
tracert google.com
```

## Show IP configuration
```cmd
ipconfig
```

## Detailed network config
```cmd
ipconfig /all
```

## Flush DNS cache
```cmd
ipconfig /flushdns
```

## Show active connections
```cmd
netstat -ano
```

---

# 29) Permissions and user info

## Current user
```cmd
whoami
```

## Show groups/privileges
```cmd
whoami /all
```

## Run as administrator
Open CMD as Administrator manually.

---

# 30) Batch file basics

## Echo text
```cmd
echo Hello
```

## Turn command echo off
```cmd
@echo off
```

## Comment
```cmd
rem This is a comment
```

## Pause script
```cmd
pause
```

## Exit script
```cmd
exit /b
```

## Use parameters
```cmd
echo %1
echo %2
```

## Conditional
```cmd
if exist file.txt echo Found
```

## Loop through files
```cmd
for %f in (*.txt) do echo %f
```

In batch file use double `%`:
```cmd
for %%f in (*.txt) do echo %%f
```

---

# 31) Useful combined examples

## Create and run .NET app
```cmd
dotnet new console -n HelloApp
cd HelloApp
dotnet run
```

## Create solution with library and app
```cmd
dotnet new sln -n Demo
dotnet new console -n App
dotnet new classlib -n Lib
dotnet sln add App\App.csproj
dotnet sln add Lib\Lib.csproj
dotnet add App\App.csproj reference Lib\Lib.csproj
```

## Build everything
```cmd
dotnet build Demo.sln
```

## Test everything
```cmd
dotnet test Demo.sln
```

---

# 32) Common CMD shortcuts

- `Tab` → autocomplete file/folder names
- `Up/Down Arrow` → command history
- `F7` → command history window
- `Ctrl + C` → cancel current command
- `Alt + Enter` → fullscreen on some systems
- `Shift + Right Click` in folder → open terminal here

---

# 33) Common mistakes

## Running `dotnet run` outside project folder
If no project is found:
```cmd
Couldn't find a project to run
```
Fix:
- `cd` into project folder
- or specify project:
```cmd
dotnet run --project MyApp\MyApp.csproj
```

## Using `setx` and expecting immediate effect
`setx` only affects new terminal windows.

## Forgetting `/d` with `cd` when changing drive
Use:
```cmd
cd /d D:\Work\Project
```

## Deleting folders with `del`
- `del` deletes files
- `rmdir` deletes folders

---

# 34) Quick reference table

## .NET CLI
| Command | Meaning |
|---|---|
| `dotnet --info` | Show SDK/runtime info |
| `dotnet new console -n App` | Create console app |
| `dotnet build` | Build project |
| `dotnet run` | Run project |
| `dotnet test` | Run tests |
| `dotnet restore` | Restore packages |
| `dotnet clean` | Clean outputs |
| `dotnet publish -c Release -o out` | Publish app |
| `dotnet add package X` | Add NuGet package |
| `dotnet add reference Y.csproj` | Add project reference |
| `dotnet list package` | List packages |
| `dotnet pack` | Create NuGet package |
| `dotnet tool list -g` | List global tools |

## CMD
| Command | Meaning |
|---|---|
| `cd` | Show/change directory |
| `dir` | List files |
| `mkdir` / `md` | Create folder |
| `rmdir` | Remove folder |
| `del` | Delete file(s) |
| `copy` | Copy files |
| `move` | Move files |
| `ren` | Rename |
| `type` | Show file content |
| `cls` | Clear screen |
| `set` | Show/set variables |
| `echo` | Print text/variables |
| `findstr` | Search text |
| `tasklist` | Show processes |
| `taskkill` | Kill process |
| `ipconfig` | Network config |
| `ping` | Test connectivity |

---

# 35) Super short daily-use commands

```cmd
dotnet new console -n App
dotnet build
dotnet run
dotnet test
dotnet add package PackageName
dotnet publish -c Release -o .\publish

cd /d D:\Projects\App
dir
mkdir test
del file.txt
rmdir /s /q folder
copy a.txt b.txt
move a.txt folder\
type file.txt
set
echo %PATH%
```

---


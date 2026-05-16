# Assemblies and NuGet Packages

## 1) The Big Picture

When you build a .NET application, your code is usually split into **reusable units**. Two important concepts appear all the time:

- **Assembly** → the compiled output of .NET code
- **NuGet package** → a distributable package used to share code and dependencies

A simple way to think about it:

> **Assembly = compiled code**  
> **NuGet package = delivery box that may contain assemblies and other files**

---

# 2) What Is an Assembly?

An **assembly** is the fundamental compiled unit in .NET.

It is typically produced when you compile a project, such as:

- a class library
- a console app
- a web app
- a desktop app

Assemblies usually have file extensions like:

- `.dll` → library
- `.exe` → executable application

## Assembly Contents

An assembly can contain:

- **Intermediate Language (IL)** code
- **metadata**
- **manifest information**
- **resources** such as strings, images, or embedded files

## Why Assemblies Matter

Assemblies are used by .NET for:

1. **deployment**
2. **versioning**
3. **reuse**
4. **security boundaries**
5. **type discovery**

---

# 3) Assembly Metadata

Every assembly carries descriptive information about itself.

## Common Metadata Examples

- assembly name
- version
- culture
- public key information
- referenced assemblies

## Example

You may see version-related information such as:

```csharp
[assembly: AssemblyVersion("2.1.0.0")]
[assembly: AssemblyFileVersion("2.1.4.0")]
```

### Meaning

- `AssemblyVersion`  
  Used by .NET when binding to an assembly.

- `AssemblyFileVersion`  
  Mostly for file/version display in the operating system.

---

# 4) Assembly Manifest

The **manifest** is a special part of the assembly that describes:

- what the assembly is
- what files belong to it
- what other assemblies it depends on

## The Manifest Usually Includes

- assembly name
- version
- culture
- strong name information
- referenced assemblies
- exported types
- included files/resources

> You can think of the **manifest** as the assembly’s **identity card**.

---

# 5) Types of Assemblies

## 5.1 Private Assembly

A **private assembly** is used only by one application and is usually stored inside that application's folder.

### Characteristics

- local to one application
- simple deployment
- low risk of affecting other applications

---

## 5.2 Shared Assembly

A **shared assembly** is intended to be used by multiple applications.

Historically, shared assemblies could be installed in places like the **Global Assembly Cache (GAC)**.

### Characteristics

- reusable across applications
- often strongly named
- requires more careful version management

---

## 5.3 Satellite Assembly

A **satellite assembly** contains **localized resources** instead of the main application logic.

Examples:

- translated text
- culture-specific resources
- localized UI content

### Example Cultures

- `en-US`
- `fr-FR`
- `es-ES`

---

# 6) Strong-Named Assemblies

A **strong name** gives an assembly a unique identity.

It includes:

- assembly name
- version
- culture
- public key token

## Why Use a Strong Name?

- avoids naming conflicts
- helps identify a specific publisher
- useful for some shared assembly scenarios

## Important Note

A strong name:

- **does not** mean the code is safe
- **does not** replace security checks
- **does** provide stronger identity

---

# 7) Referencing an Assembly

A project can use code from another assembly by adding a reference.

## What Happens When You Add a Reference?

Your project gains access to:

- public types
- public methods
- public properties
- public interfaces
- public attributes

## Example

If a library assembly contains:

```csharp
namespace UtilityKit;

public class TextFormatter
{
    public string MakeTitle(string value)
    {
        return $"~~ {value.ToUpper()} ~~";
    }
}
```

Another project can reference that assembly and use it:

```csharp
using UtilityKit;

var formatter = new TextFormatter();
Console.WriteLine(formatter.MakeTitle("inventory report"));
```

---

# 8) Assembly Dependency

Assemblies often depend on other assemblies.

For example:

- your app depends on a logging library
- the logging library depends on a JSON library
- the JSON library depends on other helper libraries

This forms a **dependency graph**.

## Why This Matters

Dependency management affects:

- application startup
- deployment
- compatibility
- runtime loading
- package restore behavior

---

# 9) What Is NuGet?

**NuGet** is the package manager for .NET.

It helps developers:

- find reusable libraries
- install dependencies
- update packages
- manage package versions
- share internal or public libraries

## In Simple Terms

> If an **assembly** is compiled code, then **NuGet** is the system used to **package and deliver** that code.

---

# 10) What Is a NuGet Package?

A **NuGet package** is a file with the extension:

```text
.nupkg
```

It is a compressed package that may include:

- one or more assemblies
- dependency information
- target framework information
- content files
- build files
- analyzers
- symbols or documentation-related files

## Important Distinction

Not every assembly is a NuGet package, and not every NuGet package contains only one assembly.

---

# 11) Assemblies vs NuGet Packages

## Quick Comparison

| Feature | Assembly | NuGet Package |
|---|---|---|
| What it is | Compiled unit | Distribution package |
| Common file type | `.dll`, `.exe` | `.nupkg` |
| Contains executable code | Yes | Often |
| Contains metadata | Yes | Yes |
| Can include dependencies | References only | Yes, explicitly |
| Used directly by runtime | Yes | Usually indirectly |
| Main purpose | Execution/reuse | Distribution/version management |

## Key Idea

- The **runtime loads assemblies**
- Developers usually **install NuGet packages**
- NuGet packages often **deliver assemblies**

---

# 12) How NuGet Works

## Typical Flow

1. A developer creates a library project.
2. The project is compiled into an assembly.
3. The library is packed into a NuGet package.
4. The package is published to a feed.
5. Another project installs the package.
6. NuGet restores dependencies.
7. The application builds and uses the included assemblies.

---

# 13) NuGet Feeds

A **feed** is a source from which packages are downloaded.

## Types of Feeds

- **public feed**  
  Accessible to many users

- **private feed**  
  Used inside an organization

- **local feed**  
  A folder on disk used as a package source

## Examples of Usage Scenarios

- team-shared utility libraries
- internal company SDKs
- reusable UI components
- testing helpers

---

# 14) Installing a NuGet Package

You can add a package to a project in different ways.

## Common Methods

- Visual Studio package manager UI
- .NET CLI
- Package Manager Console

## Example with .NET CLI

```bash
dotnet add package FastReportKit --version 4.3.2
```

This updates the project file and records the package dependency.

---

# 15) Package Restore

When a project depends on NuGet packages, those packages must be downloaded before the project can build.

This process is called **restore**.

## Restore Example

```bash
dotnet restore
```

## What Restore Does

- reads package references
- checks required versions
- downloads missing packages
- resolves dependency trees
- prepares assets for build

> Without restore, the project may know *which packages it wants* but not actually have them available locally.

---

# 16) PackageReference

Modern .NET projects usually declare packages in the project file using `PackageReference`.

## Example

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="FastReportKit" Version="4.3.2" />
    <PackageReference Include="DataGuard.Json" Version="6.1.0" />
  </ItemGroup>

</Project>
```

## Benefits of `PackageReference`

- cleaner project files
- better dependency resolution
- easier restore
- improved support for transitive dependencies

---

# 17) Direct and Transitive Dependencies

## Direct Dependency

A package you explicitly install:

```xml
<PackageReference Include="FastReportKit" Version="4.3.2" />
```

## Transitive Dependency

A package installed automatically because another package needs it.

### Example

- Your project installs `FastReportKit`
- `FastReportKit` depends on `DataGuard.Json`
- `DataGuard.Json` becomes a **transitive dependency**

## Why This Matters

Transitive dependencies can affect:

- version conflicts
- application size
- compatibility
- security patching
- debugging complexity

---

# 18) Package Versioning

NuGet packages use versions to identify releases.

## Example

```text
4.3.2
```

This commonly represents:

1. **major**
2. **minor**
3. **patch**

## General Meaning

- **major** → bigger breaking changes
- **minor** → new features, usually compatible
- **patch** → fixes and small improvements

---

# 19) Version Conflict Example

Suppose:

- your app needs `DataGuard.Json` version `6.1.0`
- another package needs `DataGuard.Json` version `7.0.0`

Now NuGet must resolve which version should be used.

## Possible Outcomes

- one version wins
- the build shows warnings
- the build fails
- runtime behavior changes if compatibility differs

## Practical Advice

When version conflicts appear:

1. inspect direct dependencies
2. inspect transitive dependencies
3. align versions where possible
4. update related packages together

---

# 20) Target Frameworks in Packages

NuGet packages can support different target frameworks.

## Examples

- `net8.0`
- `net7.0`
- `netstandard2.1`
- `net48`

A package may provide different assemblies for different frameworks.

## Why This Is Useful

It allows package authors to:

- optimize for newer runtimes
- keep compatibility with older applications
- expose framework-specific features

---

# 21) Multi-Targeting Example

A library can target multiple frameworks:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>net8.0;netstandard2.1</TargetFrameworks>
  </PropertyGroup>

</Project>
```

This means one project can produce outputs for more than one framework target.

---

# 22) Creating a NuGet Package

A developer can turn a class library into a NuGet package.

## Basic Steps

1. create a class library
2. add package metadata
3. build the project
4. pack the project
5. publish the package to a feed

## Example Project File

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageId>Northwind.Tools.Formatting</PackageId>
    <Version>1.2.0</Version>
    <Authors>Dev Team</Authors>
    <Description>Formatting helpers for business applications.</Description>
    <PackageTags>formatting;utilities;text</PackageTags>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>

</Project>
```

## Pack Command

```bash
dotnet pack
```

This creates a `.nupkg` file.

---

# 23) Publishing a Package

After packing, the package can be published to a feed.

## Example Command

```bash
dotnet nuget push bin/Release/Northwind.Tools.Formatting.1.2.0.nupkg --source InternalFeed
```

---

# 24) Package Contents Beyond Assemblies

A NuGet package may contain more than compiled libraries.

## It Can Also Include

- **build targets**
- **MSBuild props files**
- **analyzers**
- **source generators**
- **content files**
- **readme files**
- **symbol packages**

This is one reason a package is more than just a `.dll`.

---

# 25) Symbol Packages

When debugging libraries, symbols are helpful.

## Related Files

- `.pdb` → debugging symbols
- symbol package formats may be distributed separately

## Why Symbols Matter

They help with:

- debugging
- stack traces
- source mapping
- diagnosing production problems

---

# 26) Practical Example: Assembly vs Package

## Library Project

Imagine a project named `OrderTools`.

It compiles into:

```text
OrderTools.dll
```

That file is an **assembly**.

## Packaging It

Then you package it as:

```text
OrderTools.2.4.0.nupkg
```

That file is a **NuGet package**.

## Inside the Package

The package might contain:

- `lib/net8.0/OrderTools.dll`
- dependency metadata
- `README.md`
- analyzer files

So:

- `OrderTools.dll` = code unit used by runtime
- `OrderTools.2.4.0.nupkg` = container used to distribute it

---

# 27) Referencing a Project vs Referencing a Package

There are two common ways to reuse code during development.

## Project Reference

Use this when both projects are in the same solution or developed together.

```xml
<ItemGroup>
  <ProjectReference Include="..\OrderTools\OrderTools.csproj" />
</ItemGroup>
```

### Good For

- active development
- debugging across projects
- same repository solutions

---

## Package Reference

Use this when a library is distributed as a versioned package.

```xml
<ItemGroup>
  <PackageReference Include="OrderTools" Version="2.4.0" />
</ItemGroup>
```

### Good For

- reusable released libraries
- team distribution
- version-controlled dependency management

---

# 28) When to Use Each

## Use an Assembly Concept When You Need to Understand

- what the runtime loads
- compiled outputs
- metadata and manifests
- version binding
- code reuse at binary level

## Use a NuGet Concept When You Need to Understand

- dependency installation
- package distribution
- package feeds
- restore behavior
- version management across projects

---

# 29) Common Commands

## Working with Packages

```bash
dotnet add package FastReportKit
dotnet restore
dotnet list package
dotnet pack
```

## Notes

- `dotnet add package` → adds a dependency
- `dotnet restore` → downloads required packages
- `dotnet list package` → shows package references
- `dotnet pack` → creates a NuGet package

---

# 30) Common Misunderstandings

## Misunderstanding 1

> “Assembly and NuGet package are the same thing.”

**Not true.**

- An assembly is compiled code.
- A NuGet package is a distribution format.

---

## Misunderstanding 2

> “Installing a package means the runtime loads the package file.”

**Not exactly.**

The runtime typically loads the **assemblies extracted/resolved from the package**, not the `.nupkg` file itself.

---

## Misunderstanding 3

> “One package always contains one assembly.”

**Not necessarily.**

A single package may contain:

- multiple assemblies
- different assemblies for different frameworks
- no main library assembly at all, in some specialized cases

---

## Misunderstanding 4

> “Strong name means secure code.”

**Incorrect.**

A strong name improves identity, not trustworthiness or safety.

---

# 31) Memory Aid

## Short Definitions

- **Assembly** → compiled .NET unit
- **Manifest** → assembly identity and dependency description
- **NuGet** → package manager for .NET
- **NuGet package** → versioned distribution container
- **Package restore** → downloading required packages
- **Direct dependency** → package you add yourself
- **Transitive dependency** → package added because another package needs it

## Ultra-Short Analogy

| Concept | Analogy |
|---|---|
| Assembly | The actual machine |
| NuGet package | The shipping box |
| Manifest | The label/spec sheet |
| Package restore | Receiving the shipment |
| Dependency graph | The list of parts needed |

---

# 32) Mini End-to-End Example

## Step 1: Build a Library

```csharp
namespace BillingKit;

public class InvoiceCodeGenerator
{
    public string CreateCode(int number)
    {
        return $"INV-{number:0000}";
    }
}
```

This compiles into an assembly such as:

```text
BillingKit.dll
```

---

## Step 2: Pack It

Project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageId>BillingKit.Core</PackageId>
    <Version>1.0.0</Version>
    <Authors>Platform Team</Authors>
    <Description>Invoice code generation helpers.</Description>
  </PropertyGroup>

</Project>
```

Pack command:

```bash
dotnet pack
```

Output:

```text
BillingKit.Core.1.0.0.nupkg
```

---

## Step 3: Install It in Another Project

```bash
dotnet add package BillingKit.Core --version 1.0.0
```

---

## Step 4: Use It

```csharp
using BillingKit;

var generator = new InvoiceCodeGenerator();
Console.WriteLine(generator.CreateCode(27));
```

Output:

```text
INV-0027
```

---

# 33) Core Distinction to Remember

> **Assemblies are what .NET executes and loads.**  
> **NuGet packages are how libraries are packaged, shared, restored, and versioned.**
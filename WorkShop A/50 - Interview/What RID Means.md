# RID

## What RID Means

**RID** stands for **Runtime Identifier**.

It is a label used in **.NET** to describe the specific platform an application targets at runtime.

A RID usually identifies:

- **operating system**
- **CPU architecture**
- sometimes a more specific platform variation

> **RID tells the build system which runtime environment your app is meant for.**

---

## Why RID Matters

RIDs are important when you want to:

- publish for a specific platform
- include native dependencies for a target OS
- create self-contained deployments
- perform platform-specific builds
- control which runtime assets get selected

Without a RID, a build may remain more **portable**, but with fewer platform-specific decisions made ahead of time.

---

## Common RID Examples

| RID | Meaning |
|---|---|
| `win-x64` | Windows on 64-bit x86 |
| `win-arm64` | Windows on ARM64 |
| `linux-x64` | Linux on 64-bit x86 |
| `linux-arm64` | Linux on ARM64 |
| `osx-arm64` | macOS on ARM64 |

These values are strings recognized by the .NET build and publishing system.

---

## RID Structure

A RID often looks like this:

```text
<platform>-<architecture>
```

### Examples
- `win-x64`
- `linux-arm`
- `osx-arm64`

In some cases, older or more specific RIDs may include extra platform details, but the modern common pattern is simple.

---

## What a RID Is Used For

## 1. **Publishing for a Specific Platform**

When publishing an application, you can target a platform explicitly.

### Example

```bash
dotnet publish -r linux-x64
```

This tells the SDK:

- build for Linux
- target x64 architecture
- prepare platform-specific output

---

## 2. **Selecting Native Dependencies**

Some packages include native binaries for different systems.

For example, a library may provide:

- Windows native file
- Linux native file
- macOS native file

The RID helps .NET choose the correct one.

> If the target is `win-x64`, the build system prefers Windows x64 native assets.

---

## 3. **Creating Self-Contained Deployments**

If you want to ship your app together with the .NET runtime, you usually specify a RID.

### Example

```bash
dotnet publish -r win-x64 --self-contained true
```

This creates output intended to run on **Windows x64** without requiring a separately installed .NET runtime.

---

## 4. **Producing Platform-Specific Executables**

Some publish modes generate executable output tailored for a certain runtime environment.

That tailoring depends on the RID.

---

## RID vs Target Framework

These two are related, but they are **not the same**.

| Concept | What it describes |
|---|---|
| **Target Framework** | Which .NET API/runtime version you target |
| **RID** | Which operating system and architecture you target |

### Example

- Target Framework: `net9.0`
- RID: `linux-arm64`

This means:

- the app targets .NET 9 APIs
- the app is being built for Linux ARM64

---

## Example in a Project File

You may see a RID in a project file like this:

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
</PropertyGroup>
```

### Meaning
- `TargetFramework` chooses the .NET platform version
- `RuntimeIdentifier` chooses the runtime environment

---

## Multiple RIDs

Sometimes a project needs to support more than one runtime target.

### Example

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <RuntimeIdentifiers>win-x64;linux-x64;osx-arm64</RuntimeIdentifiers>
</PropertyGroup>
```

This tells the project that it may be published for several platforms.

---

## RuntimeIdentifier vs RuntimeIdentifiers

| Property | Use |
|---|---|
| `RuntimeIdentifier` | One RID |
| `RuntimeIdentifiers` | Multiple RIDs |

### Example
- `RuntimeIdentifier`: `linux-arm64`
- `RuntimeIdentifiers`: `win-x64;linux-x64;osx-arm64`

---

## RID and NuGet Packages

Some NuGet packages contain files organized by RID.

### Example idea

A package may internally include assets like:

```text
runtimes/win-x64/native/toolkit.dll
runtimes/linux-x64/native/libtoolkit.so
runtimes/osx-arm64/native/libtoolkit.dylib
```

When building or publishing, .NET uses the RID to choose the correct asset.

---

## RID and Self-Contained vs Framework-Dependent

## Framework-dependent deployment
- depends on a compatible .NET runtime already installed
- may not always require a RID

## Self-contained deployment
- includes the runtime with the app
- usually requires a RID

### Example

```bash
dotnet publish -r osx-arm64 --self-contained true
```

This creates a macOS ARM64 self-contained app.

---

## RID and AOT

RID also matters for **AOT compilation** because AOT output is platform-specific.

### Example

If you compile ahead of time for:

```text
linux-arm64
```

that native output is meant for **Linux ARM64**, not for Windows or macOS.

> AOT and self-contained publishing often depend strongly on the chosen RID.

---

## Important Limitation

A RID-specific build is usually **not portable** across unrelated platforms.

### Example
A build for:

```text
win-x64
```

will not run on:

- `linux-x64`
- `osx-arm64`

That is because the generated output and selected runtime assets are platform-specific.

---

## Common Commands

## Publish for Windows x64

```bash
dotnet publish -r win-x64
```

## Publish for Linux ARM64

```bash
dotnet publish -r linux-arm64
```

## Publish self-contained for macOS ARM64

```bash
dotnet publish -r osx-arm64 --self-contained true
```

---

## RID Graph Idea

Some runtime systems understand relationships between RIDs.

For example, a more specific RID may fall back to a broader compatible one when exact assets are unavailable.

### Conceptually

- exact match preferred
- compatible fallback used if needed

This helps package resolution, though modern RID usage is generally kept simpler than older systems.

---


## When You Usually Need to Care About RID

You often need to think about RIDs when:

- publishing an app for deployment
- working with native libraries
- building self-contained apps
- using AOT
- targeting multiple operating systems
- troubleshooting missing runtime assets

---

## Short Mental Model

> **Target Framework** says:  
> “Which .NET platform/version am I coding against?”

> **RID** says:  
> “Which OS and CPU should this built app run on?”
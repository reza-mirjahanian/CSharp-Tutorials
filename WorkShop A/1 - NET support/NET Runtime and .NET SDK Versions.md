# 🧩 .NET Runtime and .NET SDK Versions

## 1. What Is the .NET Runtime?

The **.NET Runtime** is the minimum component needed to **run** a .NET application.

If an application is **not self-contained**, the target machine must have the correct .NET Runtime installed.

> The runtime is for **running** apps, not building them.

---

## ✅ When You Need the .NET Runtime

You need the **.NET Runtime** when:

- You want to run a .NET application.
- The app was published as a **framework-dependent app**.
- The app depends on a shared .NET installation on the machine.

Example:

```bash
dotnet MyApp.dll
```

This command requires a compatible .NET Runtime to be installed.

---

## 📦 Runtime Includes

The .NET Runtime includes the components needed to execute .NET apps, such as:

| Component | Purpose |
|---|---|
| **CLR / Runtime engine** | Executes .NET code |
| **Base class libraries** | Provides common APIs such as strings, collections, files, networking, and dates |
| **Garbage collector** | Manages memory automatically |
| **JIT compiler** | Converts intermediate code into native machine code at runtime |

---

# 2. What Is the .NET SDK?

The **.NET SDK** includes everything needed to **build**, **compile**, **run**, **test**, and **publish** .NET applications.

> The SDK includes the runtime, but the runtime does not include the SDK.

---

## 🛠️ SDK Includes

The **.NET SDK** contains:

| Component | Purpose |
|---|---|
| **.NET Runtime** | Runs .NET apps |
| **C# compiler** | Compiles C# code |
| **F# compiler** | Compiles F# code |
| **VB compiler** | Compiles Visual Basic code |
| **CLI tools** | Provides commands such as `dotnet build`, `dotnet run`, and `dotnet publish` |
| **Project templates** | Creates new apps using `dotnet new` |
| **MSBuild** | Builds .NET projects |
| **NuGet tools** | Restores and manages packages |

---

## ✅ When You Need the .NET SDK

You need the **.NET SDK** when you want to:

1. Create a new project
2. Compile source code
3. Build an application
4. Run tests
5. Publish an app
6. Create NuGet packages
7. Use development tools from the command line

Example:

```bash
dotnet build
```

This command requires the **.NET SDK**, not just the runtime.

---

# 3. Runtime vs SDK

| Feature | .NET Runtime | .NET SDK |
|---|---:|---:|
| Run .NET apps | ✅ Yes | ✅ Yes |
| Build .NET apps | ❌ No | ✅ Yes |
| Compile code | ❌ No | ✅ Yes |
| Create new projects | ❌ No | ✅ Yes |
| Run `dotnet build` | ❌ No | ✅ Yes |
| Run `dotnet new` | ❌ No | ✅ Yes |
| Includes compilers | ❌ No | ✅ Yes |
| Includes runtime | ✅ Yes | ✅ Yes |

---

## Simple Rule

> Install the **Runtime** on machines that only need to run apps.  
> Install the **SDK** on machines used for development or building apps.

---

# 4. Framework-Dependent vs Self-Contained Apps

## Framework-Dependent App

A **framework-dependent app** relies on a .NET Runtime already installed on the operating system.

### Example

```bash
dotnet MyApp.dll
```

The machine must have a compatible runtime installed.

---

### Characteristics

| Feature | Framework-Dependent App |
|---|---|
| Requires installed runtime | ✅ Yes |
| Smaller application size | ✅ Yes |
| Runtime updates shared by apps | ✅ Yes |
| Easier patching | ✅ Yes |
| Can fail if runtime is missing | ⚠️ Yes |

---

## Self-Contained App

A **self-contained app** includes the .NET Runtime with the application.

The target machine does **not** need a separate .NET Runtime installation.

---

### Example Publish Command

```bash
dotnet publish -r win-x64 --self-contained true
```

---

### Characteristics

| Feature | Self-Contained App |
|---|---|
| Requires installed runtime | ❌ No |
| Includes runtime with app | ✅ Yes |
| Larger application size | ✅ Yes |
| More deployment control | ✅ Yes |
| App owner manages runtime updates | ✅ Yes |

---

# 5. .NET Runtime Versioning

The **.NET Runtime** follows **semantic versioning**.

Semantic versioning usually uses this format:

```text
MAJOR.MINOR.PATCH
```

Example:

```text
10.0.3
```

This version has:

| Part | Value | Meaning |
|---|---:|---|
| **Major** | `10` | Major version |
| **Minor** | `0` | Minor version |
| **Patch** | `3` | Patch version |

---

# 6. Semantic Versioning in the .NET Runtime

## Version Format

```text
major.minor.patch
```

Example:

```text
8.0.15
9.0.4
10.0.1
```

---

## Meaning of Each Number

| Version Part | Example | Meaning |
|---|---:|---|
| **Major** | `10` | Indicates possible breaking changes |
| **Minor** | `0` | Indicates new features |
| **Patch** | `3` | Indicates bug fixes and security fixes |

---

# 7. Major Runtime Version

## 🔴 Major Increment

A **major version increment** means the first number changes.

Example:

```text
9.0.0 → 10.0.0
```

---

## What It Usually Means

A major version may include:

- Breaking changes
- New runtime behavior
- New APIs
- Removed or changed features
- Compatibility changes

---

## Example

```text
.NET Runtime 8.0.0
.NET Runtime 9.0.0
.NET Runtime 10.0.0
```

The major versions are:

```text
8
9
10
```

---

# 8. Minor Runtime Version

## 🟡 Minor Increment

A **minor version increment** means the second number changes.

Example:

```text
10.0.0 → 10.1.0
```

---

## What It Usually Means

A minor version may include:

- New features
- New APIs
- Improvements
- Backward-compatible changes

---

## Example

```text
10.0.0 → 10.1.0
```

This means:

| Version | Meaning |
|---|---|
| `10.0.0` | Original runtime version |
| `10.1.0` | Same major version, new minor features |

---

# 9. Patch Runtime Version

## 🟢 Patch Increment

A **patch version increment** means the third number changes.

Example:

```text
10.0.0 → 10.0.1
```

---

## What It Usually Means

A patch version usually includes:

- Bug fixes
- Security fixes
- Reliability fixes
- Performance fixes

---

## Example

```text
10.0.0
10.0.1
10.0.2
10.0.3
```

Each patch update improves or fixes the same runtime version line.

---

# 10. Runtime Version Examples

| Runtime Version | Major | Minor | Patch | Meaning |
|---|---:|---:|---:|---|
| `8.0.0` | `8` | `0` | `0` | Initial .NET 8 runtime release |
| `8.0.5` | `8` | `0` | `5` | .NET 8 patch update |
| `9.0.0` | `9` | `0` | `0` | Initial .NET 9 runtime release |
| `9.0.2` | `9` | `0` | `2` | .NET 9 patch update |
| `10.0.0` | `10` | `0` | `0` | Initial .NET 10 runtime release |
| `10.0.1` | `10` | `0` | `1` | .NET 10 patch update |

---

# 11. .NET SDK Versioning

The **.NET SDK** does **not** follow semantic versioning in the same way as the runtime.

SDK versions also use three numbers:

```text
MAJOR.MINOR.FEATURE-BAND
```

Example:

```text
10.0.100
```

But the meaning is different from runtime versions.

---

## Important Difference

| Version Type | Follows Semantic Versioning? |
|---|---:|
| **.NET Runtime** | ✅ Yes |
| **.NET SDK** | ❌ No |

---

# 12. How SDK Version Numbers Work

A .NET SDK version looks like this:

```text
10.0.100
```

It can be understood as:

```text
RuntimeMajor.RuntimeMinor.SDKFeaturePatch
```

---

## Example Breakdown

| SDK Version | Runtime Match | SDK Feature/Patch Number |
|---|---:|---:|
| `10.0.100` | `.NET 10.0` | `100` |

---

## First Two Numbers

The first two numbers are tied to the runtime version.

Example:

```text
10.0.100
```

The `10.0` part means the SDK is matched with the `.NET 10.0` runtime line.

---

## Third Number

The third number uses a special SDK convention.

Example:

```text
100
```

The third number starts at `100` for the initial SDK version.

---

# 13. SDK Third Number Convention

The third number in the SDK version indicates SDK feature and patch updates.

Example:

```text
10.0.100
```

The `100` part can be thought of like this:

```text
1 00
│ └── Patch part
└──── Feature band
```

---

## Structure

```text
10.0.100
     │││
     │└┴── Patch number
     └──── Feature band
```

More clearly:

| Digit | Meaning |
|---|---|
| First digit of third number | SDK feature band |
| Last two digits of third number | SDK patch level |

---

# 14. Initial SDK Version

The initial SDK version starts at:

```text
10.0.100
```

This is equivalent to SDK feature version:

```text
10.0.0.0
```

Conceptually:

| SDK Version | Meaning |
|---|---|
| `10.0.100` | Initial SDK release for .NET 10 |

---

# 15. SDK Patch Increments

Patch increments change the last two digits of the third number.

Example:

```text
10.0.100 → 10.0.101 → 10.0.102
```

---

## Meaning

| SDK Version | Meaning |
|---|---|
| `10.0.100` | Initial SDK release |
| `10.0.101` | First SDK patch |
| `10.0.102` | Second SDK patch |
| `10.0.103` | Third SDK patch |

---

## Pattern

```text
10.0.100
10.0.101
10.0.102
10.0.103
```

Only the patch portion of the SDK changes.

---

# 16. SDK Minor / Feature Band Increments

The first digit of the third number changes when the SDK gets a new feature band.

Example:

```text
10.0.100 → 10.0.200
```

---

## Meaning

| SDK Version | Meaning |
|---|---|
| `10.0.100` | First SDK feature band |
| `10.0.200` | Second SDK feature band |
| `10.0.300` | Third SDK feature band |
| `10.0.400` | Fourth SDK feature band |

---

## Pattern

```text
10.0.100
10.0.200
10.0.300
10.0.400
```

Each jump from `100` to `200`, or from `200` to `300`, indicates a new SDK feature band.

---

# 17. SDK Version Examples

| SDK Version | Runtime Line | Feature Band | Patch Level | Meaning |
|---|---:|---:|---:|---|
| `10.0.100` | `10.0` | `1xx` | `00` | Initial SDK release |
| `10.0.101` | `10.0` | `1xx` | `01` | First patch of `10.0.100` |
| `10.0.102` | `10.0` | `1xx` | `02` | Second patch of `10.0.100` |
| `10.0.200` | `10.0` | `2xx` | `00` | New SDK feature band |
| `10.0.201` | `10.0` | `2xx` | `01` | First patch of `10.0.200` |
| `10.0.300` | `10.0` | `3xx` | `00` | Another SDK feature band |

---

# 18. Runtime Version vs SDK Version Examples

## Runtime Versions

Runtime versions follow semantic versioning:

```text
10.0.0
10.0.1
10.0.2
```

---

## SDK Versions

SDK versions follow SDK feature-band versioning:

```text
10.0.100
10.0.101
10.0.200
10.0.201
```

---

## Side-by-Side Comparison

| Runtime Version | SDK Version | What Changed? |
|---|---|---|
| `10.0.0` | `10.0.100` | Initial release |
| `10.0.1` | `10.0.101` | Patch update |
| `10.0.2` | `10.0.102` | Patch update |
| `10.0.3` | `10.0.200` | New SDK feature band with newer runtime patch |
| `10.0.4` | `10.0.201` | SDK patch update |

---

# 19. Important Example: `10.0.100`

The SDK version:

```text
10.0.100
```

does **not** mean:

```text
Major = 10
Minor = 0
Patch = 100
```

Instead, it means:

| Part | Meaning |
|---|---|
| `10` | Matches .NET Runtime major version |
| `0` | Matches .NET Runtime minor version |
| `100` | SDK feature band and patch convention |

---

# 20. Important Example: `10.0.201`

The SDK version:

```text
10.0.201
```

means:

| Part | Meaning |
|---|---|
| `10` | Matches .NET Runtime major version |
| `0` | Matches .NET Runtime minor version |
| `2xx` | Second SDK feature band |
| `01` | First patch in that feature band |

---

## Visual Breakdown

```text
10.0.201
│  │ │││
│  │ │└┴── Patch level: 01
│  │ └──── Feature band: 2xx
│  └────── Runtime minor version: 0
└───────── Runtime major version: 10
```

---

# 21. Installing Runtime vs SDK

## Runtime Installation Scenario

Install the runtime when the machine only needs to run apps.

Example machines:

- Production web server
- Application server
- User desktop running a .NET app
- Container image for running an app

---

## SDK Installation Scenario

Install the SDK when the machine needs to build apps.

Example machines:

- Developer workstation
- Build server
- CI/CD runner
- Test environment that compiles code
- Machine used for `dotnet publish`

---

# 22. Common Commands and Requirements

| Command | Requires Runtime? | Requires SDK? | Purpose |
|---|---:|---:|---|
| `dotnet MyApp.dll` | ✅ Yes | ❌ No | Run a compiled app |
| `dotnet new console` | ❌ Runtime alone is not enough | ✅ Yes | Create a new project |
| `dotnet restore` | ❌ Runtime alone is not enough | ✅ Yes | Restore NuGet packages |
| `dotnet build` | ❌ Runtime alone is not enough | ✅ Yes | Build a project |
| `dotnet test` | ❌ Runtime alone is not enough | ✅ Yes | Run tests |
| `dotnet publish` | ❌ Runtime alone is not enough | ✅ Yes | Publish an app |
| `dotnet --list-runtimes` | ✅ Yes | ✅ Yes | Show installed runtimes |
| `dotnet --list-sdks` | ❌ Runtime alone may not show SDKs | ✅ Yes | Show installed SDKs |

---

# 23. Checking Installed Versions

## Check Installed Runtimes

```bash
dotnet --list-runtimes
```

Example output:

```text
Microsoft.NETCore.App 10.0.0
Microsoft.AspNetCore.App 10.0.0
```

---

## Check Installed SDKs

```bash
dotnet --list-sdks
```

Example output:

```text
10.0.100
10.0.101
10.0.200
```

---

## Check Default SDK

```bash
dotnet --version
```

Example output:

```text
10.0.100
```

This shows the SDK version currently selected by the `dotnet` command.

---

# 24. Practical Versioning Examples

## Example 1: Runtime Patch Update

```text
10.0.0 → 10.0.1
```

Meaning:

| Change | Explanation |
|---|---|
| Runtime patch changed | `0` to `1` |
| Breaking changes expected? | ❌ Usually no |
| Purpose | Bug fixes/security fixes |

---

## Example 2: Runtime Major Update

```text
9.0.0 → 10.0.0
```

Meaning:

| Change | Explanation |
|---|---|
| Runtime major changed | `9` to `10` |
| Breaking changes possible? | ✅ Yes |
| Purpose | New major .NET release |

---

## Example 3: SDK Patch Update

```text
10.0.100 → 10.0.101
```

Meaning:

| Change | Explanation |
|---|---|
| SDK patch changed | `100` to `101` |
| Same feature band? | ✅ Yes |
| Purpose | SDK patch fixes |

---

## Example 4: SDK Feature Band Update

```text
10.0.100 → 10.0.200
```

Meaning:

| Change | Explanation |
|---|---|
| SDK feature band changed | `1xx` to `2xx` |
| New SDK feature band? | ✅ Yes |
| Runtime line still matched? | ✅ `10.0` |

---

# 25. Key Concepts to Remember

## Runtime

```text
Used to run apps
```

Example:

```text
.NET Runtime 10.0.1
```

---

## SDK

```text
Used to build apps
```

Example:

```text
.NET SDK 10.0.100
```

---

## Runtime Versioning

```text
major.minor.patch
```

Example:

```text
10.0.1
```

---

## SDK Versioning

```text
major.minor.feature-band
```

Example:

```text
10.0.100
```

---

# 26. Quick Reference Table

| Concept | Example | Meaning |
|---|---|---|
| Runtime | `10.0.0` | Initial .NET 10 runtime |
| Runtime patch | `10.0.1` | Bug/security fixes |
| SDK | `10.0.100` | Initial .NET 10 SDK |
| SDK patch | `10.0.101` | Patch to SDK feature band `1xx` |
| SDK feature band | `10.0.200` | New SDK feature band |
| Runtime only | Installed on app-running machines | Runs apps |
| SDK | Installed on developer/build machines | Builds and runs apps |
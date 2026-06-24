# 📌 .NET Version Lifecycle and Support Phases

Every version of **.NET** goes through a lifecycle. During this lifecycle, Microsoft provides different levels of support depending on the phase the version is in.

A .NET version usually moves through these phases:

1. **Preview**
2. **Go Live**
3. **Active**
4. **Maintenance**
5. **End of Life**, also called **EOL**

---

## 🧭 .NET Support Phases

| Phase | Support Level | What It Means |
|---|---:|---|
| **Preview** | ❌ Not supported | Early test versions. Not intended for production use. |
| **Go Live** | ✅ Temporarily supported | Release candidates that can be used before the final release, but must be upgraded once GA arrives. |
| **Active** | ✅ Fully supported | Monthly updates include bug fixes and security fixes. |
| **Maintenance** | ⚠️ Limited support | Only security fixes are provided. No general bug fixes. |
| **EOL** | ❌ Unsupported | No bug fixes, no security updates, and no technical support. |

---

# 1. Preview Phase

## 🔍 What Is the Preview Phase?

The **Preview** phase is the earliest public stage of a .NET version.

Preview releases are meant for:

- Testing new features
- Trying upcoming APIs
- Giving feedback
- Preparing libraries or applications for future versions

They are **not supported** for production workloads.

> **Preview versions should not be used for production applications.**

---

## Example: .NET 10 Preview

`.NET 10 Preview 1` through `.NET 10 Preview 7` were in the **Preview** phase from:

| Version Range | Phase | Time Period |
|---|---|---|
| .NET 10 Preview 1 to Preview 7 | Preview | February 2025 to August 2025 |

---

## Key Points

- ❌ No official production support
- ❌ No guaranteed stability
- ❌ APIs may change before final release
- ✅ Useful for testing and preparation

---

# 2. Go Live Phase

## 🚀 What Is the Go Live Phase?

The **Go Live** phase applies to late pre-release versions, usually called **Release Candidates**, or `RC`.

These versions are more stable than previews and may be supported temporarily.

However, once the final release becomes available, the release candidate immediately becomes unsupported.

---

## Example: .NET 10 Release Candidates

| Version | Phase | Time Period |
|---|---|---|
| .NET 10 Release Candidate 1 | Go Live | September 2025 |
| .NET 10 Release Candidate 2 | Go Live | October 2025 |

---

## Important Rule

> Once the final release, also called **GA**, becomes available, you must upgrade from the Release Candidate to the final version.

---

## What Does GA Mean?

**GA** means **General Availability**.

A GA release is the official final production-ready release.

For example:

```text
.NET 10 Preview 7       → Not production supported
.NET 10 RC 1            → Temporarily supported
.NET 10 RC 2            → Temporarily supported
.NET 10 GA              → Official final release
```

---

# 3. Active Phase

## ✅ What Is the Active Phase?

The **Active** phase is the main support period for a .NET version.

During this phase, Microsoft provides:

- Bug fixes
- Security fixes
- Monthly patch updates
- Technical support

---

## Example: .NET 10 Active Phase

`.NET 10` is in the **Active** phase from:

| Version | Phase | Time Period |
|---|---|---|
| .NET 10 | Active | November 2025 to May 2028 |

---

## Monthly Patch Updates

During the Active phase, .NET receives regular monthly releases.

These releases may include:

1. **Security fixes**
2. **Reliability fixes**
3. **Runtime fixes**
4. **SDK fixes**
5. **ASP.NET Core fixes**
6. **Performance improvements**
7. **Tooling updates**

---

## Important Support Requirement

To remain supported, you must install the latest patch updates.

For example, if your project targets:

```text
.NET 10.0
```

and Microsoft releases:

```text
.NET 10.0.1
.NET 10.0.2
.NET 10.0.3
```

you should keep updating to the latest patch version.

> Support depends on staying current with monthly patch releases.

---

# 4. Maintenance Phase

## 🛠️ What Is the Maintenance Phase?

The **Maintenance** phase is the final supported period before a .NET version reaches EOL.

During this phase, support is more limited.

Microsoft provides:

- ✅ Security fixes
- ❌ No general bug fixes
- ❌ No feature improvements

---

## Example: .NET 10 Maintenance Phase

`.NET 10` will be in the **Maintenance** phase from:

| Version | Phase | Time Period |
|---|---|---|
| .NET 10 | Maintenance | May 2028 to November 2028 |

---

## Maintenance Phase Duration

The maintenance phase usually lasts for the **last six months** of a .NET version’s supported lifetime.

```text
Active Support      → Main support period
Maintenance Support → Final 6 months of support
EOL                 → No support
```

---

## Migration Planning During Maintenance

When a .NET version enters maintenance, you should begin or complete migration to a newer supported version.

For example:

| Existing Target Version | Recommended Migration Target |
|---|---|
| .NET 8 | .NET 10 |
| .NET 9 | .NET 10 |

---

## Why Start Migration Early?

Large applications can take months to migrate.

Migration may involve:

1. Updating target frameworks
2. Updating NuGet packages
3. Fixing breaking changes
4. Updating CI/CD pipelines
5. Testing the application
6. Updating deployment environments
7. Verifying performance
8. Re-certifying production systems

> For large projects, migration can take up to six months.

---

# 5. End of Life Phase

## ⛔ What Is EOL?

**EOL** means **End of Life**.

It is also called:

- **End of support**
- **End of servicing**
- **Unsupported phase**

After a .NET version reaches EOL, Microsoft no longer provides:

- Bug fixes
- Security updates
- Technical assistance
- Compatibility fixes

---

## Example: .NET 10 EOL

| Version | EOL Date |
|---|---:|
| .NET 10 | November 2028 |

---

## What Happens After EOL?

After EOL, applications may still run, but the platform is no longer supported.

For example, if an application uses an unsupported .NET version:

```text
The application may continue to work,
but it will no longer receive security updates.
```

---

# 📅 .NET 10 Lifecycle Timeline

| Phase | Time Period | Support Level |
|---|---|---|
| **Preview** | February 2025 to August 2025 | Not supported |
| **Go Live** | September 2025 to October 2025 | Temporarily supported |
| **Active** | November 2025 to May 2028 | Fully supported |
| **Maintenance** | May 2028 to November 2028 | Security fixes only |
| **EOL** | November 2028 onward | Not supported |

---

## Visual Timeline

```text
Feb 2025 ───────── Aug 2025
Preview Phase
No production support

Sep 2025 ───────── Oct 2025
Go Live Phase
Release Candidates supported until GA

Nov 2025 ───────── May 2028
Active Phase
Bug fixes + security fixes

May 2028 ───────── Nov 2028
Maintenance Phase
Security fixes only

Nov 2028 onward
EOL
No support
```

---

# 🧠 Understanding End of Life

## What Does End of Life Mean?

**End of Life**, or **EOL**, is the date after which a .NET version no longer receives official support.

After EOL, Microsoft no longer provides:

| Support Type | Available After EOL? |
|---|---:|
| Bug fixes | ❌ No |
| Security updates | ❌ No |
| Technical support | ❌ No |
| Runtime patches | ❌ No |
| SDK patches | ❌ No |

---

## Important EOL Concept

> A project may still run after EOL, but running unsupported software creates risk.

---

# 📦 Supported Modern .NET Versions

The following versions are still relevant in the lifecycle described here:

| Version | Current/Planned Phase | Maintenance Phase | EOL |
|---|---|---|---|
| **.NET 8** | Active | May 2026 | November 2026 |
| **.NET 9** | Active | May 2026 | November 2026 |
| **.NET 10** | Active until May 2028 | May 2028 to November 2028 | November 2028 |
| **.NET 11** | Preview from February 2026 to early November 2026 | Not listed separately | November 2028 |
| **.NET 12** | Preview from February 2027, active from November 2027 | Later lifecycle phase | November 2030 |

---

# 📘 .NET 8 Lifecycle

## .NET 8 Support Timeline

| Phase | Time Period |
|---|---|
| Active | Until May 2026 |
| Maintenance | May 2026 to November 2026 |
| EOL | November 2026 |

---

## What This Means

If you have projects targeting `.NET 8`, you should plan migration before November 2026.

Example target framework:

```xml
<TargetFramework>net8.0</TargetFramework>
```

A recommended future target could be:

```xml
<TargetFramework>net10.0</TargetFramework>
```

---

# 📗 .NET 9 Lifecycle

## .NET 9 Support Timeline

| Phase | Time Period |
|---|---|
| Active | Until May 2026 |
| Maintenance | May 2026 to November 2026 |
| EOL | November 2026 |

---

## What This Means

If you have projects targeting `.NET 9`, you should also plan migration before November 2026.

Example target framework:

```xml
<TargetFramework>net9.0</TargetFramework>
```

A recommended future target could be:

```xml
<TargetFramework>net10.0</TargetFramework>
```

---

# 📙 .NET 10 Lifecycle

## .NET 10 Support Timeline

| Phase | Time Period |
|---|---|
| Preview | February 2025 to August 2025 |
| Go Live | September 2025 to October 2025 |
| Active | November 2025 to May 2028 |
| Maintenance | May 2028 to November 2028 |
| EOL | November 2028 |

---

## What This Means

`.NET 10` is a good migration target for projects currently using `.NET 8` or `.NET 9`.

Example:

```xml
<TargetFramework>net10.0</TargetFramework>
```

---

# 📕 .NET 11 Lifecycle

## .NET 11 Support Timeline

| Phase | Time Period |
|---|---|
| Preview | February 2026 to early November 2026 |
| Active | November 2026 to May 2028 |
| EOL | November 2028 |

---

## What This Means

`.NET 11` follows a shorter lifecycle than `.NET 10`.

It is useful for developers who want newer features, but it reaches EOL at the same time as `.NET 10`.

---

# 📒 .NET 12 Lifecycle

## .NET 12 Support Timeline

| Phase | Time Period |
|---|---|
| Preview | From February 2027 |
| Active | From November 2027 |
| EOL | November 2030 |

---

## What This Means

`.NET 12` has a longer support period and becomes a future migration target after `.NET 10`.

---

# ⚠️ What Happens When .NET 8 and .NET 9 Reach EOL?

`.NET 8` and `.NET 9` reach end of support in:

```text
November 2026
```

After that date, several things happen.

---

## 1. Existing Applications May Continue to Run

Applications built with `.NET 8` or `.NET 9` will not automatically stop working.

For example:

```text
A .NET 8 web app may still start.
A .NET 9 background service may still run.
A .NET 8 desktop application may still open.
```

However, continued execution does not mean continued support.

---

## 2. No New Security Updates

After EOL, Microsoft will no longer release security patches for those versions.

This means that if a vulnerability is found in:

- The .NET runtime
- ASP.NET Core
- The SDK
- Framework libraries

it will not be fixed for unsupported versions.

> Continuing to use unsupported .NET versions increases security risk over time.

---

## 3. Limited or No Technical Support

After EOL, you may not be able to receive technical support for applications using unsupported versions.

This can affect:

1. Production troubleshooting
2. Enterprise support requests
3. Cloud hosting support
4. Compliance reviews
5. Security audits

---

## 4. Build Warnings from Newer SDKs

When targeting unsupported frameworks from a newer SDK, you may see warnings.

For example, if you use the `.NET 10 SDK` to build a `.NET 8` or `.NET 9` project, you may see:

```text
NETSDK1138
```

This warning means the target framework is out of support.

---

## Example Project File

A project targeting `.NET 8`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

</Project>
```

After `.NET 8` reaches EOL, newer SDKs may warn that `net8.0` is unsupported.

---

## 5. Visual Studio Warnings

Visual Studio may also display warnings when a project targets an unsupported .NET version.

For example:

| Target Framework | Expected Warning |
|---|---|
| `net8.0` | Unsupported after November 2026 |
| `net9.0` | Unsupported after November 2026 |

---

# 🧩 How to Think About .NET Support

## Support Is Tied to Versions

Support is not just about using `.NET`.

It depends on which version you use.

For example:

```text
Using .NET 10 with current patches → Supported
Using .NET 10 without patches       → May not be fully supported
Using .NET 8 after EOL              → Unsupported
Using .NET 9 after EOL              → Unsupported
```

---

## Support Is Also Tied to Updates

During the Active and Maintenance phases, you should regularly install patch updates.

For example:

```text
.NET 10.0.0 → Initial release
.NET 10.0.1 → Patch update
.NET 10.0.2 → Patch update
.NET 10.0.3 → Patch update
```

Each patch may include important security and reliability fixes.

---

# 🛣️ Recommended Migration Planning

## If Your Project Targets .NET 8

Current target:

```xml
<TargetFramework>net8.0</TargetFramework>
```

Recommended migration target:

```xml
<TargetFramework>net10.0</TargetFramework>
```

---

## If Your Project Targets .NET 9

Current target:

```xml
<TargetFramework>net9.0</TargetFramework>
```

Recommended migration target:

```xml
<TargetFramework>net10.0</TargetFramework>
```

---

## Migration Checklist

### 1. Update the Target Framework

Change the project file.

Example:

```xml
<TargetFramework>net8.0</TargetFramework>
```

to:

```xml
<TargetFramework>net10.0</TargetFramework>
```

---

### 2. Update NuGet Packages

Update package references to versions compatible with the new .NET version.

Example:

```xml
<PackageReference Include="Example.Package" Version="10.0.0" />
```

---

### 3. Fix Build Errors

After changing the target framework, rebuild the project.

```bash
dotnet build
```

Fix any errors or warnings.

---

### 4. Run Tests

Run the test suite:

```bash
dotnet test
```

Check for:

- Failing unit tests
- Integration test issues
- Runtime behavior changes
- Dependency compatibility problems

---

### 5. Test Deployment

Test the app in a staging or pre-production environment before production.

```bash
dotnet publish -c Release
```

---

### 6. Monitor After Release

After deploying the migrated application, monitor:

- Logs
- Performance
- Error rates
- Memory usage
- Startup behavior
- API responses

---

# 🧾 Quick Reference: Lifecycle Terms

| Term | Meaning |
|---|---|
| **Preview** | Early test release; not supported for production |
| **RC** | Release Candidate; near-final version |
| **Go Live** | Temporarily supported pre-release phase |
| **GA** | General Availability; final production release |
| **Active** | Main support phase with bug fixes and security updates |
| **Maintenance** | Final support phase with security fixes only |
| **EOL** | End of Life; no support or updates |

---

# 🔔 Key Warning Codes and Messages

## NETSDK1138

You may see this warning when building a project that targets an unsupported .NET version.

Example:

```text
NETSDK1138: The target framework is out of support and will not receive security updates in the future.
```

---

## When You Might See It

You may see `NETSDK1138` when:

1. Using a newer SDK
2. Targeting an older unsupported framework
3. Building a project after that framework reaches EOL

Example:

```bash
dotnet build
```

with a project targeting:

```xml
<TargetFramework>net8.0</TargetFramework>
```

after `.NET 8` reaches EOL.

---

# 🧱 Practical Example

## Before Migration

A project targeting `.NET 8`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

</Project>
```

---

## After Migration

The same project migrated to `.NET 10`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

</Project>
```

---

# 📊 Lifecycle Comparison

| Version | Preview Starts | Active Starts | Maintenance Starts | EOL |
|---|---:|---:|---:|---:|
| .NET 8 | Earlier release cycle | Active | May 2026 | November 2026 |
| .NET 9 | Earlier release cycle | Active | May 2026 | November 2026 |
| .NET 10 | February 2025 | November 2025 | May 2028 | November 2028 |
| .NET 11 | February 2026 | November 2026 | Not specified | November 2028 |
| .NET 12 | February 2027 | November 2027 | Not specified | November 2030 |

---

# ✅ Best Practices for Staying Supported

## 1. Avoid Preview Versions in Production

Use preview versions only for testing, experimentation, and preparation.

```text
Preview → Testing only
GA      → Production use
```

---

## 2. Upgrade from Release Candidates Immediately

If you used a Release Candidate, upgrade to the GA version as soon as it is available.

```text
.NET 10 RC 2 → .NET 10 GA
```

---

## 3. Install Monthly Patches

Stay on current patch versions during Active and Maintenance phases.

```text
.NET 10.0.0 → .NET 10.0.1 → .NET 10.0.2
```

---

## 4. Start Migration Before Maintenance Ends

Do not wait until EOL.

A safer plan is:

```text
Maintenance starts → Migration should already be planned or in progress
EOL approaches     → Migration should be complete
```

---

## 5. Watch for SDK and IDE Warnings

Pay attention to warnings from:

- `dotnet build`
- Visual Studio
- CI/CD pipelines
- Package restore
- Deployment tools

Warnings such as `NETSDK1138` are signals that a target framework may no longer be supported.
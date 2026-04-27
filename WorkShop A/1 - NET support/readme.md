# .NET Release Types and Support Lifecycles

.NET versions fall into **three main categories**:

1. **LTS — Long-Term Support**
2. **STS — Standard-Term Support**
3. **Preview**

Each type has a different purpose and support period.

---

## 1. LTS — Long-Term Support

**LTS** stands for **Long-Term Support**.

An LTS release is designed for applications that need **stability and longer support**.

### ✅ Support Duration

Microsoft supports an LTS release for:

- **3 years after General Availability**, also called **GA**
  
  **or**

- **1 year after the next LTS release ships**

whichever period is **longer**.

> **GA** means *General Availability*, which is the official production-ready release date.

### Best Used For

Use **LTS** when you want:

- Long-term stability
- Fewer major upgrades
- Production systems with longer maintenance cycles
- Enterprise applications
- Apps where support duration matters more than getting the newest features quickly

---

## 2. STS — Standard-Term Support

**STS** stands for **Standard-Term Support**.

STS was previously called **Current**.

An STS release is designed for developers and teams who want access to newer .NET features sooner, while still using a supported production-ready release.

### ✅ Support Duration

Microsoft supports an STS release for:

- **2 years after General Availability**

  **or**

- **1 year after the next STS or LTS release ships**

whichever period is **longer**.

### Best Used For

Use **STS** when you want:

- Newer .NET features earlier
- A faster upgrade cycle
- Supported production releases
- To stay closer to the latest .NET platform improvements

---

## 3. Preview Releases

**Preview** releases are early versions of .NET made available for public testing.

They are **not final production releases**.

### ⚠️ Purpose of Preview Releases

Preview releases are mainly intended for:

- Testing upcoming features
- Trying new language improvements
- Exploring new libraries
- Experimenting with future app and service platforms
- Giving feedback before the final release

### Who Should Use Preview Releases?

Preview releases are suitable for:

- Developers who enjoy early access
- Programmers testing new features
- Library authors preparing for future versions
- Authors or educators who need to cover upcoming functionality

### Production Use

Preview releases are **usually not supported for production**.

However, some Preview or **RC** releases may be marked as **Go Live**.

> **RC** means *Release Candidate*.  
> A Release Candidate is close to the final version and may become the official release if no serious issues are found.

If a Preview or RC release is declared **Go Live**, Microsoft supports it in production.

However, once the final version becomes available, you should migrate to the final release as soon as possible.

---

# Quick Comparison

| Release Type | Full Name | Support Length | Production Ready? | Best For |
|---|---|---:|---|---|
| **LTS** | Long-Term Support | About **3 years** | ✅ Yes | Stable, long-lived production apps |
| **STS** | Standard-Term Support | About **2 years** | ✅ Yes | Apps that want newer features sooner |
| **Preview** | Preview Release | Limited or none | ⚠️ Usually no | Testing upcoming features |

---

# LTS vs STS

A common misunderstanding is that **LTS releases are higher quality than STS releases**.

That is not true.

## There Is No Quality Difference

There is **no difference in quality** between an **LTS** release and an **STS** release.

| Misconception | Reality |
|---|---|
| LTS releases have fewer bugs | ❌ Not necessarily |
| STS releases are more experimental | ❌ Not necessarily |
| LTS is more stable because it is better tested | ❌ Not exactly |
| STS is lower quality | ❌ No |

The .NET teams plan features, implement them, test them, and release them.

Both LTS and STS releases are production-ready.

---

## The Real Difference

The only real difference between **LTS** and **STS** is:

> **How long Microsoft promises to support the release.**

### LTS

- Longer support
- Better for slower upgrade cycles

### STS

- Shorter support
- Better for faster upgrade cycles
- Gives access to newer platform features sooner

---

# Support and Patches

Both **LTS** and **STS** releases receive important updates during their support lifetime.

These updates include:

- 🔒 **Security patches**
- 🛠️ **Reliability fixes**
- 🐞 **Critical bug fixes**

---

## Staying Supported Requires Staying Patched

To receive support, you must keep your .NET installation up to date with the latest patch version.

For example:

| Installed Version | Latest Version | Supported? |
|---|---|---|
| `.NET Runtime 10.0.0` | `.NET Runtime 10.0.1` | ❌ No, update required |
| `.NET Runtime 10.0.1` | `.NET Runtime 10.0.1` | ✅ Yes |

If your system is running:

```text
.NET Runtime 10.0.0
```

and Microsoft releases:

```text
.NET Runtime 10.0.1
```

you must install version `10.0.1` to remain supported.

---

# Patch Tuesday

.NET updates are usually released on the **second Tuesday of each month**.

This is commonly known as:

> **Patch Tuesday**

Patch Tuesday updates often include:

- Security fixes
- Runtime updates
- SDK updates
- ASP.NET Core fixes
- Reliability improvements

---

# Choosing Between LTS and STS

## Choose LTS If...

Use **LTS** when your priority is long-term support.

### Good examples

- Business-critical applications
- Enterprise systems
- Applications with slower release cycles
- Systems that are expensive or risky to upgrade frequently
- Long-running backend services

```text
Choose LTS when support duration matters most.
```

---

## Choose STS If...

Use **STS** when your priority is access to newer features.

### Good examples

- Applications that are upgraded regularly
- Teams comfortable with faster release cycles
- Projects that benefit from new framework features
- Development teams that want to stay near the latest .NET version

```text
Choose STS when newer features matter more than longer support.
```

---

## Choose Preview If...

Use **Preview** only when you intentionally want to test upcoming functionality.

### Good examples

- Experimenting with new C# features
- Testing compatibility before a future .NET release
- Preparing libraries for upcoming .NET versions
- Learning future platform features early

```text
Avoid Preview for normal production apps unless it is marked Go Live.
```

---

# Support Duration Visualization

The support periods can be imagined as bars.

- **LTS releases** have longer support windows.
- **STS releases** have shorter support windows.

```text
LTS Release:  ████████████████████████████████  about 3 years
STS Release:  █████████████████████             about 2 years
Preview:      ███                               temporary/testing period
```

---

# Example Timeline

```text
Year 1          Year 2          Year 3          Year 4
|---------------|---------------|---------------|---------------|

LTS:  ████████████████████████████████
STS:      █████████████████████
LTS:                  ████████████████████████████████
STS:                      █████████████████████
```

This shows the general idea:

- LTS releases remain supported for a longer time.
- STS releases are supported for a shorter but still production-ready period.
- New releases overlap with older supported releases, giving teams time to upgrade.

---

# Key Terms

| Term | Meaning |
|---|---|
| **LTS** | Long-Term Support |
| **STS** | Standard-Term Support |
| **GA** | General Availability; the official stable release |
| **Preview** | Early test release before final release |
| **RC** | Release Candidate; near-final test release |
| **Go Live** | Approved by Microsoft for production use |
| **Patch Tuesday** | Monthly update release day, usually the second Tuesday |
| **Runtime** | The component required to run .NET applications |
| **SDK** | Tools needed to build and develop .NET applications |

---

# Practical Rule

For most production applications:

```text
Use LTS if you want longer support.
Use STS if you want newer features sooner.
Use Preview only for testing or early access.
```

And always remember:

```text
A supported .NET version must also be patched to the latest update.
```
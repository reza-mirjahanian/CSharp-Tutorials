# 🚀 File-Based Apps in C# 14

C# 14 introduces **file-based apps**, a streamlined feature allowing developers to execute a single `.cs` file directly without the overhead of a standard project structure (`.csproj`). This shifts C# toward a more script-friendly language, similar to Python or JavaScript.

### 📊 Comparison: Project vs. File-Based

| Feature | Project-Based App | File-Based App |
| :--- | :--- | :--- |
| **Structure** | Requires `.csproj`, `bin/`, and `obj/` folders. | A single `.cs` file. |
| **Setup** | High boilerplate. | Zero boilerplate (Top-level statements). |
| **Tooling** | Full Visual Studio & JetBrains Rider support. | Command-line (CLI) only. |
| **Best For** | Production-grade applications. | Scripting, prototyping, and learning. |

---

### 🛠️ Creating and Running Your First File-Based App

Because this is a CLI-centric feature, it is currently **not supported** inside the standard Visual Studio IDE.

1.  **Prepare the Environment**: 
    *   Create a directory (e.g., `NoProject`).
2.  **Create the Script**: 
    *   Inside the folder, create a file named `hello.cs`.
    *   Add the following code:
    ```csharp
    Console.WriteLine("Hello World with no project file!");
    ```
3.  **Execute via Terminal**:
    *   Run the file using the `dotnet` command:
    `dotnet run hello.cs`
    > 💡 **Tip:** You can actually omit the `run` keyword and simply execute `dotnet hello.cs`.

---

### ⚙️ Configuring Scripts with Directives

To handle dependencies or compiler settings without a project file, C# 14 uses special `#: directives` at the very top of the file.

*   **📦 Adding NuGet Packages**
    Use the `#:package` directive followed by the package name and version.
    ```csharp
    #:package Humanizer@2.14.1
    using Humanizer;

    Console.WriteLine(TimeSpan.FromDays(1).Humanize());
    ```

*   **🔗 Adding Project References**
    Link to existing libraries or projects.
    *   `#:project ../MyClassLib/MyClassLib.csproj`

*   **🛠️ Specifying the SDK**
    Switch between different .NET SDKs, such as the Web SDK for ASP.NET Core.
    *   Standard: `#:sdk Microsoft.NET.Sdk.Web`
    *   Versioned: `#:sdk Aspire.AppHost.Sdk@9.5.0`

*   **🔧 Setting MSBuild Properties**
    Configure compiler settings, such as using the preview language version.
    *   `#:property LangVersion=preview`

*   **🐧 Linux Shebang Support**
    For Unix-based systems, you can make the script self-executable by adding a "shebang" at the first line:
    ```csharp
    #!/usr/bin/dotnet run
    Console.WriteLine("Executing as a script!");
    ```
    *Note: You must grant the file execution permissions via `chmod +x hello.cs` to run it as `./hello.cs`.*

---

### 🔄 Advanced Operations

As your script grows in complexity, you may need to transition into a formal application or distribute it.

#### 1. Converting to a Full Project
If your prototype needs to become a production app, use the conversion command:
`dotnet project convert app.cs`

#### 2. Publishing
You can compile your single file into a distributable binary. By default, file-based apps are published as **Native AOT** (Ahead-of-Time) compiled apps.
`dotnet publish app.cs`

> [!IMPORTANT]
> **Current Limitations**
> *   **Single File Only:** Currently, you can only use one `.cs` file per app. 
> *   **Future Updates:** Multi-file support is planned for **.NET 11**.
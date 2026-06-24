# **Index**
1. **Introduction to Blazor**  
2. **How Traditional Web Apps Work**  
   2.1 Request–Response Model  
   2.2 Navigation Between Pages  
3. **How Blazor Works**  
   3.1 Single Root Component  
   3.2 Component-Based Architecture  
   3.3 Component Tree & Hierarchy  
   3.4 Interactivity with C#  
4. **Comparison: Traditional MVC vs Blazor SPA**  
5. **Visual Model of Component Behavior**  
6. **Key Terms**

---

# **1. Introduction to Blazor**
Blazor is a **component-based Single Page Application (SPA) framework** that enables developers to build rich, interactive web applications using **C# instead of JavaScript**.

It shifts the mindset from *page-based development* to *component-based development*, allowing you to construct modern web interfaces in a modular and maintainable way.

---

# **2. How Traditional Web Apps Work**

## **2.1 Request–Response Model**
Traditional frameworks like **ASP.NET Core MVC** or **Razor Pages** operate using a structured flow:

- The browser sends a **request** for a page.  
- The server **routes** the request to a specific page.  
- That page generates **HTML** and sends it back as a **response**.  
- The browser **renders** the returned HTML.  

Each page exists as a separate endpoint on the server.

### Example Flow
1. User requests `/Catalog`  
2. Server returns the HTML for the Catalog page  
3. Browser displays it  
4. User clicks "Product Details"  
5. New request sent → Server returns a different page  

Every move is a round-trip with a new page load.

---

## **2.2 Navigation Between Pages**
Even though pages may look connected, each one is:

- uniquely routed  
- independently generated  
- fully refreshed in the browser  

This is typical for multi-page applications.

---

# **3. How Blazor Works**

## **3.1 Single Root Component**
In Blazor, the entire application initially loads a **single root component** (the "root page"). All navigation happens *inside* this root.

Instead of loading new HTML pages from the server, Blazor **dynamically changes components** inside this root area.

---

## **3.2 Component-Based Architecture**
A *component* in Blazor is a reusable UI element that may contain:

- markup  
- C# logic  
- styling  
- nested child components  

Each UI “screen” is simply a component, not a page.

---

## **3.3 Component Tree & Hierarchy**
Components can contain other components, forming a **multi-level tree**.

Example structure:

- Root Component  
  - Navigation Component  
  - Layout Component  
    - Content Area  
      - Catalog Component  
      - Product Details Component  
      - Profile Component  
      - etc.  

Blazor swaps components in/out of this tree rather than loading new pages.

---

## **3.4 Interactivity with C#**
Unlike JavaScript frameworks such as Angular or Vue, Blazor handles interactivity using **C#**.

Examples of interactive features handled with C#:

- button clicks  
- form updates  
- state changes  
- real-time UI refresh  

Blazor automatically updates only the parts of the page that change — similar to how modern JS frameworks work, but powered by .NET.

---

# **4. Comparison: Traditional MVC vs Blazor SPA**

| Feature | Traditional MVC / Razor Pages | Blazor SPA |
|--------|-------------------------------|------------|
| Interaction model | Page-based | Component-based |
| Navigation | Full page reloads | Component swaps inside root |
| Code-behind | C# (server only) | C# (client or server) |
| Interactivity | Mostly server-driven | Realtime C# interactivity |
| User experience | Slower, page refreshes | Fast, app-like behavior |

---

# **5. Visual Model of Component Behavior**

### 🧩 Traditional Model (Pages)
[ Browser ]
   |
   v
[ Request Page A ]  →  [ HTML for Page A ]
   |
   v
[ Request Page B ]  →  [ HTML for Page B ]


Each change requires a round-trip and new HTML.

---

### ⚡ Blazor Model (Components)
[ Root Component ]
      |
      |-- Loads Component_A
      |-- Replaces with Component_B
      |-- Replaces with Component_C


From the user's perspective, it *feels* like navigating pages, but internally, Blazor is injecting components dynamically.

---

# **6. Key Terms**

- **Blazor** – C# SPA framework.  
- **SPA (Single Page Application)** – App that loads once and updates dynamically.  
- **Component** – Reusable UI + logic unit in Blazor.  
- **Root Component** – Main entry component of a Blazor app.  
- **Component Tree** – Hierarchical structure of components.  
- **Interop** – Interaction between C# and JavaScript (when needed).  
- **MVC** – Traditional server-driven architecture (Model–View–Controller).  

---


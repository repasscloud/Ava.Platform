---
title: "Razor Directive Order in Blazor MAUI"
description: "Best-practice order for directives in Blazor MAUI hybrid components for clarity and maintainability."
summary: "A consistent Razor directive order makes your components cleaner, reduces bugs, and improves readability—especially in hybrid projects like Blazor MAUI."
date: 2025-04-22T23:00:00+10:00
lastmod: 2025-04-22T23:00:00+10:00
draft: false
weight: 905
toc: true
seo:
  title: "Recommended Razor Directive Order for Blazor MAUI Hybrid Apps"
  description: "Follow this clean and consistent order for Razor directives in Blazor MAUI hybrid apps. Improve readability and reduce surprises."
  canonical: "" # add canonical URL if cross-posted
  robots: "index,follow"
---

When working in Blazor MAUI hybrid apps, a consistent structure for your `.razor` directives goes a long way. Razor doesn't enforce a strict order, but your future self (and teammates) will thank you.

This is the recommended order.

## 📐 Full Directive Order

```razor
@page "/route"                    // Optional, only for routable components
@layout MainLayout                // Optional, overrides the default layout
@attribute [Authorize]            // Optional metadata like [Authorize]
@namespace AvaTerminal2.Pages     // Rare, usually unnecessary
@using Namespace                  // System then custom, grouped and ordered
@inject Type Name                 // Services and utilities
@implements InterfaceName         // IDisposable etc.
@inherits BaseClass               // Base Razor page class (e.g. SecurePageBase)
@typeparam T                      // Generic type parameters
```

## 🧪 Example (Clean)

```razor
@page "/client-management"
@layout MainLayout
@attribute [Authorize]
@namespace AvaTerminal2.Pages.Clients

@using System.ComponentModel.DataAnnotations
@using AvaTerminal2.Models.Dto
@using AvaTerminal2.Services

@inject IJSRuntime JS
@inject IClientService ClientService

@implements IDisposable
@inherits SecurePageBase

@typeparam TItem
```

## 💡 Notes

- `@page` only applies to components meant to be routed to directly.
- `@layout` is used to override the default layout for a specific component.
- `@attribute [Authorize]` is useful when securing Razor Pages with `[Authorize]` attributes.
- `@namespace` is rarely needed unless you're dynamically generating code or working in large multi-tenant setups.
- `@inject` should be grouped together and placed logically after `@using`.
- `@implements` comes before `@inherits`, following C# class conventions.
- `@typeparam` goes last, especially for generic components.

## ⚙️ Optional Snippet Automation

If you want to set up a VS Code snippet or Resharper template with this structure, ping me and I’ll help drop one into your tool of choice.

A little structure in your Razor pages pays off when scanning 50+ files at 2am looking for why the `[Authorize]` didn't apply.

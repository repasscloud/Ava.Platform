---
title: "OpenStreetMap Contribution"
description: "<div> vs <MudText> for proper OSM attribution"
summary: "How to correctly display attribution when using OpenStreetMap tiles in a Blazor project, and when to use <div> or <MudText>."
date: 2023-09-07T16:13:18+02:00
lastmod: 2023-09-07T16:13:18+02:00
draft: false
weight: 910
toc: true
seo:
  title: "OpenStreetMap Attribution in Blazor"
  description: "This post explains how to attribute OpenStreetMap correctly when embedding map tiles, and the differences between using <div> and <MudText> in Blazor components."
  canonical: ""
  robots: ""
---

When using OpenStreetMap data in your web application — especially when embedding maps with Leaflet.js or custom components like `RealTimeMap` — you are **legally required** to show attribution in compliance with the [OpenStreetMap Foundation's license](https://osmfoundation.org/wiki/Licence/Attribution_Guidelines).

Attribution should be:

- **Human-readable**
- **Clearly visible** whenever the map is shown
- **Linked** to the official copyright page

---

## 🧾 Which element to use? `<div>` vs `<MudText>`

In Blazor (especially with MudBlazor), you have two practical choices for adding attribution markup:

---

### ✅ Option 1: Raw `<div>` with inline styling

Use this if you want full control over styling or if you're placing attribution in a corner of a tile or overlay.

```html
<div style="font-size: 11px; text-align: right; margin-top: 4px;">
    © <a href="https://www.openstreetmap.org/copyright" target="_blank" rel="noopener noreferrer">
        OpenStreetMap contributors
    </a>
</div>
```

- Great for fine-tuned UI layout
- Doesn't rely on Material styling
- No MudBlazor dependencies


### Option 2: `<MudText>` for consistent typography

Use this if you want your attribution text to blend in with the rest of your app's styling and typography rules.

```razor
<MudText Typo="Typo.caption" Class="mt-1 d-block" Style="font-size: 11px; text-align: right;">
    © <a href="https://www.openstreetmap.org/copyright" target="_blank" rel="noopener noreferrer">
        OpenStreetMap contributors
    </a>
</MudText>
```

- Matches `Typo.caption` styling from MudBlazor
- Automatically adapts to dark/light themes
- Cleaner if your app already uses MudText everywhere

### ✅ Which should you use?

| Use Case | Recommended Tag |
|:---------|:----------------|
| You need pixel-perfect layout | `<div>` |
| You want design consistency | `<MudText>` |
| You're using raw HTML output | `<div>` |
| You're building with MudBlazor | `<MudText>` |

Either way, what matters most is that the attribution is visible and clickable while your map is shown.

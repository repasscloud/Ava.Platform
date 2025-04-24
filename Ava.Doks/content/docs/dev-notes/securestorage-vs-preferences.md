---
title: "SecureStorage vs Preferences in .NET MAUI Blazor Hybrid Apps"
description: "Understanding the difference between SecureStorage and Preferences for persisting app settings in a .NET MAUI Blazor Hybrid app on macOS and Windows."
summary: "Use SecureStorage for secrets on Windows, fallback to Preferences on macOS during development."
date: 2025-04-22T21:16:00+10:00
lastmod: 2025-04-22T21:16:00+10:00
# tags: [dotnet, maui, blazor, macos, windows, dev-tips]
draft: false
weight: 920
toc: true
seo:
  title: "SecureStorage vs Preferences in .NET MAUI"
  description: "How to handle secure and non-secure settings storage in .NET MAUI Blazor Hybrid apps targeting Windows and macOS."
  canonical: ""
  robots: "index,follow"
---

## 💡 Overview

In .NET MAUI Blazor Hybrid apps, persisting user tokens or settings securely can differ based on platform.

- ✅ Windows: `SecureStorage` works out-of-the-box
- ⚠️ macOS: `SecureStorage` requires entitlements and a valid developer identity — which you don't get without a paid Apple Developer account

To work around this during development, you can conditionally switch to `Preferences` on macOS, and still use `SecureStorage` on Windows (or mobile).

## 🔐 SecureStorage (Encrypted, Platform-secure)

Use for sensitive information such as:

- Access tokens
- API keys
- Credentials

```csharp
await SecureStorage.Default.SetAsync("token", "super-secret-token");
var token = await SecureStorage.Default.GetAsync("token");
```

> ✅ Works on Windows, Android, iOS  
> ⚠️ Requires entitlements on macOS — otherwise throws

## ⚙️ Preferences (Plaintext, Simple Key/Value)

Fallback for non-sensitive or dev-only data:

```csharp
Preferences.Set("token", "test-token");
var token = Preferences.Get("token", null);
```

> ✅ No entitlements needed  
> ❌ Not encrypted — don’t use for real secrets

## 🧪 Hybrid Strategy (Platform Conditional)

```csharp
if (OperatingSystem.IsMacOS())
{
    Preferences.Set("token", "value");
}
else
{
    await SecureStorage.Default.SetAsync("token", "value");
}
```

This allows you to:

- Develop and debug on macOS without entitlement issues
- Ship secure behavior on Windows or mobile

## 🔥 Pro Tip: Use Abstractions

Wrap your logic in a service:

```csharp
public interface IKeyValueStore
{
    Task SetAsync(string key, string value);
    Task<string?> GetAsync(string key);
    void Remove(string key);
}
```

And create a concrete class with platform-specific fallbacks. That way, your app logic stays clean.

## 👀 What About File-based Encryption?

You can store encrypted blobs to disk using `FileSystem.AppDataDirectory` and AES encryption — but it’s more involved. Use this if you need to store structured data and still control security manually.

## 🧭 Summary

| Feature | SecureStorage | Preferences |
|:--------|:--------------|:------------|
| Encrypted | ✅ Yes | ❌ No |
| Platform Support | Windows/macOS*/Mobile | All platforms |
| Needs Entitlements | ✅ Yes (macOS) | ❌ No |
| Ideal For | Secrets, Tokens | Settings, Debug Flags |

Use `SecureStorage` where possible — fallback to `Preferences` for macOS development if you’re not paying Apple yet. Just don’t ship secrets in plaintext. 💥

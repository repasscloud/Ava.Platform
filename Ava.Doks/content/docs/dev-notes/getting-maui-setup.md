

## Project Creation

```bash
dotnet new maui -n ProjectName
```

- Successfully created a new .NET MAUI project called `ProjectName`.
- Project directory and `.csproj` file were generated as expected.

### Post-Creation Restore Attempt Fails

```bash
Restoring ... ProjectName.csproj:
error NETSDK1147: To build this project, the following workloads must be installed: macos
```

- This error indicates that MAUI’s macOS workload was not yet installed.
- It suggests running:

```bash
dotnet workoad restore
```

### Restore Fails Due to Permissions

Fails with:

```bash
Inadequate permissions. Run the command with elevated privileges.
```

Because installing workloads affects system-wide SDK paths, it requires **admin rights**.

### Successful Restore with `sudo`

```bash
sudo dotnet workload restore
```

- You successfully ran the restore with elevated permissions
- Workloads were installed, including:
  - `microsoft.net.sdk.macos`
  - `microsoft.net.sdk.maui`
  - `microsoft.net.sdk.ios`, `android`, `tvos`, `maccatalyst`
- `.NET CLI` also displayed telemetry info and certificate installation notices

### Minor Warning at the End

```bash
Warning: Workload garbage collection failed...
```

- A warning was raised because a previously installed workload path was no longer found. This can happen when switching SDK versions.
- Not critical and can be ignored unless it persists.

## Your `dotnet run` Failed Due to Missing Dependencies for MAUI Platforms

This is a MAUI multi-platform build issue — and multiple targets failed:

### Android Failure

```bash
error XA5207: Could not find android.jar for API level 35
```

- Your app targets Android API level 35, but the SDK for that level isn't installed
- It's expected at:

  ```bash
  ~/Library/Developer/Xamarin/android-sdk-macosx/platforms/android-35/android.jar
  ```

With the java sdk version set to v17, i tried to build again:

dotnet build -t:InstallAndroidDependencies \
  -f net9.0-android \
  -p:AndroidSdkDirectory=$HOME/Library/Developer/Xamarin/android-sdk-macosx

Error:
  AvaTerminal.Maui net9.0-android failed with 1 error(s) (1.5s)
    /usr/local/share/dotnet/packs/Microsoft.Android.Sdk.Darwin/35.0.61/tools/Xamarin.Installer.Common.targets(19,3): error : The Android SDK license agreements were not accepted, please set `$(AcceptAndroidSDKLicenses)` to accept.


The Android SDK license agreements were not accepted. When installing Android SDK componenets manually for via `dotnet`, you must explicicty accept the SDK licenses, or hte installer will bail.

## Accept Android SDK Licenses

Run the command again with this additional parameter:

```bash
dotnet build -t:InstallAndroidDependencies \
  -f net9.0-android \
  -p:AndroidSdkDirectory=$HOME/Library/Developer/Xamarin/android-sdk-macosx \
  -p:AcceptAndroidSDKLicenses=true
```

This tells MSBuild to auto-accept the Android SDK license terms.

Once it completes successfully, you'll be able to:

```bash
dotnet build
dotnet run
```

## Xcode Is Outdated

```bash
dotnet build                                                                                                                                                                          ─╯
Restore complete (0.4s)
  AvaTerminal.Maui net9.0-ios failed with 4 error(s) and 1 warning(s) (0.9s) → bin/Debug/net9.0-ios/iossimulator-arm64/AvaTerminal.Maui.dll
    ILLINK : warning MT0079: The recommended Xcode version for Microsoft.iOS 18.4.9288 is Xcode 16.3 or later. The current Xcode version (found in /Applications/Xcode.app/Contents/Developer) is 16.2.
    ILLink : unknown error IL7000: An error occurred while executing the custom linker steps. Please review the build log for more information.
    ILLINK : error MT0180: This version of Microsoft.iOS requires the iOS 18.4 SDK (shipped with Xcode 16.3). Either upgrade Xcode to get the required header files or set the managed linker behaviour to Link Framework SDKs Only in your project's iOS Build Options > Linker Behavior (to try to avoid the new APIs).
    ILLINK : error MT2301: The linker step 'Setup' failed during processing: This version of Microsoft.iOS requires the iOS 18.4 SDK (shipped with Xcode 16.3). Either upgrade Xcode to get the required header files or set the managed linker behaviour to Link Framework SDKs Only in your project's iOS Build Options > Linker Behavior (to try to avoid the new APIs).
    /Users/danijeljw/.nuget/packages/microsoft.net.illink.tasks/9.0.3/build/Microsoft.NET.ILLink.targets(96,5): error NETSDK1144: Optimizing assemblies for size failed.
  AvaTerminal.Maui net9.0-maccatalyst failed with 4 error(s) and 1 warning(s) (0.9s) → bin/Debug/net9.0-maccatalyst/maccatalyst-arm64/AvaTerminal.Maui.dll
    ILLINK : warning MT0079: The recommended Xcode version for Microsoft.MacCatalyst 18.4.9288 is Xcode 16.3 or later. The current Xcode version (found in /Applications/Xcode.app/Contents/Developer) is 16.2.
    ILLink : unknown error IL7000: An error occurred while executing the custom linker steps. Please review the build log for more information.
    ILLINK : error MT0180: This version of Microsoft.MacCatalyst requires the MacCatalyst 18.4 SDK (shipped with Xcode 16.3). Either upgrade Xcode to get the required header files or set the managed linker behaviour to Link Framework SDKs Only in your project's iOS Build Options > Linker Behavior (to try to avoid the new APIs).
    ILLINK : error MT2301: The linker step 'Setup' failed during processing: This version of Microsoft.MacCatalyst requires the MacCatalyst 18.4 SDK (shipped with Xcode 16.3). Either upgrade Xcode to get the required header files or set the managed linker behaviour to Link Framework SDKs Only in your project's iOS Build Options > Linker Behavior (to try to avoid the new APIs).
    /Users/danijeljw/.nuget/packages/microsoft.net.illink.tasks/9.0.3/build/Microsoft.NET.ILLink.targets(96,5): error NETSDK1144: Optimizing assemblies for size failed.
```

## Use `mas` for Latest Stable

1. Install `mas` (Mac App Store CLI)

  ```bash
  brew install mas
  ```

1. Sign into Mac App Store (once)

  ```bash
  mas account
  ```

  If you're not signed in, open App Store → SIgn In.

1. Install or Update Xcode

  ```bash
  mas install 497799835
  # OR, if already installed:
  mas upgrade 497799835
  ```

  App ID `497799835` is Xcode in the Mac App Store.

1. Accept Xcode License

  ```bash
  sudo xcodebuild -license accept
  ```

1. Confirm Installation

  ```bash
  xcode-select -p
  # → should be /Applications/Xcode.app/Contents/Developer

  xcodebuild -version
  # → should say Xcode 16.3
                 Build version 16E140
  ```

## Run xcodebuild -runFirstLaunch

In your terminal:

```bash
sudo xcodebuild -runFirstLaunch
```

This will:

- Install additional required components
- Accept Xcode license if not already done
- Configure internal plugins (like `actool`) for iOS/MacCatalyst

## Accept License (if prompted or skipped)

```bash
sudo xcodebuild -license accept
```

## iOS Build Error Details

```bash
actool error : No simulator runtime version from ["21A328", "21C62", "22A3351", "22D8075"] available to use with iphonesimulator SDK version 22E235
```

What this means:

- Your current iPhone simulator runtimes (like iOS 16.0, 16.2) are too old for the SDK that came with Xcode 16.3 (which expects iOS 17.4 simulator or similar).
- MAUI is trying to build for a simulator that doesn’t exist yet on your system.

### Fix iOS Simulator Error

#### Option A: Install the Latest iOS Simulator from Xcode

Open Xcode → Settings → Platforms

Under Simulator Runtimes, install:

  - iOS 17.4 Simulator
  - watchOS / tvOS only if needed (not required for MAUI)

Once installed, rebuild:

```bash
dotnet build -f net9.0-ios
```

#### Option B: Avoid iOS for Now

If you're not testing iOS yet, focus on:

```bash
dotnet run -f net9.0-android
```

```bash
dotnet build                                                                                                                                                                          ─╯
Restore complete (0.5s)
  AvaTerminal.Maui net9.0-android succeeded (0.9s) → bin/Debug/net9.0-android/AvaTerminal.Maui.dll
  AvaTerminal.Maui net9.0-ios failed with 4 error(s) (2.4s)
    /usr/local/share/dotnet/packs/Microsoft.iOS.Sdk.net9.0_18.4/18.4.9288/tools/msbuild/Xamarin.Shared.targets(977,3): error :
      /usr/bin/xcrun exited with code 134:
      A required plugin failed to load. Please ensure system content is up-to-date — try running 'xcodebuild -runFirstLaunch'.
    /Users/danijeljw/Developer/dotnet-dev/Ava.Development/AvaPlatform/AvaTerminal.Maui/obj/Debug/net9.0-ios/iossimulator-arm64/actool/cloned-assets/Assets.xcassets : error :

      A required plugin failed to load. Please ensure system content is up-to-date — try running 'xcodebuild -runFirstLaunch'.

    /usr/local/share/dotnet/packs/Microsoft.iOS.Sdk.net9.0_18.4/18.4.9288/tools/msbuild/Xamarin.Shared.targets(977,3): error :
      actool exited with code 134

    /usr/local/share/dotnet/packs/Microsoft.iOS.Sdk.net9.0_18.4/18.4.9288/tools/msbuild/Xamarin.Shared.targets(977,3): error :
      Failed to load actool log file `obj/Debug/net9.0-ios/iossimulator-arm64/actool/asset-manifest.plist`: Failed to parse PList data type:

  AvaTerminal.Maui net9.0-maccatalyst failed with 4 error(s) (2.4s)
    /usr/local/share/dotnet/packs/Microsoft.MacCatalyst.Sdk.net9.0_18.4/18.4.9288/tools/msbuild/Xamarin.Shared.targets(977,3): error :
      /usr/bin/xcrun exited with code 134:
      A required plugin failed to load. Please ensure system content is up-to-date — try running 'xcodebuild -runFirstLaunch'.
    /Users/danijeljw/Developer/dotnet-dev/Ava.Development/AvaPlatform/AvaTerminal.Maui/obj/Debug/net9.0-maccatalyst/maccatalyst-arm64/actool/cloned-assets/Assets.xcassets : error :

      A required plugin failed to load. Please ensure system content is up-to-date — try running 'xcodebuild -runFirstLaunch'.

    /usr/local/share/dotnet/packs/Microsoft.MacCatalyst.Sdk.net9.0_18.4/18.4.9288/tools/msbuild/Xamarin.Shared.targets(977,3): error :
      actool exited with code 134

    /usr/local/share/dotnet/packs/Microsoft.MacCatalyst.Sdk.net9.0_18.4/18.4.9288/tools/msbuild/Xamarin.Shared.targets(977,3): error :
      Failed to load actool log file `obj/Debug/net9.0-maccatalyst/maccatalyst-arm64/actool/asset-manifest.plist`: Failed to parse PList data type:


Build failed with 8 error(s) in 3.4s
```
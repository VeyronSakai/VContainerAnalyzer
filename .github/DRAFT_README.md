# VContainerAnalyzer

[![Test](https://github.com/VeyronSakai/VContainerAnalyzer/actions/workflows/test.yml/badge.svg)](https://github.com/VeyronSakai/VContainerAnalyzer/actions/workflows/test.yml)
[![NuGet](https://img.shields.io/nuget/v/VContainerAnalyzer.svg)](https://www.nuget.org/packages/VContainerAnalyzer/)

Roslyn Analyzer for detecting coding errors in Unity projects
using [VContainer](https://github.com/hadashiA/VContainer).

This is in a very early stage of development. Please use with caution.

Also note that ECS is not yet supported.

## Setup

### Unity Package Manager

VContainerAnalyzer can be set up via Unity Package Manager by following the steps below.

1. Open the Package Manager in the UnityEditor.
2. Select the + button in the upper left corner.
3. Select `Add package from git URL`.
4. Enter
   `https://github.com/VeyronSakai/VContainerAnalyzer.git?path=VContainerAnalyzer.Unity/Assets/Plugins/VContainerAnalyzer#0.4.0`
   and Select `Add` button.
5. Add `VContainer.Analyzer` to the Assembly Definition References of the asmdef to which you want to apply Analyzer.

### .unitypackage

VContainerAnalyzer can be set up by using the .unitypackage available
at [Releases](https://github.com/VeyronSakai/VContainerAnalyzer/releases/latest).

## Rules

### VContainer0001

| Topic    | Value          |
|:---------|:---------------|
| Id       | VContainer0001 |
| Severity | Error          |
| Enabled  | True           |
| Category | Usage          |

An error occurs if the constructor of a class registered by the `Register()` or `RegisterEntryPoint()`
or `UseEntryPoints()` methods in the Lifetime class does not have an Attribute that extends `PreserveAttribute` such
as `InjectAttribute`.

See the [VContainer documentation](https://vcontainer.hadashikick.jp/resolving/constructor-injection) for more
information.

### VContainer0002

| Topic    | Value          |
|:---------|:---------------|
| Id       | VContainer0002 |
| Severity | Warning        |
| Enabled  | True           |
| Category | Usage          |

Warning occurs when using Property/Field Injection.

```csharp
public class FieldInjectionSandbox
{
    [Inject] private IService _field;
}

public class PropertyInjectionSandbox
{
    [Inject] private IService Property { get; set; }
}
```

See the [VContainer documentation](https://vcontainer.hadashikick.jp/resolving/constructor-injection) for more
information.

## Contribution

Bugs and new feature suggestions are welcome in issues and Pull Requests.

If you have made changes to the Analyzer itself, please run the `make all` command and commit the differences in the dll
files generated and include them in the pull request.

---
Analyzer based on the [Roslyn Analyzer Template][template].

[template]: https://github.com/DeNA/RoslynAnalyzerTemplate

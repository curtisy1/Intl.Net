# Localizati18n
A ResX Globalization alternative using simple JSON Key-Value-Pairs instead of bulky XML.

Generates strongly-typed resource classes for looking up localized strings.

## Usage

Install the [`Localizati18n.ResourceGenerator`](https://www.nuget.org/packages/Localizati18n.ResourceGenerator/) package in your resource project:

```psl
dotnet add package Localizati18n.ResourceManager
```

Make sure to copy your JSON resource files to your output directory

If you want to use a custom namespace for your resources, add a `CustomToolNamespace` tag to your embedded resource
i.e.
```xml
<EmbeddedResource Include="Localization.json">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  <CustomToolNamespace>Localization</CustomToolNamespace>
</EmbeddedResource>
```

By default, source generators will not persist the generated files to disk. In many cases, it's desirable to have the generated source in version control though.

Luckily, there's another tag you can use. Simply add `<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>` to your `PropertyGroup`
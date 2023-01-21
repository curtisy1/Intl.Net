# Intl.Net
A ResX Globalization alternative using JSON instead of bulky XML.

Generates strongly-typed resource classes for looking up localized strings.

## Usage

Install the [`Intl.Net.ResourceGenerator`](https://www.nuget.org/packages/Intl.Net.ResourceGenerator/) and [`Intl.Net.ResourceManager`](https://www.nuget.org/packages/Intl.Net.ResourceManager/) packages in your resource project:

Make sure to copy your JSON resource files to your output directory and mark them as EmbeddedResource
```xml
<EmbeddedResource Include="Localization.json">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</EmbeddedResource>
```

For a list of configurable options, as well as supported i18n providers that fetch your translations from a remote API, please see the wiki pages.

## Performance
Currently, Intl.Net is just as fast as the Resx ResourceManager shipped with dotnet itself.
In cases where trying to get a non-existing key, it is even significantly faster.

For details and the benchmarks run, check the benchmarks folder.

## Related libraries
If this library looks amazing, please do check out these similar projects and give them a star!

[TypealizR](https://github.com/earloc/TypealizR) - A source generator doing the same thing for .resx files. It's pretty much a modern Globalization alternative
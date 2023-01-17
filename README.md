# Intl.Net
A ResX Globalization alternative using simple JSON Key-Value-Pairs instead of bulky XML.

Generates strongly-typed resource classes for looking up localized strings.

## Usage

Install the [`Intl.Net.ResourceGenerator`](https://www.nuget.org/packages/Intl.Net.ResourceGenerator/) and [`Intl.Net.ResourceManager`](https://www.nuget.org/packages/Intl.Net.ResourceManager/) packages in your resource project:

Make sure to copy your JSON resource files to your output directory and mark them as EmbeddedResource

If you want to use a custom namespace for your resources, add a `CustomToolNamespace` tag to your embedded resource
i.e.
```xml
<EmbeddedResource Include="Localization.json">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  <CustomToolNamespace>Localization</CustomToolNamespace>
</EmbeddedResource>
```

By default, source generators will not persist the generated files to disk. In many cases, it's desirable to have the generated source in version control though.
I also found that Intellisense doesn't work without it, so please do copy those files if you encounter any issues

Luckily, there's another tag you can use. Simply add `<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>` to your `PropertyGroup`

You can then add a `<CompilerGeneratedFilesOutputPath></CompilerGeneratedFilesOutputPath>` if desired and copy the resource file after build.
```xml
<!--    This is important because the compiler would complain about the duplicate file otherwise -->
<Target Name="RemovePreviouslyGeneratedFile" BeforeTargets="BeforeCompile">
    <Delete Files="Localization.cs" ContinueOnError="true" />
</Target>

<Target Name="CopyGeneratedFile" AfterTargets="AfterBuild">
    <Copy SourceFiles="Generated\Intl.Net.ResourceGenerator\Intl.Net.ResourceGenerator.SourceGenerator\Localization.cs" DestinationFolder="$(ProjectDir)" />
    <RemoveDir Directories="Generated" />
</Target>
```

Note that for some reason this does not work on rebuild. If you figure out why, please submit a PR to the example project
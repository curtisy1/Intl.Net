using System.Text.Json;

namespace Intl.Net.ResourceGenerator {
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using ResourceManager;
  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;
  using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

  public sealed record GeneratorOptions(string Namespace, string ClassName);

  public sealed class Generator : IDisposable {
    private const string localizationNamespace = "System.Globalization";
    private const string resourceNamespace = "Intl.Net.ResourceManager";
    private const string privateModifier = "s_";
    private const string resourceManagerVariable = "ResourceManager";
    private const string cultureInfoVariable = "ResourceCulture";

    private readonly Stream resourceStream;
    private readonly GeneratorOptions options;

    public Generator(Stream resourceStream, GeneratorOptions options) {
      this.resourceStream = resourceStream;
      this.options = options;
    }

    private void Dispose(bool disposing) {
      if (disposing) {
        this.resourceStream.Dispose();
      }
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~Generator() => this.Dispose(false);

    private NamespaceDeclarationSyntax CreateNamespace() =>
      NamespaceDeclaration(ParseName(this.options.Namespace))
        .AddUsings(UsingDirective(IdentifierName(localizationNamespace)),
                   UsingDirective(IdentifierName(resourceNamespace)));

    private ClassDeclarationSyntax CreateClass() =>
      ClassDeclaration(this.options.ClassName)
        .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
        .AddMembers(FieldDeclaration(VariableDeclaration(NullableType(IdentifierName(nameof(JsonResourceManager)))).AddVariables(VariableDeclarator(privateModifier + resourceManagerVariable)))
                      .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword)),
                    PropertyDeclaration(IdentifierName(nameof(JsonResourceManager)), resourceManagerVariable)
                      .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                      .WithExpressionBody(ArrowExpressionClause(AssignmentExpression(SyntaxKind.CoalesceAssignmentExpression,
                                                                                     IdentifierName(privateModifier + resourceManagerVariable),
                                                                                     ObjectCreationExpression(IdentifierName(nameof(JsonResourceManager)))
                                                                                       .AddArgumentListArguments(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal($"{this.options.Namespace}.{this.options.ClassName}"))),
                                                                                                                 Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, TypeOfExpression(IdentifierName(this.options.ClassName)), IdentifierName(nameof(Type.Assembly)))),
                                                                                         Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(this.options.ClassName)))))))
                      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                    PropertyDeclaration(NullableType(IdentifierName(nameof(CultureInfo))), cultureInfoVariable)
                      .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                      .AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken))));

    private static MemberDeclarationSyntax CreateMember(string name) =>
      PropertyDeclaration(IdentifierName("string"), name.SubstituteInvalidCharacters())
        .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
        .WithExpressionBody(ArrowExpressionClause(PostfixUnaryExpression(SyntaxKind.SuppressNullableWarningExpression,
                                                                         InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                                                                                     IdentifierName(resourceManagerVariable),
                                                                                                                     IdentifierName(nameof(JsonResourceManager.GetString)))).AddArgumentListArguments(Argument(IdentifierName(Literal($"{name}").Text)),
                                                                                                                                                                                                  Argument(IdentifierName(cultureInfoVariable))))))
        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

    private CompilationUnitSyntax GetCompilationUnit(IEnumerable<MemberDeclarationSyntax> members) =>
      CompilationUnit()
        .AddMembers(this.CreateNamespace()
                      .AddMembers(this.CreateClass()
                                    .AddMembers(members.ToArray())))
        .NormalizeWhitespace();

    private static IEnumerable<string> GetNameFromProperty(JsonProperty property, string parentName = "") {
      return property.Value.ValueKind switch {
        JsonValueKind.Object => GetNameFromElement(property.Value, parentName + property.Name + "."),
        JsonValueKind.Array => GetNameFromElement(property.Value , parentName + property.Name + "."),
        _ => new[] { parentName + property.Name }
      };
    }
    
    private static IEnumerable<string> GetNameFromElement(JsonElement element, string parentName = "") {
      return element.ValueKind switch {
        JsonValueKind.Object => element.EnumerateObject().SelectMany(el => GetNameFromProperty(el, parentName)),
        JsonValueKind.Array => element.EnumerateArray().SelectMany(el => GetNameFromElement(el, parentName)),
        _ => new[] { parentName + element.GetString() }
      };
    }

    private static IEnumerable<string> ParseInputFile(Stream stream) {
      return GetNameFromElement(JsonSerializer.Deserialize<JsonElement>(stream));
    }

    public CompilationUnitSyntax Generate() =>
      this.GetCompilationUnit(ParseInputFile(this.resourceStream)
                                .Select(CreateMember));
  }
}
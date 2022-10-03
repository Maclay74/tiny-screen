using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Generators.DI;

[Generator]
public class DependencyInjectionGenerator : ISourceGenerator {
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new InjectReceiver());
    }

    public void Execute(GeneratorExecutionContext context) {
        var receiver = context.SyntaxReceiver as InjectReceiver ?? throw new Exception();

        foreach (var injectableClass in receiver.InjectClasses) {
            var unit = injectableClass.Class.GetParent<CompilationUnitSyntax>();
            
            var usings = new List<UsingDirectiveSyntax> {
                CreateDIUsing("DI")
            };
            
            usings.AddRange(unit.Usings);
            
            var newClass = CreateClass(injectableClass.Class, new List<MemberDeclarationSyntax> {
                GetInjectMethod(receiver),
                CreateReadyMethod(injectableClass.StartMethods, injectableClass.Fields, receiver.InjectorMethod)
            });
            
            unit = CreateUnit(usings, new List<MemberDeclarationSyntax> {
                CreateNamespace(unit),
                newClass
            });
            
            context.AddSource($"{injectableClass.Class.Identifier.Text}.g.cs", unit.GetText(Encoding.UTF8));
        }
    }

    private MemberDeclarationSyntax CreateNamespace(CompilationUnitSyntax unit) {
        var namespaceDeclaration = unit.Members.FirstOrDefault(m => m is BaseNamespaceDeclarationSyntax);

        switch (namespaceDeclaration) {
            case NamespaceDeclarationSyntax blockScoped:
                return FileScopedNamespaceDeclaration(blockScoped.Name);
           
            case FileScopedNamespaceDeclarationSyntax fileScoped:
                return FileScopedNamespaceDeclaration(fileScoped.Name);
        }

        return null;
    }

    private CompilationUnitSyntax CreateUnit(List<UsingDirectiveSyntax> usings, List<MemberDeclarationSyntax> members) {
        return CompilationUnit()
            .WithUsings(new(usings))
            .WithMembers(new(members))
            .NormalizeWhitespace();
    }

    private MethodDeclarationSyntax GetInjectMethod(InjectReceiver receiver) {
        return receiver.InjectorMethod.WithAttributeLists(new());
    }

    private ClassDeclarationSyntax CreateClass(ClassDeclarationSyntax baseClass, List<MemberDeclarationSyntax> members) {
        return baseClass
            .WithBaseList(null)
            .WithMembers(new(members))
            .NormalizeWhitespace();
    }

    private MethodDeclarationSyntax CreateReadyMethod(List<MethodDeclarationSyntax> startMethods,
        List<FieldDeclarationSyntax> fields, MethodDeclarationSyntax injector) {
        
        var statements = new List<StatementSyntax>();
        
        statements.Add(ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    BaseExpression(),
                    IdentifierName("_Ready")))));
        
        statements.Add(CreateContainerInvocation(injector));

        statements.AddRange(fields
            .Select(CreateInjection));

        statements.AddRange(startMethods
            .Select(method => ExpressionStatement(
                InvocationExpression(
                    IdentifierName(method.Identifier.Text)))).ToList());


        SyntaxList<StatementSyntax> body = new(statements);


        return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.VoidKeyword)),
                Identifier("_Ready"))
            .WithModifiers(
                TokenList(
                    new[] {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.OverrideKeyword),
                        Token(SyntaxKind.PartialKeyword),
                    }))
            .WithBody(Block(body));
    }
    
    private LocalDeclarationStatementSyntax CreateContainerInvocation(MethodDeclarationSyntax injectMethod) {
        return LocalDeclarationStatement(VariableDeclaration(
                IdentifierName(
                    Identifier(
                        TriviaList(),
                        SyntaxKind.VarKeyword,
                        "var",
                        "var",
                        TriviaList())))
            .WithVariables(
                SingletonSeparatedList<VariableDeclaratorSyntax>(
                    VariableDeclarator(
                            Identifier("container"))
                        .WithInitializer(
                            EqualsValueClause(
                                InvocationExpression(
                                    IdentifierName(injectMethod.Identifier)))))));
    }

    private ExpressionStatementSyntax CreateInjection(FieldDeclarationSyntax field) {
        return ExpressionStatement(AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(field.Declaration.Variables[0].Identifier),
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("container"),
                    GenericName(
                            Identifier("GetInstance"))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList(
                                    field.Declaration.Type)))))));
    }

    private UsingDirectiveSyntax CreateUsing(string name) {
        return UsingDirective(IdentifierName(name));
    }

    private UsingDirectiveSyntax CreateDIUsing(string alias) {
        return UsingDirective(
                QualifiedName(
                    IdentifierName("SimpleInjector"),
                    IdentifierName("Container")))
            .WithAlias(
                NameEquals(
                    IdentifierName(alias)));
    }
}
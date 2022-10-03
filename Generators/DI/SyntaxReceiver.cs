using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.DI;

public class InjectReceiver : ISyntaxReceiver {
    public List<InjectableClassSyntax> InjectClasses { get; } = new();

    public MethodDeclarationSyntax InjectorMethod;

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
        
        if (CheckAttributeName(syntaxNode, "Inject", out var injectAttribute)) {
            Capture(injectAttribute.GetParent<FieldDeclarationSyntax>());
            
        } else if (CheckAttributeName(syntaxNode, "Ready", out var readyAttribute)) {
            Capture(readyAttribute.GetParent<MethodDeclarationSyntax>());
            
        } else if (CheckAttributeName(syntaxNode, "Injector", out var injectorAttribute)) {
            InjectorMethod = injectorAttribute.GetParent<MethodDeclarationSyntax>();
        }
    }

    private bool CheckAttributeName(SyntaxNode node, string name, out AttributeSyntax attribute) {
        attribute = null;
        if (node is not AttributeSyntax attributeSyntax) return false;
        attribute = attributeSyntax;
        if (attributeSyntax.Name is not IdentifierNameSyntax attributeSyntaxName) return false;
        return attributeSyntaxName.Identifier.Text == name;
    }

    private void Capture<T>(T injectField) where T: SyntaxNode {
        var classSyntax = injectField.GetParent<ClassDeclarationSyntax>();
        var existingEntry = InjectClasses.Find(c => c.Class == classSyntax);

        if (existingEntry == null) {
            existingEntry = new InjectableClassSyntax(classSyntax,
                new List<FieldDeclarationSyntax>(), 
                new List<MethodDeclarationSyntax>());
            InjectClasses.Add(existingEntry);
        }
        switch (injectField) {
            case FieldDeclarationSyntax field:
                existingEntry.Fields.Add(field);
                break;
            case MethodDeclarationSyntax method:
                existingEntry.StartMethods.Add(method);
                break;
        }
    }
}
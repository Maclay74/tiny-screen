using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.DI; 

public record InjectableClassSyntax(
    ClassDeclarationSyntax Class, 
    List<FieldDeclarationSyntax> Fields, 
    List<MethodDeclarationSyntax> StartMethods
    );
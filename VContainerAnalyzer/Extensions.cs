// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System.Linq;
using Microsoft.CodeAnalysis;

namespace VContainerAnalyzer;

internal static class Extensions
{
    private const string PreserveAttributeName = "PreserveAttribute";
    private const string InjectAttributeName = "InjectAttribute";

    internal static bool IsPreserveAttribute(this ITypeSymbol attributeClass)
    {
        if (attributeClass.Name == PreserveAttributeName)
        {
            return true;
        }

        var baseType = attributeClass.BaseType;
        return baseType != null && IsPreserveAttribute(baseType);
    }

    internal static bool IsVContainerInjectAttribute(this ITypeSymbol attributeClass)
    {
        var containingNamespace = attributeClass.ContainingNamespace;
        if (containingNamespace is not { Name: "VContainer" })
        {
            return false;
        }

        containingNamespace = containingNamespace.ContainingNamespace;
        if (containingNamespace is not { Name: "" })
        {
            return false;
        }

        if (attributeClass.Name == InjectAttributeName)
        {
            return true;
        }

        var baseType = attributeClass.BaseType;
        return baseType != null && IsVContainerInjectAttribute(baseType);
    }

    internal static bool IsDefaultConstructor(this IMethodSymbol constructor)
    {
        if (constructor.IsImplicitlyDeclared)
        {
            return true;
        }

        if (!constructor.Parameters.IsEmpty)
        {
            return false;
        }

        var syntaxReference = constructor.DeclaringSyntaxReferences.FirstOrDefault();
        var methodBlockNode = syntaxReference?.GetSyntax().ChildNodes().LastOrDefault();
        if (methodBlockNode == null)
        {
            return true;
        }

        var methodContentNodes = methodBlockNode.ChildNodes();
        return !methodContentNodes.Any();
    }
}

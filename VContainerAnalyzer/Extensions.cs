// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

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
}

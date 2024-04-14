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

    internal static bool IsInjectAttribute(this ITypeSymbol attributeClass)
    {
        if (attributeClass.Name == InjectAttributeName)
        {
            return true;
        }

        var baseType = attributeClass.BaseType;
        return baseType != null && IsInjectAttribute(baseType);
    }
}

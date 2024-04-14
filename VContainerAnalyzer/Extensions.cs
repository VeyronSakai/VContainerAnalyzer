// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using Microsoft.CodeAnalysis;

namespace VContainerAnalyzer;

public static class Extensions
{
    private const string PreserveAttributeName = "PreserveAttribute";

    public static bool IsPreserveAttribute(this ITypeSymbol attributeClass)
    {
        if (attributeClass.Name == PreserveAttributeName)
        {
            return true;
        }

        var baseType = attributeClass.BaseType;
        return baseType != null && IsPreserveAttribute(baseType);
    }
}

// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using Microsoft.CodeAnalysis;

namespace VContainerAnalyzer;

public static class Rules
{
    public static readonly DiagnosticDescriptor Rule0001 = new(
        id: "VContainer0001",
        title: "Constructor have no attribute that extends PreserveAttribute, such as InjectAttribute.",
        messageFormat:
        "The constructor of '{0}' have no attribute that extends PreserveAttribute, such as InjectAttribute.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Constructor must have attribute that extends PreserveAttribute, such as InjectAttribute."
    );

    public static readonly DiagnosticDescriptor Rule0002 = new(
        id: "VContainer0002",
        title: "Property/Field Injection has been used.",
        messageFormat:
        "Injected into '{0}' using Property/Field Injection. Consider using Constructor or Method Injection.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Property/Field Injection is used. It is recommended to use Constructor or Method Injection."
    );
}

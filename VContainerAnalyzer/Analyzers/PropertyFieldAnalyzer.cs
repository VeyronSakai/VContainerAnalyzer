// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace VContainerAnalyzer.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PropertyFieldAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "VContainer0002";

    private static readonly DiagnosticDescriptor s_rule = new(
        id: DiagnosticId,
        title: "Property/Field Inject has been used.",
        messageFormat: "Injected into '{0}' using Property/Field Injection.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Property/Field Injection is used. It is recommended to use Constructor or Method Injection.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(Analyze, SymbolKind.Field, SymbolKind.Property);
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        var symbol = context.Symbol;
        var fieldSymbol = symbol as IFieldSymbol;
        var propertySymbol = symbol as IPropertySymbol;
        if (fieldSymbol == null && propertySymbol == null)
        {
            return;
        }

        if (fieldSymbol != null)
        {
            if (fieldSymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "InjectAttribute"))
            {
                context.ReportDiagnostic(Diagnostic.Create(s_rule, fieldSymbol.Locations[0], fieldSymbol.Name));
            }

            return;
        }

        if (propertySymbol != null)
        {
            if (propertySymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "InjectAttribute"))
            {
                context.ReportDiagnostic(Diagnostic.Create(s_rule, propertySymbol.Locations[0], propertySymbol.Name));
            }

            return;
        }
    }
}

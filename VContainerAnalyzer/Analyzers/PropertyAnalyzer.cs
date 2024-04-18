// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace VContainerAnalyzer.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PropertyAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rules.Rule0002);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(Analyze, SymbolKind.Property);
    }

    private static void Analyze(SymbolAnalysisContext context)
    {
        var symbol = context.Symbol;
        if (symbol is not IPropertySymbol propertySymbol)
        {
            return;
        }

        var attribute = propertySymbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass.IsVContainerInjectAttribute());

        if (attribute == null)
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rules.Rule0002,
            attribute.ApplicationSyntaxReference.GetSyntax().GetLocation(), propertySymbol.Name));
    }
}

// Copyright (c) 2020-2023 VeyronSakai.
// This software is released under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace VContainerAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class VContainerAnalyzer : DiagnosticAnalyzer
{
    internal const string DiagnosticId = "VContainer0001";

    private static readonly DiagnosticDescriptor s_rule = new(
        id: DiagnosticId,
        title: "Constructor does not have InjectAttribute.",
        messageFormat: "The constructor of '{0}' does not have InjectAttribute.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Constructor must have InjectAttribute.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(AnalyzeAttributes, OperationKind.Invocation);
    }

    private static void AnalyzeAttributes(OperationAnalysisContext context)
    {
        var invocation = (IInvocationOperation)context.Operation;
        var methodSymbol = invocation.TargetMethod;
        var namespaceSymbol = methodSymbol.ContainingNamespace;
        if (namespaceSymbol is not { Name: "Unity" })
        {
            return;
        }

        namespaceSymbol = namespaceSymbol.ContainingNamespace;
        if (namespaceSymbol is not { Name: "VContainer" })
        {
            return;
        }

        namespaceSymbol = namespaceSymbol.ContainingNamespace;
        if (namespaceSymbol is not { Name: "" })
        {
            return;
        }

        if (methodSymbol.ContainingType.Name != "ContainerBuilderUnityExtensions")
        {
            return;
        }

        var diagnostics = GetDiagnostics(invocation);
        foreach (var diagnostic in diagnostics)
        {
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static IEnumerable<Diagnostic> GetDiagnostics(IInvocationOperation invocation) =>
        invocation.TargetMethod.Name switch
        {
            "RegisterEntryPoint" => GetRegisterEntryPointDiagnostics(invocation),
            _ => Array.Empty<Diagnostic>(),
        };

    private static IEnumerable<Diagnostic> GetRegisterEntryPointDiagnostics(IInvocationOperation invocation)
    {
        if (invocation.TargetMethod.TypeArguments.SingleOrDefault() is INamedTypeSymbol concreteType &&
            Reports(concreteType))
        {
            return new[] { Diagnostic.Create(s_rule, GetMethodLocation(invocation), concreteType.Name), };
        }

        return Array.Empty<Diagnostic>();
    }

    private static Location GetMethodLocation(IOperation operation)
    {
        var memberAccessExpressionNode = operation.Syntax.ChildNodes().FirstOrDefault();
        if (memberAccessExpressionNode == null)
        {
            return operation.Syntax.GetLocation();
        }

        var methodNameNode = memberAccessExpressionNode.ChildNodes().LastOrDefault();
        if (methodNameNode == null)
        {
            return operation.Syntax.GetLocation();
        }

        var typeArgumentNode = methodNameNode.ChildNodes().FirstOrDefault();
        if (typeArgumentNode == null)
        {
            return operation.Syntax.GetLocation();
        }

        return typeArgumentNode.GetLocation();
    }

    private static bool Reports(INamedTypeSymbol type)
    {
        if (type.TypeKind != TypeKind.Class)
        {
            return false;
        }

        return !HasPreservedConstructors(type);
    }

    private static bool HasPreservedConstructors(INamedTypeSymbol type)
    {
        return type.Constructors.Any(ctor =>
        {
            return ctor.GetAttributes().Any(x => InheritsPreserveAttribute(x.AttributeClass));
        });
    }

    private static bool InheritsPreserveAttribute(ITypeSymbol attributeClass)
    {
        if (attributeClass.Name == "PreserveAttribute")
        {
            return true;
        }

        var baseType = attributeClass.BaseType;
        return baseType != null && InheritsPreserveAttribute(baseType);
    }
}

﻿// Copyright (c) 2020-2023 VeyronSakai.
// This software is released under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace VContainerAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class VContainerAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "VContainer0001";

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
        if (IsContainerBuilderUnityExtensions(namespaceSymbol, methodSymbol))
        {
            switch (invocation.TargetMethod.Name)
            {
                case "RegisterEntryPoint":
                    CheckRegisterEntryPointMethod(ref context, invocation);
                    break;
            }
        }
        else if (IsContainerBuilderExtensions(namespaceSymbol, methodSymbol))
        {
            switch (invocation.TargetMethod.Name)
            {
                case "Register":
                    CheckRegisterMethod(ref context, invocation);
                    break;
            }
        }
    }

    private static bool IsContainerBuilderUnityExtensions(INamespaceSymbol namespaceSymbol, IMethodSymbol methodSymbol)
    {
        if (namespaceSymbol is not { Name: "Unity" })
        {
            return false;
        }

        namespaceSymbol = namespaceSymbol.ContainingNamespace;
        if (namespaceSymbol is not { Name: "VContainer" })
        {
            return false;
        }

        namespaceSymbol = namespaceSymbol.ContainingNamespace;
        if (namespaceSymbol is not { Name: "" })
        {
            return false;
        }

        return methodSymbol.ContainingType.Name == "ContainerBuilderUnityExtensions";
    }

    private static bool IsContainerBuilderExtensions(INamespaceSymbol namespaceSymbol, IMethodSymbol methodSymbol)
    {
        if (namespaceSymbol is not { Name: "VContainer" })
        {
            return false;
        }

        namespaceSymbol = namespaceSymbol.ContainingNamespace;
        if (namespaceSymbol is not { Name: "" })
        {
            return false;
        }

        return methodSymbol.ContainingType.Name == "ContainerBuilderExtensions";
    }

    private static void CheckRegisterMethod(ref OperationAnalysisContext context, IInvocationOperation invocation)
    {
        var typeArgument = invocation.TargetMethod.TypeArguments.LastOrDefault();
        if (typeArgument is not INamedTypeSymbol concreteType)
        {
            return;
        }

        if (concreteType.TypeKind == TypeKind.Class)
        {
            if (HasPreservedConstructors(concreteType))
            {
                return;
            }

            var location = GetMethodLocation(invocation);
            if (location != default)
            {
                context.ReportDiagnostic(Diagnostic.Create(s_rule, location, concreteType.Name));
            }
        }
        else if (concreteType.TypeKind == TypeKind.Interface)
        {
            var parameter = invocation.TargetMethod.Parameters.ElementAtOrDefault(1);
            if (parameter == null)
            {
                return;
            }

            // if(parameter.Type == typeof(Func<>))
        }
    }

    private static void CheckRegisterEntryPointMethod(ref OperationAnalysisContext context,
        IInvocationOperation invocation)
    {
        if (invocation.TargetMethod.TypeArguments.SingleOrDefault() is not INamedTypeSymbol concreteType)
        {
            return;
        }

        if (concreteType.TypeKind != TypeKind.Class)
        {
            return;
        }

        if (HasPreservedConstructors(concreteType))
        {
            return;
        }

        var location = GetMethodLocation(invocation);
        if (location != default)
        {
            context.ReportDiagnostic(Diagnostic.Create(s_rule, location, concreteType.Name));
        }
    }

    private static Location? GetMethodLocation(IOperation operation)
    {
        var memberAccessExpressionNode = operation.Syntax.ChildNodes().FirstOrDefault();
        if (memberAccessExpressionNode == null)
        {
            return default;
        }

        var methodNameNode = memberAccessExpressionNode.ChildNodes().LastOrDefault();
        var genericNode = methodNameNode?.ChildNodes().FirstOrDefault();
        if (genericNode == null)
        {
            return default;
        }

        var typeArgumentNode = genericNode.ChildNodes().LastOrDefault();
        return typeArgumentNode == null ? operation.Syntax.GetLocation() : typeArgumentNode.GetLocation();
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

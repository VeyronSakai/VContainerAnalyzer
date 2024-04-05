// Copyright (c) 2020-2023 VeyronSakai.
// This software is released under the MIT License.

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
                    AnalyzeRegisterEntryPointMethod(ref context, invocation);
                    break;
            }
        }
        else if (IsContainerBuilderExtensions(namespaceSymbol, methodSymbol))
        {
            switch (invocation.TargetMethod.Name)
            {
                case "Register":
                    AnalyzeRegisterMethod(ref context, invocation);
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

    private static void AnalyzeRegisterMethod(ref OperationAnalysisContext context, IInvocationOperation invocation)
    {
        var typeArgument = invocation.TargetMethod.TypeArguments.LastOrDefault();
        if (typeArgument is not INamedTypeSymbol concreteType)
        {
            return;
        }

        if (concreteType.TypeKind != TypeKind.Class)
        {
            return;
        }

        if (HasConstructorWithPreserveAttribute(concreteType) || !HasCustomConstructor(concreteType))
        {
            return;
        }

        var typeArgumentLocation = GetTypeArgumentLocation(invocation);
        var targetLocation = typeArgumentLocation == default ? invocation.Syntax.GetLocation() : typeArgumentLocation;
        context.ReportDiagnostic(Diagnostic.Create(s_rule, targetLocation, concreteType.Name));
    }

    private static void AnalyzeRegisterInstanceMethod(ref OperationAnalysisContext context,
        IInvocationOperation invocation)
    {
        var typeArgument = invocation.TargetMethod.TypeArguments.FirstOrDefault();
        if (typeArgument is not INamedTypeSymbol concreteType)
        {
            return;
        }

        if (concreteType.TypeKind != TypeKind.Class)
        {
            return;
        }

        if (HasConstructorWithPreserveAttribute(concreteType) || !HasCustomConstructor(concreteType))
        {
            return;
        }

        Location? targetLocation = default;

        var typeArgumentLocation = GetTypeArgumentLocation(invocation);
        if (typeArgumentLocation != default)
        {
            targetLocation = typeArgumentLocation;
        }
        else
        {
            var argumentLocation = GetArgumentLocation(invocation);
            if (argumentLocation != default)
            {
                targetLocation = argumentLocation;
            }
        }

        if (targetLocation != default)
        {
            context.ReportDiagnostic(Diagnostic.Create(s_rule, targetLocation, concreteType.Name));
        }
    }

    private static void AnalyzeRegisterEntryPointMethod(ref OperationAnalysisContext context,
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

        if (HasConstructorWithPreserveAttribute(concreteType) || !HasCustomConstructor(concreteType))
        {
            return;
        }

        var location = GetTypeArgumentLocation(invocation);
        if (location != default)
        {
            context.ReportDiagnostic(Diagnostic.Create(s_rule, location, concreteType.Name));
        }
    }

    private static Location? GetTypeArgumentLocation(IOperation operation)
    {
        var memberAccessExpressionNode = operation.Syntax.ChildNodes().FirstOrDefault();
        var methodNameNode = memberAccessExpressionNode?.ChildNodes().LastOrDefault();
        var genericNode = methodNameNode?.ChildNodes().FirstOrDefault();
        var typeArgumentNode = genericNode?.ChildNodes().LastOrDefault();
        return typeArgumentNode?.GetLocation();
    }

    private static Location? GetArgumentLocation(IOperation operation)
    {
        var location = operation.Syntax.ChildNodes().ElementAtOrDefault(1);
        return location?.GetLocation();
    }

    private static bool HasCustomConstructor(INamedTypeSymbol type)
    {
        return type.Constructors.Any(methodSymbol => !IsDefaultConstructor(methodSymbol));
    }

    private static bool IsDefaultConstructor(IMethodSymbol constructor)
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

    private static bool HasConstructorWithPreserveAttribute(INamedTypeSymbol type)
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var constructor in type.Constructors)
        {
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var attribute in constructor.GetAttributes())
            {
                if (InheritsPreserveAttribute(attribute.AttributeClass))
                {
                    return true;
                }
            }
        }

        return false;
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

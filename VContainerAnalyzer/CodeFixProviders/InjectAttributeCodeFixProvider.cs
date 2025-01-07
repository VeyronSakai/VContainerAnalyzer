// // Copyright (c) 2020-2024 VeyronSakai.
// // This software is released under the MIT License.
//
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Composition;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CodeActions;
// using Microsoft.CodeAnalysis.CodeFixes;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
//
// namespace VContainerAnalyzer.CodeFixProviders;
//
// [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InjectAttributeCodeFixProvider)), Shared]
// public sealed class InjectAttributeCodeFixProvider : CodeFixProvider
// {
//     public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(Rules.Rule0002.Id);
//
//     public override FixAllProvider GetFixAllProvider()
//     {
//         return WellKnownFixAllProviders.BatchFixer;
//     }
//
//     public override async Task RegisterCodeFixesAsync(CodeFixContext context)
//     {
//         var root = await context
//             .Document
//             .GetSyntaxRootAsync(context.CancellationToken)
//             .ConfigureAwait(false);
//
//         var diagnostic = context.Diagnostics.First();
//         var diagnosticSpan = diagnostic.Location.SourceSpan;
//         var declaration = root?
//             .FindToken(diagnosticSpan.Start)
//             .Parent?
//             .AncestorsAndSelf()
//             .OfType<MemberDeclarationSyntax>()
//             .FirstOrDefault();
//
//         if (declaration == null)
//         {
//             return;
//         }
//
//         if (declaration is not FieldDeclarationSyntax && declaration is not PropertyDeclarationSyntax)
//         {
//             return;
//         }
//
//         context.RegisterCodeFix(
//             CodeAction.Create(
//                 "Remove InjectAttribute",
//                 cancellationToken =>
//                     RemoveInjectAttribute(context.Document, declaration, cancellationToken),
//                 FixableDiagnosticIds.Single()),
//             context.Diagnostics);
//     }
//
//     private static async Task<Document> RemoveInjectAttribute(Document document, MemberDeclarationSyntax declaration,
//         CancellationToken cancellationToken)
//     {
//         var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
//         var model = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
//         var newAttributeLists = new List<AttributeListSyntax>();
//
//         foreach (var attributeList in declaration.AttributeLists)
//         {
//             var nodesToRemove = new List<AttributeSyntax>();
//
//             foreach (var attribute in attributeList.Attributes)
//             {
//                 var attributeType = model?.GetTypeInfo(attribute).Type;
//                 if (attributeType != null && attributeType.IsVContainerInjectAttribute())
//                 {
//                     nodesToRemove.Add(attribute);
//                 }
//             }
//
//             var newAttributes = attributeList.RemoveNodes(nodesToRemove, SyntaxRemoveOptions.KeepNoTrivia);
//             if (newAttributes.Attributes.Any())
//             {
//                 newAttributeLists.Add(newAttributes);
//             }
//         }
//
//         var newDeclaration = declaration
//             .WithAttributeLists(SyntaxFactory.List(newAttributeLists))
//             .WithLeadingTrivia(declaration.GetLeadingTrivia());
//
//         var newRoot = root?.ReplaceNode(declaration, newDeclaration);
//         return newRoot == null ? document : document.WithSyntaxRoot(newRoot);
//     }
// }

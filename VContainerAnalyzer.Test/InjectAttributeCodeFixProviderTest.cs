// // Copyright (c) 2020-2024 VeyronSakai.
// // This software is released under the MIT License.
//
// using System.Threading.Tasks;
// using NUnit.Framework;
// using VerifyField =
//     Microsoft.CodeAnalysis.CSharp.Testing.NUnit.CodeFixVerifier<VContainerAnalyzer.Analyzers.FieldAnalyzer,
//         VContainerAnalyzer.CodeFixProviders.InjectAttributeCodeFixProvider>;
// using VerifyProperty =
//     Microsoft.CodeAnalysis.CSharp.Testing.NUnit.CodeFixVerifier<VContainerAnalyzer.Analyzers.PropertyAnalyzer,
//         VContainerAnalyzer.CodeFixProviders.InjectAttributeCodeFixProvider>;
//
// namespace VContainerAnalyzer.Test;
//
// [TestFixture]
// public class InjectAttributeCodeFixProviderTest
// {
//     [Test]
//     public async Task RemoveInjectAttribute_FieldHasInjectAttribute_CodeFixed()
//     {
//         var (source, _) = Helper.GetJoinedFilesContentText("FieldInjectionClass.cs", "EmptyClassStub.cs");
//         var (fixedSource, offset) =
//             Helper.GetJoinedFilesContentText("FieldInjectionClassFixed.txt", "EmptyClassStub.cs");
//
//         var expected = VerifyField.Diagnostic()
//             .WithSpan(6 + offset, 10, 6 + offset, 16)
//             .WithArguments("_field1");
//
//         await VerifyField.VerifyCodeFixAsync(source, expected, fixedSource);
//     }
//
//     [Test]
//     public async Task RemoveInjectAttribute_PropertyHasInjectAttribute_CodeFixed()
//     {
//         var (source, _) = Helper.GetJoinedFilesContentText("PropertyInjectionClass.cs", "EmptyClassStub.cs");
//         var (fixedSource, offset) =
//             Helper.GetJoinedFilesContentText("PropertyInjectionClassFixed.txt", "EmptyClassStub.cs");
//
//         var expected = VerifyProperty.Diagnostic()
//             .WithSpan(6 + offset, 10, 6 + offset, 16)
//             .WithArguments("Property1");
//
//         await VerifyProperty.VerifyCodeFixAsync(source, expected, fixedSource);
//     }
// }

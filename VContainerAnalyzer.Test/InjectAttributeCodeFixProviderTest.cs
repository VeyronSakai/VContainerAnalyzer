// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System.Threading.Tasks;
using NUnit.Framework;
using Verify =
    Microsoft.CodeAnalysis.CSharp.Testing.NUnit.CodeFixVerifier<VContainerAnalyzer.Analyzers.FieldAnalyzer,
        VContainerAnalyzer.CodeFixProviders.InjectAttributeCodeFixProvider>;

namespace VContainerAnalyzer.Test;

[TestFixture]
public class InjectAttributeCodeFixProviderTest
{
    [Test]
    public async Task TypeNameContainingLowercase_CodeFixed()
    {
        var source = Helper.GetJoinedFilesContentText("FieldInjectionClass.cs", "EmptyClassStub.cs");
        var fixedSource = Helper.GetJoinedFilesContentText("FieldInjectionClassFixed.txt", "EmptyClassStub.cs");

        var expected = Verify.Diagnostic()
            .WithSpan(22, 10, 22, 16)
            .WithArguments("_field1");

        await Verify.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}

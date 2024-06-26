// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Dena.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using VContainerAnalyzer.Analyzers;
using Assert = NUnit.Framework.Assert;

namespace VContainerAnalyzer.Test;

[TestFixture]
public class FieldAnalyzerTest
{
    /// <summary>
    /// Test analyze for empty source code
    /// </summary>
    [Test]
    public async Task EmptySourceCode_NoDiagnosticReport()
    {
        const string Source = "";
        var analyzer = new FieldAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Source);

        Assert.That(diagnostics, Is.Empty);
    }

    [Test]
    public async ValueTask Analyze_FieldInjection_ReportDiagnostic()
    {
        var source = Helper.GetFileContentTexts("FieldInjectionClass.cs", "EmptyClassStub.cs");
        var analyzer = new FieldAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .Where(x => x.Id != "CS0169") // Ignore "The private field 'class member' is never used"
            .ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(actual.First().Id, Is.EqualTo("VContainer0002"));
            Assert.That(actual.First().GetMessage(),
                Is.EqualTo(
                    "Injected into '_field1' using Property/Field Injection. Consider using Constructor or Method Injection."));
        });

        var expectedPositions = new[] { new { Start = new LinePosition(9, 9), End = new LinePosition(9, 15) }, };

        Assert.That(actual, Has.Length.EqualTo(expectedPositions.Length));

        for (var i = 0; i < expectedPositions.Length; i++)
        {
            LocationAssert.HaveTheSpan(
                expectedPositions[i].Start,
                expectedPositions[i].End,
                actual[i].Location
            );
        }
    }
}

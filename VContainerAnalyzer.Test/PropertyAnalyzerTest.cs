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
public class PropertyAnalyzerTest
{
    /// <summary>
    /// Test analyze for empty source code
    /// </summary>
    [Test]
    public async Task EmptySourceCode_NoDiagnosticReport()
    {
        const string Source = "";
        var analyzer = new PropertyAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Source);

        Assert.That(diagnostics, Is.Empty);
    }

    [Test]
    public async ValueTask Analyze_PropertyInjection_ReportDiagnostic()
    {
        var source = Helper.ReadCodes("PropertyInjectionClass.cs", "EmptyClassStub.cs");
        var analyzer = new PropertyAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(actual.First().Id, Is.EqualTo("VContainer0002"));
            Assert.That(actual.First().GetMessage(),
                Is.EqualTo(
                    "Injected into 'Property1' using Property/Field Injection. Consider using Constructor or Method Injection."));
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

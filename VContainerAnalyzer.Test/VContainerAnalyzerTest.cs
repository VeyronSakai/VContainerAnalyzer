// Copyright (c) 2020-2023 VeyronSakai.
// This software is released under the MIT License.

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dena.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace VContainerAnalyzer.Test;

[TestFixture]
public class VContainerAnalyzerTest
{
    /// <summary>
    /// Test analyze for empty source code
    /// </summary>
    [Test]
    public async Task EmptySourceCode_NoDiagnosticReport()
    {
        const string Source = "";
        var analyzer = new VContainerAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Source);

        Assert.That(diagnostics, Is.Empty);
    }

    [Test]
    public async ValueTask ConstructorWithoutInjectAttribute_ReportOneDiagnostic()
    {
        const string VContainerDirectory = "VContainer";
        var source = ReadCodes("ConstructorWithoutInjectAttribute.cs",
            $"{Path.Combine(VContainerDirectory, "Container.cs")}",
            $"{Path.Combine(VContainerDirectory, "ContainerBuilder.cs")}",
            $"{Path.Combine(VContainerDirectory, "ContainerBuilderExtensions.cs")}",
            $"{Path.Combine(VContainerDirectory, "ContainerBuilderUnityExtensions.cs")}",
            $"{Path.Combine(VContainerDirectory, "RegistrationBuilder.cs")}",
            "FooLifetimeScope.cs");

        var analyzer = new VContainerAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(actual.First().Id, Is.EqualTo("VContainer0001"));
            Assert.That(actual.First().GetMessage(),
                Is.EqualTo(
                    "The constructor of 'ConstructorWithoutInjectAttributeClass' does not have InjectAttribute."));
        });

        var expectedPositions = new[]
        {
            new { Start = new LinePosition(14, 39), End = new LinePosition(14, 77) },
            new { Start = new LinePosition(15, 39), End = new LinePosition(15, 77) },
            new { Start = new LinePosition(16, 29), End = new LinePosition(16, 67) },
            new { Start = new LinePosition(17, 42), End = new LinePosition(17, 80) },
            new { Start = new LinePosition(18, 55), End = new LinePosition(18, 93) },
            new { Start = new LinePosition(19, 68), End = new LinePosition(19, 106) },
            new { Start = new LinePosition(21, 29), End = new LinePosition(21, 67) },
            new { Start = new LinePosition(23, 29), End = new LinePosition(23, 67) },
            new { Start = new LinePosition(24, 37), End = new LinePosition(24, 75) },
            new { Start = new LinePosition(26, 36), End = new LinePosition(26, 82) },
            new { Start = new LinePosition(29, 36), End = new LinePosition(29, 46) },
        };

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

    private static string[] ReadCodes(params string[] sources)
    {
        const string Path = "../../../TestData";
        return sources.Select(file => File.ReadAllText($"{Path}/{file}", Encoding.UTF8)).ToArray();
    }
}

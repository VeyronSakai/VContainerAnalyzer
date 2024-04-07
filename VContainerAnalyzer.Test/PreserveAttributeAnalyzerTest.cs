// Copyright (c) 2020-2023 VeyronSakai.
// This software is released under the MIT License.

using System.Collections.Generic;
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
public class PreserveAttributeAnalyzerTest
{
    private const string VContainerDirectory = "VContainer";

    private static IEnumerable<string> VContainerSourcePaths { get; } =
        GetVContainerFiles.Select(file => $"{Path.Combine(VContainerDirectory, file)}");

    private static IEnumerable<string> GetVContainerFiles =>
    [
        "Container.cs",
        "ContainerBuilder.cs",
        "ContainerBuilderExtensions.cs",
        "ContainerBuilderUnityExtensions.cs",
        "RegistrationBuilder.cs"
    ];

    private static string[] ReadCodes(params string[] sources)
    {
        const string TestDataDirPath = "../../../TestData";
        return sources
            .Concat(VContainerSourcePaths)
            .Select(file => File.ReadAllText($"{TestDataDirPath}/{file}", Encoding.UTF8)).ToArray();
    }

    /// <summary>
    /// Test analyze for empty source code
    /// </summary>
    [Test]
    public async Task EmptySourceCode_NoDiagnosticReport()
    {
        const string Source = "";
        var analyzer = new PreserveAttributeAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Source);

        Assert.That(diagnostics, Is.Empty);
    }

    [Test]
    public async ValueTask AnalyzeRegisterEntryPointMethod_ConstructorDoesNotHaveInjectAttribute_ReportDiagnostics()
    {
        var source = ReadCodes("ConstructorWithoutInjectAttributeClass.cs",
            "Interfaces.cs",
            "RegisterEntryPointConstructorWithoutInjectAttributeClassLifetimeScope.cs");

        var analyzer = new PreserveAttributeAnalyzer();
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
                    "The constructor of 'ConstructorWithoutInjectAttributeClass' have no attribute that extends PreserveAttribute, such as InjectAttribute."));
        });

        var expectedPositions = new[]
        {
            new { Start = new LinePosition(14, 39), End = new LinePosition(14, 77) },
            new { Start = new LinePosition(15, 39), End = new LinePosition(15, 77) },
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

    [Test]
    public async ValueTask AnalyzeRegisterEntryPointMethod_ConstructorDoesNotExist_ReportNoDiagnostics()
    {
        var source = ReadCodes("NoConstructorClass.cs",
            "Interfaces.cs",
            "RegisterEntryPointNoConstructorClassLifetimeScope.cs");

        var analyzer = new PreserveAttributeAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.That(actual.Length, Is.EqualTo(0));
    }

    [Test]
    public async ValueTask AnalyzeRegisterMethod_ConstructorDoesNotHaveInjectAttribute_ReportDiagnostics()
    {
        var source = ReadCodes("ConstructorWithoutInjectAttributeClass.cs",
            "Interfaces.cs",
            "RegisterConstructorWithoutInjectAttributeClassLifetimeScope.cs");

        var analyzer = new PreserveAttributeAnalyzer();
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
                    "The constructor of 'ConstructorWithoutInjectAttributeClass' have no attribute that extends PreserveAttribute, such as InjectAttribute."));
        });

        var expectedPositions = new[]
        {
            new { Start = new LinePosition(12, 29), End = new LinePosition(12, 67) },
            new { Start = new LinePosition(13, 29), End = new LinePosition(13, 67) },
            new { Start = new LinePosition(14, 42), End = new LinePosition(14, 80) },
            new { Start = new LinePosition(15, 55), End = new LinePosition(15, 93) },
            new { Start = new LinePosition(16, 68), End = new LinePosition(16, 106) },
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

    [Test]
    public async ValueTask AnalyzeRegisterMethod_ConstructorDoesNotExist_ReportNoDiagnostics()
    {
        var source = ReadCodes("NoConstructorClass.cs",
            "Interfaces.cs",
            "RegisterNoConstructorClassLifetimeScope.cs");

        var analyzer = new PreserveAttributeAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.That(actual.Length, Is.EqualTo(0));
    }
}

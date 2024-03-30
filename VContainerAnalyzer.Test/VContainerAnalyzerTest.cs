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
            $"{Path.Combine(VContainerDirectory, "ContainerBuilderUnityExtensions.cs")}",
            "FooLifetimeScope.cs");

        var analyzer = new VContainerAnalyzer();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.That(actual, Has.Length.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(actual.Single().Id, Is.EqualTo("VContainer0001"));
            Assert.That(actual.Single().GetMessage(),
                Is.EqualTo(
                    "The constructor of 'ConstructorWithoutInjectAttributeClass' does not have InjectAttribute"));
        });
        LocationAssert.HaveTheSpan(
            new LinePosition(14, 38),
            new LinePosition(14, 78),
            actual.First().Location
        );
    }

    private static string[] ReadCodes(params string[] sources)
    {
        const string Path = "../../../TestData";
        return sources.Select(file => File.ReadAllText($"{Path}/{file}", Encoding.UTF8)).ToArray();
    }
}

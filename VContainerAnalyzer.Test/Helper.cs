// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VContainerAnalyzer.Test;

internal static class Helper
{
    private const string TestDataDirPath = "../../../TestData";
    private const string VContainerDirectoryName = "VContainer";

    private static IEnumerable<string> VContainerSourcePaths { get; } =
        GetVContainerFiles.Select(file => $"{Path.Combine(VContainerDirectoryName, file)}").ToArray();

    private static IEnumerable<string> GetVContainerFiles =>
    [
        "Container.cs",
        "ContainerBuilder.cs",
        "ContainerBuilderExtensions.cs",
        "ContainerBuilderUnityExtensions.cs",
        "RegistrationBuilder.cs",
        "InjectAttribute.cs",
    ];

    internal static string[] GetFileContentTexts(params string[] sourcePaths)
    {
        return sourcePaths
            .Concat(VContainerSourcePaths)
            .Select(file => File.ReadAllText($"{TestDataDirPath}/{file}", Encoding.UTF8)).ToArray();
    }

    internal static (string, int offset) GetJoinedFilesContentText(params string[] sources)
    {
        var fileBodyBuilder = new StringBuilder();
        var usingStatementsBuilder = new StringBuilder();

        var offset = 0;

        foreach (var filePath in sources.Concat(VContainerSourcePaths))
        {
            var fileContent = File.ReadAllText($"{TestDataDirPath}/{filePath}", Encoding.UTF8);
            var tree = CSharpSyntaxTree.ParseText(fileContent);
            var root = tree.GetRoot();
            var usingDirectives = root.DescendantNodes().OfType<UsingDirectiveSyntax>().ToList();
            var newRoot = root.RemoveNodes(usingDirectives, SyntaxRemoveOptions.KeepNoTrivia);
            var usingStatements = string.Join(Environment.NewLine, usingDirectives.Select(u => u.ToFullString()));
            offset += usingStatements.Split(Environment.NewLine).Length - 1;
            fileBodyBuilder.Append(newRoot?.ToFullString());
            usingStatementsBuilder.Append(usingStatements);
        }

        return (usingStatementsBuilder.Append(fileBodyBuilder).ToString(), offset);
    }
}

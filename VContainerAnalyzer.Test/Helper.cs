// Copyright (c) 2020-2024 VeyronSakai.
// This software is released under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VContainerAnalyzer.Test;

public static class Helper
{
    private const string VContainerDirectory = "VContainer";

    private static IEnumerable<string> VContainerSourcePaths { get; } =
        GetVContainerFiles.Select(file => $"{Path.Combine(VContainerDirectory, file)}").ToArray();

    private static IEnumerable<string> GetVContainerFiles =>
    [
        "Container.cs",
        "ContainerBuilder.cs",
        "ContainerBuilderExtensions.cs",
        "ContainerBuilderUnityExtensions.cs",
        "RegistrationBuilder.cs",
        "InjectAttribute.cs",
    ];

    public static string[] ReadCodes(params string[] sources)
    {
        const string TestDataDirPath = "../../../TestData";
        return sources
            .Concat(VContainerSourcePaths)
            .Select(file => File.ReadAllText($"{TestDataDirPath}/{file}", Encoding.UTF8)).ToArray();
    }
}

using System.Linq;
using UnityEditor;

namespace Editor
{
    public class PackageExporter
    {
        private const string AnalyzerPluginPath = "Assets/Plugins/VContainerAnalyzer/";
        private const string ExportPath = "./VContainerAnalyzer.unitypackage";

        [MenuItem("Tools/ExportPackage")]
        private static void Export()
        {
            var assetPathNames = AssetDatabase.FindAssets("", new[] { AnalyzerPluginPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();

            AssetDatabase.ExportPackage(
                assetPathNames,
                ExportPath,
                ExportPackageOptions.IncludeDependencies);
        }
    }
}

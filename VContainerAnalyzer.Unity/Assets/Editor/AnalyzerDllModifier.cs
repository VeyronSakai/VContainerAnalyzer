using UnityEditor;

namespace Editor
{
    public class AnalyzerDllModifier : AssetPostprocessor
    {
        private const string RoslynAnalyzerLabel = "RoslynAnalyzer";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
            {
                if (assetPath != "Assets/Plugins/VContainerAnalyzer/VContainerAnalyzer.dll")
                {
                    continue;
                }

                var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.SetCompatibleWithAnyPlatform(false);

                AssetDatabase.SetLabels(importer, new[] { RoslynAnalyzerLabel });

                importer.SaveAndReimport();
            }
        }
    }
}

using UnityEditor;

namespace Editor
{
    public class AnalyzerDllModifier : AssetPostprocessor
    {
        private const string AnalyzerDLLPath = "Assets/Plugins/VContainerAnalyzer/VContainerAnalyzer.dll";
        private const string RoslynAnalyzerLabel = "RoslynAnalyzer";

        private void OnPreprocessAsset()
        {
            if (assetPath != AnalyzerDLLPath)
            {
                return;
            }

            var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
            if (importer == null)
            {
                return;
            }

            importer.SetCompatibleWithAnyPlatform(false);

            AssetDatabase.SetLabels(importer, new[] { RoslynAnalyzerLabel });
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
            {
                if (assetPath != AnalyzerDLLPath)
                {
                    continue;
                }

                var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.SaveAndReimport();
            }
        }
    }
}

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

            if (!importer.importSettingsMissing)
            {
                return;
            }

            importer.SetCompatibleWithAnyPlatform(false);
            AssetDatabase.SetLabels(importer, new[] { RoslynAnalyzerLabel });
        }
    }
}

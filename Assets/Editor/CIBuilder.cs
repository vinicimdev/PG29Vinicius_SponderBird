using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System;

public class CIBuilder
{
    [MenuItem("CI/BuildWebGL")]
    public static void PerformWebGLBuild()
    {
        try
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("No scenes are added in the Build Settings.");
                return;
            }

            string buildPath = "Builds/WebGL";
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
                Debug.Log($"Created directory at {buildPath}");
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;
            buildPlayerOptions.locationPathName = buildPath;
            buildPlayerOptions.target = BuildTarget.WebGL;
            buildPlayerOptions.options = BuildOptions.None;

            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            UnityEditor.Build.Reporting.BuildSummary summary = report.summary;

            if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log("Build Succeeded: " + summary.totalSize + " bytes");
            }
            else if (summary.result == UnityEditor.Build.Reporting.BuildResult.Failed)
            {
                Debug.LogError("Build Failed my dude. 404? 504? Who knows. Try again later! Total Errors: " + $"{summary.totalErrors}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Critical error {ex.Message}");
        }
    }
}

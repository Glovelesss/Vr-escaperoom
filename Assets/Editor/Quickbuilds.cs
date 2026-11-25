using System.IO;
using UnityEditor;
using UnityEngine;

public static class QuickBuild
{
    const string folder = "/home/mick/Documents/Builds/";
    const string buildName = "VR-EscapeRoom";

    [MenuItem("Build/Build to Builds folder")]
    public static void Build()
    {
        Directory.CreateDirectory(folder);

        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string extension = target == BuildTarget.Android ? ".apk"
                         : target == BuildTarget.StandaloneLinux64 ? ".x86_64"
                         : target == BuildTarget.StandaloneWindows64 ? ".exe"
                         : "";

        var options = new BuildPlayerOptions
        {
            scenes = System.Array.ConvertAll(
                System.Array.FindAll(EditorBuildSettings.scenes, s => s.enabled),
                s => s.path),
            locationPathName = Path.Combine(folder, buildName + extension),
            target = target,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);
        Debug.Log("Build result: " + report.summary.result + " -> " + options.locationPathName);
    }
}
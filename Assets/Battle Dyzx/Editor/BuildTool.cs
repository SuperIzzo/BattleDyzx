using UnityEditor;
using UnityEngine;

public class BuildTool
{
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildServer();
        BuildClient();
    }

    [MenuItem("Build/Build Server")]
    public static void BuildServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Battle Dyzx/Scenes/BattleServer.unity" };
        buildPlayerOptions.locationPathName = "Builds/Server/BDXServer.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Build Client")]
    public static void BuildClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Battle Dyzx/Scenes/BattleClient.unity" };
        buildPlayerOptions.locationPathName = "Builds/Client/BDXClient.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
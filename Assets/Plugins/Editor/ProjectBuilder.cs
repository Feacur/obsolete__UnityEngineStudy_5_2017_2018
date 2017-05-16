using UnityEditor;
using UnityEngine;

///
/// Testing out project building
///
public static class ProjectBuilder {
	public static string Build (BuildTarget buildTarget, string locationPathName)
	{
		var assetBundleManifest = AssetBundlesBuilder.Build(buildTarget);

		var options = new BuildPlayerOptions();
		options.locationPathName = locationPathName;
		options.options = BuildOptions.None;
		options.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
		options.assetBundleManifestPath = string.Format("{0}/AssetBundles.manifest", AssetBundlesBuilder.AssetBundlesPath);
		options.target = buildTarget;
		return BuildPipeline.BuildPlayer(options);
	}

	[MenuItem ("WGTestAssignment/Build, Windows")]
	public static string BuildProjectWindows ()
	{
		return Build(
			BuildTarget.StandaloneWindows,
			"../Builds/WGTestAssignment Windows/Tanks.exe"
		);
	}
}

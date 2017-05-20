using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

///
/// Wannabe generic build scripts for project
///
/// There are functions exposed to editor to use:
/// <see cref="BuildActiveTarget"> builds project using current chosen platform.
/// <see cref="BuildWithAssetBundlesActiveTarget"> builds asset bundles AND project using current chosen platform.
///
public static class ProjectBuilder {
	private static readonly Dictionary<BuildTarget, string> locationPathTemplates = new Dictionary<BuildTarget, string> {
		{BuildTarget.StandaloneWindows,        "Builds/Windows/{0}.exe"},
		{BuildTarget.StandaloneOSXUniversal,   "Builds/OSX/{0}.app"},
		{BuildTarget.StandaloneLinuxUniversal, "Builds/Linux/{0}.app"},
		{BuildTarget.WebGL,                    "Builds/WebGL/{0}"},
		{BuildTarget.iOS,                      "Builds/iOS/{0}"},
		{BuildTarget.Android,                  "Builds/Android/{0}"}
	};

	private static readonly Dictionary<BuildTarget, BuildOptions> buildOptions = new Dictionary<BuildTarget, BuildOptions> {
		{BuildTarget.StandaloneWindows,        BuildOptions.None},
		{BuildTarget.StandaloneOSXUniversal,   BuildOptions.None},
		{BuildTarget.StandaloneLinuxUniversal, BuildOptions.None},
		{BuildTarget.WebGL,                    BuildOptions.None},
		{BuildTarget.iOS,                      BuildOptions.None},
		{BuildTarget.Android,                  BuildOptions.None}
	};

	public static string Build (BuildTarget buildTarget) {
		var options = new BuildPlayerOptions();
		options.locationPathName = string.Format(locationPathTemplates[buildTarget], PlayerSettings.productName);
		options.options = buildOptions[buildTarget];
		options.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
		options.assetBundleManifestPath = AssetBundlesBuilder.AssetBundlesManifestPath;
		options.target = buildTarget;
		return BuildPipeline.BuildPlayer(options);
	}
	
	public static string BuildWithAssetBundles (BuildTarget buildTarget) {
		AssetBundlesBuilder.Build(buildTarget);
		return Build(buildTarget);
	}

	[MenuItem ("WGTestAssignment/Build project only")]
	public static string BuildActiveTarget () {
		return Build(EditorUserBuildSettings.activeBuildTarget);
	}

	[MenuItem ("WGTestAssignment/Build project with asset bundles")]
	public static string BuildWithAssetBundlesActiveTarget () {
		return BuildWithAssetBundles(EditorUserBuildSettings.activeBuildTarget);
	}
}

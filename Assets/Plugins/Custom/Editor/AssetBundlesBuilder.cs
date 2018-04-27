using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

///
/// Wannabe generic build scripts for asset bundles.
///
/// There is a function exposed to editor to use:
/// <see cref="BuildActiveTarget"> builds asset bundles using current chosen platform.
///
public static class AssetBundlesBuilder {
	private static readonly string assetBundlesSubPath = "AssetBundles";

	private static readonly Dictionary<BuildTarget, BuildAssetBundleOptions> buildOptions = new Dictionary<BuildTarget, BuildAssetBundleOptions> {
		{BuildTarget.StandaloneWindows,        BuildAssetBundleOptions.None},
		{BuildTarget.StandaloneOSX,            BuildAssetBundleOptions.None},
		{BuildTarget.StandaloneLinuxUniversal, BuildAssetBundleOptions.None},
		{BuildTarget.WebGL,                    BuildAssetBundleOptions.None},
		{BuildTarget.iOS,                      BuildAssetBundleOptions.None},
		{BuildTarget.Android,                  BuildAssetBundleOptions.None}
	};

	public static string AssetBundlesPath => $"{Application.streamingAssetsPath}/{assetBundlesSubPath}";

	public static string AssetBundlesManifestPath => $"{AssetBundlesPath}/AssetBundles.manifest";

	public static AssetBundleManifest Build (BuildTarget buildTarget) {
		Directory.CreateDirectory(AssetBundlesPath);
		return BuildPipeline.BuildAssetBundles(
			AssetBundlesPath, buildOptions[buildTarget], buildTarget
		);
	}

	[MenuItem ("Custom/List asset bundles")]
	public static void ListAssetBundles () {
		var names = AssetDatabase.GetAllAssetBundleNames();
		Debug.LogFormat("Found asset bundles: {0}", string.Join(", ", names));
	}

	[MenuItem ("Custom/* Build asset bundles")]
	public static AssetBundleManifest BuildActiveTarget () {
		return Build(EditorUserBuildSettings.activeBuildTarget);
	}
}

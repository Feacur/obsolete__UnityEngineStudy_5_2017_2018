using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

///
/// Testing out asset bundles building
///
public static class AssetBundlesBuilder {
	
	private static readonly string[] assetBundlesPathRaw = {
		"Assets", "StreamingAssets", "AssetBundles"
	};

	private static readonly Dictionary<BuildTarget, BuildAssetBundleOptions> buildOptions = new Dictionary<BuildTarget, BuildAssetBundleOptions> {
		{BuildTarget.StandaloneWindows,        BuildAssetBundleOptions.None},
		{BuildTarget.StandaloneOSXUniversal,   BuildAssetBundleOptions.None},
		{BuildTarget.StandaloneLinuxUniversal, BuildAssetBundleOptions.None},
		{BuildTarget.WebGL,                    BuildAssetBundleOptions.None},
		{BuildTarget.iOS,                      BuildAssetBundleOptions.None},
		{BuildTarget.Android,                  BuildAssetBundleOptions.None}
	};

	public static string AssetBundlesPath {
		get {
			return string.Join("/", assetBundlesPathRaw);
		}
	}

	public static string AssetBundlesManifestPath {
		get {
			return string.Format("{0}/AssetBundles.manifest", AssetBundlesPath);
		}
	}
	
	public static AssetBundleManifest Build (BuildTarget buildTarget)
	{
		EnsureAssetBundlesFolderExists();
		return BuildPipeline.BuildAssetBundles(
			AssetBundlesPath, buildOptions[buildTarget], buildTarget
		);
	}

	[MenuItem ("WGTestAssignment/Build asset bundles")]
	public static AssetBundleManifest BuildActiveTarget ()
	{
		return Build(EditorUserBuildSettings.activeBuildTarget);
	}
	
	private static void EnsureAssetBundlesFolderExists ()
	{
		StringBuilder assetBundlesPathBuilder = new StringBuilder();
		assetBundlesPathBuilder.Append(assetBundlesPathRaw[0]);
		for	(int i = 1; i < assetBundlesPathRaw.Length; i++) {
			var folderName = assetBundlesPathRaw[i];

			var parentFolder = assetBundlesPathBuilder.ToString();

			assetBundlesPathBuilder.Append('/').Append(folderName);
			var childFolder = assetBundlesPathBuilder.ToString();

			if (!AssetDatabase.IsValidFolder(childFolder)) {
				AssetDatabase.CreateFolder(parentFolder, folderName);
			}
		}
	}
}

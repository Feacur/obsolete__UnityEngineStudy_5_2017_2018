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

	public static string AssetBundlesPath {
		get {
			return string.Join("/", assetBundlesPathRaw);
		}
	}
	
	public static AssetBundleManifest Build (BuildTarget buildTarget)
	{
		EnsureAssetBundlesFolderExists();
		return BuildPipeline.BuildAssetBundles(
			AssetBundlesPath,
			BuildAssetBundleOptions.None,
			buildTarget
		);
	}

	[MenuItem ("WGTestAssignment/Build asset bundles, Windows")]
	public static AssetBundleManifest BuildAssetBundlesWindows ()
	{
		return Build(BuildTarget.StandaloneWindows);
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

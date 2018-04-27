using System.Collections.Generic;
using UnityEditor;

public static class HangarBuilder {
	private static readonly string subprojectSubPath = "Demo/Hangar";

	private static Dictionary<string, string> assetBundles = new Dictionary<string, string> {
		{"AssetBundles/Environment", "environment"},
		{"AssetBundles/Scenes",      "scenes"},
		{"AssetBundles/Tanks",       "tanks"},
	};

	[MenuItem ("Custom/Hangar/* Build asset bundles")]
	public static void BuildAssetBundles () {
		PrepareStreamingAssets();
		MarkAssetBundles();
		AssetBundlesBuilder.BuildActiveTarget();
	}

	[MenuItem ("Custom/Hangar/** Build project only")]
	public static void BuildActiveTarget () {
		PrepareStreamingAssets();
		MarkAssetBundles();
		ProjectBuilder.BuildActiveTarget();
	}

	[MenuItem ("Custom/Hangar/*** Build project with asset bundles")]
	public static void BuildActiveTarget_WithAssetBundles () {
		PrepareStreamingAssets();
		MarkAssetBundles();
		ProjectBuilder.BuildActiveTarget_WithAssetBundles();
	}

	[MenuItem ("Custom/Hangar/Prepare streaming assets")]
	public static void PrepareStreamingAssets () {
		SubprojectBuilder.PrepareStreamingAssets(subprojectSubPath);
	}

	[MenuItem ("Custom/Hangar/Mark asset bundles")]
	public static void MarkAssetBundles () {
		foreach (var assetBundle in assetBundles) {
			string path = $"Assets/{subprojectSubPath}/{assetBundle.Key}";
			var assetImporter = AssetImporter.GetAtPath(path);
			assetImporter.assetBundleName = assetBundle.Value;
		}
	}

	[MenuItem ("Custom/Hangar/Unmark asset bundles")]
	public static void UnmarkAssetBundles () {
		foreach (var assetBundle in assetBundles) {
			string path = $"Assets/{subprojectSubPath}/{assetBundle.Key}";
			var assetImporter = AssetImporter.GetAtPath(path);
			assetImporter.assetBundleName = string.Empty;
		}

		AssetDatabase.RemoveUnusedAssetBundleNames();
	}
}

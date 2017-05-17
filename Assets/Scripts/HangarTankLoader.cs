using System.Collections;
using UnityEngine;

///
/// Testing out asset bundles loading
///
public class HangarTankLoader : MonoBehaviour {
	public string assetBundleSubPath = "AssetBundles/tanks";
	public string prefabPath = "Assets/Prefabs/Tanks/Tank 1.prefab";
	public Transform tankParentTransform;

	private IEnumerator Start () {
		string tanksAssetBundleUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, assetBundleSubPath);
		
		// Load asset bundle
		AssetBundle tanksAssetBundle = null;
		using(var www = new WWW(tanksAssetBundleUrl)) {
			yield return www;
			if (!string.IsNullOrEmpty(www.error)) {
				Debug.LogError(www.error);
			}
			else if (www.bytesDownloaded == 0) {
				Debug.LogErrorFormat("Loaded zero bytes for {0}", tanksAssetBundleUrl);
			}
			else {
				tanksAssetBundle = www.assetBundle;
			}
		}
		
		// Get prefab from asset bundle
		GameObject prefab = null;
		if (!tanksAssetBundle) {
			Debug.LogErrorFormat("No matching asset bundle found for {0}", tanksAssetBundleUrl);
		}
		else if (!tanksAssetBundle.Contains(prefabPath)) {
			Debug.LogErrorFormat("No matching prefab found for {0}", prefabPath);
		}
		else {
			var loadAssetAsync = tanksAssetBundle.LoadAssetAsync(prefabPath);
			yield return loadAssetAsync;
			prefab = loadAssetAsync.asset as GameObject;
		}

		// Instantiate prefab
		if (!prefab) {
			Debug.LogErrorFormat("Can't load {0} as a GameObject", prefabPath);
		}
		else {
			var instance = Instantiate(prefab);
			instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
		}
	}
}

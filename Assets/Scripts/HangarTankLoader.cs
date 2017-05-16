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
		string streamingAssetsUrl = string.Format("file://{0}", Application.streamingAssetsPath);
		string tanksAssetBundleUrl = string.Format("{0}/{1}", streamingAssetsUrl, assetBundleSubPath);
		WWW www = new WWW(tanksAssetBundleUrl);
		yield return www;

		if (!string.IsNullOrEmpty(www.error)) {
			Debug.LogError(www.error);
			yield break;
		}

		if (www.bytesDownloaded == 0) {
			Debug.LogErrorFormat("Loaded zero bytes for {0}", tanksAssetBundleUrl);
			yield break;
		}

		var tanksAssetBundle = www.assetBundle;
		if (!tanksAssetBundle) {
			Debug.LogErrorFormat("No matching asset bundle found for {0}", tanksAssetBundleUrl);
			yield break;
		}
		
		if (tanksAssetBundle.Contains(prefabPath)) {
			var loadAssetAsync = tanksAssetBundle.LoadAssetAsync(prefabPath);
			yield return loadAssetAsync;

			var prefab = loadAssetAsync.asset as GameObject;
			if (!prefab) {
				Debug.LogErrorFormat("Can't load {0} as a GameObject", prefabPath);
				yield break;
			}

			var instance = Instantiate(prefab);
			instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
		}
		else {
			Debug.LogErrorFormat("No matching prefab found for {0}", prefabPath);
		}
	}
}

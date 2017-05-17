using System;
using System.Collections;
using UnityEngine;

///
/// Testing out asset bundles loading
///
public class HangarTankLoader : MonoBehaviour {
	public Transform tankParentTransform;

	private IEnumerator Start () {
		// Get tanks asset bundle
		AssetBundle tanksAssetBundle = null;
		string tanksAssetBundleUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, "AssetBundles/tanks");
		yield return LoadData<AssetBundle>(tanksAssetBundleUrl, (value) => {
			tanksAssetBundle = value;
		});

		// Get tanks collection config
		string tankConfigSubPath = string.Empty;
		string tanksCollectionUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, "tanks.yml");
		yield return LoadData<string>(tanksCollectionUrl, (value) => {
			var config = YamlWrapper.Deserialize<TanksCollectionConfig>(value);
			tankConfigSubPath = config.entries[0];
		});

		// Get prefab path
		string prefabPath = string.Empty;
		string tankConfigUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfigSubPath);
		yield return LoadData<string>(tankConfigUrl, (value) => {
			var config = YamlWrapper.Deserialize<TankConfig>(value);
			prefabPath = config.prefab;
		});
		
		// Get prefab
		GameObject prefab = null;
		yield return LoadData<GameObject>(tanksAssetBundle, prefabPath, (value) => {
			prefab = value;
		});

		// Instantiate prefab
		if (prefab) {
			var instance = Instantiate(prefab);
			instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
		}
	}

	private Coroutine LoadData<T>(AssetBundle assetBundle, string assetName, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadCoroutine(assetBundle, assetName, callback));
	}


	private Coroutine LoadData<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadCoroutine(url, callback));
	}
}

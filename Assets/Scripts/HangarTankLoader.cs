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
		yield return LoadDataAsync<AssetBundle>(tanksAssetBundleUrl, (resultValue) => {
			tanksAssetBundle = resultValue;
		});

		// Get tanks collection config
		string tankConfigSubPath = string.Empty;
		string tanksCollectionUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, "tanks.yml");
		yield return LoadDataAsync<string>(tanksCollectionUrl, (resultValue) => {
			var config = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
			tankConfigSubPath = config.entries[0];
		});

		// Get prefab path
		string prefabPath = string.Empty;
		string tankConfigUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfigSubPath);
		yield return LoadDataAsync<string>(tankConfigUrl, (resultValue) => {
			var config = YamlWrapper.Deserialize<TankConfig>(resultValue);
			prefabPath = config.prefab;
		});
		
		// Get prefab
		GameObject prefab = null;
		yield return LoadDataAsync<GameObject>(tanksAssetBundle, prefabPath, (resultValue) => {
			prefab = resultValue;
		});

		// Instantiate prefab
		if (prefab) {
			var instance = Instantiate(prefab);
			instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
		}
	}

	private Coroutine LoadDataAsync<T>(AssetBundle assetBundle, string assetName, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(assetBundle, assetName, callback));
	}


	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}

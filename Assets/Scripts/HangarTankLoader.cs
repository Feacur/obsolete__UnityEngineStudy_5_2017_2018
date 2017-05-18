using System;
using System.Collections;
using UnityEngine;

///
/// Testing out asset bundles loading
///
public class HangarTankLoader : MonoBehaviour {
	public string tanksCollectionConfigSubPath = "tanks.yml";
	public Transform tankParentTransform;

	private IEnumerator Start () {
		// Get tanks collection config
		TanksCollectionConfig tanksCollectionConfig = null;
		string tanksCollectionUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tanksCollectionConfigSubPath);
		yield return LoadDataAsync<string>(tanksCollectionUrl, (resultValue) => {
			tanksCollectionConfig = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
		});

		// Get one tank config
		TankConfig tankConfig = null;
		string tankConfigUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tanksCollectionConfig.entries[0]);
		yield return LoadDataAsync<string>(tankConfigUrl, (resultValue) => {
			tankConfig = YamlWrapper.Deserialize<TankConfig>(resultValue);
		});

		// Get tanks asset bundle
		AssetBundle tanksAssetBundle = null;
		string tanksAssetBundleUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfig.assetBundle);
		yield return LoadDataAsync<AssetBundle>(tanksAssetBundleUrl, (resultValue) => {
			tanksAssetBundle = resultValue;
		});
		
		// Get tank prefab
		GameObject tankPrefab = null;
		yield return LoadDataAsync<GameObject>(tanksAssetBundle, tankConfig.prefab, (resultValue) => {
			tankPrefab = resultValue;
		});

		// Instantiate prefab
		var instance = Instantiate(tankPrefab);
		instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
	}

	private Coroutine LoadDataAsync<T>(AssetBundle assetBundle, string assetName, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(assetBundle, assetName, callback));
	}


	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}

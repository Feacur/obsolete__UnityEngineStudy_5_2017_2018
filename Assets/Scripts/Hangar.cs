using System;
using System.Collections;
using UnityEngine;

///
/// Hangar scene representation
///
public class Hangar : StaticInstanceMonoBehaviour<Hangar> {
	public Transform tankParentTransform;
	
	private Coroutine createTankCoroutine;
	private TankConfig tankConfig;
	
	public Coroutine SetTankInfo(TankConfig tankConfig) {
		if (createTankCoroutine != null) {
			StopCoroutine(createTankCoroutine);
		}
		createTankCoroutine = StartCoroutine(CreateTankCoroutine(tankConfig));
		return createTankCoroutine;
	}
	
	private IEnumerator CreateTankCoroutine(TankConfig tankConfig) {
		this.tankConfig = tankConfig;

		tankParentTransform.DestroyChildren();

		// Get tanks asset bundle
		AssetBundle tanksAssetBundle = null;
		string tanksAssetBundleUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfig.assetBundle);
		yield return CachedAssetBundleLoader.LoadAsync(tanksAssetBundleUrl, (resultValue) => {
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
		instance.name = string.Format("Tank: {0}", tankConfig.name);
	}

	private Coroutine LoadDataAsync<T>(AssetBundle assetBundle, string assetName, Action<T> callback) where T : class {
		return StartCoroutine(assetBundle.LoadCoroutine(assetName, callback));
	}
}
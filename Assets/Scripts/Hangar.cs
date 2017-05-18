using System;
using System.Collections;
using UnityEngine;

///
/// Hangar scene representation
///
public class Hangar : MonoBehaviour {
	private static Hangar _instance;
	public static Hangar instance {
		get {
			if (!_instance) {
				_instance = FindObjectOfType<Hangar>();
			}
			return _instance;
		}
	}

	public Transform tankParentTransform;
	
	private AssetBundle tanksAssetBundle;
	private Coroutine createTankCoroutine;
	
	public Coroutine CreateTank(TankConfig tankConfig) {
		if (createTankCoroutine != null) {
			StopCoroutine(createTankCoroutine);
		}
		createTankCoroutine = StartCoroutine(CreateTankCoroutine(tankConfig));
		return createTankCoroutine;
	}
	
	private IEnumerator CreateTankCoroutine(TankConfig tankConfig) {
		tankParentTransform.DestroyChildren();

		// Get tanks asset bundle
		if (!tanksAssetBundle) {
			string tanksAssetBundleUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfig.assetBundle);
			yield return LoadDataAsync<AssetBundle>(tanksAssetBundleUrl, (resultValue) => {
				tanksAssetBundle = resultValue;
			});
		}
		
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
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(assetBundle, assetName, callback));
	}

	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}
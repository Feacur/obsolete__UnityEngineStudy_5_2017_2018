using System;
using UnityEngine;

///
/// Hangar UI representation
///
public class HangarUI : MonoBehaviour {
	[Header("Config")]
	public string tanksCollectionConfigSubPath = "tanks.yml";
	[Header("Collection")]
	public TankUI tankUIPrefab;
	public RectTransform tanksCollectionParentTransform;

	private TanksCollectionConfig tanksCollectionConfig;
	
	private void OnEnable() {
		string tanksCollectionUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tanksCollectionConfigSubPath);
		LoadDataAsync<string>(tanksCollectionUrl, (resultValue) => {
			tanksCollectionConfig = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
			UpdateUI();
		});
	}

	private void OnDisable() {
		if (this) {
			tanksCollectionParentTransform.DestroyChildren();
		}
	}

	private void UpdateUI() {
		tanksCollectionParentTransform.DestroyChildren();

		foreach (string tankConfigSubPath in tanksCollectionConfig.entries) {
			TankConfig tankConfig = null;
			string tankConfigUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfigSubPath);
			LoadDataAsync<string>(tankConfigUrl, (resultValue) => {
				tankConfig = YamlWrapper.Deserialize<TankConfig>(resultValue);
				CreateTankUI(tankConfig);
			});
		}
	}

	private TankUI CreateTankUI(TankConfig tankConfig) {
		var instance = Instantiate(tankUIPrefab);
		instance.tankConfig = tankConfig;
		instance.transform.SetParent(tanksCollectionParentTransform, false);
		return instance;
	}

	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}
using System;
using UnityEngine;
using UnityEngine.UI;

///
/// Hangar UI representation
///
public class HangarUI : StaticInstanceBehaviour<HangarUI> {
	[Header("Config")]
	public string tanksCollectionConfigSubPath = "tanks.yml";
	[Header("Collection")]
	public TankUI tankUIPrefab;
	public RectTransform tanksCollectionParentTransform;
	[Header("Tank info")]
	public Text tankCaption;
	public Text tankType;
	public Text tankMass;
	public Text tankSpeed;
	public Text tankPrice;

	private TanksCollectionConfig tanksCollectionConfig;
	
	public void SetTankInfo(TankConfig tankConfig) {
		tankCaption.text = tankConfig.name;
		tankType.text = tankConfig.type;
		tankMass.text = string.Format("{0:N1} Ton", tankConfig.mass / 1000);
		tankSpeed.text = string.Format("{0:N1} Km/h", tankConfig.speed * 3600 / 1000);
		tankPrice.text = string.Format("{0:N0} {1}", tankConfig.price, tankConfig.currency);
	}
	
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
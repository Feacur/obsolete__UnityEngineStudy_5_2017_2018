using System;
using UnityEngine;
using UnityEngine.UI;

///
/// Hangar UI representation
///
public class HangarUI : StaticInstanceMonoBehaviour<HangarUI> {
	[Header("Config")]
	public string userConfigSubPath = "user.yml";
	public string tanksCollectionConfigSubPath = "tanks.yml";
	[Header("User")]
	public Text userSilver;
	public Text userGold;
	[Header("Tanks collection")]
	public TankUI tankUIPrefab;
	public RectTransform tanksCollectionParentTransform;
	[Header("Tank info")]
	public TankInfoEntryUI tankInfoEntryUIPrefab;
	public RectTransform tankInfoParentTransform;

	private UserConfig userConfig;
	private TanksCollectionConfig tanksCollectionConfig;
	private TankConfig tankConfig;
	
	public void SetTankInfo(TankConfig tankConfig) {
		this.tankConfig = tankConfig;
		tankInfoParentTransform.DestroyChildren();
		CreateTankInfoEntryUI("Caption", tankConfig.name);
		CreateTankInfoEntryUI("Type", tankConfig.type);
		CreateTankInfoEntryUI("Weight", string.Format("{0:N1} ton", tankConfig.mass / 1000));
		CreateTankInfoEntryUI("Speed", string.Format("{0:N1} km/h", tankConfig.speed * 3600 / 1000));
		CreateTankInfoEntryUI("Price", string.Format("{0:N0} {1}", tankConfig.price, tankConfig.currency));
	}
	
	private void OnEnable() {
		var persistentUserConfig = PersistentData.ReadYaml<UserConfig>(userConfigSubPath);
		if (persistentUserConfig != null) {
			SetUserInfo(persistentUserConfig);
		}
		else {
			string userConfigUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, userConfigSubPath);
			LoadDataAsync<string>(userConfigUrl, (resultValue) => {
				var userConfig = YamlWrapper.Deserialize<UserConfig>(resultValue);
				SetUserInfo(userConfig);
			});
		}

		string tanksCollectionUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tanksCollectionConfigSubPath);
		LoadDataAsync<string>(tanksCollectionUrl, (resultValue) => {
			var tanksCollectionConfig = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
			SetTanksCollectionInfo(tanksCollectionConfig);
		});
	}

	private void OnDisable() {
		tanksCollectionParentTransform.DestroyChildren();
	}

	private void SetTanksCollectionInfo(TanksCollectionConfig tanksCollectionConfig) {
		this.tanksCollectionConfig = tanksCollectionConfig;
		tanksCollectionParentTransform.DestroyChildren();
		foreach (string tankConfigSubPath in tanksCollectionConfig.entries) {
			string tankConfigUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, tankConfigSubPath);
			LoadDataAsync<string>(tankConfigUrl, (resultValue) => {
				var tankConfig = YamlWrapper.Deserialize<TankConfig>(resultValue);
				CreateTankUI(tankConfig);
			});
		}
	}

	private void SetUserInfo(UserConfig userConfig) {
		this.userConfig = userConfig;
		userSilver.text = string.Format("{0:N0}", userConfig.silver);
		userGold.text = string.Format("{0:N0}", userConfig.gold);
	}
	
	private void CreateTankUI(TankConfig tankConfig) {
		var instance = Instantiate(tankUIPrefab);
		instance.transform.SetParent(tanksCollectionParentTransform, false);
		instance.SetTankInfo(tankConfig);
	}

	private void CreateTankInfoEntryUI(string text1, string text2) {
		var instance = Instantiate(tankInfoEntryUIPrefab);
		instance.transform.SetParent(tankInfoParentTransform, false);
		instance.SetText(text1, text2);
	}

	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}
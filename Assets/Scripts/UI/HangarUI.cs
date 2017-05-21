using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

///
/// Hangar UI representation
///
public class HangarUI : StaticInstanceMonoBehaviour<HangarUI> {
	[Header("Config")]
	public string userConfigSubPath = "user.yml";
	public string tanksCollectionConfigSubPath = "tanks.yml";
	[Header("Navigation")]
	public Button battleButton;
	public Button purchaseButton;
	public Button sellButton;
	[Header("User")]
	public Text userSilver;
	public Text userGold;
	[Header("Tanks collection")]
	public TankUI tankUIPrefab;
	public RectTransform tanksCollectionParentTransform;
	[Header("Tank info")]
	public TankInfoEntryUI tankInfoEntryUIPrefab;
	public RectTransform tankInfoParentTransform;
	
	public UserConfig userConfig { get; private set; }
	public TanksCollectionConfig tanksCollectionConfig { get; private set; }
	public TankConfig tankConfig { get; private set; }

	public void SetTankInfo(TankConfig tankConfig) {
		this.tankConfig = tankConfig;
		UpdateUI();

		if (Hangar.instance) {
			Hangar.instance.SetTankInfo(tankConfig);
		}

		tankInfoParentTransform.DestroyChildren();
		CreateTankInfoEntryUI("Type", tankConfig.type);
		CreateTankInfoEntryUI("Weight", string.Format("{0:N1} ton", tankConfig.mass / 1000));
		CreateTankInfoEntryUI("Speed", string.Format("{0:N1} km/h", tankConfig.speed * 3600 / 1000));
		CreateTankInfoEntryUI("Price", string.Format("{0:N0} {1}", tankConfig.price, tankConfig.currency));
	}
	
	public void UpdateUI() {
		if (userConfig == null) { return; }
		userSilver.text = string.Format("{0:N0}", userConfig.silver);
		userGold.text = string.Format("{0:N0}", userConfig.gold);

		if (tankConfig == null) { return; }
		bool owned = userConfig.ownedTanksUids.Any(uid => uid == tankConfig.uid);
		battleButton.gameObject.SetActive(owned);
		purchaseButton.gameObject.SetActive(!owned);
		sellButton.gameObject.SetActive(owned);
	}

	private void OnEnable() {
		battleButton.onClick.AddListener(RequestBattle);
		purchaseButton.onClick.AddListener(RequestPurchase);
		sellButton.onClick.AddListener(RequestSell);

		var persistentUserConfig = PersistentData.ReadYaml<UserConfig>(userConfigSubPath);
		if (persistentUserConfig != null) {
			SetUserInfo(persistentUserConfig);
		}
		else {
			StreamingData.LoadDataAsync<string>(userConfigSubPath, (resultValue) => {
				var userConfig = YamlWrapper.Deserialize<UserConfig>(resultValue);
				SetUserInfo(userConfig);
			});
		}

		StreamingData.LoadDataAsync<string>(tanksCollectionConfigSubPath, (resultValue) => {
			var tanksCollectionConfig = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
			SetTanksCollectionInfo(tanksCollectionConfig);
		});
	}

	private void OnDisable() {
		battleButton.onClick.RemoveListener(RequestBattle);
		purchaseButton.onClick.RemoveListener(RequestPurchase);
		sellButton.onClick.RemoveListener(RequestSell);
		tanksCollectionParentTransform.DestroyChildren();
	}

	private void SetTanksCollectionInfo(TanksCollectionConfig tanksCollectionConfig) {
		this.tanksCollectionConfig = tanksCollectionConfig;

		tanksCollectionParentTransform.DestroyChildren();
		foreach (string tankConfigSubPath in tanksCollectionConfig.tankConfigs) {
			StreamingData.LoadDataAsync<string>(tankConfigSubPath, (resultValue) => {
				var tankConfig = YamlWrapper.Deserialize<TankConfig>(resultValue);
				CreateTankUI(tankConfig);
			});
		}
	}

	private void SetUserInfo(UserConfig userConfig) {
		this.userConfig = userConfig;
		UpdateUI();
	}
	
	private void CreateTankUI(TankConfig tankConfig) {
		if (this.tankConfig == null) { SetTankInfo(tankConfig); }

		var instance = Instantiate(tankUIPrefab);
		instance.transform.SetParent(tanksCollectionParentTransform, worldPositionStays: false);
		instance.SetTankInfo(tankConfig);
	}

	private void CreateTankInfoEntryUI(string text1, string text2) {
		var instance = Instantiate(tankInfoEntryUIPrefab);
		instance.transform.SetParent(tankInfoParentTransform, worldPositionStays: false);
		instance.SetText(text1, text2);
	}

	private void RequestBattle() {
		
	}

	private void RequestPurchase() {
		var purchasePanel = PanelsRegistry.Get<PurchasePanel>();
		purchasePanel.SetUserInfo(userConfig);
		purchasePanel.SetTankInfo(tankConfig);
		purchasePanel.gameObject.SetActive(true);
	}

	private void RequestSell() {
		var sellPanel = PanelsRegistry.Get<SellPanel>();
		sellPanel.SetUserInfo(userConfig);
		sellPanel.SetTankInfo(tankConfig);
		sellPanel.gameObject.SetActive(true);
	}
}

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

///
/// Hangar UI representation
/// Responsible for UI updates
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
	
	private TanksCollectionConfig tanksCollectionConfig;
	private Coroutine loadDataCoroutine;

	private void Start() {
		battleButton.interactable = false;
		purchaseButton.interactable = false;
		sellButton.interactable = false;
	}

	private void OnEnable() {
		battleButton.onClick.AddListener(RequestBattle);
		purchaseButton.onClick.AddListener(RequestPurchase);
		sellButton.onClick.AddListener(RequestSell);

		HangarDataProvider.instance.onUserChanged.AddListener(OnUserChanged);
		HangarDataProvider.instance.onTankSelected.AddListener(OnTankSelected);

		if (loadDataCoroutine != null) {
			StopCoroutine(loadDataCoroutine);
		}
		loadDataCoroutine = StartCoroutine(LoadDataCoroutine());
	}
	
	private void OnDisable() {
		battleButton.onClick.RemoveListener(RequestBattle);
		purchaseButton.onClick.RemoveListener(RequestPurchase);
		sellButton.onClick.RemoveListener(RequestSell);

		if (!HangarDataProvider.destroyed) {
			HangarDataProvider.instance.onUserChanged.RemoveListener(OnUserChanged);
			HangarDataProvider.instance.onTankSelected.RemoveListener(OnTankSelected);
		}
		
		if (loadDataCoroutine != null) {
			StopCoroutine(loadDataCoroutine);
		}

		tanksCollectionParentTransform.DestroyChildren();
	}

	private void OnUserChanged(UserConfig userConfig) {
		userSilver.text = string.Format("{0:N0}", userConfig.silver);
		userGold.text = string.Format("{0:N0}", userConfig.gold);

		foreach(var tankUI in tanksCollectionParentTransform.GetComponentsInChildren<TankUI>()) {
			tankUI.UpdateAquiredState(userConfig);
		}

		if (HangarDataProvider.instance.selectedTank != null) {
			UpdateButtons(userConfig, HangarDataProvider.instance.selectedTank);
		}
		
		PersistentData.WriteYaml(userConfigSubPath, userConfig);
	}
	
	private void OnTankSelected(TankConfig tankConfig) {
		tankInfoParentTransform.DestroyChildren();
		CreateTankInfoEntryUI("Type", tankConfig.type);
		CreateTankInfoEntryUI("Weight", string.Format("{0:N1} ton", tankConfig.mass / 1000));
		CreateTankInfoEntryUI("Speed", string.Format("{0:N1} km/h", tankConfig.speed * 3600 / 1000));
		CreateTankInfoEntryUI("Price", string.Format("{0:N0} {1}", tankConfig.price, tankConfig.currency));
		
		foreach(var tankUI in tanksCollectionParentTransform.GetComponentsInChildren<TankUI>()) {
			tankUI.UpdateSelectedState(tankConfig.uid);
		}
		
		if (HangarDataProvider.instance.user != null) {
			UpdateButtons(HangarDataProvider.instance.user, tankConfig);
		}
	}
	
	private IEnumerator LoadDataCoroutine() {
		yield return StreamingData.LoadDataAsync<string>(tanksCollectionConfigSubPath, (resultValue) => {
			var tanksCollectionConfig = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
			SetTanksCollectionInfo(tanksCollectionConfig);
		});

		var persistentUserConfig = PersistentData.ReadYaml<UserConfig>(userConfigSubPath);
		if (persistentUserConfig != null) {
			HangarDataProvider.SetUser(persistentUserConfig);
			yield break;
		}
		
		yield return StreamingData.LoadDataAsync<string>(userConfigSubPath, (resultValue) => {
			var userConfig = YamlWrapper.Deserialize<UserConfig>(resultValue);
			HangarDataProvider.SetUser(userConfig);
		});
	}

	private void SetTanksCollectionInfo(TanksCollectionConfig tanksCollectionConfig) {
		this.tanksCollectionConfig = tanksCollectionConfig;

		tanksCollectionParentTransform.DestroyChildren();
		foreach (string tankConfigSubPath in tanksCollectionConfig.tankConfigs) {
			StreamingData.LoadDataAsync<string>(tankConfigSubPath, (resultValue) => {
				var tankConfig = YamlWrapper.Deserialize<TankConfig>(resultValue);
				CreateTankUI(tankConfig);

				if (HangarDataProvider.instance.selectedTank == null) {
					HangarDataProvider.SetSelectedTank(tankConfig);
				}
			});
		}
	}
	
	private void CreateTankUI(TankConfig tankConfig) {
		var instance = Instantiate(tankUIPrefab);
		instance.transform.SetParent(tanksCollectionParentTransform, worldPositionStays: false);
		instance.SetTankInfo(tankConfig);
	}

	private void CreateTankInfoEntryUI(string text1, string text2) {
		var instance = Instantiate(tankInfoEntryUIPrefab);
		instance.transform.SetParent(tankInfoParentTransform, worldPositionStays: false);
		instance.SetText(text1, text2);
	}

	private void UpdateButtons(UserConfig user, TankConfig selectedTank) {
		bool owned = user.HasTank(selectedTank.uid);
		bool hasTanksToBuy = (tanksCollectionConfig.tankConfigs.Length > 1);
		bool hasTanksToSell = (user.ownedTanksUids.Count > 1);

		battleButton.gameObject.SetActive(owned);
		battleButton.interactable = owned;
		
		purchaseButton.gameObject.SetActive(!owned);
		purchaseButton.interactable = !owned && hasTanksToBuy;

		sellButton.gameObject.SetActive(owned);
		sellButton.interactable = owned && hasTanksToSell;
	}

	private void RequestBattle() {
		
	}

	private void RequestPurchase() {
		var purchasePanel = PanelsRegistry.Get<PurchasePanel>();
		purchasePanel.SetUserInfo(HangarDataProvider.instance.user);
		purchasePanel.SetTankInfo(HangarDataProvider.instance.selectedTank);
		purchasePanel.gameObject.SetActive(true);
	}

	private void RequestSell() {
		var sellPanel = PanelsRegistry.Get<SellPanel>();
		sellPanel.SetUserInfo(HangarDataProvider.instance.user);
		sellPanel.SetTankInfo(HangarDataProvider.instance.selectedTank);
		sellPanel.gameObject.SetActive(true);
	}
}

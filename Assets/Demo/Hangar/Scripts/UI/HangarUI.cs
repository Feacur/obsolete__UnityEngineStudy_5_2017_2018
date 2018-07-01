using Custom.Singleton;
using UnityEngine;
using UnityEngine.UI;
using Custom.Utils;

namespace Demo.Hangar {
	///
	/// Hangar UI representation
	/// Responsible for UI updates
	///
	/// Responsibilities of <see cref="OnUserChanged">
	/// * Display user UI (silver and gold)
	/// * Update tanks aquired state
	/// * Update buttons
	/// * Save user state
	///
	/// Responsibilities of <see cref="OnTanksCollectionChanged">
	/// * Display tanks collection
	/// * Select default tank
	///
	/// Responsibilities of <see cref="OnTankSelected">
	/// * Display tank info
	/// * Update tanks selected state
	/// * Update buttons
	///
	public class HangarUI : StaticInstanceMonoBehaviour<HangarUI>
	{
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

		private PanelsRegistry PanelsRegistry;
		private HangarConfigProvider HangarConfigProvider;
		
		private new void Awake() {
			base.Awake();
			this.PanelsRegistry = PanelsRegistry.instance;
			this.HangarConfigProvider = HangarConfigProvider.instance;
		}
		
		private void Start() {
			battleButton.interactable = false;
			purchaseButton.interactable = false;
			sellButton.interactable = false;
		}

		private void OnEnable() {
			battleButton.onClick.AddListener(RequestBattle);
			purchaseButton.onClick.AddListener(RequestPurchase);
			sellButton.onClick.AddListener(RequestSell);

			HangarConfigProvider.onUserChanged.AddListener(OnUserChanged);
			HangarConfigProvider.onTanksCollectionChanged.AddListener(OnTanksCollectionChanged);
			HangarConfigProvider.onTankSelected.AddListener(OnTankSelected);
		}
		
		private void OnDisable() {
			battleButton.onClick.RemoveListener(RequestBattle);
			purchaseButton.onClick.RemoveListener(RequestPurchase);
			sellButton.onClick.RemoveListener(RequestSell);

			if (!HangarConfigProvider.destroyed) {
				HangarConfigProvider.onUserChanged.RemoveListener(OnUserChanged);
				HangarConfigProvider.onTanksCollectionChanged.RemoveListener(OnTanksCollectionChanged);
				HangarConfigProvider.onTankSelected.RemoveListener(OnTankSelected);
			}

			tanksCollectionParentTransform.DestroyChildren();
		}

		private void OnUserChanged(UserConfig userConfig) {
			userSilver.text = $"{userConfig.silver:N0}";
			userGold.text = $"{userConfig.gold:N0}";

			foreach(var tankUI in tanksCollectionParentTransform.GetComponentsInChildren<TankUI>()) {
				tankUI.UpdateAquiredState(userConfig);
			}

			if (HangarConfigProvider.selectedTank != null) {
				// HangarConfigProvider.tanksCollection should be non-null by now
				UpdateButtons(userConfig, HangarConfigProvider.tanksCollection, HangarConfigProvider.selectedTank);
			}
		}

		private void OnTanksCollectionChanged(TankConfig[] tankConfigs) {
			tanksCollectionParentTransform.DestroyChildren();
			foreach (var tankConfig in tankConfigs) {
				CreateTankUI(tankConfig);
			}
			if (tankConfigs.Length == 0) { return; }
			HangarConfigProvider.SetSelectedTank(tankConfigs[0]);
		}
		
		private void OnTankSelected(TankConfig tankConfig) {
			tankInfoParentTransform.DestroyChildren();
			CreateTankInfoEntryUI("Type", tankConfig.type);
			CreateTankInfoEntryUI("Weight", $"{tankConfig.mass / 1000:N1} ton");
			CreateTankInfoEntryUI("Speed", $"{tankConfig.speed * 3600 / 1000:N1} km/h");
			CreateTankInfoEntryUI("Price", $"{tankConfig.price:N0} {tankConfig.currency}");
			
			foreach(var tankUI in tanksCollectionParentTransform.GetComponentsInChildren<TankUI>()) {
				tankUI.UpdateSelectedState(tankConfig.uid);
			}
			
			if (HangarConfigProvider.user != null) {
				// HangarConfigProvider.tanksCollection should be non-null by now
				UpdateButtons(HangarConfigProvider.user, HangarConfigProvider.tanksCollection, tankConfig);
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

		private void UpdateButtons(UserConfig user, TankConfig[] tanksCollection, TankConfig selectedTank) {
			bool owned = user.HasTank(selectedTank.uid);
			bool hasTanksToBuy = (tanksCollection.Length > 1);
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
			purchasePanel.Open(HangarConfigProvider.user, HangarConfigProvider.selectedTank);
		}

		private void RequestSell() {
			var sellPanel = PanelsRegistry.Get<SellPanel>();
			sellPanel.Open(HangarConfigProvider.user, HangarConfigProvider.selectedTank);
		}
	}
}

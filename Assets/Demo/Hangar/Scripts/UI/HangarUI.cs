using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
	public class HangarUI : StaticInstanceMonoBehaviour<HangarUI> {
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
		
		private void Start() {
			battleButton.interactable = false;
			purchaseButton.interactable = false;
			sellButton.interactable = false;
		}

		private void OnEnable() {
			battleButton.onClick.AddListener(RequestBattle);
			purchaseButton.onClick.AddListener(RequestPurchase);
			sellButton.onClick.AddListener(RequestSell);

			HangarConfigProvider.instance.onUserChanged.AddListener(OnUserChanged);
			HangarConfigProvider.instance.onTanksCollectionChanged.AddListener(OnTanksCollectionChanged);
			HangarConfigProvider.instance.onTankSelected.AddListener(OnTankSelected);
		}
		
		private void OnDisable() {
			battleButton.onClick.RemoveListener(RequestBattle);
			purchaseButton.onClick.RemoveListener(RequestPurchase);
			sellButton.onClick.RemoveListener(RequestSell);

			if (!HangarConfigProvider.destroyed) {
				HangarConfigProvider.instance.onUserChanged.RemoveListener(OnUserChanged);
				HangarConfigProvider.instance.onTanksCollectionChanged.RemoveListener(OnTanksCollectionChanged);
				HangarConfigProvider.instance.onTankSelected.RemoveListener(OnTankSelected);
			}

			tanksCollectionParentTransform.DestroyChildren();
		}

		private void OnUserChanged(UserConfig userConfig) {
			userSilver.text = string.Format("{0:N0}", userConfig.silver);
			userGold.text = string.Format("{0:N0}", userConfig.gold);

			foreach(var tankUI in tanksCollectionParentTransform.GetComponentsInChildren<TankUI>()) {
				tankUI.UpdateAquiredState(userConfig);
			}

			if (HangarConfigProvider.instance.selectedTank != null) {
				// HangarConfigProvider.instance.tanksCollection should be non-null by now
				UpdateButtons(userConfig, HangarConfigProvider.instance.tanksCollection, HangarConfigProvider.instance.selectedTank);
			}
		}

		private void OnTanksCollectionChanged(TankConfig[] tankConfigs) {
			tanksCollectionParentTransform.DestroyChildren();
			foreach (var tankConfig in tankConfigs) {
				CreateTankUI(tankConfig);
			}
			HangarConfigProvider.SetSelectedTank(tankConfigs[0]);
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
			
			if (HangarConfigProvider.instance.user != null) {
				// HangarConfigProvider.instance.tanksCollection should be non-null by now
				UpdateButtons(HangarConfigProvider.instance.user, HangarConfigProvider.instance.tanksCollection, tankConfig);
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
			purchasePanel.Open(HangarConfigProvider.instance.user, HangarConfigProvider.instance.selectedTank);
		}

		private void RequestSell() {
			var sellPanel = PanelsRegistry.Get<SellPanel>();
			sellPanel.Open(HangarConfigProvider.instance.user, HangarConfigProvider.instance.selectedTank);
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

namespace Demo.Hangar {
	///
	/// Tank UI representation
	///
	public class TankUI : MonoBehaviour
		, IPointerClickHandler
	{
		public Image image;
		public GameObject aquiredMark;
		public Text caption;
		[Header("Selection")]
		public Color normalColor = Color.white;
		public Color selectedColor = Color.green;

		private HangarConfigProvider HangarConfigProvider;
		private void Awake() {
			this.HangarConfigProvider = HangarConfigProvider.instance;
		}

		private TankConfig tankConfig;
		public void SetTankInfo(TankConfig tankConfig) {
			this.tankConfig = tankConfig;
			name = string.Format("Tank UI: {0}", tankConfig.name);
			caption.text = tankConfig.name;
			
			if (HangarConfigProvider.instance.user != null) {
				UpdateAquiredState(HangarConfigProvider.instance.user);
			}
		}

		public void UpdateSelectedState(string selectedTankUid) {
			bool selected = (tankConfig.uid == selectedTankUid);
			image.color = selected ? selectedColor : normalColor;
		}

		public void UpdateAquiredState(UserConfig user) {
			bool aquired = user.HasTank(tankConfig.uid);
			aquiredMark.SetActive(aquired);
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
			HangarConfigProvider.SetSelectedTank(tankConfig);
		}
	}
}

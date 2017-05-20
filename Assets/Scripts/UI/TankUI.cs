using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

///
/// Tank UI representation
///
public class TankUI : MonoBehaviour, IPointerClickHandler {
	public Text caption;

	private TankConfig tankConfig;
	
	public void SetTankInfo(TankConfig tankConfig) {
		this.tankConfig = tankConfig;
		name = string.Format("Tank UI: {0}", tankConfig.name);
		caption.text = tankConfig.name;
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
		HangarUI.instance.SetTankInfo(tankConfig);
	}
}

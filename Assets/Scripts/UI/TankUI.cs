using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

///
/// Tank UI representation
///
public class TankUI : MonoBehaviour, IPointerClickHandler {
	public Image image;
	public GameObject aquiredMark;
	public Text caption;
	[Header("Selection")]
	public Color normalColor = Color.white;
	public Color selectedColor = Color.green;

	public TankConfig tankConfig { get; private set; }
	
	public void SetTankInfo(TankConfig tankConfig) {
		this.tankConfig = tankConfig;
		name = string.Format("Tank UI: {0}", tankConfig.name);
		caption.text = tankConfig.name;
	}

	public void SetSelectedState(bool value) {
		image.color = value ? selectedColor : normalColor;
	}

	public void SetAquiredState(bool value) {
		aquiredMark.SetActive(value);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
		HangarUI.instance.SetTankInfo(tankConfig);
	}
}

using System;
using System.Collections;
using UnityEngine;

///
/// Hangar scene representation
///
public class Hangar : StaticInstanceMonoBehaviour<Hangar> {
	public Transform tankParentTransform;
	
	public TankConfig tankConfig { get; private set; }
	
	private Coroutine createTankCoroutine;

	private void Start() {
		if (tankConfig == null && HangarUI.instance && HangarUI.instance.tankConfig != null) {
			SetTankInfo(HangarUI.instance.tankConfig);
		}
	}
	
	public Coroutine SetTankInfo(TankConfig tankConfig) {
		if (createTankCoroutine != null) {
			StopCoroutine(createTankCoroutine);
		}
		createTankCoroutine = StartCoroutine(CreateTankCoroutine(tankConfig));
		return createTankCoroutine;
	}
	
	private IEnumerator CreateTankCoroutine(TankConfig tankConfig) {
		this.tankConfig = tankConfig;

		tankParentTransform.DestroyChildren();
		
		// Get tank prefab
		GameObject tankPrefab = null;
		yield return StreamingData.LoadAssetAsync<GameObject>(tankConfig.assetBundle, tankConfig.prefab, (resultValue) => {
			tankPrefab = resultValue;
		});

		// Instantiate prefab
		var instance = Instantiate(tankPrefab);
		instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
		instance.name = string.Format("Tank: {0}", tankConfig.name);
	}
}

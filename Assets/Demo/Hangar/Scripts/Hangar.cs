using System;
using System.Collections;
using Custom.Data;
using Custom.Singleton;
using UnityEngine;
using Custom.Utils;

namespace Demo.Hangar {
	///
	/// Hangar scene representation
	/// Responsible for tank model updates
	///
	public class Hangar : StaticInstanceMonoBehaviour<Hangar>
	{
		public Transform tankParentTransform;
		
		//
		// "Injects"
		//

		private StreamingData StreamingData;
		private HangarConfigProvider HangarConfigProvider;
		
		//
		// Callbacks from Unity
		//

		new protected void Awake() {
			base.Awake();
			this.StreamingData = StreamingData.instance;
			this.HangarConfigProvider = HangarConfigProvider.instance;
		}

		private void OnEnable() {
			HangarConfigProvider.onTankSelected.AddListener(OnTankSelected);
			if (HangarConfigProvider.selectedTank != null) {
				OnTankSelected(HangarConfigProvider.selectedTank);
			}
		}
		
		private void OnDisable() {
			if (!HangarConfigProvider.destroyed) {
				HangarConfigProvider.onTankSelected.RemoveListener(OnTankSelected);
			}
			
			if (createTankCoroutine != null) {
				StopCoroutine(createTankCoroutine);
				createTankCoroutine = null;
			}
		}
		
		//
		//
		//

		private Coroutine createTankCoroutine;
		private void OnTankSelected(TankConfig tankConfig) {
			if (createTankCoroutine != null) {
				StopCoroutine(createTankCoroutine);
			}
			createTankCoroutine = StartCoroutine(CreateTankCoroutine(tankConfig));
		}
		
		private IEnumerator CreateTankCoroutine(TankConfig tankConfig) {
			tankParentTransform.DestroyChildren();
			
			// Get tank prefab
			GameObject tankPrefab = null;
			yield return StreamingData.LoadAssetAsync<GameObject>(tankConfig.assetBundle, tankConfig.prefab, (resultValue) => {
				tankPrefab = resultValue;
			});

			if (!tankPrefab) {
				Debug.LogError($"Tank prefab hasn't been loaded: {tankConfig.assetBundle}, {tankConfig.prefab}");
				yield break;
			}

			// Instantiate prefab
			var instance = Instantiate(tankPrefab);
			instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
			instance.name = $"Tank: {tankConfig.name}";
		}
	}
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Demo.Hangar {
	///
	/// Configs storage for hangar
	/// Sources: <see cref="userConfigSubPath">, and <see cref="tanksCollectionConfigSubPath">
	///
	/// Great advantage of using this script is code decoupling
	///
	public class HangarConfigProvider : AutoInstanceMonoBehaviour<HangarConfigProvider> {
		private static readonly string userConfigSubPath = "user.yml";
		private static readonly string tanksCollectionConfigSubPath = "tanks.yml";

		private StreamingData StreamingData;
		protected override void AutoInstanceInit() {
			this.StreamingData = StreamingData.instance;
			
			if (loadCoroutine != null) {
				StopCoroutine(loadCoroutine);
			}
			loadCoroutine = StartCoroutine(LoadCoroutine());
		}

		public UserConfig user { get; private set; }
		public TankConfig[] tanksCollection { get; private set; }
		public TankConfig selectedTank { get; private set; }

		public UserConfigEvent onUserChanged = new UserConfigEvent();
		public TanksCollectionEvent onTanksCollectionChanged = new TanksCollectionEvent();
		public TankConfigEvent onTankSelected = new TankConfigEvent();

		private Coroutine loadCoroutine;

		public void SetUser(UserConfig userConfig) {
			user = userConfig;
			PersistentData.WriteYaml(userConfigSubPath, userConfig);
			onUserChanged.Invoke(userConfig);
		}

		public void SetTanksCollection(TankConfig[] tankConfigs) {
			tanksCollection = tankConfigs;
			onTanksCollectionChanged.Invoke(tankConfigs);
		}

		public void SetSelectedTank(TankConfig tankConfig) {
			if (selectedTank != tankConfig) {
				selectedTank = tankConfig;
				onTankSelected.Invoke(tankConfig);
			}
		}

		private IEnumerator LoadCoroutine() {
			TanksCollectionConfig tanksCollectionConfig = null;
			yield return StreamingData.LoadDataAsync<string>(tanksCollectionConfigSubPath, (resultValue) => {
				tanksCollectionConfig = YamlWrapper.Deserialize<TanksCollectionConfig>(resultValue);
			});

			int tankConfigIndex = 0;
			var tankConfigs = new TankConfig[tanksCollectionConfig.tankConfigs.Length];
			foreach (string tankConfigSubPath in tanksCollectionConfig.tankConfigs) {
				yield return StreamingData.LoadDataAsync<string>(tankConfigSubPath, (resultValue) => {
					var tankConfig = YamlWrapper.Deserialize<TankConfig>(resultValue);
					tankConfigs[tankConfigIndex] = tankConfig;
					tankConfigIndex++;
				});
			}
			SetTanksCollection(tankConfigs);

			var persistentUserConfig = PersistentData.ReadYaml<UserConfig>(userConfigSubPath);
			if (persistentUserConfig != null) {
				SetUser(persistentUserConfig);
				yield break;
			}
			
			yield return StreamingData.LoadDataAsync<string>(userConfigSubPath, (resultValue) => {
				var userConfig = YamlWrapper.Deserialize<UserConfig>(resultValue);
				SetUser(userConfig);
			});
		}

		[Serializable]
		public class UserConfigEvent : UnityEvent<UserConfig> { }

		[Serializable]
		public class TanksCollectionEvent : UnityEvent<TankConfig[]> { }
		
		[Serializable]
		public class TankConfigEvent : UnityEvent<TankConfig> { }
	}
}

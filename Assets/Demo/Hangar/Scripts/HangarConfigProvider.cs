using System;
using System.Collections;
using System.Collections.Generic;
using Custom.Data;
using Custom.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Demo.Hangar {
	///
	/// Configs storage for hangar
	/// Sources: <see cref="userConfigSubPath">, and <see cref="tanksCollectionConfigSubPath">
	///
	/// Great advantage of using this script is code decoupling
	///
	public class HangarConfigProvider : AutoInstanceMonoBehaviour<HangarConfigProvider>
	{
		private static readonly string userConfigSubPath = "user.yml";
		private static readonly string tanksCollectionConfigSubPath = "tanks.yml";

		public UserConfig user { get; private set; }
		public TankConfig[] tanksCollection { get; private set; }
		public TankConfig selectedTank { get; private set; }

		public UserConfigEvent onUserChanged = new UserConfigEvent();
		public TanksCollectionEvent onTanksCollectionChanged = new TanksCollectionEvent();
		public TankConfigEvent onTankSelected = new TankConfigEvent();

		//
		// "Injects"
		//

		private StreamingData StreamingData;

		//
		// API
		//

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

		//
		// Callbacks from Unity
		//
		
		private Coroutine loadCoroutine;
		new protected void Awake() {
			base.Awake();
			this.StreamingData = StreamingData.instance;
			
			
			if (loadCoroutine != null) {
				StopCoroutine(loadCoroutine);
			}
			loadCoroutine = StartCoroutine(LoadCoroutine());
		}

		//
		//
		//

		private IEnumerator LoadCoroutine() {
			string[] tankConfigPaths = null;
			yield return LoadConfig<TanksCollectionConfig>(tanksCollectionConfigSubPath, (resultValue) => {
				tankConfigPaths = (resultValue != null)
					? resultValue.tankConfigs
					: new string[0];
			});

			var tankConfigs = new List<TankConfig>();
			foreach (var tankConfigPath in tankConfigPaths) {
				yield return LoadConfig<TankConfig>(tankConfigPath, (resultValue) => {
					if (resultValue != null) { tankConfigs.Add(resultValue); }
				});
			}
			SetTanksCollection(tankConfigs.ToArray());

			var userConfig = PersistentData.ReadYaml<UserConfig>(userConfigSubPath);
			if (userConfig == null) {
				yield return LoadConfig<UserConfig>(userConfigSubPath, (resultValue) => {
					userConfig = resultValue ?? new UserConfig {
						ownedTanksUids = new List<string>()
					};
				});
			}
			SetUser(userConfig);
		}
		
		public Coroutine LoadConfig<T>(string subPath, Action<T> callback)
			where T : class
		{
			return StreamingData.LoadDataAsync<string>(subPath, (resultValue) => {
				T result = string.IsNullOrEmpty(resultValue)
					? default(T)
					: YamlWrapper.Deserialize<T>(resultValue);
				callback.Invoke(result);
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

using System;

namespace Demo.Hangar {
	///
	/// Tanks collection configuration file layout.
	///
	/// <see cref="HangarConfigProvider">
	/// See "Assets/StreamingAssets/tanks.yml"
	///
	[Serializable]
	public class TanksCollectionConfig {
		public string[] tankConfigs { get; set; }
	}
}

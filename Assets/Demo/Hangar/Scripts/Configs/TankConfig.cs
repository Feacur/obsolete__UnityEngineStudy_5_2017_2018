using System;

namespace Demo.Hangar {
	///
	/// Specific tank configuration file layout.
	///
	/// <see cref="HangarConfigProvider">
	/// See "StreamingAssets/Tanks collection/*.yml"
	///
	[Serializable]
	public class TankConfig {
		public string uid { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		// visuals
		public string assetBundle { get; set; }
		public string prefab { get; set; }
		// physics
		public float mass { get; set; } // kg
		public float speed { get; set; } // meters/sec
		// price
		public decimal price { get; set; }
		public CurrencyType currency { get; set; }
	}
}

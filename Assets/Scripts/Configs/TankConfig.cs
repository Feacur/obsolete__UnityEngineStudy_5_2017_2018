using System;

///
/// Specific tank configuration file layout.
///

[Serializable]
public class TankConfig {
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

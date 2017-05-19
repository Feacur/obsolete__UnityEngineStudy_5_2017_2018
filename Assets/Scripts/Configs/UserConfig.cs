using System;

///
/// User configuration file layout.
///

[Serializable]
public class UserConfig {
	public int silver { get; set; }
	public int gold { get; set; }
	public string[] tanks { get; set; }
}
using System;

///
/// User configuration file layout.
///

[Serializable]
public class UserConfig {
	public decimal silver { get; set; }
	public decimal gold { get; set; }
	public string[] ownedTanksUids { get; set; }
}

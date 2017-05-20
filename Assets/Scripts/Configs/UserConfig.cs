using System;
using System.Collections.Generic;

///
/// User configuration file layout.
///

[Serializable]
public class UserConfig {
	public decimal silver { get; set; }
	public decimal gold { get; set; }
	public List<string> ownedTanksUids { get; set; }
}

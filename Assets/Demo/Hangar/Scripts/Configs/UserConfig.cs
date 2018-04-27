using System;
using System.Collections.Generic;

namespace Demo.Hangar {
	///
	/// User configuration file layout.
	///
	/// <see cref="HangarConfigProvider">
	/// See "StreamingAssets/user.yml"
	///
	[Serializable]
	public class UserConfig {
		public decimal silver { get; set; }
		public decimal gold { get; set; }
		public List<string> ownedTanksUids { get; set; }
	}

	public static class UserConfigExtensions {
		public static bool HasTank(this UserConfig userConfig, string uid) {
			return userConfig.ownedTanksUids.Exists(ownedTankUid => ownedTankUid == uid);
		}
	}
}

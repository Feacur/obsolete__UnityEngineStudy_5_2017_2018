namespace Custom.Utils
{
	///
	/// Converts seconds into a formatted string [XXt:XXt]
	///
	public static class TimeFormatter
	{
		public static string SecondsToFormat(int totalSeconds)
		{
			int seconds = totalSeconds % 60;
			int totalMinutes = totalSeconds / 60;
			if (totalMinutes == 0)
			{
				return $"{0:D2}m:{seconds:D2}s";
			}

			int minutes = totalMinutes % 60;
			int totalHours = totalMinutes / 60;
			if (totalHours == 0)
			{
				return $"{minutes:D2}m:{seconds:D2}s";
			}

			int hours = totalHours % 24;
			int totalDays = totalHours / 24;
			if (totalDays == 0)
			{
				return $"{hours:D2}h:{minutes:D2}m";
			}

			int days = totalDays % 7;
			int totalWeeks = totalDays / 7;
			if (totalWeeks == 0)
			{
				return $"{days:D2}d:{hours:D2}h";
			}

			return $"{totalWeeks:D2}w:{days:D2}d";
		}
	}
}

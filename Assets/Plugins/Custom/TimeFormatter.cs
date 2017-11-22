///
/// Код для перевода секунд в формат XXt:XXt
///
public static class TimeFormatter {
	private static readonly string SECONDS_FORMAT = "{0:D2}s";
	private static readonly string MINUTES_FORMAT = "{0:D2}m:{1:D2}s";
	private static readonly string HOURS_FORMAT   = "{0:D2}h:{1:D2}m";
	private static readonly string DAYS_FORMAT    = "{0:D2}d:{1:D2}h";
	private static readonly string WEEKS_FORMAT   = "{0:D2}w:{1:D2}d";

	public static string SecondsToFormat(int totalSeconds) {
		int seconds = totalSeconds % 60;
		int totalMinutes = totalSeconds / 60;
		if (totalMinutes == 0) {
			return string.Format(SECONDS_FORMAT, seconds);
		}

		int minutes = totalMinutes % 60;
		int totalHours = totalMinutes / 60;
		if (totalHours == 0) {
			return string.Format(MINUTES_FORMAT, minutes, seconds);
		}

		int hours = totalMinutes % 24;
		int totalDays = totalMinutes / 24;
		if (totalDays == 0) {
			return string.Format(HOURS_FORMAT, hours, minutes);
		}

		int days = totalDays % 7;
		int totalWeeks = totalDays / 7;
		if (totalWeeks == 0) {
			return string.Format(DAYS_FORMAT, days, hours);
		}

		return string.Format(WEEKS_FORMAT, totalWeeks, days);
	}
}

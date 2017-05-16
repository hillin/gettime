using System;

namespace Hillinworks.Utilities.GetTime
{
	internal static class DateTimeHelpers
	{
		public static DateTime Parse(string dateTimeString, out DateTimeMode mode)
		{
			if (string.IsNullOrWhiteSpace(dateTimeString))
			{
				mode = DateTimeMode.Time;
				return DateTime.Now;
			}

			if (DateTime.TryParse(dateTimeString, out var exactTime))
			{
				mode = exactTime.TimeOfDay.Equals(TimeSpan.Zero) ? DateTimeMode.Date : DateTimeMode.Time;
				return exactTime;
			}

			switch (dateTimeString.ToLower())
			{
				case "now":
					mode = DateTimeMode.Time;
					return DateTime.Now;
				case "today":
					mode = DateTimeMode.Date;
					return DateTime.Now;
				case "tomorrow":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddDays(1);
				case "yesterday":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddDays(-1);
			}

			var words = dateTimeString.Split('-');

			if (words.Length != 2 && words.Length != 3)
			{
				throw new ArgumentException("unrecognizable DateTime", nameof(dateTimeString));
			}

			int factor;
			switch (words[0].ToLower())
			{
				case "next":
					factor = 1;
					break;
				case "previous":
				case "last":
					factor = -1;
					break;
				default:
					throw new ArgumentException("unrecognizable DateTime", nameof(dateTimeString));
			}

			var amount = 1.0;
			if (words.Length == 3)
			{
				if (!double.TryParse(words[1], out amount))
				{
					throw new ArgumentException($"unexpected '{words[1]}'", nameof(dateTimeString));
				}
			}

			amount *= factor;

			var unitWord = words[words.Length - 1];
			switch (unitWord.ToLower())
			{
				case "millisecond":
				case "milliseconds":
				case "ms":
					mode = DateTimeMode.Time;
					return DateTime.Now.AddMilliseconds(amount);
				case "second":
				case "seconds":
				case "s":
					mode = DateTimeMode.Time;
					return DateTime.Now.AddSeconds(amount);
				case "minute":
				case "minutes":
				case "min":
				case "mins":
				case "m":
					mode = DateTimeMode.Time;
					return DateTime.Now.AddMinutes(amount);
				case "hour":
				case "hours":
				case "h":
					mode = DateTimeMode.Time;
					return DateTime.Now.AddHours(amount);
				case "day":
				case "days":
				case "d":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddDays(amount);
				case "week":
				case "weeks":
				case "w":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddWeeks(amount);
				case "month":
				case "months":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddMonths(amount);
				case "year":
				case "years":
				case "yr":
				case "yrs":
				case "y":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddYears(amount);
				case "decade":
				case "decades":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddDecades(amount);
				case "century":
				case "centuries":
					mode = DateTimeMode.Date;
					return DateTime.Now.AddCenturies(amount);
				default:
					throw new ArgumentException("unrecognizable DateTime", nameof(dateTimeString));
			}

		}

		public static DateTime AddWeeks(this DateTime datetime, double amount)
		{
			return datetime.AddDays(amount * 7);
		}

		public static DateTime AddMonths(this DateTime datetime, double amount)
		{
			return amount.Equals((int)amount)
				? datetime.AddMonths((int)amount)
				: datetime.AddDays((int)(amount * 365.25 / 12));
		}

		public static DateTime AddYears(this DateTime datetime, double amount)
		{
			return amount.Equals((int)amount)
				? datetime.AddYears((int)amount)
				: datetime.AddDays((int)(amount * 365.25));
		}

		public static DateTime AddDecades(this DateTime datetime, double amount)
		{
			return amount.Equals((int)amount)
				? datetime.AddYears((int)amount * 10)
				: datetime.AddDays((int)(amount * 3652.5));
		}

		public static DateTime AddCenturies(this DateTime datetime, double amount)
		{
			return amount.Equals((int)amount)
				? datetime.AddYears((int)amount * 100)
				: datetime.AddDays((int)(amount * 36525));
		}
	}
}

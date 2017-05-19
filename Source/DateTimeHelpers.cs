using System;
using System.Text.RegularExpressions;

namespace Hillinworks.Utilities.GetTime
{
	internal static class DateTimeHelpers
	{
		public static DateTime Parse(string dateTimeString, out DateTimeMode mode)
		{
			Exception UnrecognizableException()
			{
				return new ArgumentException("unrecognizable DateTime", nameof(dateTimeString));
			}

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


			var match = Regex.Match(dateTimeString, @"^([\+\-])([\d\.]+)(\w+)$");
			if (match.Success)
			{
				int factor;
				switch (match.Groups[1].Value)
				{
					case "+":
						factor = 1;
						break;
					case "-":
						factor = -1;
						break;
					default:
						throw UnrecognizableException();
				}
				if (!double.TryParse(match.Groups[2].Value, out var amount))
				{
					throw UnrecognizableException();
				}
				var unit = match.Groups[3].Value;

				return DateTimeHelpers.GetTimeFromNow(factor, amount, unit, true, out mode);
			}

			match = Regex.Match(dateTimeString, @"^(next|previous|last)\-(([\d\.]+)(\-)?)?(\w+)$", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				int factor;
				switch (match.Groups[1].Value.ToLower())
				{
					case "next":
						factor = 1;
						break;
					case "previous":
					case "last":
						factor = -1;
						break;
					default:
						throw UnrecognizableException();
				}
				var amountString = match.Groups[3].Value;
				var amount = 1.0;
				if (!string.IsNullOrEmpty(amountString) && !double.TryParse(amountString, out amount))
				{
					throw UnrecognizableException();
				}

				var unit = match.Groups[5].Value;

				return DateTimeHelpers.GetTimeFromNow(factor, amount, unit, string.IsNullOrEmpty(match.Groups[4].Value), out mode);
			}

			match = Regex.Match(dateTimeString, @"^([\d\.]+)(\-)?(\w+)-(ago|later)$", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				int factor;
				switch (match.Groups[4].Value.ToLower())
				{
					case "later":
						factor = 1;
						break;
					case "ago":
						factor = -1;
						break;
					default:
						throw UnrecognizableException();
				}
				var amountString = match.Groups[1].Value;
				var amount = 1.0;
				if (!string.IsNullOrEmpty(amountString) && !double.TryParse(amountString, out amount))
				{
					throw UnrecognizableException();
				}

				var unit = match.Groups[3].Value;

				return DateTimeHelpers.GetTimeFromNow(factor, amount, unit, string.IsNullOrEmpty(match.Groups[2].Value), out mode);
			}

			throw UnrecognizableException();
		}

		private static DateTime GetTimeFromNow(int factor, double amount, string unit, bool shortUnit, out DateTimeMode mode)
		{
			amount *= factor;

			if (shortUnit)
			{
				switch (unit.ToLower())
				{
					case "ms":
						mode = DateTimeMode.Time;
						return DateTime.Now.AddMilliseconds(amount);
					case "s":
						mode = DateTimeMode.Time;
						return DateTime.Now.AddSeconds(amount);
					case "min":
					case "mins":
					case "m":
						mode = DateTimeMode.Time;
						return DateTime.Now.AddMinutes(amount);
					case "h":
						mode = DateTimeMode.Time;
						return DateTime.Now.AddHours(amount);
					case "d":
						mode = DateTimeMode.Date;
						return DateTime.Now.AddDays(amount);
					case "w":
						mode = DateTimeMode.Date;
						return DateTime.Now.AddWeeks(amount);
					case "yr":
					case "yrs":
					case "y":
						mode = DateTimeMode.Date;
						return DateTime.Now.AddYears(amount);
					default:
						throw new ArgumentException("unrecognizable DateTime");
				}
			}

			switch (unit.ToLower())
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
					throw new ArgumentException("unrecognizable DateTime");
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

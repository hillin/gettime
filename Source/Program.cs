using System;
using System.Globalization;
using CommandLine;

namespace Hillinworks.Utilities.GetTime
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			if (Parser.Default.ParseArguments(args, options))
			{
				Program.Output(options);
			}
			else
			{
				Console.WriteLine(options.GetUsage());
			}
		}

		private static void Output(Options options)
		{
			try
			{
				DateTimeMode mode;

				var time = DateTimeHelpers.Parse(options.Base, out mode);
				time = time.AddMilliseconds(options.OffsetMilliseconds);
				time = time.AddSeconds(options.OffsetSeconds);
				time = time.AddMinutes(options.OffsetMinutes);
				time = time.AddHours(options.OffsetHours);
				time = time.AddDays(options.OffsetDays);
				time = time.AddWeeks(options.OffsetWeeks);
				time = time.AddMonths(options.OffsetMonths);
				time = time.AddYears(options.OffsetYears);
				time = time.AddDecades(options.OffsetDecades);
				time = time.AddCenturies(options.OffsetCenturies);
				time = time.Add(options.Offset);

				if (!string.IsNullOrWhiteSpace(options.OutputFormat))
				{
					Console.WriteLine(time.ToString(options.OutputFormat));
					return;
				}

				mode = options.DateTimeMode == DateTimeMode.Auto ? mode : options.DateTimeMode;

				if (options.LongFormat)
				{
					Console.WriteLine(mode == DateTimeMode.Date ? time.ToLongDateString() : time.ToLongTimeString());
					return;
				}

				if (options.UnixFormat)
				{
					var unixTime = (DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks) / TimeSpan.TicksPerSecond;
					Console.WriteLine(unixTime);
					return;
				}

				if (options.ShortFormat)
				{
					Console.WriteLine(mode == DateTimeMode.Date ? time.ToShortDateString() : time.ToShortTimeString());
					return;
				}

				if (options.DateTimeMode == DateTimeMode.Auto)
				{
					Console.WriteLine(time.ToString(CultureInfo.CurrentCulture));
					return;
				}

				Console.WriteLine(mode == DateTimeMode.Date ? time.ToShortDateString() : time.ToShortTimeString());
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex.Message);
				Environment.Exit(-1);
			}
		}

	}
}

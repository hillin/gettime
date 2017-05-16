using System;
using CommandLine;
using CommandLine.Text;

namespace Hillinworks.Utilities.GetTime
{
	internal class Options
	{
		[ValueOption(0)]
		[Option(HelpText = "The base time, could be an absolute time, or 'now'(default), 'next-second', 'tomorrow' etc.", DefaultValue = "now")]
		public string Base { get; set; }

		[Option('a', "add", HelpText = "Offset the current time", MutuallyExclusiveSet = "Add")]
		public TimeSpan Offset { get; set; }
		[Option("add-milliseconds", HelpText = "Offset the current time by specified milliseconds", MutuallyExclusiveSet = "Add")]
		public double OffsetMilliseconds { get; set; }
		[Option("add-seconds", HelpText = "Offset the current time by specified seconds", MutuallyExclusiveSet = "Add")]
		public double OffsetSeconds { get; set; }
		[Option("add-minutes", HelpText = "Offset the current time by specified minutes", MutuallyExclusiveSet = "Add")]
		public double OffsetMinutes { get; set; }
		[Option("add-hours", HelpText = "Offset the current time by specified hours", MutuallyExclusiveSet = "Add")]
		public double OffsetHours { get; set; }
		[Option("add-days", HelpText = "Offset the current time by specified days", MutuallyExclusiveSet = "Add")]
		public double OffsetDays { get; set; }
		[Option("add-weeks", HelpText = "Offset the current time by specified weeks", MutuallyExclusiveSet = "Add")]
		public double OffsetWeeks { get; set; }
		[Option("add-months", HelpText = "Offset the current time by specified months", MutuallyExclusiveSet = "Add")]
		public double OffsetMonths { get; set; }
		[Option("add-years", HelpText = "Offset the current time by specified years", MutuallyExclusiveSet = "Add")]
		public double OffsetYears { get; set; }
		[Option("add-decades", HelpText = "Offset the current time by specified decades", MutuallyExclusiveSet = "Add")]
		public double OffsetDecades { get; set; }
		[Option("add-centuries", HelpText = "Offset the current time by specified years", MutuallyExclusiveSet = "Add")]
		public double OffsetCenturies{ get; set; }
		[Option('f', "format", HelpText = "Specify the output format. See https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx for details.", MutuallyExclusiveSet = "Format")]
		public string OutputFormat { get; set; }
		[Option('l', "long", HelpText = "Output in long format", MutuallyExclusiveSet = "Format")]
		public bool LongFormat { get; set; }
		[Option('s', "short", HelpText = "Output in short format", MutuallyExclusiveSet = "Format")]
		public bool ShortFormat { get; set; }
		[Option('u', "unix", HelpText = "Output in Unix time format", MutuallyExclusiveSet = "Format")]
		public bool UnixFormat { get; set; }
		[Option('m', "mode", HelpText = "Output date or time", DefaultValue = DateTimeMode.Auto)]
		public DateTimeMode DateTimeMode { get; set; }

		// todo: support time-zone
		//[Option('z', "time-zone", HelpText ="Time zone of input and output time. Could be overridden by input-time-zone and output-time-zone")]
		//public string TimeZone { get; set; }
		//[Option("input-time-zone", HelpText = "Time zone of input time")]
		//public string InputTimeZone { get; set; }
		//[Option("output-time-zone", HelpText = "Time zone of output time")]
		//public string OutputTimeZone { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}

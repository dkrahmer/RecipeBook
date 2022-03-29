using Common.Structs;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
	public class Helpers
	{
		private static readonly Regex _scalableRegEx = new Regex(@"<[^<>]+>", RegexOptions.Compiled);

		public static string UpdateScalableNumbers(string str, Amount scaleAmount)
		{
			if (string.IsNullOrWhiteSpace(str))
				return str;

			var matches = _scalableRegEx.Matches(str);
			foreach (Match match in matches)
			{
				var findValue = match.Value;
				var matchParts = findValue.Trim(new char[] { '<', '>' }).Split(':');

				string replacementValueStr;
				try
				{
					var replacementValue = new Amount(matchParts[0]) * scaleAmount;
					char convertType = '\0';
					int decimalPlaces = 3; // default
					if (matchParts.Length >= 2)
					{
						string convertTypePhrase = matchParts[1];
						if (convertTypePhrase.StartsWith('.'))
						{
							convertType = '.';
							if (convertTypePhrase.Length > 1)
							{
								int.TryParse(convertTypePhrase.Substring(1), out decimalPlaces);
							}
						}
						else if (convertTypePhrase.StartsWith('/'))
						{
							convertType = '/';
						}
					}

					if (convertType == '.')
					{
						replacementValueStr = replacementValue.ToDecimal(decimalPlaces).ToString();
					}
					else if (convertType == '/')
					{
						replacementValueStr = replacementValue.ToRational().ToString("W");
					}
					else
					{
						replacementValueStr = replacementValue.ToString();
					}
				}
				catch (Exception ex)
				{
					replacementValueStr = $"<error: {ex.Message}>";
				}

				str = str.Replace(findValue, replacementValueStr);
			}

			return str;
		}

		private static readonly Regex CleanLineRegex = new Regex(@"^(?<Number>[0-9]+[ \t]*[-\.\)\]\}\>][ \t]*)(?<Text>.+)$", RegexOptions.Compiled);

		public static string GetCleanLines(string lines)
		{
			var outString = new StringBuilder();
			using (var reader = new StringReader(lines))
			{
				for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
				{
					var match = CleanLineRegex.Match(line);
					if (match.Success)
						line = match.Groups["Text"].Value;
					outString.Append(line);
					outString.Append("\n");
				}
			}

			return outString.ToString();
		}
	}
}

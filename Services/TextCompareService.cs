using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System.Text;

namespace ConflictResolution.Services
{
	public class TextCompareService
	{
		public string Compare(string original, string update)
		{
			var diffBuilder = new InlineDiffBuilder(new Differ());
			var diffResult = diffBuilder.BuildDiffModel(
				 string.Join('\n', SplitHtmlIntoLines(original)),
				 string.Join('\n', SplitHtmlIntoLines(update))
			);

			StringBuilder sb = DiffConsoleLogStringBuilder(diffResult);
			return sb.ToString();
		}

		private static StringBuilder DiffConsoleLogStringBuilder(DiffPaneModel diffResult)
		{
			var sb = new StringBuilder();

			foreach (var line in diffResult.Lines)
			{
				switch (line.Type)
				{
					case ChangeType.Inserted:
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"+ {line.Text}");
						sb.AppendLine($"+ {line.Text}");
						break;

					case ChangeType.Deleted:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"- {line.Text}");
						sb.AppendLine($"- {line.Text}");
						break;

					case ChangeType.Unchanged:
						Console.ForegroundColor = ConsoleColor.Gray;
						Console.WriteLine($"  {line.Text}");
						sb.AppendLine($"  {line.Text}");
						break;
				}
			}

			Console.ResetColor();
			return sb;
		}

		private List<string> SplitHtmlIntoLines(string htmlText)
		{
			if (string.IsNullOrWhiteSpace(htmlText))
				return new List<string>();

			var lines = new List<string>();

			// Normalize whitespace
			htmlText = htmlText.Replace("\r", "").Replace("\n", "");

			// Regex that matches tags or text between tags
			var regex = new System.Text.RegularExpressions.Regex(@"(<[^>]+>|[^<]+)");
			var matches = regex.Matches(htmlText);

			foreach (System.Text.RegularExpressions.Match match in matches)
			{
				var part = match.Value.Trim();
				if (!string.IsNullOrEmpty(part))
					lines.Add(part);
			}

			return lines;
		}
	}
}

using System.Text.Json;

public class DataLoaderService
{
	public async Task<IEnumerable<ReportTextBlocks>> GetAllTextBlocks(string dataFolder)
	{
		if (!Directory.Exists(dataFolder))
		{
			Console.WriteLine($"Data folder not found: {dataFolder}");
			return Enumerable.Empty<ReportTextBlocks>();
		}

		// Get all JSON files in Data/
		var jsonFiles = Directory.GetFiles(dataFolder, "*.json", SearchOption.TopDirectoryOnly);

		var reportTextBlocksList = new List<ReportTextBlocks>();

		foreach (var file in jsonFiles)
		{
			try
			{
				string jsonContent = await File.ReadAllTextAsync(file);

				var report = JsonSerializer.Deserialize<ReportTextBlocks>(
					 jsonContent,
					 new JsonSerializerOptions
					 {
						 PropertyNameCaseInsensitive = true,
						 ReadCommentHandling = JsonCommentHandling.Skip,
						 AllowTrailingCommas = true
					 });

				if (report != null)
				{
					reportTextBlocksList.Add(report);
					Console.WriteLine($"Loaded: {Path.GetFileName(file)}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading {file}: {ex.Message}");
			}
		}

		return reportTextBlocksList;
	}
}

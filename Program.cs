using ConflictResolution.Models;
using ConflictResolution.Services;

var dataLoader = new DataLoaderService();
var textCompare = new TextCompareService();
var conflictResolver = new ConflictResolutionService();

var data = await dataLoader.GetAllTextBlocks(Path.Combine(AppContext.BaseDirectory, "Data"));
Console.WriteLine("---\nTotal test cases: {0}", data.Count());
foreach (var item in data)
{
	Console.WriteLine("---Compare Result---");
	var persistedChanges = textCompare.Compare(item.OriginalHtmlText, item.LatestHtmlPersisted);
	var requestedChanges = textCompare.Compare(item.OriginalHtmlText, item.NewHtmlText);
	try
	{
		var resolved = conflictResolver.Resolve(persistedChanges, requestedChanges);
		Console.Write("Resolved:");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine(resolved);
	} catch (TextConflictException e)
	{
		Console.ResetColor();
		Console.Write("Observe conflict on update:");
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write($" - {e.PersistedLine}");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($" + {e.RequestedLine}");

	}
	Console.ResetColor();
}

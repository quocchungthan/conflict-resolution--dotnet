using ConflictResolution.Services;

var dataLoader = new DataLoaderService();
var textCompare = new TextCompareService();

var data = await dataLoader.GetAllTextBlocks(Path.Combine(AppContext.BaseDirectory, "Data"));
Console.WriteLine("---\nTotal test cases: {0}", data.Count());
foreach (var item in data)
{
	Console.WriteLine("---Compare Result---");
	textCompare.Compare(item.OriginalHtmlText, item.NewHtmlText);
}

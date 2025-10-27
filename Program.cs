var dataLoader = new DataLoaderService();
var data = await dataLoader.GetAllTextBlocks(Path.Combine(AppContext.BaseDirectory, "Data"));
Console.WriteLine("---\nTotal test cases: {0}", data.Count());
foreach (var item in data)
{
	Console.WriteLine("OriginalHtml={0}, Latest={0}, NewHtml={0}", item.OriginalHtmlText, item.LatestHtmlPersisted, item.NewHtmlText);
}
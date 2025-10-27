var dataLoader = new DataLoaderService();
var data = await dataLoader.GetAllTextBlocks(Path.Combine(AppContext.BaseDirectory, "Data"));
Console.WriteLine(data.Count());
foreach (var item in data)
{
	Console.WriteLine(item);
}
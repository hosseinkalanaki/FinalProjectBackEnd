public interface ITcpDataCollectorService
{
	Task<List<CenterViewModel>> CollectDataAsync();
}

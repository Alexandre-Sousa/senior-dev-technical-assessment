public class Downloader {
	
	private static List<string> cache = new List<string>();
    private static readonly HttpClient client = new HttpClient();
    private static readonly object lockObj = new object();

    public static async Task Main(string[] args){
        
        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            string url = "https://example.com/data/" + i;
            tasks.Add(DownloadAsync(url));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine("Downloads completed");
        Console.WriteLine("Cache size: " + cache.Count);

    }

    private static async Task DownloadAsync(string url){
        
    	var data = await client.GetStringAsync(url);

    	lock(lockObj){
            cache.Add(data);
        }

    }

}
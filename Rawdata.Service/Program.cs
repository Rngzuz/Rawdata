using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Rawdata.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                // Instruct the use of Kestrel (think IIS is the default) server
                .UseKestrel()
                .UseStartup<Startup>();
    }
}

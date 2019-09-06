using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


// We can simply replace a this web server with kestrel also.
namespace SampleDotNetCoreApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EventSourceLogging.Log.Startup();
            CreateWebHostBuilder(args).Build().Run();
            // We need to see if we can override some functions which we can override while cancellation also.
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
    
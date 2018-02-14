using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace MSRedis11
{
    // https://dotnetcoretutorials.com/2017/01/06/using-redis-cache-net-core/
    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.redis.rediscache?view=aspnetcore-1.1
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using WebApiTestingSkeleton.Core.Web;

namespace WebApiTestingSkeleton.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = WebHostBuilderFactory.Build<Startup>(args);
            var host = hostBuilder.Build();
            host.Run();
        }
    }
}

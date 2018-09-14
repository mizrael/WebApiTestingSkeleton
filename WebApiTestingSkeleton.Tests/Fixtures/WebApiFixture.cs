using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace WebApiTestingSkeleton.Tests.Fixtures
{
    public class WebApiFixture<TStartup> : IDisposable
        where TStartup : class
    {
        private readonly TestServer _server;

        public WebApiFixture()
        {
            var hostBuilder = Core.Web.WebHostBuilderFactory.Build<TStartup>(new string[]{});
            
            _server = new TestServer(hostBuilder);
            
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost");
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
        }
    }
}

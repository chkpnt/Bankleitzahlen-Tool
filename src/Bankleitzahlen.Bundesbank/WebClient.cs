using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Bankleitzahlen.Bundesbank
{
    public interface IWebClient : IDisposable
    {
        Task<Stream> OpenReadTaskAsync(Uri address);
    }

    class DefaultWebClient : WebClient, IWebClient
    {

    }

    public interface IWebClientFactory
    {
        IWebClient CreateWebClient();
    }

    class WebClientFactory : IWebClientFactory
    {
        public IWebClient CreateWebClient() => new DefaultWebClient();
    }
}
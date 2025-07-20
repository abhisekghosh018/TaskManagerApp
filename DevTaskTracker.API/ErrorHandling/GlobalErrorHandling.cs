
namespace DevTaskTracker.API.ErrorHandling
{
    public class GlobalErrorHandling : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            throw new NotImplementedException();
        }
    }
}

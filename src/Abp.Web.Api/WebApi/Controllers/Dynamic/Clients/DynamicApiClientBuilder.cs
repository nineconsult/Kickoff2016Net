namespace Abp.WebApi.Controllers.Dynamic.Clients
{
    /// <summary>
    /// </summary>
    public static class DynamicApiClientBuilder
    {
        public static IApiClientBuilder<TService> For<TService>(string url)
        {
            return new ApiClientBuilder<TService>(url);
        }
    }
}
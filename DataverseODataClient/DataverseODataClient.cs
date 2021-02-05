using System.Net.Http;
using Simple.OData.Client;

namespace DataverseODataClient
{
    // this class is needed to enable dependency injection of ODataClientSettings
    public class DataverseODataClient : ODataClient
    {
        public DataverseODataClient(HttpClient httpClient)
            : base(new ODataClientSettings(httpClient))
        {
        }
    }
}
using Simple.OData.Client;

namespace DataverseODataClient
{
    // this class is needed to enable dependency injection of ODataClientSettings
    internal class DataverseODataClient : ODataClient
    {
        public DataverseODataClient(ODataClientSettings clientSettings)
            : base(clientSettings)
        {
        }
    }
}
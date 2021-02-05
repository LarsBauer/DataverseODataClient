# Dataverse OData Client

This package provides a ready-to-use OData Client for [Microsoft Dataverse Web API](https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/overview).

## Features

- based on the very popular and feature-rich [Simple.OData.Client](https://github.com/simple-odata-client/Simple.OData.Client)
- seamless integration in dependency injection and configuration concepts of .NET
- makes use of `IHttpClientFactory` to delegate `HttpClient` lifecycle to framework
- token handling based on [Azure Identity](https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme), which offers various ways to authenticate against Dataverse
- supports e2e-traceability of requests via correlation id
- developed and targeted at Azure Functions and Azure Web Apps

## Get started

To get started with this package just install the latest version from NuGet using the dotnet CLI:

```bash
dotnet add package BauerApps.DataverseODataClient
```

After installation you can register the client in `Startup.cs`...

```csharp
using System;
using DataverseODataClient.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DataverseODataClient.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataverseODataClient(options =>
            {
                options.OrganizationUrl = new Uri("https://my-organization.crm4.dynamics.com");
                options.ManagedIdentityClientId = "d0f19fa6-76ef-46cb-93ac-fcde5a4a6143"; // optional
            });
        }
    }
}
```

... and inject it for example into your `Controller`. As mentioned above this client basically is a preconfigured version of `Simple.OData.Client`, which means you can use it the same way. If you are new to `Simple.OData.Client` head over to the [wiki](https://github.com/simple-odata-client/Simple.OData.Client/wiki) for available features.

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.OData.Client;

namespace DataverseODataClient.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IODataClient _client;

        public SampleController(IODataClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<Person> Get(string id)
        {
            return await _client
                .For<Person>()
                .Key(id)
                .FindEntryAsync();
        }
    }
}
```

## Configuration

You can configure Dataverse OData Client using `DataverseODateClientOptions`

| Option                  | Required | Description                                                                                                                                                                                                                                          |
| ----------------------- | :------: | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| OrganizationUrl         |    ✔     | The base url of your Dataverse organization                                                                                                                                                                                                          |
| ManagedIdentityClientId |          | When using a Azure user-assigned [managed identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview) for authentication you have to specify the client id of the corresponding managed identity. |

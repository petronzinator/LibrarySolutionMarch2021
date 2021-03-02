using LibraryApi;
using LibraryApi.Controllers;
using LibraryApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApiIntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // this runs after Configure in our startup runs
            // All the services (including our ILookupServerStatus) are setup and ready to go.
            // We are goin to reach in and replace the WillsHealthCheckServerStatus with Folger's Chrystals.
            // We are going to FAKE IT
            builder.ConfigureServices(services =>
            {
                var descriptor = services.Single(services =>
                services.ServiceType == typeof(ILookupServerStatus)
                );
                services.Remove(descriptor);
                services.AddTransient<ILookupServerStatus, FakeServerStatus>();

            });
        }
    }

    public class FakeServerStatus: ILookupServerStatus
    {
        public StatusResponse GetStatusFor()
        {
            return new StatusResponse
            {
                Message = "Mike Was Here",
                LastChecked = new DateTime(1969, 4, 20, 23, 59, 00)
            };
        }

    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MapboxApi
{
    public class MapboxController
    {
        private readonly HttpClient _client;

        public MapboxController(HttpClient httpClient)
        {
            _client = httpClient;
        }

        [FunctionName("Health")]
        public async Task<IActionResult> RunHealthCheck(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for Health Check.");

            try
            {
                //var response = await _client.GetAsync($"");
                //response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                return new OkObjectResult(HealthCheckResult.Unhealthy("An unhealthy result.", exception));
            }

            return new OkObjectResult(HealthCheckResult.Healthy("A healthy result."));
        }
    }
}

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
using MapboxApi.Client;

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
                var response = await _client.GetAsync("https://api.mapbox.com/directions/v5/mapbox/");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                return new OkObjectResult(HealthCheckResult.Unhealthy("An unhealthy result.", exception));
            }

            return new OkObjectResult(HealthCheckResult.Healthy("A healthy result."));
        }

        [FunctionName("Route")]
        public async Task<IActionResult> RunGetPlaces(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
           ILogger log)
        {
            DirectionsResponse result = new DirectionsResponse();

            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                var routeTypeQueryParameter = req.Query["routetype"].ToString();
                if (string.IsNullOrEmpty(routeTypeQueryParameter))
                    return new BadRequestObjectResult("Please pass a route type parameter");

                var startLatQueryParameter = req.Query["startlat"].ToString();
                if (string.IsNullOrEmpty(startLatQueryParameter))
                    return new BadRequestObjectResult("Please pass a lat parameter");

                var startLonQueryParameter = req.Query["startlon"].ToString();
                if (string.IsNullOrEmpty(startLonQueryParameter))
                    return new BadRequestObjectResult("Please pass a lon parameter");

                var endLatQueryParameter = req.Query["endlat"].ToString().Trim('f', 'F');
                if (string.IsNullOrEmpty(endLatQueryParameter))
                    return new BadRequestObjectResult("Please pass a lat parameter");

                var endLonQueryParameter = req.Query["endlon"].ToString().Trim('f', 'F');
                if (string.IsNullOrEmpty(endLonQueryParameter))
                    return new BadRequestObjectResult("Please pass a lon parameter");

                // Need to move access key secure storage
                _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                var response = await _client.GetAsync($"https://api.mapbox.com/directions/v5/mapbox/{routeTypeQueryParameter}/{startLatQueryParameter}%2C{startLonQueryParameter}%3B{endLatQueryParameter}%2C{endLonQueryParameter}?alternatives=false&geometries=geojson&steps=false&access_token=pk.eyJ1Ijoicndvb2xsY290dCIsImEiOiJja2FnaWlsMHQwNnYyMnpvNWhhbTd1OTRiIn0.5pL3D0LvtE8A6Yuz40RhIA");
                result = await response.Content.ReadAsAsync<DirectionsResponse>();
            }
            catch (Exception exception)
            {
                // Error
            }

            return new OkObjectResult(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiyProjectCalc.TestHelpers.TestFixtures;

public abstract class BaseAPIEndpointClassFixture : BaseDatabaseClassFixture
{
    protected string apiBaseUrl = string.Empty;
    protected HttpClient _client;

    public BaseAPIEndpointClassFixture(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        var application = new WebApplicationFactory<DiyProjectCalc.Program>()
        .WithWebHostBuilder(builder =>
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "integrationsettings.json");
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile(configPath);
            });
        });
        _client = application.CreateClient();
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    protected async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        return await _client.GetAsync($"api/{endpoint}");
    }

    protected async Task<HttpResponseMessage> PostAsync(string endpoint, object content)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return await _client.PostAsync($"api/{endpoint}", stringContent);
    }

    protected async Task<HttpResponseMessage> PutAsync(string endpoint, object content)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return await _client.PutAsync($"api/{endpoint}", stringContent);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        return await _client.DeleteAsync($"api/{endpoint}");
    }

    protected async Task<T> Deserialize<T>(HttpResponseMessage response)
    {
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())!;
    }
}
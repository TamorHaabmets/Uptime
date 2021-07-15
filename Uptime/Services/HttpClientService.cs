using Domain;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Uptime.Services
{
    public class HttpClientService
    {
        private readonly IHttpClientFactory _clientFactory;
        public HttpClientService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<PriceList> GetTravelPrice()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = await JsonSerializer.DeserializeAsync<PriceList>(responseStream, options);

            return result;
        }

    }
}

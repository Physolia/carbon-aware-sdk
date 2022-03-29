using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CarbonAware.Plugins.WattTime.Model;

namespace CarbonAware.Plugins.WattTime
{
    public class CarbonAwareWattTimeClient
    {
        private readonly string baseUri = "https://api2.watttime.org/v2/";
        private HttpClient client;
        private string authToken;

        public CarbonAwareWattTimeClient()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseUri);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IEnumerable<GridEmissionDataPoint>> GetDataAsync(string balancingAuthority, string startTime, string endTime, int retries = 1)
        {
            var response = await this.client.GetAsync($"data?ba={balancingAuthority}&starttime={startTime}&endtime={endTime}");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<GridEmissionDataPoint>>(data.Result);
            } else if (retries > 0 && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                retries--;
                await this.GetAuthTokenAsync();
                return await this.GetDataAsync(balancingAuthority, startTime, endTime, retries);
            } else {
                throw new System.Exception($"Error getting data from WattTime: {response.StatusCode}");
            }
        }

        public async Task<Forecast> GetCurrentForecastAsync(string balancingAuthority, int retries = 1){
            var response = await this.client.GetAsync($"forecast?ba={balancingAuthority}");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Forecast>(data.Result);
            } else if (retries > 0 && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                retries--;
                await this.GetAuthTokenAsync();
                return await this.GetCurrentForecastAsync(balancingAuthority, retries);
            } else {
                throw new System.Exception($"Error getting data from WattTime: {response.StatusCode}");
            }
        }

        private void SetClientAuthToken()
        {
            this.client.DefaultRequestHeaders.Authorization = new 
                AuthenticationHeaderValue("Bearer", this.authToken);
        }

        private async Task GetAuthTokenAsync(){
            var authToken = Encoding.ASCII.GetBytes($"USER:PASS");
            client.DefaultRequestHeaders.Authorization = new 
                AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            var data = await client.GetStringAsync("login");
            var result = JsonSerializer.Deserialize<LoginResult>(data);
            this.authToken = result.token;
            this.SetClientAuthToken();
        }
    }
}

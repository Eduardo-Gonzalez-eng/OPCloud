using OPCloud.Client.Models;
using System.Net.Http.Json;

namespace OPCloud.Client.Services
{
    public class SensorService
    {
        private readonly HttpClient _http;

        public SensorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<InstrumentData>> GetSensoresAsync()
        {
            return await _http.GetFromJsonAsync<List<InstrumentData>>("api/instrument")
                   ?? new List<InstrumentData>();
        }

        // 👉 Obtiene SOLO el count desde Mongo (GET api/instrument/mongocount)
        public async Task<long> GetTodayMongoCountAsync()
        {
            return await _http.GetFromJsonAsync<long>("api/instrument/mongocount");
        }
    }
}

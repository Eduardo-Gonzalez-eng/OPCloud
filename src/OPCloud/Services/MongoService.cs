using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OPCloud.Models;
using OPCloud.Settings;

namespace OPCloud.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<InstrumentData> _collection;

        public MongoService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<InstrumentData>(settings.Value.CollectionName);
        }

        public async Task<List<InstrumentData>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();
        public async Task<(List<InstrumentData> Datos, long Count)> GetTodayWithCountAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var filter = Builders<InstrumentData>.Filter.Gte(d => d.FechaActual, today) &
                         Builders<InstrumentData>.Filter.Lt(d => d.FechaActual, tomorrow);

            // Traer los datos del día ordenados por fecha descendente
            var allToday = await _collection.Find(filter)
                .SortByDescending(d => d.FechaActual)
                .ToListAsync();

            // Agrupar por instante de tiempo (ej: segundo exacto)
            var groupedByTime = allToday
                .GroupBy(d => d.FechaActual)
                .OrderByDescending(g => g.Key)     // últimos tiempos primero
                .Take(5)                           // solo los últimos 5 tiempos
                .SelectMany(g => g)                // aplanar en una sola lista
                .OrderBy(d => d.FechaActual)       // opcional: volver a ordenar ascendente
                .ToList();

            var count = allToday.Count;

            return (groupedByTime, count);
        }


        /*
         public async Task<(List<InstrumentData> Datos, long Count)> GetTodayWithCountAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var filter = Builders<InstrumentData>.Filter.Gte(d => d.FechaActual, today) &
                         Builders<InstrumentData>.Filter.Lt(d => d.FechaActual, tomorrow);

            var datos = await _collection.Find(filter).Limit(5).ToListAsync();
            var count = await _collection.CountDocumentsAsync(filter);

            return (datos, count);
        }*/

        public async Task AddAsync(InstrumentData data)
            => await _collection.InsertOneAsync(data);

        public async Task AddManyAsync(List<InstrumentData> dataList)
            => await _collection.InsertManyAsync(dataList);
    }
}

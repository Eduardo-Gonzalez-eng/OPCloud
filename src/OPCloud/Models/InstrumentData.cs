using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OPCloud.Models
{
    public class InstrumentData
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string TipoSensor { get; set; } = string.Empty;
        public string NombreSensor { get; set; } = string.Empty;
        public DateTime FechaActual { get; set; }
        public double ValorActual { get; set; }
    }
}

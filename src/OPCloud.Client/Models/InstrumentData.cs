namespace OPCloud.Client.Models
{
    public class InstrumentData
    {
        public string TipoSensor { get; set; } = string.Empty;
        public string NombreSensor { get; set; } = string.Empty;
        public DateTime FechaActual { get; set; }
        public double ValorActual { get; set; }
    }
}

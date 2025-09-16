using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OPCloud.Models;
using OPCloud.Services;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OPCloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly int _mongoDbUploadFreq = 5;
        private int _mongoDbUploadCounter = 0;
        private readonly IHubContext<InstrumentHub> _hubContext;
        private readonly MongoService _mongoService;
        private readonly InstrumentBufferData _bufferService;

        public InstrumentController(MongoService mongoService, IHubContext<InstrumentHub> hubContext, InstrumentBufferData bufferService)
        {
            _hubContext = hubContext;
            _mongoService = mongoService;
            _bufferService = bufferService;
        }

        // GET: api/<InstrumentController>
        [HttpGet]
        public IEnumerable<InstrumentData> Get()
        {
            var lista = new List<InstrumentData>
            {
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 19.5,
                    FechaActual =  new DateTime(2025, 09, 15, 10, 10, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 19.8,
                    FechaActual = new DateTime(2025, 09, 15, 10, 15, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 12.5,
                    FechaActual = new DateTime(2025, 09, 15, 10, 20, 00)
                }, 
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 11.5,
                    FechaActual = new DateTime(2025, 09, 15, 10, 25, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 15.5,
                    FechaActual = new DateTime(2025, 09, 15, 10, 30, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 16.5,
                    FechaActual = new DateTime(2025, 09, 15, 10, 35, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 16.4,
                    FechaActual = new DateTime(2025, 09, 15, 10, 40, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 16.1,
                    FechaActual = new DateTime(2025, 09, 15, 10, 45, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 16.64,
                    FechaActual = new DateTime(2025, 09, 15, 10, 50, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 16.64,
                    FechaActual = new DateTime(2025, 09, 15, 10, 55, 00)
                },
                new InstrumentData
                {
                    NombreSensor = "Temperatura",
                    TipoSensor = "Temperatura",
                    ValorActual = 16.8,
                    FechaActual = new DateTime(2025, 09, 15, 11, 00, 00)
                },
            };

            return lista;
        }

        // 👉 Endpoint GET: api/database/count
        [HttpGet("mongocount")]
        public async Task<ActionResult<long>> GetTodayCount()
        {
            var count = await _mongoService.GetTodayWithCountAsync();
            long dataSet = count.Count;

            return Ok(dataSet); // devuelve solo el número
        }

        [HttpGet("buffer")]
        public ActionResult<IEnumerable<InstrumentData>> GetBuffer()
        {
            return Ok(_bufferService.GetBuffer());
        }

        // 👉 Nuevo endpoint que devuelve la lista de hoy
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<InstrumentData>>> GetTodayData()
        {
            var result = await _mongoService.GetTodayWithCountAsync();

            return Ok(result.Datos); // ⚡ devuelve la lista de documentos de hoy
        }

        // POST api/<InstrumentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<InstrumentData> dataList)
        {
            DateTime currentTimeStamp = DateTime.Now;

            foreach (var d in dataList)
                d.FechaActual = currentTimeStamp;

            // 👉 usar el servicio de buffer
            _bufferService.Add(dataList);

            // notificar por SignalR
            await _hubContext.Clients.All.SendAsync("NewInstrument", dataList);



            // guardar en Mongo
            await _mongoService.AddManyAsync(dataList);
            var count = await _mongoService.GetTodayWithCountAsync();
            await _hubContext.Clients.All.SendAsync("NewMongoDBSet", count.Count);

            return Ok(new
            {
                Mensaje = "Datos recibidos correctamente",
                Cantidad = dataList.Count,
                Datos = dataList
            });
        }

    }
}

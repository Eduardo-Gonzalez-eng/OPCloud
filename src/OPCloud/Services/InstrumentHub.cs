using Microsoft.AspNetCore.SignalR;
using OPCloud.Client.Models;

namespace OPCloud.Services
{
    public class InstrumentHub : Hub
    {
        // Método que el servidor puede invocar para notificar nuevos datos
        public async Task EnviarInstrumento(InstrumentData data)
        {
            await Clients.All.SendAsync("NewInstrument", data);
        }
    }
}

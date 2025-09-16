using OPCloud.Models;

namespace OPCloud.Services
{
    public class InstrumentBufferData
    {
        private readonly List<List<InstrumentData>> _buffer = new();
        private readonly object _lock = new(); // asegurar thread-safety

        public void Add(List<InstrumentData> dataList)
        {
            lock (_lock)
            {
                _buffer.Add(dataList);
                if (_buffer.Count > 5)
                    _buffer.RemoveAt(0);
            }
        }

        public List<InstrumentData> GetBuffer()
        {
            lock (_lock)
            {
                return _buffer.SelectMany(b => b).ToList();
            }
        }
    }
}

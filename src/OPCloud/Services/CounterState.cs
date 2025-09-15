namespace OPCloud.Services
{
    public class CounterState
    {
        private int _value;
        private readonly object _lock = new();

        public int Get() { lock (_lock) return _value; }
        public int Increment(int step = 1)
        {
            lock (_lock) { _value += step; return _value; }
        }
        public void Reset() { lock (_lock) { _value = 0; } }
    }
}

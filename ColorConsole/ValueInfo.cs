namespace ColorConsole
{
    internal readonly struct ValueInfo<T>
    {
        public T Value { get; init; }
        public string Information { get; init; }
        public int Number { get; init; }
    }
}
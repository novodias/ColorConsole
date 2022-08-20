namespace ColorConsole
{
    internal readonly struct ValueInfo<T>
    {
        public readonly T Value { get; init; }
        public readonly string Information { get; init; }
        public readonly int Number { get; init; }
    }
}
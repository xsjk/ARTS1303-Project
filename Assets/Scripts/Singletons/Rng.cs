using System;

public sealed class Rng
{
    
    private Random _rand = new Random(Guid.NewGuid().GetHashCode());
    
    private static readonly Lazy<Rng> Lazy = new (() => new Rng());

    public static Random Rand => Lazy.Value._rand;
    public static void Seed(string seed) => Lazy.Value._rand = new Random(seed.GetHashCode());
}
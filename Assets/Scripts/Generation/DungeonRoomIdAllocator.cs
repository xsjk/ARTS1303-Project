using System;

public sealed class DungeonRoomIdAllocator
{
    private int _counter;

    private static readonly Lazy<DungeonRoomIdAllocator> Lazy = new(() => new DungeonRoomIdAllocator());

    public static int Next()
    {
        Lazy.Value._counter++;
        return Lazy.Value._counter;
    }
}

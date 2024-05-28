using System;
using UnityEngine;
using RectpackSharp;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;


public enum WallOrientation
{
    North = 0,
    East = 270,
    South = 180,
    West = 90,
}

public interface IDungeonRoom
{
    // The Precision For The Packing Algorithm
    // As the algorithm only accepts an integer value for the width and height of the rectangle
    // We need to multiply the float value by this precision to get the integer value
    public const float Precision = 1000.0f;
    
    // The ID of the Room
    public int Id { get; }
    
    // The Padding of the Room
    // The Padding is the space between the Room and the other Rooms
    public Vector2 Padding { get; }
    
    // The Size of the Room
    public Vector2 Size { get; }
    
    // The position of the Room
    public Vector2 Position { get; }

    // Place the Room at the given position
    public void Place(Vector2 position);

    public void DecreaseEnemyCount();

    public bool RoomCleared();

    public Dictionary<WallOrientation, int> GetAvailableOrientation();

    public GameObject PopWall(WallOrientation wo);

    public void Connect(IDungeonRoom other)
    {
        var thisAvailable = GetAvailableOrientation();
        var otherAvailable = other.GetAvailableOrientation();
        var availableWallOrientation = new List <(WallOrientation, int)> ();
        foreach (WallOrientation wo in Enum.GetValues(typeof(WallOrientation)))
        {
            var oppositeWo = WallOrientationToOpposite(wo);
            if (thisAvailable[wo] > 0 && otherAvailable[oppositeWo] > 0)
            {
                availableWallOrientation.Add((wo, thisAvailable[wo] + otherAvailable[oppositeWo]));
            }
        }

        var (thisWallOrientation, _) = availableWallOrientation.OrderBy(t => t.Item2).First();
        var otherWallOrientation = WallOrientationToOpposite(thisWallOrientation);

        var thisWall = PopWall(thisWallOrientation);
        var otherWall = other.PopWall(otherWallOrientation);

        var thisHallway = ReplaceGameObjectWithPrefab(thisWall, DungeonRoomPrefabManager.Instance.hallwayPrefab);
        var otherHallway = ReplaceGameObjectWithPrefab(otherWall, DungeonRoomPrefabManager.Instance.hallwayPrefab);

        var thisTeleport = thisHallway.GetComponentInChildren<EntryTeleport>();
        var otherTeleport = otherHallway.GetComponentInChildren<EntryTeleport>();

        thisTeleport.teleportTarget = otherTeleport.gameObject;
        otherTeleport.teleportTarget = thisTeleport.gameObject;
    }
    
    PackingRectangle Pack()
    {
        return new PackingRectangle
        {
            Id = Id,
            Width = (uint)((Size.x + Padding.x * 2) * Precision),
            Height = (uint)((Size.y + Padding.y * 2) * Precision)
        };
    }

    static float SampleFromNormalDistribution(System.Random rand, float mean, float stdDev)
    {
        var u1 = 1.0f - (float)rand.NextDouble();
        var u2 = 1.0f - (float)rand.NextDouble();
        var randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        var randStdNormalInThreeSigma = Mathf.Clamp(randStdNormal, -3.0f, 3.0f);
        var randNormal = mean + stdDev * randStdNormalInThreeSigma;
        return randNormal;
    }

    static Vector2 SampleFromNormalDistribution(System.Random rand, Vector2 mean, Vector2 stdDev)
    {
        return new Vector2(
            SampleFromNormalDistribution(rand, mean.x, stdDev.x),
            SampleFromNormalDistribution(rand, mean.y, stdDev.y)
        );
    }

    static WallOrientation WallOrientationToOpposite(WallOrientation wo)
    {
        switch (wo)
        {
            case WallOrientation.North:
                return WallOrientation.South;
            case WallOrientation.East:
                return WallOrientation.West;
            case WallOrientation.South:
                return WallOrientation.North;
            case WallOrientation.West:
                return WallOrientation.East;
            default:
                throw new ArgumentOutOfRangeException(nameof(wo), wo, null);
        }
    }
    
    public static GameObject ReplaceGameObjectWithPrefab(GameObject old, GameObject prefab)
    {
        var gameObject = Object.Instantiate(prefab, old.transform.parent);
        gameObject.transform.localPosition = old.transform.localPosition;
        gameObject.transform.localRotation = old.transform.localRotation;
        gameObject.layer = old.layer;
        Object.Destroy(old);
        return gameObject;
    }
}
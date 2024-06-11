using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EmptyDungeonRoom : IDungeonRoom
{
    public int Id { get; } = DungeonRoomIdAllocator.Next();

    private static readonly Vector2 Mean = new(20.0f, 20.0f);
    private static readonly Vector2 StdDev = new(2.5f, 2.5f);
    private static readonly int WallLayer = LayerMask.NameToLayer("Wall");

    protected GameObject _room { get; private set; }

    private readonly Dictionary<WallOrientation, List<GameObject>> _hallwaySlots = new()
    {
        { WallOrientation.North, new() },
        { WallOrientation.East, new() },
        { WallOrientation.South, new() },
        { WallOrientation.West, new() },
    };

    public Vector2 Padding { get; } = new(40.0f, 40.0f);
    public Vector2 Size { get; } = IDungeonRoom.SampleFromNormalDistribution(Rng.Rand, Mean, StdDev);
    public Vector2 Position { get; private set; }

    public List<IDungeonRoom> ConnectedRooms { get; } = new();

    private static bool ShouldAddToHallwaySlots(int numberOfWalls, int index, int spaceBetween = 4)
    {
        if (index == numberOfWalls - 1 || index == 0)
        {
            return false;
        }

        numberOfWalls -= 2;
        index -= 1;

        var numberOfRooms = (numberOfWalls + spaceBetween) / (spaceBetween + 1);
        var occupiedSpace = (1 + spaceBetween) * numberOfRooms - spaceBetween;
        var leftSpace = (numberOfWalls - occupiedSpace) / 2;
        if (index < leftSpace)
        {
            return false;
        }

        index -= leftSpace;

        return index % (spaceBetween + 1) == 0;
    }
    
    private void CreateWall(GameObject parent, string name, WallOrientation rotate, Vector3 beginPosition,
        Vector3 endPosition)
    {
        var wall = new GameObject
        {
            name = name,
            transform =
            {
                parent = parent.transform,
                localPosition = beginPosition,
            }
        };
        var length = Vector3.Distance(beginPosition, endPosition);
        var wallLength = DungeonRoomPrefabManager.Instance.WallBounds.size.x;
        var numberOfWalls = Mathf.CeilToInt(length / wallLength);
        var halfLengthDiff = (numberOfWalls * wallLength - length) / 2;
        var halfLengthDiffVector = ((int)rotate % 180 == 90 ? Vector3.forward : Vector3.right) * halfLengthDiff;
        var halfWallLengthVector = ((int)rotate % 180 == 90 ? Vector3.forward : Vector3.right) * wallLength / 2;
        var endPositionLocal = endPosition - beginPosition - halfWallLengthVector + halfLengthDiffVector;
        for (var i = 0; i < numberOfWalls; i++)
        {
            var wallInstance = Object.Instantiate(DungeonRoomPrefabManager.Instance.WallPrefab, wall.transform);
            wallInstance.layer = WallLayer;
            wallInstance.transform.localPosition =
                Vector3.Lerp(halfWallLengthVector - halfLengthDiffVector, endPositionLocal, i / (float)(numberOfWalls - 1));
            wallInstance.AddComponent<BoxCollider>();
            wallInstance.transform.localRotation = Quaternion.Euler(0, (int)rotate, 0);

            if (ShouldAddToHallwaySlots(numberOfWalls, i))
            {
                _hallwaySlots[rotate].Add(wallInstance);
            }
        }
    }

    private void CreateFloor(GameObject parent, string name, bool rotate, Vector3 beginPosition,
        Vector3 endPosition,
        GameObject prefab, Vector3 size)
    {
        var rows = Mathf.CeilToInt((endPosition.x - beginPosition.x) / size.x);
        var cols = Mathf.CeilToInt((endPosition.z - beginPosition.z) / size.z);
        var actualRowDim = rows * size.x;
        var actualColDim = cols * size.z;
        var alignedBeginPosition = new Vector3(beginPosition.x - (actualRowDim - (endPosition.x - beginPosition.x)) / 2,
            beginPosition.y, beginPosition.z - (actualColDim - (endPosition.z - beginPosition.z)) / 2);
        alignedBeginPosition += new Vector3(size.x / 2, 0, size.z / 2);
        var floor = new GameObject
        {
            name = name,
            transform =
            {
                parent = parent.transform,
                localPosition = alignedBeginPosition,
            }
        };

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var floorInstance = Object.Instantiate(prefab, floor.transform);
                floorInstance.AddComponent<BoxCollider>();
                floorInstance.transform.localPosition = new Vector3(i * size.x, 0, j * size.z);
                if (rotate)
                {
                    floorInstance.transform.localRotation = Quaternion.Euler(180, 0, 0);
                }
            }
        }
    }

    public virtual void Place(Vector2 position)
    {
        Position = position;
        Debug.Log($"We are placing room {Id} at {position} with size {Size} and padding {Padding}");
        _room = new GameObject($"Room {Id}")
        {
            transform =
            {
                position = new Vector3(position.x, 0, position.y)
            }
        };
        // build walls
        CreateWall(_room, "North Wall", WallOrientation.North, new Vector3(-Size.x / 2, 0, -Size.y / 2),
            new Vector3(Size.x / 2, 0, -Size.y / 2));
        CreateWall(_room, "South Wall", WallOrientation.South, new Vector3(-Size.x / 2, 0, Size.y / 2),
            new Vector3(Size.x / 2, 0, Size.y / 2));
        CreateWall(_room, "East Wall", WallOrientation.East, new Vector3(Size.x / 2, 0, -Size.y / 2),
            new Vector3(Size.x / 2, 0, Size.y / 2));
        CreateWall(_room, "West Wall", WallOrientation.West, new Vector3(-Size.x / 2, 0, -Size.y / 2),
            new Vector3(-Size.x / 2, 0, Size.y / 2));
        CreateFloor(_room, "Floor", false, new Vector3(-Size.x / 2, 0, -Size.y / 2),
            new Vector3(Size.x / 2, 0, Size.y / 2), DungeonRoomPrefabManager.Instance.FloorPrefab,
            DungeonRoomPrefabManager.Instance.FloorBounds.size);
        CreateFloor(_room, "Ceiling", true, new Vector3(-Size.x / 2, DungeonRoomPrefabManager.Height, -Size.y / 2),
            new Vector3(Size.x / 2, DungeonRoomPrefabManager.Height, Size.y / 2),
            DungeonRoomPrefabManager.Instance.CeilingPrefab,
            DungeonRoomPrefabManager.Instance.CeilingBounds.size);
    }

    public Dictionary<WallOrientation, int> GetAvailableOrientation()
    {
        var result = new Dictionary<WallOrientation, int>();
        foreach (WallOrientation wo in Enum.GetValues(typeof(WallOrientation)))
        {
            result[wo] = _hallwaySlots[wo].Count;
        }

        return result;
    }

    public GameObject PopWall(WallOrientation wo)
    {
        if (_hallwaySlots[wo].Count == 0)
        {
            throw new Exception("Not found");
        }

        var r = Rng.Rand.Next(_hallwaySlots[wo].Count);
        var go = _hallwaySlots[wo][r];
        _hallwaySlots[wo].RemoveAt(r);
        return go;
    }

    public virtual void DecreaseEnemyCount()
    {
    }

    public virtual bool RoomCleared()
    {
        return true;
    }
}
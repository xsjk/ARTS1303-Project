using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using Libraries;
using RectpackSharp;
using UnityEngine;

class DungeonRoomPlacementValue
{
    public IDungeonRoom Room;
    public Vector2 Position;
}

struct Edge
{
    public int From;
    public int To;
    public float Distance;
}

public class DungeonPlacement
{
    public void PlaceRooms(List<IDungeonRoom> rooms)
    {
        var roomValues = rooms.ConvertAll(r => new DungeonRoomPlacementValue
        {
            Room = r
        });

        var roomIdLookup = roomValues.ToDictionary(r => r.Room.Id, r => r);

        var roomRectangles = rooms.ConvertAll(r => r.Pack()).ToArray();
        RectanglePacker.Pack(roomRectangles, out PackingRectangle _);

        foreach (var room in roomRectangles)
        {
            var value = roomIdLookup[room.Id];
            value.Position = new Vector2(room.X / IDungeonRoom.Precision, room.Y / IDungeonRoom.Precision);
            value.Room.Place(value.Position);
            roomIdLookup[room.Id] = value;
        }

        // https://stackoverflow.com/questions/64352847/unity-custom-editor-raycast-not-working-right-after-when-instantiate-objects
        Physics.SyncTransforms();
        Physics.simulationMode = SimulationMode.Script;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.simulationMode = SimulationMode.FixedUpdate;


        var edges = DelaunateRooms(roomValues);
        edges.Sort((x, y) => x.Distance.CompareTo(y.Distance));

        var disjointSet = new DisjointSet(roomValues.Count);

        foreach (var edge in edges)
        {
            if (disjointSet.Find(edge.From) != disjointSet.Find(edge.To))
            {
                Debug.Log($"Connecting Room {roomValues[edge.From].Room.Id} to Room {roomValues[edge.To].Room.Id}");
                var succeeded = roomValues[edge.From].Room.Connect(roomValues[edge.To].Room);
                if (succeeded)
                {
                    disjointSet.Union(edge.From, edge.To);
                }
            }
        }

        for (var i = 1; i < roomValues.Count; i++)
        {
            if (disjointSet.Find(0) != disjointSet.Find(i))
            {
                // If there exists two rooms that are not connected, throw an exception
                throw new System.Exception("Not all rooms are connected");
            }
        }
    }

    private List<Edge> DelaunateRooms(List<DungeonRoomPlacementValue> rooms)
    {
        if (rooms.Count <= 2)
        {
            Edge[] edges = new Edge[rooms.Count / 2];
            if (rooms.Count == 2)
            {
                edges[0] = new Edge
                {
                    From = 0,
                    To = 1,
                    Distance = Vector2.Distance(rooms[0].Position, rooms[1].Position)
                };
            }

            return edges.ToList();
        }

        IPoint[] points = rooms.ConvertAll(r => (IPoint)new Point(r.Position.x, r.Position.y)).ToArray();

        var delaunator = new Delaunator(points);
        var result = new List<Edge>();
        foreach (var triangle in delaunator.GetTriangles())
        {
            result.Add(new Edge
            {
                From = delaunator.Triangles[3 * triangle.Index],
                To = delaunator.Triangles[3 * triangle.Index + 1],
                Distance = Vector2.Distance(rooms[delaunator.Triangles[3 * triangle.Index]].Position,
                    rooms[delaunator.Triangles[3 * triangle.Index + 1]].Position)
            });
            result.Add(new Edge
            {
                From = delaunator.Triangles[3 * triangle.Index + 1],
                To = delaunator.Triangles[3 * triangle.Index + 2],
                Distance = Vector2.Distance(rooms[delaunator.Triangles[3 * triangle.Index + 1]].Position,
                    rooms[delaunator.Triangles[3 * triangle.Index + 2]].Position)
            });
            result.Add(new Edge
            {
                From = delaunator.Triangles[3 * triangle.Index + 2],
                To = delaunator.Triangles[3 * triangle.Index],
                Distance = Vector2.Distance(rooms[delaunator.Triangles[3 * triangle.Index + 2]].Position,
                    rooms[delaunator.Triangles[3 * triangle.Index]].Position)
            });
        }

        return result;
    }
}
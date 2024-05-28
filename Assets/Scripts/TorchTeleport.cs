using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class TorchTeleport : MonoBehaviour
{
    private IDungeonRoom _room;
    
    public void SetDungeonRoom(IDungeonRoom room)
    {
        _room = room;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!_room.RoomCleared())
        {
            return;
        }

        // foreach (var otherRoom in _room.ConnectedRooms)
        // {
        //     if (!otherRoom.RoomCleared())
        //     {
        //         other.transform.position = new UnityEngine.Vector3(
        //             otherRoom.Position.x,
        //             2,
        //             otherRoom.Position.y);
        //         return;
        //     }
        // }
    }
}
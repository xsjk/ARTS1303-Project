using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject player;
    private List<IDungeonRoom> _rooms;

    void Start()
    {
        _rooms = new List<IDungeonRoom>();
        _rooms.Add(new EnemyRoom1());
        for (int i = 0; i < 19; i++)
        {
            _rooms.Add(new EmptyDungeonRoom());
        }

        var placement = new DungeonPlacement();
        placement.PlaceRooms(_rooms);
        player.transform.position = new Vector3(_rooms[0].Position.x, 2, _rooms[0].Position.y);
        player.GetComponent<InputLogic>().enabled = true;
    }
}
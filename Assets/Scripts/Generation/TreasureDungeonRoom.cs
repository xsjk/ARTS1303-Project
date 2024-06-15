using System.Collections.Generic;
using Entities;
using Items;
using Items.Implementation;
using UnityEngine;

public class TreasureDungeonRoom : TriggerableDungeonRoom
{
    private static readonly GameObject Chest = Resources.Load<GameObject>("Prefabs/Chest");

    protected override int SpawnMobs()
    {
        // No mobs in treasure rooms
        return 0;
    }

    protected override void OnRoomCleared()
    {
        // Spawn the treasure chest
        GameObject chest = Object.Instantiate(Chest, Room.transform);
        var logic = chest.GetComponent<ChestLogic>();
        logic.BindItemStack(new List<IInstantiableItem>
        {
            new Coin(),
            new Coin(),
            new Coin(),
            new Coin(),
        });
    }
}
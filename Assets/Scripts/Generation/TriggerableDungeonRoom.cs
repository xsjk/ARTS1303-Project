using UnityEngine;

internal class TriggerLogic: MonoBehaviour
{
    public TriggerableDungeonRoom Room;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Room.SpawnMobsWrapper();
        }
    }
}

public abstract class TriggerableDungeonRoom : EmptyDungeonRoom
{
    private bool _mobSpawned;
    private int _mobCount;
    public override void Place(Vector2 position)
    {
        base.Place(position);
        // Place the Trigger Collider
        var collider = Room.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(Size.x, DungeonRoomPrefabManager.Height, Size.y);
        var logic = Room.AddComponent<TriggerLogic>();
        logic.Room = this;
    }

    internal void SpawnMobsWrapper()
    {
        if (_mobSpawned)
        {
            return;
        }
        _mobSpawned = true;
        _mobCount = SpawnMobs();
        if (_mobCount == 0)
        {
            OnRoomCleared();
        }
    }
    
    // Spawn mobs in the room, return the number of mobs spawned
    protected abstract int SpawnMobs();

    public void DecreaseMobCount()
    {
        if (_mobCount <= 0)
        {
            return;
        }
        _mobCount -= 1;
        if (_mobCount == 0)
        {
            OnRoomCleared();
        }
    }

    protected abstract void OnRoomCleared();
}
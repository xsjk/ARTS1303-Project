using Entities;
using Items.Implementation;
using UnityEngine;

public class ShopDungeonRoom : TriggerableDungeonRoom
{
    private static readonly Vector2 Mean = new(40.0f, 40.0f);
    private static readonly Vector2 StdDev = new(2.5f, 2.5f);
    public override Vector2 Size { get; } = IDungeonRoom.SampleFromNormalDistribution(Rng.Rand, Mean, StdDev);
    private static readonly GameObject ShopCabinet = Resources.Load<GameObject>("Prefabs/Shop Cabinet");

    private const float ItemMean = 5.5f;
    private const float ItemStdDev = 1.0f;

    protected override int SpawnMobs()
    {
        // No mobs in shops
        return 0;
    }

    protected override void OnRoomCleared()
    {
        // Spawn the items
        var itemCount = Mathf.RoundToInt(IDungeonRoom.SampleFromNormalDistribution(Rng.Rand, ItemMean, ItemStdDev));
        var cabinetSize = ShopCabinet.GetComponent<BoxCollider>().size;
        var cabinetGap = cabinetSize.x;
        var totalWidth = itemCount * cabinetSize.x + (itemCount - 1) * cabinetGap;
        var begin = new Vector3(-totalWidth / 2, 0, 0);
        var end = new Vector3(totalWidth / 2, 0, 0);
        for (var i = 0; i < itemCount; i++)
        {
            GameObject cabinet = Object.Instantiate(ShopCabinet, Room.transform);
            cabinet.transform.localPosition = Vector3.Lerp(begin, end, (float)i / (itemCount - 1));
            var logic = cabinet.GetComponent<ShopCabinetLogic>();
            if (i % 2 == 0)
            {
                logic.BindItemStack(new HealthBottle());
            }
            else
            {
                logic.BindItemStack(new Coin());
            }
        }
    }
}
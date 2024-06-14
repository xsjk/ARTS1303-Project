using System.Collections.Generic;
using UnityEngine;


public enum EnemyType {
    Skeleton_Minion,
    Skeleton_Rogue,
    Skeleton_Warrior,
    Skeleton_Warrior_Blade,
    Skeleton_Warrior_Axe,
    Skeleton_Warrior_TwoBlade,
    Skeleton_Warrior_TwoAxe,
    Skeleton_Warrior_AxeBlade,
    Skeleton_Warrior_BladeShield,
    Skeleton_Warrior_AxeShield,
    Skeleton_Mage,
}

public class EnemyManager {

    private static Dictionary<EnemyType, GameObject> _enemyPrefabs = new Dictionary<EnemyType, GameObject> {
        { EnemyType.Skeleton_Minion, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Minion") },
        { EnemyType.Skeleton_Rogue, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Rogue") },
        { EnemyType.Skeleton_Warrior, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior") },
        { EnemyType.Skeleton_Warrior_Blade, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_Blade") },
        { EnemyType.Skeleton_Warrior_Axe, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_Axe") },
        { EnemyType.Skeleton_Warrior_TwoBlade, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_TwoBlade") },
        { EnemyType.Skeleton_Warrior_TwoAxe, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_TwoAxe") },
        { EnemyType.Skeleton_Warrior_AxeBlade, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_AxeBlade") },
        { EnemyType.Skeleton_Warrior_BladeShield, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_BladeShield") },
        { EnemyType.Skeleton_Warrior_AxeShield, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior_AxeShiled") },
        { EnemyType.Skeleton_Mage, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Mage") },
    };

    public static GameObject Spawn(EnemyType enemyType, Transform parent = null) {
        if (_enemyPrefabs.ContainsKey(enemyType))
            return Object.Instantiate(_enemyPrefabs[enemyType], parent);
        else
            throw new System.Exception("Unknown enemy type: " + enemyType);
    }
}

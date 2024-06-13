using System.Collections.Generic;
using UnityEngine;


public enum EnemyType {
    Skeleton_Minion,
    Skeleton_Rogue,
    Skeleton_Warrior,
    Skeleton_Mage,
}

public class EnemyManager {

    private static Dictionary<EnemyType, GameObject> _enemyPrefabs = new Dictionary<EnemyType, GameObject> {
        { EnemyType.Skeleton_Minion, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Minion") },
        { EnemyType.Skeleton_Rogue, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Rogue") },
        { EnemyType.Skeleton_Warrior, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Warrior") },
        { EnemyType.Skeleton_Mage, Resources.Load<GameObject>("Prefabs/Enemies/Skeleton_Mage") },
    };

    public static GameObject Spawn(EnemyType enemyType, Transform parent = null) {
        if (_enemyPrefabs.ContainsKey(enemyType))
            return Object.Instantiate(_enemyPrefabs[enemyType], parent);
        else
            throw new System.Exception("Unknown enemy type: " + enemyType);
    }
}

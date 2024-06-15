using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyRoom1 : EmptyDungeonRoom
{

    private GameObject makeEnemy(Vector2 position)
    {
        var enemy = EnemyManager.Spawn(EnemyType.Skeleton_Minion, Room.transform);
        enemy.transform.localPosition = new Vector3(position.x, 0, position.y);
        enemy.GetComponent<EnemyController>().SetRoom(this);
        return enemy;
    }

    public override void Place(Vector2 position)
    {
        base.Place(position);

        int n = 2;
        float r = 4;

        for (int i = 0; i < n; i++)
            makeEnemy(Vector2Extensions.FromAngle(2 * Mathf.PI / n * i) * r);
    }
    
}
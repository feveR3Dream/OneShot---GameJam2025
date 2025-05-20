using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructContainer { }

#region BossDamaged: BOSS PHASE CONTROLLER -> PLAYER CONTROLLER
public struct BossDamaged { };
#endregion

#region BulletSpawn: SpawnBullet
public struct BulletSpawn 
{
    public float timer;
};
#endregion

#region BossHurt: PROJECTILE -> BOSS PHASE CONTROLLER
public struct BossHurt { };
#endregion
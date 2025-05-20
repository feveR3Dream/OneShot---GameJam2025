using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructContainer { }

#region BOSS PHASE CONTROLLER -> PLAYER CONTROLLER
public struct BossDamaged { };
#endregion

#region SpawnBullet
public struct BulletSpawn 
{
    public float timer;
};
#endregion

#region BOSS GET AWAY FROM ME -> BOSS PHASE CONTROLLER
public struct PlayerGotTooClose { };
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructContainer { }

#region BossHurt: PROJECTILE -> BOSS PHASE CONTROLLER
public struct BossHurt 
{
    public Vector2 projectilePos;
};
#endregion

#region BossWhiffed PROJECTILE -> BOSS PHASE CONTROLLER
public struct BossWhiffed
{
    
}
#endregion

#region BossChangePhase: BOSSPHASECONTROLLER -> CAMERA MANAGER / BOSS TRIGGER SHOCKWAVE
public struct BossChangePhase 
{
    public int Phase;
}
#endregion
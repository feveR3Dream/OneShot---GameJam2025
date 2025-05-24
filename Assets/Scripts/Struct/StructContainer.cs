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

#region BossWhiffed: PROJECTILE -> BOSS PHASE CONTROLLER
public struct BossWhiffed
{
}
#endregion

#region BossChangePhase: BOSSPHASECONTROLLER -> CAMERA MANAGER / BOSS TRIGGER SHOCKWAVE
public struct BossChangePhase 
{
    public int CurrentPhase;
}
#endregion

#region CameraShakeEvent: 
public struct CameraShakeEvent 
{
    public float ShakeMagnitude;
    public float ShakeDuration;
}
#endregion

#region PickUpEvent:
public struct PickUpEvent
{
    public GameObject PickUpObj;
}
#endregion

#region PlayerWin: WIN GAME CONDITION
public struct PlayerWin { }
#endregion

#region PlayerDie: GAME RESTART CONDITION
public struct PlayerDie { }
#endregion

#region ShootIndicator:
public struct ShootIndicator
{
    public Color color;
    public float timer;
}
#endregion

#region GamePaused:
public struct GamePaused 
{
    public bool paused;
}
#endregion
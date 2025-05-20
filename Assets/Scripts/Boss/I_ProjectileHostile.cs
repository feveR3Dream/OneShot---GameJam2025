using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_ProjectileHostile
{
    public void SetOwner(GameObject obj) { }
    public void SetTarget(GameObject obj) { }
    public void Fire() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    public static bool Invulnerable = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && !Invulnerable)
        {
            CallPlayerDeath();
        }
    }

    public void CallPlayerDeath()
    {
        Destroy(this.gameObject);
        EventDispatcher.Instance.SendEvent(new PlayerDie());
        SoundManager.PlaySound(SoundType.DEATH, 0.25f);
        SoundManager.PlaySound(SoundType.PITCHED_SHATTERING, 0.25f);
    }
}

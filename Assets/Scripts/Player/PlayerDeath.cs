using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null)
        {
            Destroy(this.gameObject);
            EventDispatcher.Instance.SendEvent(new PlayerDie());
            SoundManager.PlaySound(SoundType.DEATH, 0.5f);
            SoundManager.PlaySound(SoundType.PITCHED_SHATTERING, 0.5f);
        }
    }
}

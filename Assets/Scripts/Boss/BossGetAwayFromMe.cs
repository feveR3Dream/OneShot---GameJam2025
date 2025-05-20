using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGetAwayFromMe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
            if (collision.CompareTag("Player") == true)
                EventDispatcher.Instance.SendEvent(new PlayerGotTooClose());

    }
}

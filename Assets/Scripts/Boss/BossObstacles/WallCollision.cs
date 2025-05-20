using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
        // You can add more logic here, e.g., destroy player, bounce, etc.
    }
}

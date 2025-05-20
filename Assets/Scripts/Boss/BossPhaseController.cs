using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossPhaseController : MonoBehaviour
{
    public GameObject[] ProjectilesPrefab;
    public GameObject Weakspot;
    public CircleCollider2D Circle;

    private int Phase;
    private bool Casting = false; //Used for moves that need delay and will not cast other abilities simultaneously

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossHurt>(Hurt);
        EventDispatcher.Instance.Subscribe<BossWhiffed>(PlayerWhiffed);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossHurt>(Hurt);
        EventDispatcher.Instance.Unsubscribe<BossWhiffed>(PlayerWhiffed);
    }
    
    private void FireBeam()
    {
        GameObject beam = Instantiate(ProjectilesPrefab[0], transform.position, Quaternion.identity);
        beam.SetActive(true);
        beam.GetComponent<I_ProjectileHostile>().Fire();
        Destroy(beam, 1.5f);
        
    }

    #region Boss Change Weakspot
    private void ChangeWeakspot()
    {
        // Get the center and radius in world space
        Vector3 center = Circle.transform.position + (Vector3)(Circle.offset * Circle.transform.lossyScale);
        float radius = Circle.radius * Mathf.Max(Circle.transform.lossyScale.x, Circle.transform.lossyScale.y); // uniform scaling assumed

        // Shrink the radius slightly to move inward from the edge
        float inwardOffset = Random.Range(0.1f, 0.4f); ; // tweak this value for how deep you want to go inside
        float adjustedRadius = radius - inwardOffset;

        // Random angle in radians (0 to 2π)
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate position on the edge
        float x = Mathf.Cos(angle) * adjustedRadius;
        float y = Mathf.Sin(angle) * adjustedRadius;

        Vector3 edgePosition = center + new Vector3(x, y, 0f);

        Weakspot.transform.position = edgePosition;
    }
    #endregion
    private void PhaseIncrease()
    {
        Phase += 1;
        switch (Phase)
        {
            default:
                //LOLOLOL
                break;
        }
    }

    private void PlayerWhiffed(BossWhiffed e)
    {
        FireBeam();
    }

    private void Hurt(BossHurt e)
    {
        //Some other visual shit here
        PhaseIncrease();
        ChangeWeakspot();
    }

}

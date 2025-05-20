using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AbilityName
{
    Spikey,
    Shield,
    EggProducer,
}

public class AbilityCooldown
{
    public bool Unlocked;
    public float Cooldown;
    public AbilityCooldown(bool unlock, float cd)
    {
        Unlocked = unlock;
        Cooldown = cd;
    }
}
public class BossPhaseController : MonoBehaviour
{
    public GameObject[] ProjectilesPrefab;
    public GameObject Weakspot;
    public CircleCollider2D Circle;

    private int Phase;
    private bool Casting = false; //Used for moves that need delay and will not cast other abilities simultaneously
    private Dictionary<AbilityName, AbilityCooldown> abilitiesCooldown = new Dictionary<AbilityName, AbilityCooldown>() 
    {
        {AbilityName.Spikey, new AbilityCooldown(true, 5f)},
        {AbilityName.Shield, new AbilityCooldown(true, 4f)},
        {AbilityName.EggProducer, new AbilityCooldown(true, 2f)},
    };

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

    private void Update()
    {
        //CooldownProcessor();
    }

    private void CooldownProcessor()
    {
        if (Casting) { return; }
        foreach (AbilityName ability in abilitiesCooldown.Keys)
        {
            if (!abilitiesCooldown[ability].Unlocked)
                continue;
            if (abilitiesCooldown[ability].Cooldown > 0f)
            {
                abilitiesCooldown[ability].Cooldown -= Time.deltaTime;
            }
            else
            {
                switch (ability)
                {
                    case AbilityName.Spikey:
                        //Ability1(ability);
                        break;
                    case AbilityName.Shield:
                        //Ability2(ability);
                        break;
                    case AbilityName.EggProducer:
                        //Ability3(ability);
                        break;
                }
            }
        }
            
    }
    #region Boss Mechanics Function
    private void FireBeam()
    {
        GameObject LaserBeam = Instantiate(ProjectilesPrefab[0], transform.position, Quaternion.identity);
        LaserBeam.SetActive(true);
        LaserBeam.GetComponent<I_ProjectileHostile>().Fire();
        Destroy(LaserBeam, 1.5f);
    }
    private void Ability1(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 5f;
        /*GameObject WallRotator = Instantiate(ProjectilesPrefab[1], transform.position, Quaternion.identity);
        WallRotator.SetActive(true);
        Destroy(WallRotator, 10f);*/
    }

    private void Ability2(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 4f;
        //AuraPush();
    }

    private void Ability3(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 5f;
        FireBeam();
    }
    #endregion

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

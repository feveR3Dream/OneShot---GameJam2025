using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

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
    private int Phase;
    private bool Casting = false; //Used for moves that need delay and will not cast other abilities simultaneously
    private Dictionary<AbilityName, AbilityCooldown> abilitiesCooldown = new Dictionary<AbilityName, AbilityCooldown>() 
    {
        {AbilityName.Spikey, new AbilityCooldown(true, 5f)},
        {AbilityName.Shield, new AbilityCooldown(true, 2f)},
        {AbilityName.EggProducer, new AbilityCooldown(true, 4f)},
    };

    private void Update()
    {
        CooldownProcessor();
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
                        Ability1(ability);
                        break;
                    case AbilityName.Shield:
                        Ability2(ability);
                        break;
                    case AbilityName.EggProducer:
                        Ability3(ability);
                        break;
                }
            }
        }
            
    }
    #region Abilities Function
    private void AuraPush()
    {
        GameObject ForceClone = Instantiate(ProjectilesPrefab[0], transform.position, Quaternion.identity);
        ForceClone.GetComponent<I_ProjectileHostile>().SetOwner(gameObject);
        ForceClone.SetActive(true);
        Destroy(ForceClone, 1f);
    }
    private void Ability1(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 5f;
    }

    private void Ability2(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 2f;
        EventDispatcher.Instance.SendEvent(new BossDamaged());
    }

    private void Ability3(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 4f;
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

    public void Hurt()
    {
        //Some other visual shit here
        AuraPush();
        PhaseIncrease();

        EventDispatcher.Instance.SendEvent(new BossDamaged());
    }

    
}

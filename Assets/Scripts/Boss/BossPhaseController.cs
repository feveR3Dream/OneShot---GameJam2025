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
    private int Phase;
    private Dictionary<AbilityName, AbilityCooldown> abilitiesCooldown = new Dictionary<AbilityName, AbilityCooldown>() 
    {
        {AbilityName.Spikey, new AbilityCooldown(true, 5f)},
        {AbilityName.Shield, new AbilityCooldown(true, 3f)},
        {AbilityName.EggProducer, new AbilityCooldown(true, 4f)},
    };

    private void Update()
    {
        CooldownProcessor();
    }

    private void CooldownProcessor()
    {
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
    private void Ability1(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 5f;
        Debug.Log("ability1 fired");
    }

    private void Ability2(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 3f;
        Debug.Log("ability2 fired");
    }

    private void Ability3(AbilityName name)
    {
        abilitiesCooldown[name].Cooldown = 4f;
        Debug.Log("ability3 fired");
    }
    #endregion
    public void Hurt()
    {
        //Some other visual shit here
        PhaseIncrease();
    }

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
}

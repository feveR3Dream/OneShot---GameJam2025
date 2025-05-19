using JetBrains.Annotations;
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
    private Dictionary<AbilityName, AbilityCooldown> abilitiesCooldown = new Dictionary<AbilityName, AbilityCooldown>() 
    {
        {AbilityName.Spikey, new AbilityCooldown(false, 5f)},
        {AbilityName.Shield, new AbilityCooldown(false, 5f)},
        {AbilityName.EggProducer, new AbilityCooldown(false, 5f)},
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
                abilitiesCooldown[ability].Cooldown -= 0f;
            else
            {
                switch (ability)
                {
                    case AbilityName.Spikey:
                        //fire spikey function;
                        break;
                    case AbilityName.Shield:
                        //fire shield function
                        break;
                    case AbilityName.EggProducer:
                        //fire egg prodcuer function
                        break;
                }
            }
        }
            
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Healing")]
public class Healing : Ability
{
    public int baseHealing = 1;
    [Range(0,10)]
    public float intMultiplier = 1;
    public override void Trigger(Unit caster, Unit target){
        Heal(caster, target);
    }

    void Heal(Unit caster, Unit target){
        PlayParticlesOnTarget(target);
        PingNumberOnTarget(caster.GetInteligence + baseHealing,false,target);
        target.ReceiveHealing(caster.GetInteligence + baseHealing);
    }

}

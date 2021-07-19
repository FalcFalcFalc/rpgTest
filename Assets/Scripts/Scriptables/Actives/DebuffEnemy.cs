using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Support/Debuff Enemy")]
public class DebuffEnemy : Ability
{
    [SerializeField] Enum.Stat statToBuff;
    [SerializeField] int strength;


    public override void Trigger(Unit caster, Unit target){
        Debuff(caster, target);
    }

    void Debuff(Unit caster, Unit target){
        PlayParticlesOnTarget(target, Vector3.up);
        target.Debuff(statToBuff,strength);
        PingNumberOnTarget("- "+statToBuff.ToString(),true,target);
    }

}

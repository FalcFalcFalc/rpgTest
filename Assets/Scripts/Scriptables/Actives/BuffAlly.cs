using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Support/Buff Ally")]
public class BuffAlly : Support
{
    [SerializeField] Keywords.Buff statToBuff;
    [SerializeField] int strength;
    [SerializeField] Color particleColor;

    protected override void SupportAlly(Unit caster, Unit target){
        PlayParticlesOnTarget(target).startColor = particleColor;
        target.Buff(statToBuff, strength);
    }
}


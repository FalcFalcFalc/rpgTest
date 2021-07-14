using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Support/Buff Ally")]
public class BuffAlly : Support
{
    [SerializeField] Enum.Stat statToBuff;
    [SerializeField] int strength;
    [SerializeField] Color particleColor;

    protected override void SupportAlly(Unit caster, Unit target){
        if(target.canBuff(statToBuff)){
            PlayParticlesOnTarget(target).startColor = particleColor;
            target.Buff(statToBuff, strength);
            PingNumberOnTarget("+"+statToBuff.ToString(),true,target);
        }
        else
        {
            PingNumberOnTarget("Can't buff",false,target);
            TurnHandler.current.OneMore();
        }

        
    }
}

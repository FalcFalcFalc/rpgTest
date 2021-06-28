using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Support/Debuff Enemy")]
public class DebuffEnemy : Ability
{
    [SerializeField] Enum.Stat statToBuff;
    [SerializeField] Color particleColor;


    public override void Trigger(Unit caster, Unit target){
        Debuff(caster, target);
    }

    void Debuff(Unit caster, Unit target){
        ParticleSystem inst = PlayParticlesOnTarget(target);
        inst.startColor = particleColor;
        inst.gameObject.transform.position = new Vector3(inst.transform.position.x, inst.transform.position.y +1, inst.transform.position.z);
        target.Debuff(statToBuff);
        PingNumberOnTarget("- "+statToBuff.ToString(),true,target);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Support/Debuff Enemy")]
public class DebuffEnemy : Ability
{
    [SerializeField] Keywords.Buff statToBuff;
    [SerializeField] int strength;
    [SerializeField] Color particleColor;


    public override void Trigger(Unit caster, Unit target, bool dat){
        Debuff(caster, target);
    }

    void Debuff(Unit caster, Unit target){
        ParticleSystem inst = PlayParticlesOnTarget(target);
        inst.startColor = particleColor;
        inst.gameObject.transform.position = new Vector3(inst.transform.position.x, inst.transform.position.y +1, inst.transform.position.z);
        target.Debuff(statToBuff,strength);
        if(!caster.playable) PressTurnSystem.current.Next();
    }

}

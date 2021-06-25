using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/Life Steal")]
public class LifeSteal : Pasive
{
    [SerializeField] int damagePercentage = 2;
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onDamage += (value) => steal(value,self);
    }
    public override void Disable(Unit self){
        self.onDamage -= (value) => steal(value,self);
    }

    public void steal(int damage, Unit self){
        float howMuchToHeal = damage * damagePercentage / 100f;
        PlayParticlesOnTarget(self);
        PingNumberOnTarget(Mathf.RoundToInt(howMuchToHeal).ToString(),false,self);
        self.ReceiveHealing(Mathf.RoundToInt(howMuchToHeal));
    }

    public override void Trigger(Unit caster, Unit target){}

}

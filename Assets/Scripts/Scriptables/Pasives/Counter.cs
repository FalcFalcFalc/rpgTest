using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/Counter")]
public class Counter : Pasive
{
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onEvade += (target) => Trigger(self,target);
    }
    public override void Disable(Unit self){
        self.onEvade -= (target) => Trigger(self,target);
    }

    public override void Trigger(Unit caster, Unit target){
        BattleLog.current.AddLog(caster.name + " is countering " + target.name + "'s attack.");
        PlayParticlesOnTarget(caster);
        PingNumberOnTarget("DODGED",true,caster);
        caster.Attack(target,false);
    }

}

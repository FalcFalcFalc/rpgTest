using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/Counter")]
public class Counter : Pasive
{
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onEvade += (target) => Trigger(self,target,false);
    }
    public override void Disable(Unit self){
        self.onEvade -= (target) => Trigger(self,target,false);
    }

    public override void Trigger(Unit caster, Unit target, bool dat){
        BattleLog.current.AddLog(caster.name + " is countering " + target.name + "'s attack.");
        PlayParticlesOnTarget(caster);
        MiniTextGenerator.current.CreateText("DODGED",caster.transform);
        caster.Attack(target,dat);
    }

}

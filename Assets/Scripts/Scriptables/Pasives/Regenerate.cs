using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/Regenerate")]
public class Regenerate : Pasive
{
    [SerializeField] int hpPercentage = 2;
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onDeactivate += () => Trigger(self,null,false);
    }
    public override void Disable(Unit self){
        self.onDeactivate -= () => Trigger(self,null,false);
    }

    public override void Trigger(Unit caster, Unit target,bool dat){
        float howMuchToHeal = caster.getMaxHP * hpPercentage / 100f;
        PlayParticlesOnTarget(caster);
        MiniTextGenerator.current.CreateText(Mathf.RoundToInt(howMuchToHeal).ToString(),caster.transform,Keywords.Elements.Blessing);
        caster.ReceiveHealing(Mathf.RoundToInt(howMuchToHeal));
    }

}

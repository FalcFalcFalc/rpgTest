using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/Regenerate")]
public class Regenerate : Pasive
{
    int hpPercentage = 2;
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onDeactivate += () => Trigger(self,null);
    }
    public override void Disable(Unit self){
        self.onDeactivate -= () => Trigger(self,null);
    }

    public override void Trigger(Unit caster, Unit target){
        float howMuchToHeal = caster.GetMaxHP * hpPercentage / 100f;
        PlayParticlesOnTarget(caster);
        PingNumberOnTarget(Mathf.RoundToInt(howMuchToHeal),false,caster);
        caster.ReceiveHealing(Mathf.RoundToInt(howMuchToHeal));
    }

}

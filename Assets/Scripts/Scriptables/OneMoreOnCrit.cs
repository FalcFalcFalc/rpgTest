using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/OneMoreOnCrit")]
public class OneMoreOnCrit : Pasive
{
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onCrit += () => Trigger(self,null);
    }
    public override void Disable(Unit self){
        self.onCrit -= () => Trigger(self,null);
    }

    public override void Trigger(Unit caster, Unit target){
        TurnHandler.current.OneMore(caster);
    }
}

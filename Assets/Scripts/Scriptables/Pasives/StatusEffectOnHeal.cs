using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/S.E. on Heal")]
public class StatusEffectOnHeal : Pasive
{
    [Header("Status Effect on Heal")]
    [SerializeField] StatusEffect effectToApply;
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onHealed += () => Trigger(self,null);
    }
    public override void Disable(Unit self){
        self.onHealed -= () => Trigger(self,null);
    }

    public override void Trigger(Unit caster, Unit target){
        BattleLog.current.AddLog(caster.name + " is now  " + effectToApply.description + ".");
        PingNumberOnTarget("DEFENDING",false,caster);
        caster.AddStatusEffect(effectToApply);
    }
}

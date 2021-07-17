using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defend", menuName = "Status Effects/Defend", order = 0)]
public class Defend : StatusEffect {

    [Range(0,1)]
    [SerializeField] float damageReduction;
    [SerializeField] bool stopAfterHit = false;

    public override void Enable(Unit self){ 
        self.ModifyDamageMultiplier(1 - damageReduction, false);
        if(stopAfterHit) self.onSurvive += (noDamageValueToPass) => self.RemoveStatusEffect(this);
    }
    public override void Disable(Unit self){
        self.ModifyDamageMultiplier(1 - damageReduction, true); //ESTO NO FUNCIONA
        if(stopAfterHit) {
            self.onSurvive -= (noDamageValueToPass) => Disable(self);
        }
    } 
    
}

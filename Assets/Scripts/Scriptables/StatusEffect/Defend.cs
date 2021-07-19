using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defend", menuName = "Status Effects/Defend", order = 0)]
public class Defend : StatusEffect {

    [Range(0,1)]
    [SerializeField] float damageReduction;
    Action<int> desuscripcion;

    public override void Enable(Unit self){ 
        PlaceIcon(self);
        //PlayParticlesOnTarget(self);
        desuscripcion = (noDamageValueToPass) => Trigger(self, null);
        self.ModifyDamageMultiplier(1 - damageReduction, false);
        if(autoStops) self.onSurvive += desuscripcion;
    }

    public override void Trigger(Unit self, Unit target){
        self.RemoveStatusEffect(this);
        Debug.Log("recibi daño, desuscribiendo");
    }

    public override void Disable(Unit self){
        self.ModifyDamageMultiplier(1 - damageReduction, true);
        if(autoStops) {
            self.onSurvive -= desuscripcion;
        }
        DestroyIcon();
    } 
    
}

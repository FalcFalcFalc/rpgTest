using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Poison", menuName = "Status Effects/Posion")]
public class Poison : StatusEffect
{
    [SerializeField] int damage;
    [SerializeField] bool basedOnMaxHP;
    public static Action takePoisonDamage = null;
    public override void Enable(Unit self){ 
        PlaceIcon(self);
        if(takePoisonDamage == null) takePoisonDamage = () => Trigger(self, null);
        self.onDeactivate += takePoisonDamage;
    }

    public override void Trigger(Unit self, Unit target){
        Debug.Log("aplicando veneno a " + self.name);
        float howMuchDamage = damage * (basedOnMaxHP ? self.getMaxHP / 100f : 1f);
        self.ReceiveDamage(Mathf.RoundToInt(howMuchDamage));
        //PlayParticlesOnTarget(self);
        PingNumberOnTarget(Mathf.RoundToInt(howMuchDamage).ToString(),false,self);
    }

    public override void Disable(Unit self){
        self.onDeactivate -= takePoisonDamage;
        DestroyIcon();
    } 
}

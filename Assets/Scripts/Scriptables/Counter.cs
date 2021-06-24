﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pasives/Counter")]
public class Counter : Pasive
{
    int hpPercentage = 2;
    public override void Enable(Unit self){ //opa, existen las expresiones lambda en c#! wujú!
        self.onEvade += (target) => Trigger(self,target);
    }
    public override void Disable(Unit self){
        self.onEvade -= (target) => Trigger(self,target);
    }

    public override void Trigger(Unit caster, Unit target){
        PlayParticlesOnTarget(caster);
        PingNumberOnTarget("DODGE",true,caster);
        caster.Attack(target,false);
    }

}

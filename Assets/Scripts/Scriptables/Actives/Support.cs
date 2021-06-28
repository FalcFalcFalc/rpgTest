﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Support : Ability
{
    public override void Trigger(Unit caster, Unit target){
        BattleLog.current.AddLog(caster.name + " used " + abilityName + " on " + target.name + ".");
        SupportAlly(caster, target);
    }

    protected abstract void SupportAlly(Unit caster, Unit target);

}

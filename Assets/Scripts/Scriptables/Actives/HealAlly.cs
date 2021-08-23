﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Support/Heal Ally")]
public class HealAlly : Support
{
    [Range(0,10)]
    public float intMultiplier = 1;
    [SerializeField] int baseHealing = 0;
    protected override void SupportAlly(Unit caster, Unit target){
        PlayParticlesOnTarget(target);
        MiniTextGenerator.current.CreateText((caster.getInteligence+baseHealing).ToString(),caster.transform,Keywords.Elements.Blessing);

        float heal = caster.getInteligence + baseHealing;
        ModifyHealing(ref heal,target);
        target.ReceiveHealing(Mathf.RoundToInt(heal));
    }

    protected virtual void ModifyHealing(ref float value, Unit target){}
}

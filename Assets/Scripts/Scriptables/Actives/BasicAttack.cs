using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Attack/Basic Attack")]
public class BasicAttack : Ability
{
    [Header("Attack Basic Stats")]
    public int baseDamage = 1;
    [Range(0,10)]
    public float atkMultiplier = 1;
    public Keywords.Elements attackType;
    [Range(-20,20)]
    public int hitModifier = 0;
    [Range(0,20)]
    public int critChance = 5;
    [Range(1,10)]
    public float critMultiplier = 2;

    public override void Trigger(Unit caster, Unit target, bool doesntAffectTurn){
        BattleLog.current.AddLog(caster.name + " used " + abilityName + " on " + target.name + ".");
        if(!playParticlesOnMiss)
            Attack(caster, target, doesntAffectTurn);
        else
            PlayParticlesOnTarget(target).GetComponent<PlayParticles>().trigger += () => Attack(caster, target, doesntAffectTurn);
    }

    protected virtual void OnCritical(){
        CameraHandler.ScreenShake(2,.25f);
    }

    void Attack(Unit caster, Unit target, bool doesntAffectTurn){
        float damage = caster.getAttack * atkMultiplier + baseDamage;

        ModifyDamage(ref damage, target);

        bool didDodge = target.doesDodge(hitModifier,caster);
        
        if(!didDodge){
            bool isCrit = caster.doesCrit(critChance);

            if(isCrit){
                OnCritical();
                damage *= critMultiplier;
            }
            int netDamage = caster.DealDamage(target,damage,attackType, doesntAffectTurn);
            if(!playParticlesOnMiss) PlayParticlesOnTarget(target);
            //MiniTextGenerator.current.CreateText(netDamage.ToString(),caster.transform,attackType);
        }
        else
        {
            PressTurnSystem.current.Next(false);
        }
        

    }
    protected virtual void ModifyDamage(ref float value, Unit target){}

}

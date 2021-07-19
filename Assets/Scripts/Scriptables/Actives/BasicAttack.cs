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
    [Range(-20,20)]
    public int hitModifier = 0;
    [Range(0,20)]
    public int critChance = 5;
    [Range(1,10)]
    public float critMultiplier = 2;

    public override void Trigger(Unit caster, Unit target){
        BattleLog.current.AddLog(caster.name + " used " + abilityName + " on " + target.name + ".");
        Attack(caster, target);
    }

    protected virtual void OnCritical(){
        CameraHandler.ScreenShake(2,.25f);
    }

    void Attack(Unit caster, Unit target){
        float damage = caster.getAttack * atkMultiplier + baseDamage;

        ModifyDamage(ref damage, target);

        bool didDodge = target.doesDodge(hitModifier,caster);
        
        if(!didDodge){
            bool isCrit = caster.doesCrit(critChance);

            if(isCrit){
                OnCritical();
                damage *= critMultiplier;
            }
            int netDamage = target.ReceiveDamage(Mathf.RoundToInt(damage));
            caster.DealtDamage(netDamage);
            PlayParticlesOnTarget(target);
            PingNumberOnTarget(netDamage.ToString(),isCrit,target);
            OnHit(caster, target);
        }
        

    }
    protected virtual void ModifyDamage(ref float value, Unit target){}
    protected virtual void OnHit(Unit caster, Unit target){}

}

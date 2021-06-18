using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Basic Attack")]
public class BasicAttack : Ability
{
    [Header("Attack Basic Stats")]
    public int baseDamage = 1;
    [Range(0,10)]
    public float atkMultiplier = 1;
    [Range(0,100)]
    public int critChance = 5;
    [Range(1.01f,10)]
    public float critMultiplier = 2;

    public override void Trigger(Unit caster, Unit target){
        Attack(caster, target);
    }

    protected virtual void OnCritical(){
        CameraHandler.ScreenShake(3,.5f);
    }

    void Attack(Unit caster, Unit target){
        float damage = caster.GetAttack() * atkMultiplier + baseDamage;

        ModifyDamage(ref damage, target);

        bool didDodge = Random.Range(0,100) <= target.GetAgility();
        
        if(!didDodge){
            bool isCrit = Random.Range(0,100) <= critChance;

            if(isCrit){
                OnCritical();
                damage *= critMultiplier;
            }
            int netDamage = target.ReceiveDamage(Mathf.RoundToInt(damage));

            PlayParticlesOnTarget(target);
            if(caster.playable){
                PingNumberOnTarget(netDamage,isCrit,target);
            }
        }
        else if(caster.playable){
            LeanTween.cancel(target.gameObject);
            LeanTween.move(target.gameObject, Vector3.left * .25f, .75f).setEasePunch();
        }
        

    }
    protected virtual void ModifyDamage(ref float value, Unit target){}

}

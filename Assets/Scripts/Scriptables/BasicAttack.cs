using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Basic Attack")]
public class BasicAttack : Abilities
{
    [Header("Attack Basic Stats")]
    public int baseDamage = 1;
    [Range(0,10)]
    public float atkMultiplier = 1;
    [Range(0,100)]
    public int critChance = 5;
    [Range(1.01f,10)]
    public float critMultiplier = 2;

    public override void Trigger(Unit caster, Unit[] targets){
        Attack(caster, targets[0]);
    }

    void Attack(Unit caster, Unit target){
        int damage = Mathf.RoundToInt(caster.GetAttack() * atkMultiplier) + baseDamage;
        ModifyDamage(ref damage, target);
        bool critical = Random.Range(0,100) <= critChance;
        damage = target.ReceiveDamage(Mathf.RoundToInt(damage * (critical ? critMultiplier : 1)));

        PlayParticlesOnTarget(target);
        if(caster.GetComponent<Player>()){
            PingNumberOnTarget(damage,critical,target);
        }

    }
    protected virtual void ModifyDamage(ref int value, Unit target){}

}

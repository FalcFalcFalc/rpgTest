using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Attack/Attack that Applies Status")]
public class AttackEffectChance : BasicAttack
{
    [SerializeField] StatusEffect effectToApply;
    [Tooltip("If true, applies to self, else, applies to attack target")]
    [SerializeField] bool selfApply;
    [Range(0,20)]
    [SerializeField] int statusChance;
    protected override void OnHit(Unit caster, Unit target){
        float valor = FalcTools.RandomZeroToInt(20);
        if(valor <= statusChance){
            (selfApply ? caster : target).AddStatusEffect(effectToApply);
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Attack Wounds")]
public class AttackWounds : BasicAttack
{
    [Header("Attack Wounds Stats")]
    [Range(0,1f)]
    [Tooltip("If health is at 0, damage will be amplified by (1+value).")]
    public float missingHealthMultiplier = .25f;
    
    protected override void ModifyDamage(ref int value, Unit target){
        value = Mathf.RoundToInt(value * (1 + (1 - target.GetCurrentHpPercentage()) * missingHealthMultiplier));
    }


}

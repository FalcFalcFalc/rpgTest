using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttacks")]
public class BasicAttacks : Abilities
{
    public int baseDamage = 1;
    [Range(0,10)]
    public float atkMultiplier = 1;
    [Range(0,100)]
    public int critChance = 5;
    [Range(1.01f,10)]
    public float critMultiplier = 2;
    public override void Trigger(Unit caster, Unit[] targets){
        ParticleSystem ps = Instantiate(particles, targets[0].transform);
        ps.Play();
        if(Random.Range(0,100) <= critChance)   targets[0].ReceiveDamage(Mathf.RoundToInt(caster.GetAttack() * atkMultiplier * critMultiplier) + baseDamage);
        else                                    targets[0].ReceiveDamage(Mathf.RoundToInt(caster.GetAttack() * atkMultiplier)                  + baseDamage);

    }

}

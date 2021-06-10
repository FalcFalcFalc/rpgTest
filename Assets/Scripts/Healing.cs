using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Healing")]
public class Healing : Abilities
{
    public int baseHealing = 1;
    [Range(0,10)]
    public float intMultiplier = 1;
    public override void Trigger(Unit caster, Unit[] targets){
        ParticleSystem ps = Instantiate(particles, targets[0].transform);
        ps.Play();
        targets[0].ReceiveHealing(caster.GetInteligence() + baseHealing);

    }

}

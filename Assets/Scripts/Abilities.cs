using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abilities : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
    public ParticleSystem particles;

    public abstract void Trigger(Unit caster, Unit[] targets);

}

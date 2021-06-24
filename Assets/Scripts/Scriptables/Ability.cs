using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [Header("Description")]
    public string abilityName;
    public string abilityDescription;
    public Sprite image;
    
    [Header("Visual FX")]
    public ParticleSystem particles;
    public BattleNumber indicator;
    [Header("Stats")] //in other classes this will make sense, since this is abstract
    int invis = 0;

    public abstract void Trigger(Unit caster, Unit target);

    protected void PlayParticlesOnTarget(Unit target){
        Instantiate(particles, target.transform).GetComponent<ParticleSystem>().Play();
    }
    protected void PingNumberOnTarget(string txt, bool critical, Unit target){
        Instantiate(indicator, target.transform).GetComponent<BattleNumber>().Set(numb,critical);
    }
}

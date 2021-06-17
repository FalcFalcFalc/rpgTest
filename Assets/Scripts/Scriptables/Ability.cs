using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [Header("Ability Description")]
    public string abilityName;
    public string abilityDescription;
    public Sprite image;
    
    [Header("Visual FX")]
    public ParticleSystem particles;
    public BattleNumber indicator;
    

    public abstract void Trigger(Unit caster, Unit target);

    protected void PlayParticlesOnTarget(Unit target){
        Instantiate(particles, target.transform).GetComponent<ParticleSystem>().Play();
    }
    protected void PingNumberOnTarget(int numb, bool critical, Unit target){
        Instantiate(indicator, target.transform).GetComponent<BattleNumber>().Set(numb,critical);
    }
}

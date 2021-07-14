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

    protected ParticleSystem PlayParticlesOnTarget(Unit target){
        ParticleSystem inst = Instantiate(particles, target.transform);
        inst.Play();
        return inst;
        //inst.transform.parent = null;
    }
    protected void PingNumberOnTarget(string txt, bool critical, Unit target){
        //BattleNumber inst = Instantiate(indicator, target.transform);
        BattleNumber inst = Instantiate(indicator, target.GetOriginalPosition(), Quaternion.identity);
        inst.Set(txt,critical);
        //inst.transform.parent = null;
    }
}

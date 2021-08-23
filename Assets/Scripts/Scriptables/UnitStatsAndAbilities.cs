using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class UnitStatsAndAbilities : ScriptableObject
{


    public string unitName;

    /*
    
        might & speed -> phy dmg dealer
        might & will  -> meat tank
        might & mind  -> heavy mag dmg dealer
        speed & will  -> blink tank
        speed & mind  -> dps mag dmg dealer
        will  & mind  -> healer

    */
    [Range(1,20)]
    public int might = 10, speed = 10, will = 10, mind = 10, luck = 10;

    public List<Ability> atk;
    public List<Support> sup;
    public List<Pasive> perk;

    //public int randomPool = 0;

    public int maxHp{
        get{return will * (2+might) + 20;}
    }
    public int maxMp{
        get{return mind * (1+might) + 15;}
    }

    public SerializableDictionary<Keywords.Elements,Keywords.DamageResistances> resistances = new SerializableDictionary<Keywords.Elements, Keywords.DamageResistances>(){
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Absolute, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Light, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Dark, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Fire, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Water, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Wind, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Plant, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Slash, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Strike, Keywords.DamageResistances.Normal),
        new SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair(Keywords.Elements.Pierce, Keywords.DamageResistances.Normal)
    };
    
}

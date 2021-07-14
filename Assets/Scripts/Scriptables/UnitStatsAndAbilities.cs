using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class UnitStatsAndAbilities : ScriptableObject
{

    public string unitName;

    /*
    
        might & speed -> phy dmg dealer
        might & will  -> blood tank
        might & mind  -> heavy mag dmg dealer
        speed & will  -> blink tank
        speed & mind  -> dps mag dmg dealer
        will  & mind  -> healer

    */
    [Range(1,20)]
    public int might, speed, will, mind;

    public List<Ability> atk;
    public List<Support> sup;
    public List<Pasive> perk;

    public int randomPool = 0;

    public int maxHp{
        get{return will * (1+might);}
    }
    public int maxMp{
        get{return mind * (1+might);}
    }

}

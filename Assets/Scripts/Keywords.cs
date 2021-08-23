using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Keywords{

    public static T Next<T>(this T src) where T : struct
    {
        //if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length==j) ? Arr[0] : Arr[j];            
    }

    public enum Stat
    {
        ATK,
        INT,
        DEX,
        DEF,
        LUK
    }

    public enum Elements
    {
        Strike, //PHYS
        Pierce,
        Slash,
        Fire, //ELEMENTAL
        Water,
        Wind,
        Plant,
        Light, //HOLY
        Dark,
        Blessing, //SUPPORT
        Curse,
        Absolute //TRUE
    }

    public enum Buff
    {
        DMG,
        DEF,
        DEX
    }

    public enum DamageResistances
    {
        Normal,
        Strong,
        Weak,
        Fragile,
        Void,
        Absorb,
        Reject,
        Endure
    }

    public enum SelectionPriority{
        MostBuffed,
        LeastBuffed,
        MostATK,
        MostINT,
        MostDEX,
        MostHP,
        LeastATK,
        LeastINT,
        LeastDEX,
        LeastHP,
        NeedsHealing,
        Random
    }
}

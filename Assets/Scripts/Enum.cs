using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum{

    public enum Stat
    {
        ATK,
        INT,
        DEF,
        DEX
    }

    public enum SelectionPriority{
        MostBuffed,
        LeastBuffed,
        MostATK,
        MostINT,
        MostDEX,
        MostDEF,
        MostHP,
        LeastATK,
        LeastINT,
        LeastDEX,
        LeastDEF,
        LeastHP,
        NeedsHealing,
        Random
    }
}

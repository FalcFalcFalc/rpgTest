using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum{

    public enum Stat
    {
        ATK,
        INT,
        DEX
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

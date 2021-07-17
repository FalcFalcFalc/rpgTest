using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject {
    public string effectName, description;
    public int duration;

    public abstract void Enable(Unit self);
    public abstract void Disable(Unit self);

}
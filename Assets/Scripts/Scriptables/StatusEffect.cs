using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : Pasive {
    public string effectName, description;
    public int duration;
    public bool autoStops = false;

    public override abstract void Enable(Unit self);
    public override abstract void Disable(Unit self);

}
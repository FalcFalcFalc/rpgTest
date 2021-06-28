using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pasive : Ability
{
    public abstract void Enable(Unit self);
    public abstract void Disable(Unit self);

}

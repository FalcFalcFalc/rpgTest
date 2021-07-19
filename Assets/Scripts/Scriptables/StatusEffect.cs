using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : Pasive {
    public string effectName, description;
    public int duration;
    public bool autoStops = false;
    public GameObject icon;
    GameObject iconInstance;
    public Vector3 iconOffset;

    protected void PlaceIcon(Unit target){
        iconInstance = Instantiate(icon, target.transform.position + iconOffset, Quaternion.identity);
        iconInstance.transform.parent = target.transform;
    }

    protected void DestroyIcon(){
        //Debug.Log("destruyendo " + iconInstance);
        Destroy(iconInstance);
    }

    public override abstract void Enable(Unit self);
    public override abstract void Disable(Unit self);

}
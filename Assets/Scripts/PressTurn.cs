using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressTurn : MonoBehaviour
{
    Animator anim;
    [SerializeField]public bool triggered {get; private set;} = false;
    public void UseHalf()
    {
        triggered = true;
        anim.SetTrigger("blinking");
    }
    public void UseWhole()
    {
        anim.SetTrigger("used");
    }
    public void Miss(){
        anim.SetTrigger("missed");
    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }
}

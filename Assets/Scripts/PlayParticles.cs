using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem psStart;
    [SerializeField] ParticleSystem psStop;

    public void PLAY_PARTICLES(){
        psStart.Play();
    }

    public void STOP_PARTICLES(){
        psStart.Stop();
    }

    public void TRIGGER(){
        if(trigger != null) trigger();
    }

    public void CALL_BLACKOUT(){
        Background.current.BlackOut();
    }

    public Action trigger;
}

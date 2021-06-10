using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    ParticleSystem ps;
    private void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if(!ps.IsAlive()){
            Destroy(gameObject);
        }
    }


}

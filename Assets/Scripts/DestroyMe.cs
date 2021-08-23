using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public int autoDestroy = -1;

    private void Start() {
        if(autoDestroy > 0){
            Destroy(gameObject,autoDestroy);
        }
    }

    public void DESTROY_ME(){
        Destroy(gameObject);
    }
}

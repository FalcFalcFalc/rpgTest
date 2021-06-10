using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float alcanceAbsoluto;
    public float velocidad;
    public Vector3 direccion;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direccion.normalized * velocidad;
        if(transform.position.magnitude >= alcanceAbsoluto){
            transform.position = transform.position - direccion.normalized * alcanceAbsoluto;
        }
    }
}

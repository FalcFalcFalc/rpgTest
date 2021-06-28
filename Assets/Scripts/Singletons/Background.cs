using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background current;
    public float alcanceAbsoluto;
    float alc;
    public float velocidad;
    Vector3 direccion;
    public Sprite[] fondos;

    void Awake() {
        current = this;
    }

    void Start() {
        do
        {
            direccion = new Vector3(
                Mathf.RoundToInt(Random.Range(-1.5f,1.5f)),
                Mathf.RoundToInt(Random.Range(-1.5f,1.5f)),
                0);
        } while (direccion == Vector3.zero);
        alc = (direccion*alcanceAbsoluto).magnitude;

        int a = Random.Range(0,fondos.Length);                
        foreach (Transform item in transform)
        {
            item.GetComponent<SpriteRenderer>().sprite = fondos[a];
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direccion.normalized * velocidad * Time.deltaTime;
        if(transform.position.magnitude >= alc){
            transform.position = transform.position - direccion.normalized * alc;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background current;
    [SerializeField] float alcanceAbsoluto;
    float alc;
    [SerializeField] float velocidad;
    Vector3 direccion;
    [SerializeField] Sprite[] fondos;
    [SerializeField] float tensionVelocityMultiplier = 1.66666f;

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

    public void Accelerate(){
        velocidad *= tensionVelocityMultiplier;
    }

    public void Decelerate(){
        velocidad /= tensionVelocityMultiplier;
    }


}

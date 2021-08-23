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
    
    [ContextMenu("BlackOut")]
    public void BlackOut(){
        if (currentlyFading != null) StopCoroutine(currentlyFading);
        
        foreach (Transform item in transform)
        {
            item.GetComponent<SpriteRenderer>().color = Color.black;
        }
        currentlyFading = StartCoroutine(FadeToNormal());
    }

    Coroutine currentlyFading;

    IEnumerator FadeToNormal(){
        yield return new WaitForSeconds(0.1f);

        Color starting = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.02f);
            foreach (Transform item in transform)
            {
                item.GetComponent<SpriteRenderer>().color = Color.Lerp(starting, Color.white, i/20f);
            }
        }
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
        if((transform.position - Vector3.forward * transform.position.z).magnitude >= alc){
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

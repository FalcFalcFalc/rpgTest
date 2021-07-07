using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageImage : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    [SerializeField] Vector2 delay = Vector2.one * 0.2f;
    

    Sprite currentImage {
        get{return images[index];}
    }

    bool stopped = false;


    Image imagen;
    int index = 0;

    private void Start() {
        stopped = false;

        StartCoroutine(ImageChange());
        imagen = GetComponent<Image>();
        imagen.sprite = currentImage;
    }

    IEnumerator ImageChange(){
        yield return new WaitForSeconds(FalcTools.RandomFloatFromVector2(delay));

        int newIndex = index;
        while((newIndex = FalcTools.RandomZeroToInt(images.Length)) == index);

        index = newIndex;

        imagen.sprite = currentImage;

        if(!stopped) StartCoroutine(ImageChange());

    }
}

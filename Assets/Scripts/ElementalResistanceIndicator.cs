using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ElementalResistanceIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI texto;
    Keywords.DamageResistances dmgRST;
    Keywords.Elements elmt;

    public void Set(Sprite img, string res, string action, Color clr, Keywords.Elements elmt){
        texto.SetText(res);
        texto.color = clr;
        GetComponent<Image>().sprite = img;
        GetComponent<Image>().color = clr;
        GetComponent<InfoDisplayerTrigger>().Set("","This unit " + action + " " + elmt + " based attacks.");
    }   
}

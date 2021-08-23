using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleNumber : MonoBehaviour
{
    public void SetCritical(){
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text + "!");
    }

    public BattleNumber SetText(string txt){
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(txt);
        return this;
    }

    public BattleNumber SetColor(Color color){
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
        return this;
    }
}

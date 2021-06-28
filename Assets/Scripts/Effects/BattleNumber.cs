using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleNumber : MonoBehaviour
{
    public void Set(string txt, bool crit){
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(txt + (crit ? "!" : ""));
    }

    public void DestroyMe(){
        Destroy(gameObject);
    }
}

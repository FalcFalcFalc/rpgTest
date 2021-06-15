using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleNumber : MonoBehaviour
{
    public void Set(int dmg, bool crit){
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(dmg + (crit ? "!" : ""));
    }

    public void DestroyMe(){
        Destroy(gameObject);
    }
}

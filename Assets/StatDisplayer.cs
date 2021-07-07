using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayer : MonoBehaviour
{
    public static StatDisplayer current;
    List<TextMeshProUGUI> texts;


    private void Awake() {
        current = this;
    }

    void Start()
    {
        texts = new List<TextMeshProUGUI>();
        texts.Add(GameObject.Find("strText").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("dexText").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("defText").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("intText").GetComponent<TextMeshProUGUI>());

        Hide();
    }

    public void Show(Unit stats){

        int str = stats.getAttack;
        int dex = stats.getAgility;
        int def = stats.getDefense;
        int intel = stats.getInteligence;

        texts[0].SetText(str.ToString());
        texts[1].SetText(dex.ToString());
        texts[2].SetText(def.ToString());
        texts[3].SetText(intel.ToString());

        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    private void Update() {
        transform.position = Input.mousePosition + 75 * Vector3.up;
    }
}

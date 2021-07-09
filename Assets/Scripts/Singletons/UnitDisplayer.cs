using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitDisplayer : MonoBehaviour
{
    [SerializeField] GameObject imagePlaceholder;
    public static UnitDisplayer current;
    List<TextMeshProUGUI> texts;

    Transform lista;


    private void Awake() {
        current = this;
    }

    void Start()
    {
        lista = GameObject.Find("listOfAbilities").transform;
        ClearAbilities();
        texts = new List<TextMeshProUGUI>();

        texts.Add(GameObject.Find("strTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("dexTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("defTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("intTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("nameTooltipField").GetComponent<TextMeshProUGUI>());


        Hide();
    }

    public void Show(Unit stats){

        ClearAbilities();

        int str = stats.getAttack,
            dex = stats.getAgility,
            def = stats.getDefense,
            intel = stats.getInteligence;

        texts[0].SetText(str.ToString());
        texts[1].SetText(dex.ToString());
        texts[2].SetText(def.ToString());
        texts[3].SetText(intel.ToString());
        texts[4].SetText(stats.name);

        foreach (Ability item in stats.attackMoves)
        {
            AddAbility(item);
        }
        foreach (Ability item in stats.supportMoves)
        {
            AddAbility(item);
        }
        foreach (Ability item in stats.perks)
        {
            AddAbility(item);
        }

        gameObject.SetActive(true);
    }

    void AddAbility(Ability obj){
        AbilityDispalyTrigger ab = Instantiate(imagePlaceholder,lista).GetComponent<AbilityDispalyTrigger>();
        ab.SetAbility(obj);
    }

    void ClearAbilities(){
        foreach (Transform item in lista)
        {
            Destroy(item.gameObject);
        }
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    private void Update() {
        //transform.position = Input.mousePosition + 75 * Vector3.up;
    }
}

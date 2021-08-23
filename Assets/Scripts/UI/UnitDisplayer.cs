using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitDisplayer : MonoBehaviour
{
    [SerializeField] GameObject abilityPrefab, elementPrefab;
    public static UnitDisplayer current;
    List<TextMeshProUGUI> texts;

    Transform habilidades,resistencias;

    [SerializeField] SerializableDictionary<Keywords.Elements,Sprite> elementIcons = new SerializableDictionary<Keywords.Elements,Sprite>();

    [SerializeField] SerializableDictionary<Keywords.DamageResistances,string> resistanceTxts = new SerializableDictionary<Keywords.DamageResistances, string>(){
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Normal, "-"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Strong, "STR"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Weak, "WK"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Fragile, "FRG"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Void, "NULL"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Absorb, "ABS"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Reject, "REJ"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Endure, "SRV")
    };

    [SerializeField] SerializableDictionary<Keywords.DamageResistances,string> actionTXT = new SerializableDictionary<Keywords.DamageResistances, string>(){
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Normal, "-"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Strong, "resists"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Weak, "is weak against"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Fragile, "is fragile against"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Void, "doesn't get hurt against"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Absorb, "absorbs"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Reject, "rejects"),
        new SerializableDictionary<Keywords.DamageResistances,string>.Pair(Keywords.DamageResistances.Endure, "can't die against"),
    };


    private void Awake() {
        current = this;
    }

    void Start()
    {
        habilidades = GameObject.Find("listOfAbilities").transform;
        resistencias = GameObject.Find("listOfElementalResistances").transform;
        ClearAbilities();
        texts = new List<TextMeshProUGUI>();

        texts.Add(GameObject.Find("strTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("dexTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("defTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("nameTooltipField").GetComponent<TextMeshProUGUI>());


        Hide();
    }

    public void Show(Unit stats){

        ClearAbilities();
        ClearResistances();

        int str = stats.getAttack,
            dex = stats.getAgility,
            def = stats.getDefense;

        texts[0].SetText(str.ToString());
        texts[1].SetText(dex.ToString());
        texts[2].SetText(def.ToString());
        texts[3].SetText(stats.name);

        foreach (SerializableDictionary<Keywords.Elements,Keywords.DamageResistances>.Pair item in stats.resistances)
        {
            if(item.Value == Keywords.DamageResistances.Normal)
                continue;
            AddElementalRes(item.Key,item.Value);
        }

        foreach (Ability item in stats.getAttackMoves())
        {
            AddAbility(item);
        }
        foreach (Ability item in stats.getSupportMoves())
        {
            AddAbility(item);
        }
        foreach (Ability item in stats.getPerks())
        {
            AddAbility(item);
        }

        gameObject.SetActive(true);
    }

    void AddAbility(Ability obj){
        InfoDisplayerTrigger ab = Instantiate(abilityPrefab,habilidades).GetComponent<InfoDisplayerTrigger>();
        ab.SetAbility(obj);
    }

    void AddElementalRes(Keywords.Elements elem,Keywords.DamageResistances res){
        ElementalResistanceIndicator er = Instantiate(elementPrefab,resistencias).GetComponent<ElementalResistanceIndicator>();
        er.Set(elementIcons[elem], resistanceTxts[res], actionTXT[res], MiniTextGenerator.current.elementalColors[elem], elem);
    }

    void ClearAbilities(){
        foreach (Transform item in habilidades)
        {
            Destroy(item.gameObject);
        }
    }

    void ClearResistances(){
        foreach (Transform item in resistencias)
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

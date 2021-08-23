using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniTextGenerator : MonoBehaviour
{
    public static MiniTextGenerator current;
    private void Awake() {
        current = this;
    }

    [SerializeField] BattleNumber prefab;

    [SerializeField]
    public SerializableDictionary<Keywords.Elements,Color> elementalColors = new SerializableDictionary<Keywords.Elements, Color>(){
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Absolute,    Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Light,       Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Dark,        Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Fire,        Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Water,       Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Wind,        Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Plant,       Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Slash,       Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Strike,      Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Blessing,    Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Curse,       Color.white),
        new SerializableDictionary<Keywords.Elements,Color>.Pair(Keywords.Elements.Pierce,      Color.white)
    };

    public void CreateText(string text,Transform where,Keywords.Elements element){
        if(element == Keywords.Elements.Absolute){
            StartCoroutine(AbsoluteColor(Instantiate(prefab,where.position,Quaternion.identity).SetText(text)));
        }
        else
        {        
            print(where);
            Instantiate(prefab,where.position,Quaternion.identity).SetText(text).SetColor(elementalColors[element]);
        }
    }

    public void CreateText(string text,Transform where){      
        Instantiate(prefab,where.position,Quaternion.identity).SetText(text);
    }
    
    IEnumerator AbsoluteColor(BattleNumber assigned){
        float eval = Random.Range(0,1f);
        for (int i = 0; i < 45; i++)
        {
            eval += 0.05f;
            if(eval > 1) eval--;
            assigned.SetColor(rainbow.Evaluate(eval));
            yield return new WaitForSeconds(0.015f);
        }
    }

    [SerializeField] Gradient rainbow;

    public void CreateText(string text,Transform where,Keywords.Elements element,bool critical){
        BattleNumber a = Instantiate(prefab,where.position,Quaternion.identity).SetText(text).SetColor(elementalColors[element]);
        if (critical) a.SetCritical();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoDisplayer : MonoBehaviour
{
    [SerializeField] GameObject imagePlaceholder;
    public static InfoDisplayer current;
    List<TextMeshProUGUI> texts;
    RectTransform rectTransform;

    private void Awake() {
        current = this;
    }

    void Start()
    {
        texts = new List<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        texts.Add(GameObject.Find("abilityNameTooltipField").GetComponent<TextMeshProUGUI>());
        texts.Add(GameObject.Find("descriptionNameTooltipField").GetComponent<TextMeshProUGUI>());


        Hide();
    }

    public void Show(Ability description){


        string name  = description.abilityName,
                desc = description.abilityDescription;

        texts[0].SetText(name.ToString());
        texts[1].SetText(desc.ToString());

        gameObject.SetActive(true);
    }

    public void Show(string title, string desc){

        texts[0].SetText(title.ToString());
        texts[1].SetText(desc.ToString());

        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    private void Update() {
        Vector2 posicion = Input.mousePosition;
        float pivotX = posicion.x / Screen.width,
              pivotY = posicion.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = posicion + 30 * Vector2.up;
    }
}

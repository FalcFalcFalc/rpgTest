using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InfoDisplayerTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string title, description;
    Ability desc = null;
    public void SetAbility(Ability input){
        desc = input;
        GetComponent<Image>().sprite = desc.image;
    }

    public void Set(string text, string subtext){
        title = text;
        description = subtext;
    }

    private void OnMouseEnter() {
        InfoDisplayer.current.Show(desc);
    }

    private void OnMouseExit() {
        InfoDisplayer.current.Hide();
    }

    public void OnPointerEnter(PointerEventData context) {
        if(desc != null) InfoDisplayer.current.Show(desc);
        else InfoDisplayer.current.Show(title, description);
    }

    public void OnPointerExit(PointerEventData context) {
        InfoDisplayer.current.Hide();
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)){
            StartCoroutine(delayUpdateStats());
        }
    }

    IEnumerator delayUpdateStats(){
        yield return new WaitForSeconds(0.06f);
        InfoDisplayer.current.Show(desc);
    }
}

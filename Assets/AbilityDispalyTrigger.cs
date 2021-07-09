using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class AbilityDispalyTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Ability desc;
    public void SetAbility(Ability input){
        desc = input;
        GetComponent<Image>().sprite = desc.image;
    }

    private void OnMouseEnter() {
        AbilityDispalyer.current.Show(desc);
    }

    private void OnMouseExit() {
        AbilityDispalyer.current.Hide();
    }

    public void OnPointerEnter(PointerEventData context) {
        AbilityDispalyer.current.Show(desc);
    }

    public void OnPointerExit(PointerEventData context) {
        AbilityDispalyer.current.Hide();
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)){
            StartCoroutine(delayUpdateStats());
        }
    }

    IEnumerator delayUpdateStats(){
        yield return new WaitForSeconds(0.06f);
        AbilityDispalyer.current.Show(desc);
    }
}

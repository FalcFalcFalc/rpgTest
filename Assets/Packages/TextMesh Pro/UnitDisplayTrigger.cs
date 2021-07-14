using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDisplayTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Unit stats;
    void Start()
    {
        stats = GetComponent<Unit>();
    }


    private void OnMouseEnter() {
        UnitDisplayer.current.Show(stats);
    }

    private void OnMouseExit() {
        //UnitDisplayer.current.Hide();
    }

    public void OnPointerEnter(PointerEventData context) {
        UnitDisplayer.current.Show(stats);
    }

    public void OnPointerExit(PointerEventData context) {
        //UnitDisplayer.current.Hide();
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)){
            StartCoroutine(delayUpdateStats());
        }
    }

    IEnumerator delayUpdateStats(){
        yield return new WaitForSeconds(0.06f);
        UnitDisplayer.current.Show(stats);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatDisplayTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Unit stats;
    void Start()
    {
        stats = GetComponent<Unit>();
    }


    private void OnMouseEnter() {
        StatDisplayer.current.Show(stats);
    }

    private void OnMouseExit() {
        StatDisplayer.current.Hide();
    }

    public void OnPointerEnter(PointerEventData context) {
        StatDisplayer.current.Show(stats);
    }

    public void OnPointerExit(PointerEventData context) {
        StatDisplayer.current.Hide();
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)){
            StartCoroutine(delayUpdateStats());
        }
    }

    IEnumerator delayUpdateStats(){
        yield return new WaitForSeconds(0.06f);
        StatDisplayer.current.Show(stats);
    }
}

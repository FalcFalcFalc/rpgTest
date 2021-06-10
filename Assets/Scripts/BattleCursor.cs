using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCursor : MonoBehaviour
{
    public Unit selected;
    int idSelectionAnim = 0;

    public void SelectNewUnit(Unit target){
        LeanTween.cancel(gameObject,idSelectionAnim);

        idSelectionAnim = LeanTween.move(gameObject, target.transform.position, .5f).setEaseInOutBack().id;
        selected = target;
    }

    public void Bump(){
        LeanTween.move(gameObject, transform.position + Vector3.up, .5f).setEase(LeanTweenType.easeSpring);
    }

    public void ChangeColor(Color color){
        LeanTween.color(gameObject, color, .75f).setEaseInBack();
    }

}

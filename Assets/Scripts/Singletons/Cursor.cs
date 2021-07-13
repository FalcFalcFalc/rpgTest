using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public static Cursor current;
    void Awake() {
        current = this;
        //UnityEngine.Cursor.visible = false;
    } 

    void Start() {
        ts = TurnHandler.current;
    }
    
    public Unit selected;
    int idSelectionAnim = 0;
    TurnHandler ts;

    public void Select(Unit target){
        lerpOg = target.transform.position;
        lerpOg.z = Camera.main.nearClipPlane + 2;

        StopAnimations();
        
        idSelectionAnim = LeanTween.move(gameObject, lerpOg, .25f).setEaseOutCubic().id;
        selected = target;
    }

    void StopAnimations(){
        LeanTween.cancel(gameObject,idSelectionAnim);
    }

    public void Deselect(){
        //StopAnimations();
        selected = null;

    }

     

    Vector3 lerpOg;
    Vector3 mousePosition {
        get{
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 2;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    public void Bump(){
        LeanTween.move(gameObject, transform.position + Vector3.up, .75f).setEase(LeanTweenType.punch);
    }

    private void Update() {
        GetComponent<Animator>().SetBool("selecting",!(selected == null) && ts.isPlayerActing());


        if(selected != null)
        {
            if(ts.isPlayerActing() && Input.GetMouseButtonUp(0))
            {
                Unit caster = ts.GetCurrentUnit();
                if(selected.playable && caster.hasSupportMoves)
                    ts.GetCurrentUnit().Support(selected);
                else if(!selected.playable && caster.hasAttackMoves)
                    caster.Attack(selected);
                Bump();
                //Deselect();
            }
        }
        
    }

}

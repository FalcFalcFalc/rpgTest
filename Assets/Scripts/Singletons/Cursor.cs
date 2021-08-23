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
        //ts = TurnHandler.current;
        pts = PressTurnSystem.current;
    }
    
    public Unit selected;
    int idSelectionAnim = 0;
    TurnHandler ts;
    PressTurnSystem pts;

    public void Select(Unit target){
        lerpOg = target.GetOriginalPosition();;
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
        LeanTween.move(gameObject, transform.position + 2*Vector3.up, .75f).setEase(LeanTweenType.punch);
    }

    private void Update() {
        GetComponent<Animator>().SetBool("selecting",!(selected == null) && pts.alliesActive);


        if(selected != null)
        {
            if(pts.alliesActive && Input.GetMouseButtonUp(0))
            {
                Unit currentUnit = pts.currentUnit;
                if(Input.GetMouseButtonUp(0) && currentUnit.isActive)
                {
                    if(selected.playerParty() && currentUnit.hasSupportMoves)
                    {
                        currentUnit.Support(selected);
                    }
                    else if(!selected.playerParty() && currentUnit.hasAttackMoves)
                    {
                        currentUnit.Attack(selected);
                    }
                    currentUnit.Deactivate();
                    Bump();
                }                
            }
        }
        
    }

}

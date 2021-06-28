using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCursor : MonoBehaviour
{

    public static BattleCursor current;
    void Awake() {
        current = this;
    } 

    void Start() {
        ts = TurnHandler.current;
    }
    
    public Unit selected;
    int idSelectionAnim = 0;
    TurnHandler ts;

    public void SelectNewUnit(Unit target){
        LeanTween.cancel(gameObject,idSelectionAnim);
        
        idSelectionAnim = LeanTween.move(gameObject, target.transform.position, .5f).setEaseInOutCirc().id;
        selected = target;
    }

    public void Bump(){
        LeanTween.move(gameObject, transform.position + Vector3.up, .5f).setEase(LeanTweenType.punch);
    }

    Color destino = Color.red;
    public void ChangeColor(Color color){
        if(color != destino) {
            LeanTween.color(gameObject, color, .75f).setEaseInBack();
            destino = color;
        }
    }

    private void Update() {
        if(ts.isPlayerActing()){
            //ChangeColor(Color.cyan);
            if(Input.GetMouseButtonUp(0) && selected != null){
                Unit caster = ts.GetCurrentUnit();
                if(selected.playable && caster.hasSupportMoves){
                    ts.GetCurrentUnit().Support(selected);
                }
                else if(!selected.playable && caster.hasAttackMoves)
                {
                    caster.Attack(selected);
                }
                Bump();
                selected = null;
            }
        }
        else
        {
            //ChangeColor(Color.red);
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Unit
{
    new void Start() {
        base.Start();
    }

    private void OnMouseOver() {
        if(turnHandler.isPlayerActing() && cursor.selected != this) cursor.SelectNewUnit(this);
    }

    IEnumerator ArtificialInteligence(){
        if(cursor.selected != this) cursor.SelectNewUnit(this);
        else cursor.Bump();
        yield return new WaitForSeconds(.75f);
        if(hasHealingMoves())
        {   
            Enemigo mostWoundedAlly = turnHandler.GetEnemigos()[0];
            for(int i = 1; i < turnHandler.GetEnemigos().Count; i++){

                if(mostWoundedAlly.GetCurrentHpPercentage() >= turnHandler.GetEnemigos()[i].GetCurrentHpPercentage()){

                    mostWoundedAlly = turnHandler.GetEnemigos()[i];
                }
            }
            if(mostWoundedAlly.GetCurrentHpPercentage() < .45f || !hasAttackMoves()){
                // CameraHandler.Focus(mostWoundedAlly.transform);
                // yield return new WaitForSeconds(.65f);
                Heal(mostWoundedAlly);
            }
            else
            {
                Attack(turnHandler.GetPlayer()[Random.Range(0,turnHandler.GetPlayer().Count)]);
            }
        }
        else if(turnHandler.GetPlayer().Count > 0 && hasAttackMoves())
        {
            Attack(turnHandler.GetPlayer()[Random.Range(0,turnHandler.GetPlayer().Count)]);
        }
        else
        {
            NextTurn();
        }

        
    }


    Coroutine ia;
    protected override void OnActivate(){
        ia = StartCoroutine(ArtificialInteligence());
    }
    protected override void OnTurnEnd(){
    }

}

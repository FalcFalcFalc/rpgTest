using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Unit
{
    void OnDisable() {
        onActivate -= StartAI;
    }

    void OnEnable() {
        onActivate += StartAI;
    }

    void StartAI(){
        StartCoroutine(ArtificialInteligence());
    }

    IEnumerator ArtificialInteligence(){
        yield return new WaitForSeconds(.75f);
        if(hasHealingMoves)
        {   
            Enemigo mostWoundedAlly = turnHandler.GetEnemigos()[0];
            for(int i = 1; i < turnHandler.GetEnemigos().Count; i++){
                if(mostWoundedAlly.GetCurrentHpPercentage >= turnHandler.GetEnemigos()[i].GetCurrentHpPercentage){
                    mostWoundedAlly = turnHandler.GetEnemigos()[i];
                }
            }
            if(mostWoundedAlly.GetCurrentHpPercentage < .35f || !hasAttackMoves){
                Heal(mostWoundedAlly);
            }
            else
            {
                Attack(turnHandler.GetPlayer()[Random.Range(0,turnHandler.GetPlayer().Count)]);
            }
        }
        else if(turnHandler.GetPlayer().Count > 0 && hasAttackMoves)
        {
            Attack(turnHandler.GetPlayer()[Random.Range(0,turnHandler.GetPlayer().Count)]);
        }
        else
        {
            NextTurn();
        }
    }

}

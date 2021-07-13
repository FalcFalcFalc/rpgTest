using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnHandler : MonoBehaviour
{
    public static TurnHandler current;
    public float duration = .75f;
    void Awake() {
        current = this;
    } 

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    [Serializable]
    private class Iniciativa{
        public Unit who;
        [Range(1,5)]
        public int acc = 1;
        [HideInInspector]
        public int currentAcc;
    }
    [SerializeField] List<Iniciativa> units;

    [SerializeField] RectTransform winScreen;
    [SerializeField] RectTransform loseScreen;
    [SerializeField] RectTransform restart;

    Unit died = null;
    public void RemoveUnitFromInitiative(Unit who){
        died = who;
        if(who.playable)
            Background.current.Accelerate();
        else
            Background.current.Decelerate();
    }

    private void Start() {
        foreach (Iniciativa item in units)
        {
            item.currentAcc = item.acc;
        }
        currentUnit++;
        GetCurrentUnit().Activate();
    }

    public List<Unit> GetEnemigos(){
        List<Unit> retorno = new List<Unit>();
        foreach (Iniciativa item in units)
        {
            if(!item.who.playable){
                retorno.Add(item.who);
            }
        }
        return retorno;
    }

    public List<Unit> GetPlayer(){
        List<Unit> retorno = new List<Unit>();
        foreach (Iniciativa item in units)
        {
            if(item.who.playable){
                retorno.Add(item.who);
            }
        }
        return retorno;
    }
    bool playerActing = false;
    public bool isPlayerActing(){
        return playerActing;
    }

    int currentUnit = -1;
    public Unit GetCurrentUnit(){
        if(currentUnit >= units.Count){
            currentUnit = 0;
        }

        return units[currentUnit].who;
    }

    public void OneMore(Unit who){
        GetInitiative(who).currentAcc++;
    }

    Iniciativa GetInitiative(Unit whose){
        foreach (Iniciativa item in units)
        {
            if(item.who == whose) return item; 
        }
        return null;
    }
    
    public void NextTurn(){
        if(died != null){        
            units.Remove(GetInitiative(died));
            died = null;
        }

        bool lastUnitWasPlayer = false;
        if(currentUnit > -1) {
            lastUnitWasPlayer = GetCurrentUnit().playable;
        }

        if(GetPlayer().Count == 0){
            loseScreen.gameObject.SetActive(true);
            restart.gameObject.SetActive(true);
        }
        else if(GetEnemigos().Count == 0)
        {
            winScreen.gameObject.SetActive(true);
            restart.gameObject.SetActive(true);
        }
        else
        {
            bool newUnit = --units[currentUnit].currentAcc == 0;
            if(newUnit){
                units[currentUnit].currentAcc = units[currentUnit].acc;
                currentUnit++;
            }
        
            playerActing = GetCurrentUnit().playable;
            float delay = .2f;
            if(lastUnitWasPlayer && !playerActing){
                delay += .1f;
            }
            StartCoroutine(EndTurnDelay(delay));
        }
    }

    IEnumerator EndTurnDelay(float time){
        yield return new WaitForSeconds(time);
        GetCurrentUnit().Activate();
    }

    // List<Unit> turnOrder;
    // enum SortOrder
    // {
    //     byAgility,
    //     byStrength,
    //     byDefense,
    //     basic
    // }

    // void ReorderInitiative(SortOrder criteria){
    //     switch (criteria)
    //     {
    //         case SortOrder.byAgility:
    //             turnOrder = new List<Unit>();
    //             foreach (Unit item in units)
    //             {
    //                 if
    //             }
    //             break;
    //         case SortOrder.byDefense:
    //             break;
    //         case SortOrder.byStrength:
    //             break;
    //         case SortOrder.basic:
    //             break;
    //         default:
    //     }
    // }
}

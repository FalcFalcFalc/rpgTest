using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    [SerializeField] List<Unit> units;

    [SerializeField] RectTransform winScreen;
    [SerializeField] RectTransform loseScreen;

    Unit died = null;
    public void RemoveUnitFromInitiative(Unit who){
        died = who;
    }

    private void Start() {
        NextTurn();
    }

    public List<Enemigo> GetEnemigos(){
        List<Enemigo> retorno = new List<Enemigo>();
        foreach (Unit item in units)
        {
            if(!item.playable){
                retorno.Add(item.GetComponent<Enemigo>());
            }
        }
        return retorno;
    }

    public List<Player> GetPlayer(){
        List<Player> retorno = new List<Player>();
        foreach (Unit item in units)
        {
            if(item.playable){
                retorno.Add(item.GetComponent<Player>());
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
        print(units.Count + " y " + currentUnit);

        if(currentUnit >= units.Count){
            currentUnit = 0;
            print("Fuimos a 0: " + units.Count + " y " + currentUnit);
        }


        return units[currentUnit];
    }
    
    public void NextTurn(){
        if(died != null){        
            units.Remove(died);
            died = null;
        }

        bool lastUnitWasPlayer = false;
        if(currentUnit > -1) {
            lastUnitWasPlayer = GetCurrentUnit().playable;
        }

        if(GetPlayer().Count == 0){
            loseScreen.gameObject.SetActive(true);
        }
        else if(GetEnemigos().Count == 0)
        {
            winScreen.gameObject.SetActive(true);
        }
        else
        {
            currentUnit++;
            playerActing = GetCurrentUnit().playable;
            float delay = .1f;
            if(lastUnitWasPlayer && !playerActing){
                delay += .2f;
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

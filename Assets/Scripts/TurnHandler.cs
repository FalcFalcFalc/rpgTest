using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    [SerializeField] List<Unit> units;

    [SerializeField] RectTransform winScreen;
    [SerializeField] RectTransform loseScreen;

    public void RemoveUnitFromInitiative(Unit who){
        units.Remove(who);
    }

    private void Start() {
        NextTurn();
    }

    public List<Enemigo> GetEnemigos(){
        List<Enemigo> retorno = new List<Enemigo>();
        foreach (Unit item in units)
        {
            if(item.GetComponent<Enemigo>()){
                retorno.Add(item.GetComponent<Enemigo>());
            }
        }
        return retorno;
    }

    public List<Player> GetPlayer(){
        List<Player> retorno = new List<Player>();
        foreach (Unit item in units)
        {
            if(item.GetComponent<Player>()){
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
        return units[currentUnit];
    }
    
    public void NextTurn(){
        //print(units.Count);
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
            if(currentUnit >= units.Count){
              currentUnit = 0;
            }
            GetCurrentUnit().Activate();
            playerActing = GetCurrentUnit().GetComponent<Player>();
        }
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

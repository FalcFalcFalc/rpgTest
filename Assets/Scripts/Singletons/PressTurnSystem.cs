using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressTurnSystem : MonoBehaviour
{
    [SerializeField]public List<Unit> allyParty ;
    [SerializeField]public List<Unit> enemyParty;

    int turnsLeft;

    [SerializeField] List<PressTurn> turns;

    public bool alliesActive {get; private set;} = false;
    int initiativeIndex = -1;

    List<Unit> activeParty{
        get{return (alliesActive ? allyParty : enemyParty);}
    }
    List<Unit> unactiveParty{
        get{return (!alliesActive ? allyParty : enemyParty);}
    }
    List<Unit> allBatlers{
        get{
            List<Unit> retorno = new List<Unit>();
            retorno.AddRange(enemyParty);
            retorno.AddRange(allyParty);
            return retorno;
        }
    }
    public Unit currentUnit{
        get{return activeParty[initiativeIndex];}
    }

    [HideInInspector] [SerializeField] PressTurn prefab;
    public static PressTurnSystem current;
    void Awake()
    {
        current = this;
    }

    public void UnitDied(Unit dead){
        allyParty.Remove(dead);
        enemyParty.Remove(dead);
    }

    public void Next(Keywords.DamageResistances state){
        switch (state)
        {
            case Keywords.DamageResistances.Absorb:
            case Keywords.DamageResistances.Reject:
                RemoveAllTurns();
                break;
            case Keywords.DamageResistances.Void:
                RemoveTwoTurns();
                break;
            case Keywords.DamageResistances.Strong:
            case Keywords.DamageResistances.Normal:
            case Keywords.DamageResistances.Weak:
                RemoveTurn();
                break;
            case Keywords.DamageResistances.Fragile:
                RemoveHalfTurn();
                break;
        }
        StartCoroutine(WhatToDoNext());
    }

    IEnumerator WhatToDoNext(){
        yield return new WaitForSeconds(0.1f);
        if(!restartingRound)
            if(turns.Count > 0) AdvanceUnit();
            else                End();
    }

    public void Next(bool critOrMiss){
        Next(critOrMiss ? Keywords.DamageResistances.Fragile : Keywords.DamageResistances.Void);
    }
    public void Next(){
        Next(Keywords.DamageResistances.Normal);
    }

    void RemoveHalfTurn(){
        int i = 0;
        while(i < turns.Count && turns[i].triggered ) i++;
        if(i < turns.Count)
            turns[i].UseHalf();
        else
            RemoveTurn();

    }
    void RemoveTurn(){
        if(turns.Count > 0){
            turns[0].UseWhole();
            turns.RemoveAt(0);
        }
    }
    void RemoveTwoTurns(){
        for (int i = 0; i < 2; i++)
        {
            if(turns.Count > 0){
                turns[0].Miss();
                turns.RemoveAt(0);
            }
        }
    }
    void RemoveAllTurns(){
        foreach (PressTurn item in turns)
            item.Miss();
        turns = new List<PressTurn>();
    }

    void AdvanceUnit(){
        initiativeIndex++;
        if(initiativeIndex >= activeParty.Count) initiativeIndex = 0;

        currentUnit.Activate();
    }

    public void End(){
        initiativeIndex = 0;
        alliesActive = !alliesActive;
        FillTurns(true);
    }

    bool restartingRound = false;

    void FillTurns(bool a){
        restartingRound = true;
        foreach (var item in turns)
        {
            Destroy(item);
        }
        turns = new List<PressTurn>();

        StartCoroutine(_fill(a));
    }

    IEnumerator _fill(bool a){
        if (a) yield return new WaitForSeconds(.3f);

        initiativeIndex = -1;
        for (int i = 0; i < activeParty.Count; i++)
        {
            turns.Add(Instantiate(prefab,transform));
            yield return new WaitForSeconds(0.1f);
        }
        restartingRound = false;
        AdvanceUnit();
    }

    private void Start() {
        FillTurns(false);
    }
}

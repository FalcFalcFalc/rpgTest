﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemigo : Unit
{
    [SerializeField] Enum.SelectionPriority attackPriority, supportPriority;

    Unit SelectTarget(List<Unit> units, Enum.SelectionPriority priority){
        List<Unit> sortedUnits = units;
        print(priority);

        switch(priority){
            case Enum.SelectionPriority.MostBuffed:
                sortedUnits = units.OrderByDescending(input=>input.buffAgility+input.buffAttack+input.buffDefense+input.buffInteligence).ToList();
                break;
            case Enum.SelectionPriority.LeastBuffed:
                sortedUnits = units.OrderBy(input=>input.buffAgility+input.buffAttack+input.buffDefense+input.buffInteligence).ToList();
                break;
            case Enum.SelectionPriority.MostATK:
                sortedUnits = units.OrderByDescending(input=>input.getAttack).ToList();
                break;
            case Enum.SelectionPriority.MostINT:
                sortedUnits = units.OrderByDescending(input=>input.getInteligence).ToList();
                break;
            case Enum.SelectionPriority.MostDEX:
                sortedUnits = units.OrderByDescending(input=>input.getAgility).ToList();
                break;
            case Enum.SelectionPriority.MostDEF:
                sortedUnits = units.OrderByDescending(input=>input.getDefense).ToList();
                break;
            case Enum.SelectionPriority.MostHP:
                sortedUnits = units.OrderByDescending(input=>input.GetCurrentHP).ToList();
                break;
            case Enum.SelectionPriority.LeastATK:
                sortedUnits = units.OrderBy(input=>input.getAttack).ToList();
                break;
            case Enum.SelectionPriority.LeastINT:
                sortedUnits = units.OrderBy(input=>input.getInteligence).ToList();
                break;
            case Enum.SelectionPriority.LeastDEX:
                sortedUnits = units.OrderBy(input=>input.getAgility).ToList();
                break;
            case Enum.SelectionPriority.LeastDEF:
                sortedUnits = units.OrderBy(input=>input.getDefense).ToList();
                break;
            case Enum.SelectionPriority.LeastHP:
                sortedUnits = units.OrderBy(input=>input.GetCurrentHP).ToList();
                break;
            case Enum.SelectionPriority.NeedsHealing:
                sortedUnits = units.OrderBy(input=>input.GetCurrentHpPercentage).ToList();
                while(sortedUnits.Count > 0 && sortedUnits[0].GetCurrentHpPercentage > .35f)
                    sortedUnits.Remove(sortedUnits[0]);
                break;
            case Enum.SelectionPriority.Random:
                sortedUnits = units.OrderByDescending(input=>Random.Range(0f,1f)).ToList();
                break;
        }
        if (sortedUnits.Count > 0)
        {
            return sortedUnits[0];
        }
        else
        {
            return null;
        }
    }

    void OnDisable() {
        base.OnEnable();
        onActivate -= StartAI;
    }

    void OnEnable() {
        base.OnEnable();
        onActivate += StartAI;
    }

    void StartAI(){
        StartCoroutine(ArtificialInteligence());
    }

    IEnumerator ArtificialInteligence(){
        yield return new WaitForSeconds(turnHandler.duration);
        Unit mostWoundedAlly = SelectTarget(turnHandler.GetEnemigos(), supportPriority),
             attackTarget = SelectTarget(turnHandler.GetPlayer(), attackPriority);

        if(hasSupportMoves && mostWoundedAlly != null)
        {
            Support(mostWoundedAlly);
        }
        else if(turnHandler.GetPlayer().Count > 0 && hasAttackMoves && attackTarget != null)
        {
            Attack(attackTarget);
        }
        else
        {
            NextTurn();
        }
    }

}

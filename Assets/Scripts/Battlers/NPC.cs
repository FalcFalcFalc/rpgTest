using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC : Unit
{
    [SerializeField] Keywords.SelectionPriority attackPriority, supportPriority;
    [SerializeField] bool enemy;

    List<Unit> myEnemies{
        get{return (enemy ? pts.allyParty : pts.enemyParty);}
    }

    List<Unit> myAllies{
        get{return (enemy ? pts.enemyParty : pts.allyParty);}
    }

    public override bool playerParty(){
        return !enemy;
    }

    Unit SelectTarget(List<Unit> units, Keywords.SelectionPriority priority){
        List<Unit> sortedUnits = units;

        switch(priority){
            case Keywords.SelectionPriority.MostBuffed:
                sortedUnits = units.OrderByDescending(input=>input.buffDEX+input.buffDMG+input.buffDEF).ToList();
                break;
            case Keywords.SelectionPriority.LeastBuffed:
                sortedUnits = units.OrderBy(input=>input.buffDEX+input.buffDMG+input.buffDEF).ToList();
                break;
            case Keywords.SelectionPriority.MostATK:
                sortedUnits = units.OrderByDescending(input=>input.getAttack).ToList();
                break;
            case Keywords.SelectionPriority.MostINT:
                sortedUnits = units.OrderByDescending(input=>input.getInteligence).ToList();
                break;
            case Keywords.SelectionPriority.MostDEX:
                sortedUnits = units.OrderByDescending(input=>input.getAgility).ToList();
                break;
            case Keywords.SelectionPriority.MostHP:
                sortedUnits = units.OrderByDescending(input=>input.getCurrentHP).ToList();
                break;
            case Keywords.SelectionPriority.LeastATK:
                sortedUnits = units.OrderBy(input=>input.getAttack).ToList();
                break;
            case Keywords.SelectionPriority.LeastINT:
                sortedUnits = units.OrderBy(input=>input.getInteligence).ToList();
                break;
            case Keywords.SelectionPriority.LeastDEX:
                sortedUnits = units.OrderBy(input=>input.getAgility).ToList();
                break;
            case Keywords.SelectionPriority.LeastHP:
                sortedUnits = units.OrderBy(input=>input.getCurrentHP).ToList();
                break;
            case Keywords.SelectionPriority.NeedsHealing:
                sortedUnits = units.OrderBy(input=>input.getCurrentHpPercentage).ToList();
                while(sortedUnits.Count > 0 && sortedUnits[0].getCurrentHpPercentage > .35f)
                    sortedUnits.Remove(sortedUnits[0]);
                break;
            case Keywords.SelectionPriority.Random:
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

    new void OnDisable() {
        base.OnEnable();
        onActivate -= StartAI;
    }

    new void OnEnable() {
        base.OnEnable();
        onActivate += StartAI;
    }

    void StartAI(){
        StartCoroutine(ArtificialInteligence());
    }

    IEnumerator ArtificialInteligence(){
        yield return new WaitForSeconds(.35f);
        Unit mostWoundedAlly = SelectTarget(myAllies, supportPriority),
             attackTarget = SelectTarget(myEnemies, attackPriority);

        if(hasSupportMoves && mostWoundedAlly != null)
        {
            Support(mostWoundedAlly);
        }
        else if(myEnemies.Count > 0 && hasAttackMoves && attackTarget != null)
        {
            Attack(attackTarget);
        }
        Deactivate();
    }

}

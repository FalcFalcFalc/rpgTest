using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public void Start() {
        SetUpHPBar();
    }
    public bool activeUnit = false;

    [Header("Stats")]
    [SerializeField] protected int baseAttack;
    [SerializeField] protected int baseInteligence;
    [SerializeField] protected int baseDefense;
    [SerializeField] protected int baseAgility;

    public int GetAttack(){
        return baseAttack;
    }
    public int GetInteligence(){
        return baseInteligence;
    }
    public int GetDefense(){
        return baseDefense;
    }
    public int GetAgility(){
        return baseAgility;
    }

    [SerializeField] protected int maxHp;
    protected int currentHp;  
    public int GetCurrentHP(){
        return currentHp;
    }
    public int GetMaxHP(){
        return maxHp;
    }
    public float GetCurrentHpPercentage(){
        return currentHp / (float) maxHp;
    }

    [Header("Abilities")]
    public List<Abilities> moves;

    [Header("Dependancies")]
    [SerializeField] Slider hpBar;
    [SerializeField] protected TurnHandler turnHandler;
    [SerializeField] protected BattleCursor cursor;

    int totalDamage = 0;

    public void Activate(){
        activeUnit = true;
        OnActivate();
    }

    protected abstract void OnActivate();
    protected abstract void OnTurnEnd();

    protected bool hasAttackMoves(){
        int i = 0;
        bool retorno = false;
        while (i < moves.Count && !retorno){
            retorno = moves[i].GetType() == typeof(BasicAttacks);

            i++;
        }
        return retorno;
    }

    protected bool hasHealingMoves(){
        int i = 0;
        bool retorno = false;
        while (i < moves.Count && !retorno){
            retorno = moves[i].GetType() == typeof(Healing);

            i++;
        }
        return retorno;
    }

    public void Attack(Unit target){
        moves[0].Trigger(this,new[] {target});
        //target.ReceiveDamage(baseAttack + 1);
        NextTurn();
    }

    public void Heal(Unit target){
        moves[1].Trigger(this,new[] {target});
        NextTurn();
    }

    protected void NextTurn(){
        activeUnit = false;
        OnTurnEnd();
        StartCoroutine(DelayBetweenTurns());
    }

    IEnumerator DelayBetweenTurns(){
        yield return new WaitForSeconds(.20f);
        turnHandler.NextTurn();
    }

    public void ReceiveDamage(int value){
        if(value > baseDefense){
            totalDamage = value - baseDefense;
            LeanTween.value(gameObject, currentHp, currentHp - totalDamage, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
            currentHp -= totalDamage;
        }
    }

    public void ReceiveHealing(int value){
        LeanTween.value(gameObject, currentHp, currentHp + value, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
        currentHp += value;
        if(currentHp > maxHp) currentHp = maxHp;
    }

    void AnimateHPBar(float value){
        hpBar.value = value;
        if(value <= 0){
            LeanTween.cancel(gameObject); // hacemos que la animacion pare y esta funcion no vuelva a ser llamada
            turnHandler.RemoveUnitFromInitiative(this);
            if(GetComponent<Enemigo>()){
                StartCoroutine(AnimateDeath());
            }
        }
    }

    IEnumerator AnimateDeath(){
        if(cursor.selected == this && turnHandler.GetEnemigos().Count > 0) cursor.SelectNewUnit(turnHandler.GetEnemigos()[0]);
        for (int i = 0; i < 3; i++)
        {        
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.1f);    
        }
        Destroy(gameObject);
    }

    void SetUpHPBar(){
        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
    }
}

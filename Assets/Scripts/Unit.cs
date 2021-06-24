using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    Vector3 ogPos;
    public Vector3 GetOriginalPosition(){
        return ogPos;
    }

    void Awake() {
        ogPos = transform.position;
    }

    public void Start() {

        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
        
        turnHandler = TurnHandler.current;
        cursor = BattleCursor.current;
    }

    public bool activeUnit = false;

    [Header("Stats")]
    [SerializeField] protected int baseAttack;
    [SerializeField] protected int baseInteligence;
    [SerializeField] protected int baseDefense;
    [SerializeField] protected int baseAgility;

    public int GetAttack{
        get{return baseAttack;}
    }
    public int GetInteligence{
        get{return baseInteligence;}
    }
    public int GetDefense{
        get{return baseDefense;}
    }
    public int GetAgility{
        get{return baseAgility;}
    }

    [SerializeField] protected int maxHp;
    protected int currentHp;  
    public int GetCurrentHP{
        get{return currentHp;}
    }
    public int GetMaxHP{
        get{return maxHp;}
    }
    public float GetCurrentHpPercentage{
        get{return currentHp / (float) maxHp;}
    }

    public int ReceiveDamage(int value){
        int totalDamage = 0;
    
        if(value > baseDefense){
            totalDamage = value - baseDefense;
            LeanTween.value(gameObject, currentHp, currentHp - totalDamage, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
            currentHp -= totalDamage;
        }

        if(currentHp <= 0){
            turnHandler.RemoveUnitFromInitiative(this);
            StartCoroutine(AnimateDeath());
        }
        else
        {
            Survived();
        }
        return totalDamage;
    }

    public void ReceiveHealing(int value){
        currentHp += value;
        LeanTween.value(gameObject, currentHp, currentHp + value, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
        if(currentHp > maxHp) currentHp = maxHp;
    }

    public bool doesDodge(Unit attacker){
        bool retorno = UnityEngine.Random.Range(0,100) <= GetAgility;
        if(retorno){
            Evaded(attacker);
            LeanTween.move(gameObject, transform.position + Vector3.left * .25f, .75f).setEasePunch();
        }

        return retorno; 
    }

    public bool doesCrit(int value){
        bool retorno = UnityEngine.Random.Range(0,100) <= value;
        if(retorno){
            print("Animacion de esquivar");
            LeanTween.move(gameObject, transform.position + Vector3.left * .25f, .75f).setEasePunch();
        }

        return retorno; 
    }

    void AnimateHPBar(float value){
        hpBar.value = value;
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

    [Header("Abilities")]
    public List<Ability> attackMoves;
    public List<Ability> healMoves;
    public List<Pasive> perks;

    void OnEnable() {
        foreach (Pasive item in perks)
        {
            item.Enable(this);
        }
    }

    void OnDisable() {
        foreach (Pasive item in perks)
        {
            item.Disable(this);
        }
    }

    public event Action<Unit> onEvade;
    public event Action onSurvive;
    public event Action onHealed;
    public event Action onActivate;
    public event Action onDeactivate;

    protected void Evaded(Unit target){
        if(onEvade != null) onEvade(target);
    }

    protected void Survived(){
        if(onSurvive != null) onSurvive();
    }

    protected void Healed(){
        if(onHealed != null) onHealed();
    }

    public bool hasAttackMoves{
        get {return (attackMoves.Count > 0);}
    }

    public bool hasHealingMoves{
        get {return (healMoves.Count > 0);}
    }

    public void Attack(Unit target){
        attackMoves[0].Trigger(this,target);
        NextTurn();
    }

    public void Attack(Unit target,bool skip){
        attackMoves[0].Trigger(this,target);
        if(skip) NextTurn();
    }

    public void Heal(Unit target){
        healMoves[0].Trigger(this,target);
        NextTurn();
    }

    [Header("Dependancies")]
    [SerializeField] Slider hpBar;
    protected TurnHandler turnHandler;
    protected BattleCursor cursor;

    public void Activate(){
        activeUnit = true;
        StepUp();
        if(onActivate != null) onActivate();
    }
    protected virtual void Deactivate(){
        StepDown();
        if(onDeactivate != null) onDeactivate();
    }

    protected void NextTurn(){
        activeUnit = false;
        Deactivate();
        turnHandler.NextTurn();
    }

    public bool playable{
        get{ return GetComponent<Player>();}
    }

    private void OnMouseOver() {
        if(turnHandler.isPlayerActing() && cursor.selected != this) cursor.SelectNewUnit(this);
    }

    int movement;

    protected void StepUp(){
        LeanTween.cancel(movement);
        movement = LeanTween.move(gameObject,ogPos + (playable ? -1 : 1) * 3 * Vector3.right,.75f).setEaseOutBack().id;
    }

    protected void StepDown(){
        LeanTween.cancel(movement);
        movement = LeanTween.move(gameObject,ogPos,.75f).setEaseOutBack().id;
    }
}

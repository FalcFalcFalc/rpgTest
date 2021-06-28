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

        print("hi");

        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
        
        turnHandler = TurnHandler.current;
        cursor = BattleCursor.current;
    }

    public bool activeUnit = false;

    

    [Header("Stats")]
    [SerializeField] int baseAttack;
    [SerializeField] int baseInteligence;
    [SerializeField] int baseDefense;
    [SerializeField] int baseAgility;

    float buffStrength = Mathf.Pow(2,.25f);

    float buffAttack = 1;
    float buffInteligence = 1;
    float buffDefense = 1;
    float buffAgility = 1;

    public void Buff(Enum.Stat what){
        switch (what)
        {
            case Enum.Stat.ATK:
                buffAttack *= buffStrength;
                break;
            case Enum.Stat.INT:
                buffInteligence *= buffStrength;
                break;
            case Enum.Stat.DEF:
                buffDefense *= buffStrength;
                break;
            case Enum.Stat.DEX:
                buffAgility *= buffStrength;
                break;
        }
        BattleLog.current.AddLog(name + " got their " + what.ToString() + " increased.");
    }

    public void Debuff(Enum.Stat what){
        switch (what)
        {
            case Enum.Stat.ATK:
                buffAttack /= buffStrength;
                break;
            case Enum.Stat.INT:
                buffInteligence /= buffStrength;
                break;
            case Enum.Stat.DEF:
                buffDefense /= buffStrength;
                break;
            case Enum.Stat.DEX:
                buffAgility /= buffStrength;
                break;
        }
        
        BattleLog.current.AddLog(name + " got their " + what.ToString() + " lowered.");
    }

    public int getAttack{
        get{return Mathf.RoundToInt(baseAttack * buffAttack);}
    }
    public int getInteligence{
        get{return Mathf.RoundToInt(baseInteligence * buffInteligence);}
    }
    public int getDefense{
        get{return Mathf.RoundToInt(baseDefense * buffDefense);}
    }
    public int getAgility{
        get{return Mathf.RoundToInt(baseAgility * buffAgility);}
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
            BattleLog.current.AddLog(name + " died.");
            StartCoroutine(AnimateDeath());
        }
        else
        {
            Survived(totalDamage);
        }
        return totalDamage;
    }

    public void ReceiveHealing(int value){
        currentHp += value;
        LeanTween.value(gameObject, currentHp, currentHp + value, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
        if(currentHp > maxHp) currentHp = maxHp;
    }

    public bool doesDodge(int accuracy, Unit attacker){
        bool retorno = UnityEngine.Random.Range(0,100) <= getAgility - accuracy;
        if(retorno){
            BattleLog.current.AddLog(name + " dodged " + attacker.name + "'s attack.");

            Evaded(attacker);
            LeanTween.move(gameObject, transform.position + Vector3.left * .25f, .75f).setEasePunch();
        }
        return retorno; 
    }

    public bool doesCrit(int value){
        bool retorno = UnityEngine.Random.Range(0,100) <= value;
        if(retorno){
            BattleLog.current.AddLog(name + " scored a critical hit.");

            CrititicalHit();
            LeanTween.move(gameObject, transform.position + Vector3.left * .25f, .75f).setEasePunch();
        }
        return retorno; 
    }

    void AnimateHPBar(float value){
        hpBar.value = value;
    }

    IEnumerator AnimateDeath(){
        //if(cursor.selected == this && turnHandler.GetEnemigos().Count > 0) cursor.SelectNewUnit(turnHandler.GetEnemigos()[0]);
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
    public List<Ability> supportMoves;
    public List<Pasive> perks;

    protected void OnEnable() {
        foreach (Pasive item in perks)
        {
            item.Enable(this);
        }
    }

    protected void OnDisable() {
        foreach (Pasive item in perks)
        {
            item.Disable(this);
        }
    }

    public event Action<Unit> onEvade;
    public event Action<int> onDamage;
    public event Action<int> onSurvive;
    public event Action onHealed;
    public event Action onActivate;
    public event Action onDeactivate;
    public event Action onCrit;

    protected void Evaded(Unit target){
        if(onEvade != null) onEvade(target);
    }

    public void DealtDamage(int dmg){
        if(onDamage != null) onDamage(dmg);
    }

    protected void Survived(int value){
        if(onSurvive != null) onSurvive(value);
    }

    protected void Healed(){
        if(onHealed != null) onHealed();
    }

    protected void CrititicalHit(){
        if(onCrit != null) onCrit();
    }

    public bool hasAttackMoves{
        get {return (attackMoves.Count > 0);}
    }

    public bool hasSupportMoves{
        get {return (supportMoves.Count > 0);}
    }

    public void Attack(Unit target){
        attackMoves[0].Trigger(this,target);

        NextTurn();
    }

    public void Attack(Unit target,bool skip){
        attackMoves[0].Trigger(this,target);


        if(skip) NextTurn();
    }

    public void Support(Unit target){
        supportMoves[0].Trigger(this,target);

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
        movement = LeanTween.move(gameObject,ogPos + (playable ? -1 : 1) * 2 * Vector3.right,turnHandler.duration).setEaseOutBack().id;
    }

    protected void StepDown(){
        LeanTween.cancel(movement);
        movement = LeanTween.move(gameObject,ogPos,turnHandler.duration).setEaseOutBack().id;
    }
}

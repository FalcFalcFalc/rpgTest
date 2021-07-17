using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] UnitStatsAndAbilities statsAndAbilities;

    [Header("Stats")]
    int baseAttack;
    int baseInteligence;
    int baseAgility;

    public int getAttack{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseAttack + buffAttack),1,20);}
    }
    public int getInteligence{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseInteligence + buffInteligence),1,20);}
    }
    public int getAgility{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseAgility + buffAgility),1,20);}
    }

    public int buffAttack { get; private set; } = 0;
    public int buffInteligence { get; private set; } = 0;
    public int buffAgility { get; private set; } = 0;

    int maxBuffAmount = 4;

    public bool canBuff(Enum.Stat what){
        int which = 0;
        switch (what)
        {
            case Enum.Stat.ATK:
                which = buffAttack;
            break;
            
            case Enum.Stat.INT:
                which = buffInteligence;
            break;
            
            case Enum.Stat.DEX:
                which = buffAgility;
            break;
        }
        return Mathf.Abs(which) < maxBuffAmount;
    } 
    public void Buff(Enum.Stat what, int strengthOfBuffs){
        switch (what)
        {
            case Enum.Stat.ATK:
                buffAttack += strengthOfBuffs;
                buffAttack = Mathf.Clamp(buffAttack, -maxBuffAmount, maxBuffAmount);
            break;
            
            case Enum.Stat.INT:
                buffInteligence += strengthOfBuffs;
                buffInteligence = Mathf.Clamp(buffInteligence, -maxBuffAmount, maxBuffAmount);
            break;
            
            case Enum.Stat.DEX:
                buffAgility += strengthOfBuffs;
                buffAgility = Mathf.Clamp(buffAgility, -maxBuffAmount, maxBuffAmount);
            break;
        }

    }
    public void Debuff(Enum.Stat what, int strengthOfBuffs){
        switch (what)
        {
            case Enum.Stat.ATK:
                buffAttack -= strengthOfBuffs;
                buffAttack = Mathf.Clamp(buffAttack, -maxBuffAmount, maxBuffAmount);
                break;
            case Enum.Stat.INT:
                buffInteligence -= strengthOfBuffs;
                buffInteligence = Mathf.Clamp(buffInteligence, -maxBuffAmount, maxBuffAmount);
                break;
            case Enum.Stat.DEX:
                buffAgility -= strengthOfBuffs;
                buffAgility = Mathf.Clamp(buffAgility, -maxBuffAmount, maxBuffAmount);
                break;
        }
    }

    protected int maxHp;
    protected int currentHp;  
    public int getCurrentHP{
        get{return currentHp;}
    }
    public int getMaxHP{
        get{return maxHp;}
    }
    public float getCurrentHpPercentage{
        get{return currentHp / (float) maxHp;}
    }

    void Awake() {
        ogPos = transform.position;
    }
    public void Start() {
//        hpBar = transform.Find("Slider").GetComponent<Slider>();

        CopyStats();
        CopyAbilities();

        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
        
        turnHandler = TurnHandler.current;
    }
    void CopyStats(){
        baseAttack = statsAndAbilities.might;
        baseAgility = statsAndAbilities.speed;
        baseInteligence = statsAndAbilities.mind;

        maxHp = statsAndAbilities.maxHp;
//        print(maxHp + " " + name);
        statusEffects = new List<StatusEffectCountdown>();
    }
    void CopyAbilities(){
        attackMoves = statsAndAbilities.atk;
        supportMoves = statsAndAbilities.sup;
        perks = statsAndAbilities.perk;
        OnEnable();
    }

    //CALCULATIONS

    public float damageMultiplier = 1;
    public void ModifyDamageMultiplier(float value, bool restore){
        print(value + " " + restore);
        if(!restore){
            damageMultiplier *= value;
        }
        else
        {
            damageMultiplier /= value;
        }
    }

    public int ReceiveDamage(int value){
        LeanTween.value(gameObject, currentHp, currentHp -value, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
        value = Mathf.RoundToInt(value * damageMultiplier);

        currentHp -= value;

        if(currentHp <= 0){
            turnHandler.RemoveUnitFromInitiative(this);
            BattleLog.current.AddLog(name + " died.");
            StartCoroutine(AnimateDeath());
        }
        else
        {
            Survived(value);
        }
        return Mathf.RoundToInt(value);
    }
    public void ReceiveHealing(int value){
        if(value > 0){
            currentHp += value;
            LeanTween.value(gameObject, currentHp, currentHp + value, .19f).setEaseOutBack().setOnUpdate(AnimateHPBar);
            if(currentHp > maxHp) currentHp = maxHp;
            Healed();
        }
    }
    public bool doesDodge(int accuracy, Unit attacker){
        bool retorno = UnityEngine.Random.Range(0,20) <= getAgility/2 - accuracy;
        if(retorno){
            BattleLog.current.AddLog(name + " dodged " + attacker.name + "'s attack.");

            Evaded(attacker);
            LeanTween.move(gameObject, transform.position + Vector3.left * .25f, .75f).setEasePunch();
        }
        return retorno; 
    }
    public bool doesCrit(int value){
        bool retorno = UnityEngine.Random.Range(0,20) <= value;
        if(retorno){
            BattleLog.current.AddLog(name + " scored a critical hit.");

            CrititicalHit();
            LeanTween.move(gameObject, transform.position + Vector3.left * .25f, .75f).setEasePunch();
        }
        return retorno; 
    }

    //EVENTS
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

    bool firstEnable = true;
    protected void OnEnable() {
        if(firstEnable) firstEnable = false;
        else
            foreach (Pasive item in perks)
                item.Enable(this);
        
    }
    protected void OnDisable() {
        if(perks.Count > 0)
            foreach (Pasive item in perks)
                item.Disable(this);
    }

    //ACTIONS
    [Header("Abilities")]
    List<Ability> attackMoves;
    List<Support> supportMoves;
    List<Pasive> perks;

    public List<Ability> getAttackMoves(){
        return attackMoves;
    }
    public List<Support> getSupportMoves(){
        return supportMoves;
    }
    public List<Pasive> getPerks(){
        return perks;
    }

    public bool hasAttackMoves{
        get {return (attackMoves.Count > 0);}
    }
    public bool hasSupportMoves{
        get {return (supportMoves.Count > 0);}
    }
    public bool hasSecondaryAttackMoves{
        get {return (attackMoves.Count > 1);}
    }
    public bool hasSecondarySupportMoves{
        get {return (supportMoves.Count > 1);}
    }

    public void Attack(Unit target){
        attackMoves[0].Trigger(this,target);

        NextTurn();
    }
    public void AttackSecondary(Unit target){
        attackMoves[1].Trigger(this,target);

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
    public void SupportSecondary(Unit target){
        supportMoves[1].Trigger(this,target);

        NextTurn();
    }

    [Serializable]
    public class StatusEffectCountdown{
        public StatusEffect se { get; private set;}
        public int countdown;
        public StatusEffectCountdown(StatusEffect value){
            se = value;
            countdown = se.duration;
        }
        public void Enable(Unit self){
            se.Enable(self);
        }
        public void Disable(Unit self){
            se.Disable(self);
        }
        public bool Countdown(){
            return --countdown == 0;
        }
    }

    //STATUS EFFECTS
    [SerializeField] List<StatusEffectCountdown> statusEffects;
    public void AddStatusEffect(StatusEffect newStatus){
        if(!AlreadyAffected(newStatus)){
            statusEffects.Add(new StatusEffectCountdown(newStatus));
            newStatus.Enable(this);
        }
    }
    public bool AlreadyAffected(StatusEffect which){
        bool retorno = false;
        if(statusEffects.Count > 0){
            int i = 0;
            while(i < statusEffects.Count && !retorno){
                retorno = which == statusEffects[i].se;
            }
        }
        return retorno;
    }
    public void TickDownEffects(){
        Queue<StatusEffectCountdown> finishedStatusses = new Queue<StatusEffectCountdown>();
        foreach (StatusEffectCountdown item in statusEffects)
        {
            if(item.Countdown())
                finishedStatusses.Enqueue(item);
        }
        foreach (StatusEffectCountdown item in finishedStatusses)
        {
            statusEffects.Remove(item);
            item.Disable(this); //FIJARSE ACA
        }
    }
    public void RemoveStatusEffect(StatusEffect remove){
        int i = 0;
        StatusEffectCountdown eliminar = null;
        while(i < statusEffects.Count && eliminar == null)
            if(remove == statusEffects[i].se)
                eliminar = statusEffects[i];
        statusEffects.Remove(eliminar);
    }

    [Header("Dependancies")]
    [SerializeField] Slider hpBar;
    protected TurnHandler turnHandler;

    Vector3 ogPos;
    public Vector3 GetOriginalPosition(){
        return ogPos;
    }

    //TURN MANAGEMENT
    public void Activate(){
        StepUp();
        if(onActivate != null) onActivate();
    }
    public virtual void Deactivate(){
        StepDown();
        if(onDeactivate != null) onDeactivate();
    }
    protected void NextTurn(){
        turnHandler.NextTurn();
    }


    //INFO ABOUT PLAYABILITY OF CLASS
    public bool playable{
        get{ return GetComponent<Player>();}
    }

    public virtual bool playerParty(){
        return playable;
    }

    //ANIMATION
    void AnimateHPBar(float value){
        hpBar.value = value;
    }
    IEnumerator AnimateDeath(){
        for (int i = 0; i < 3; i++)
        {        
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.1f);    
        }
        Destroy(gameObject);
    }

    int steppingAnimationID;
    protected void StepUp(){
        LeanTween.cancel(steppingAnimationID);
        steppingAnimationID = LeanTween.move(gameObject,ogPos + (playerParty() ? -1 : 1) * 2 * Vector3.right,turnHandler.duration).setEaseOutBack().id;
    }
    protected void StepDown(){
        LeanTween.cancel(steppingAnimationID);
        steppingAnimationID = LeanTween.move(gameObject,ogPos,turnHandler.duration).setEaseOutBack().id;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] UnitStatsAndAbilities statsAndAbilities;

    [Header("Stats")]
    int baseAttack,
        baseInteligence,
        baseAgility,
        baseDefense,
        baseLuck;

    public int getAttack{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseAttack * (Mathf.Pow(buffSTR,buffDMG))),1,20);}
    }
    public int getInteligence{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseInteligence * (Mathf.Pow(buffSTR,buffDMG))),1,20);}
    }
    public int getAgility{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseAgility * (Mathf.Pow(buffSTR,buffDEX))),1,20);}
    }
    public int getDefense{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseDefense * (Mathf.Pow(buffSTR,buffDEF))),1,20);}
    }
    public int getLuck{
        get{return Mathf.Clamp(Mathf.RoundToInt(baseLuck),1,20);}
    }

    public int buffDMG { get; private set; } = 0;
    public int buffDEF { get; private set; } = 0;
    public int buffDEX { get; private set; } = 0;


    public bool canBuff(Keywords.Buff what){
        int which = 0;
        switch (what)
        {
            case Keywords.Buff.DMG:
                which = buffDMG;
            break;
            
            case Keywords.Buff.DEF:
                which = buffDEF;
            break;
            
            case Keywords.Buff.DEX:
                which = buffDEX;
            break;
        }
        return Mathf.Abs(which) < maxBuffAmount;
    } 

    [SerializeField]int maxBuffAmount = 4;
    [SerializeField]Vector2 buffStrength = new Vector3(25,4);
    float buffSTR {get{return ((buffStrength.y!=0)?Mathf.Pow(1 + buffStrength.x/100,1/buffStrength.y):0);}}
    public void Buff(Keywords.Buff what, int strengthOfBuffs){
        if(canBuff(what)){
            switch (what)
            {
                case Keywords.Buff.DMG:
                    buffDMG += strengthOfBuffs;
                    buffDMG = Mathf.Clamp(buffDMG, -maxBuffAmount, maxBuffAmount);
                break;
                
                case Keywords.Buff.DEF:
                    buffDEF += strengthOfBuffs;
                    buffDEF = Mathf.Clamp(buffDEF, -maxBuffAmount, maxBuffAmount);
                break;
                
                case Keywords.Buff.DEX:
                    buffDEX += strengthOfBuffs;
                    buffDEX = Mathf.Clamp(buffDEX, -maxBuffAmount, maxBuffAmount);
                break;
            }
            if(!canBuff(what))
                MiniTextGenerator.current.CreateText(what.ToString() + " MAXed!",transform,(playable?Keywords.Elements.Blessing:Keywords.Elements.Curse));
            else
                MiniTextGenerator.current.CreateText(what.ToString() + "+",transform,(playable?Keywords.Elements.Blessing:Keywords.Elements.Curse));
        }
        else
        {
            MiniTextGenerator.current.CreateText("ALRD MAXd",transform,Keywords.Elements.Curse);
        }

    }
    public void Debuff(Keywords.Buff what, int strengthOfBuffs){
        if(canBuff(what))
        {
            switch (what)
            {
                case Keywords.Buff.DMG:
                    buffDMG -= strengthOfBuffs;
                    buffDMG = Mathf.Clamp(buffDMG, -maxBuffAmount, maxBuffAmount);
                    break;
                case Keywords.Buff.DEF:
                    buffDEF -= strengthOfBuffs;
                    buffDEF = Mathf.Clamp(buffDEF, -maxBuffAmount, maxBuffAmount);
                    break;
                case Keywords.Buff.DEX:
                    buffDEX -= strengthOfBuffs;
                    buffDEX = Mathf.Clamp(buffDEX, -maxBuffAmount, maxBuffAmount);
                    break;
            }
            if(!canBuff(what))
                MiniTextGenerator.current.CreateText(what.ToString() + " MINed!",transform,(!playable?Keywords.Elements.Blessing:Keywords.Elements.Curse));
            else
                MiniTextGenerator.current.CreateText(what.ToString() + "-!",transform,(!playable?Keywords.Elements.Blessing:Keywords.Elements.Curse));
        
        }
        else
        {
            MiniTextGenerator.current.CreateText("ALRD MAXd",transform,Keywords.Elements.Curse);
        }
    }

    [SerializeField]protected int maxHp;
    [SerializeField]protected int currentHp;  
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
        //hpBar = transform.Find("Slider").GetComponent<Slider>();

        CopyStats();
        CopyAbilities();

        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
        
        //turnHandler = TurnHandler.current;
        pts = PressTurnSystem.current;
    }
    void CopyStats(){
        baseAttack = statsAndAbilities.might;
        baseInteligence = statsAndAbilities.mind;
        baseDefense = statsAndAbilities.will;
        baseAgility = statsAndAbilities.speed;
        baseLuck = statsAndAbilities.luck;

        maxHp = statsAndAbilities.maxHp;
        //print(maxHp + " " + name);
        statusEffects = new List<StatusEffectCountdown>();
    }
    void CopyAbilities(){
        attackMoves = statsAndAbilities.atk;
        supportMoves = statsAndAbilities.sup;
        perks = new List<Pasive>();
        foreach (Pasive item in statsAndAbilities.perk)
            perks.Add(item);
        OnEnable();
    }

    //CALCULATIONS

    public float damageMultiplier = 1;
    public void ModifyDamageMultiplier(float value, bool restore){
        if(!restore){
            damageMultiplier *= value;
        }
        else
        {
            damageMultiplier /= value;
        }
    }

    public int ReceiveDamage(Unit attacker, int value, Keywords.Elements type, bool doesntAffectTurn, bool dontReflect){
        value = Mathf.RoundToInt(value * (1-getDefense/100));

        Keywords.DamageResistances whatDo = statsAndAbilities.resistances[type];
        switch (whatDo)
        {
            default:
            
            case Keywords.DamageResistances.Strong:
                value /= 2;
                GotHit(value,type);
                    break;
            case Keywords.DamageResistances.Weak:
            case Keywords.DamageResistances.Fragile:
                value *= 2;
                CameraHandler.ScreenShake(2,.25f);
                GotHit(value,type);
                    break;
            case Keywords.DamageResistances.Normal:
                GotHit(value,type);
                    break;
            case Keywords.DamageResistances.Void:
                MiniTextGenerator.current.CreateText("VOID",transform);
                    break;
            case Keywords.DamageResistances.Reject:
                if (!dontReflect) DealDamage(attacker,value,type,true,true);
                else MiniTextGenerator.current.CreateText("VOID",transform);
                    break;
            case Keywords.DamageResistances.Absorb:
                ReceiveHealing(value);
                    break;            
        }

        if(!doesntAffectTurn){
            pts.Next(statsAndAbilities.resistances[type]);
        }

        return Mathf.RoundToInt(value);
    }

    void GotHit(int value, Keywords.Elements type){
        MiniTextGenerator.current.CreateText(value.ToString(),transform,type);
        LeanTween.value(gameObject, currentHp, currentHp -value, 2f).setEaseOutExpo().setOnUpdate(AnimateHPBar);
        currentHp -= value;
        if(currentHp <= 0){
            //turnHandler.RemoveUnitFromInitiative(this);
            pts.UnitDied(this);
            BattleLog.current.AddLog(name + " died.");
            StartCoroutine(AnimateDeath());
        }
        else
        {
            Survived(value,type);
        }
    }

    public void ReceiveHealing(int value){
        if(value > 0){
            MiniTextGenerator.current.CreateText(value.ToString(),transform,Keywords.Elements.Blessing);
            LeanTween.value(gameObject, currentHp, currentHp + value, 2f).setEaseOutExpo().setOnUpdate(AnimateHPBar);
            currentHp += value;
            if(currentHp > maxHp) currentHp = maxHp;
            Healed();
        }
    }
    public bool doesDodge(int accuracy, Unit attacker){
        bool retorno = UnityEngine.Random.Range(0,100) + accuracy * 5 < getAgility;
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

    public int DealDamage(Unit target, float dmg, Keywords.Elements type){
        return DealDamage(target,dmg,type,false);
    }

    public int DealDamage(Unit target, float dmg, Keywords.Elements type, bool doesntAffectTurn){
        return DealDamage(target,dmg,type,false,false);
    }

    public int DealDamage(Unit target, float dmg, Keywords.Elements type, bool doesntAffectTurn, bool dontReflect){
        int retorno = target.ReceiveDamage(this, Mathf.RoundToInt(dmg),type, doesntAffectTurn, dontReflect);
        DealtDamage(retorno,type);
        return retorno;
    }

    //RESISTANCES
    public SerializableDictionary<Keywords.Elements,Keywords.DamageResistances> resistances {get{return statsAndAbilities.resistances;}}

    //EVENTS
    public event Action<Unit> onEvade;
    public event Action<int,Keywords.Elements> onDamage;
    public event Action<int,Keywords.Elements> onSurvive;
    public event Action onHealed;
    public event Action onActivate;
    public event Action onDeactivate;
    public event Action onCrit;

    protected void Evaded(Unit target){

        if(onEvade != null) onEvade(target);
    }
    public void DealtDamage(int dmg, Keywords.Elements type){
        if(onDamage != null) onDamage(dmg,type);
    }
    protected void Survived(int value, Keywords.Elements type){
        if(onSurvive != null) onSurvive(value,type);
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
        attackMoves[0].Trigger(this,target, false);
    }
    public void Attack(Unit target,bool skip){
        attackMoves[0].Trigger(this,target, skip);
    }
    public void Support(Unit target){
        supportMoves[0].Trigger(this,target,true);

        pts.Next();
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
        List<StatusEffectCountdown> finishedStatusses = new List<StatusEffectCountdown>();
        foreach (StatusEffectCountdown item in statusEffects)
        {
            if(item.Countdown())
                finishedStatusses.Add(item);
        }
        foreach (StatusEffectCountdown item in finishedStatusses)
        {
            statusEffects.Remove(item);
            if(!item.se.autoStops) item.Disable(this); //FIJARSE ACA
        }
    }
    public void RemoveStatusEffect(StatusEffect remove){
        int i = 0;
        StatusEffectCountdown eliminar = null;
        while(i < statusEffects.Count && eliminar == null){
            if(remove == statusEffects[i].se)
                eliminar = statusEffects[i];
            i++;
        }
        if(eliminar != null){
            eliminar.Disable(this);
            statusEffects.Remove(eliminar);
        }
        else if(perks.Contains(remove))
        {
            remove.Disable(this);
            perks.Remove(remove);
        }
    }

    [Header("Dependancies")]
    [SerializeField] Slider hpBar;
    //protected TurnHandler turnHandler;
    protected PressTurnSystem pts;

    Vector3 ogPos;
    public Vector3 GetOriginalPosition(){
        return ogPos;
    }

    [HideInInspector]public bool isActive = false; 
    //TURN MANAGEMENT
    public void Activate(){
        isActive = true;
        StepUp();
        if(onActivate != null) onActivate();
    }
    public virtual void Deactivate(){
        isActive = false;
        StepDown();
        if(onDeactivate != null) onDeactivate();
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
        steppingAnimationID = LeanTween.move(gameObject,ogPos + (playerParty() ? -1 : 1) * 2 * Vector3.right,.5f).setEaseOutBack().id;
//        print("me movi " + steppingAnimationID);

    }
    protected void StepDown(){
        LeanTween.cancel(steppingAnimationID);
        steppingAnimationID = LeanTween.move(gameObject,ogPos,.5f).setEaseOutBack().id;
    }
}

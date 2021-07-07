using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleLog : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static BattleLog current;

    [SerializeField] int length;
    [SerializeField] RectTransform bg;
    [SerializeField] RectTransform ogPos;
    Queue<TextMeshProUGUI> logs;
    Queue<TextMeshProUGUI> logsAwaiting;
    [SerializeField] TextMeshProUGUI prefab;

    void LogMovement(float value){
        bg.localPosition = Vector3.right * value;
    }

    public void OnPointerEnter(PointerEventData context) {
        LeanTween.value(bg.gameObject,bg.localPosition.x,-300,.75f).setEaseOutBack().setOnUpdate(LogMovement);
    }

    public void OnPointerExit(PointerEventData context) {
        LeanTween.value(bg.gameObject,bg.localPosition.x,-570,.75f).setEaseOutBack().setOnUpdate(LogMovement);
    }

    void Awake() {
        current = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        logsAwaiting = new Queue<TextMeshProUGUI>();
        logs = new Queue<TextMeshProUGUI>();
        AddLog("Battle Started.");
    }

    bool duringAnimation = false;


    public void AddLog(string text){
        
        TextMeshProUGUI newLog = Instantiate(prefab,bg);
        newLog.SetText(text);
        logsAwaiting.Enqueue(newLog);
        newLog.rectTransform.position = ogPos.position;
        newLog.rectTransform.rotation = Quaternion.identity;

        if(logMove == null) logMove = StartCoroutine(MoveLogs());

    }
    Coroutine logMove = null;
    IEnumerator MoveLogs(){
        while(logsAwaiting.Count > 0){
            TextMeshProUGUI nuevo = logsAwaiting.Dequeue();
            logs.Enqueue(nuevo);
            //print(nuevo.text);
            if (logs.Count > length){
                Destroy(logs.Dequeue());
            }
            foreach (TextMeshProUGUI item in logs)
            {
                item.rectTransform.position = item.rectTransform.position + 60*bg.up;
            }
            yield return new WaitForSeconds(.25f);
        }
        logMove = null;
    }

}

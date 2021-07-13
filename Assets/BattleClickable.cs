using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleClickable : MonoBehaviour
{
    Cursor cursor;
    private void Start() {
        cursor = Cursor.current;
    }

    private void OnMouseEnter()
    {
        Focus();
    }

    private void OnMouseExit()
    {
        Defocus();
    }
    

    void Focus(){
        cursor.Select(GetComponent<Unit>());
    }

    void Defocus(){
        cursor.Deselect();
    }
}

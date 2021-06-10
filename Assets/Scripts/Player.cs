using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private void Update() {
        if(Input.GetMouseButtonUp(0) && activeUnit && cursor.selected != null){
            Attack(cursor.selected);
        }
    }
    
    protected override void OnActivate(){
        cursor.ChangeColor(Color.cyan);
    }
    protected override void OnTurnEnd(){
        cursor.ChangeColor(Color.red);
    }

}

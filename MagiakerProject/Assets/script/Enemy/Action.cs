using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {
   public bool FlagReset;
    //継承する
    public virtual void ActionEnter(GameObject target, GameObject self ) {
        
        
    }
    public virtual void StopCoroutines()
    {
        StopAllCoroutines();
        FlagReset = true;
    }
}

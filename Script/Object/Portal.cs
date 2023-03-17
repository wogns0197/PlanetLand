using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : FieldObject
{
    bool bTriggered;
    float TriggerTime;

    GameObject GO = null;
    void Start()
    {
        TriggerTime = 0f;
    }
    void Update()
    {
        if ( bTriggered ) {
            TriggerTime += Time.deltaTime;
        }

        if ( 2f < TriggerTime && bTriggered)
        {
            Character character = GO.GetComponent<Character>();
            if ( character != null ) 
            {
                character.OnTriggerFieldObject(GetType());
                bTriggered = false;
                TriggerTime = 0;
            }
        }
    }

    public override void Init ()
    {
        _Type = EFieldTrigger.Portal;
    }

    private void OnCollisionEnter(Collision other) {
        if ( other.gameObject.tag == "Character" ) 
        {
            bTriggered = true;
            GO = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other) {
        if ( other.gameObject.tag == "Character" )
        {
            TriggerTime = 0f;
            GO = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : FieldObject
{
    bool bTriggered;
    float TriggerTime;

    void Start()
    {
        TriggerTime = 0f;
        _Type = EFieldTrigger.Portal;
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
                character.OnTriggerFieldObject(_Type);
                bTriggered = false;
                TriggerTime = 0;
            }
        }
    }

    protected override void OnCollEnter(Collision other)
    {
        bTriggered = true;
    }

    protected override void OnCollExit(Collision other)
    {
        TriggerTime = 0f;
    }
}

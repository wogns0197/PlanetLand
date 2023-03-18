using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFieldTrigger
{
    None,
    Vehicle,
    PickUp,
    Portal,
}

public class FieldObject : MonoBehaviour
{
    protected GameObject GO = null;
    protected EFieldTrigger _Type;
    void Load() {}
    void OnLoad() {}
    private void OnDestroy() {
        
    }
    void Start()
    {
    }

    void Update()
    {    
    }

    protected virtual void OnCollEnter(Collision other) {}

    protected virtual void OnCollExit(Collision other) {}

    private  void OnCollisionEnter(Collision other) {
        if ( other.gameObject.tag == "Character" ) 
        {
            GO = other.gameObject;
        }

        OnCollEnter(other);
    }

    private void OnCollisionExit(Collision other) {
        if ( other.gameObject.tag == "Character" )
        {
            GO = null;
        }

        OnCollExit(other);
    }
}

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
    protected EFieldTrigger _Type;
    void Load() {}
    void OnLoad() {}

    public virtual void Init()
    {
        _Type = EFieldTrigger.None;
    }

    public EFieldTrigger GetType() { return _Type; }
    private void OnDestroy() {
        
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

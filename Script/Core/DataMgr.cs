using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ====================================================
// ====================================================
//          Must be Attached to Character !!!!!
// ====================================================
// ====================================================

public class DataMgr : MonoBehaviour
{
    private long dMoney;
    public delegate void OnMoneyUpdateDelegate(int num);

    public static OnMoneyUpdateDelegate OnMoneyUpdate;

    void Start()
    {
        dMoney = 10902037;
        OnMoneyUpdate += new OnMoneyUpdateDelegate(OnCallBackSelf);
    }
    void OnCallBackSelf(int outDelegateNum) { /* TODO */ }
    public static OnMoneyUpdateDelegate GetMoneyUpdateDeleget() { return OnMoneyUpdate; }
    
}

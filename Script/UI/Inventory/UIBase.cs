using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    void Start() {
    }

    void Update() {}

    protected bool bShow = false;

    bool GetIsShow() { return bShow; }
    void SetIsShow(bool v) { bShow = v; }

    virtual public void Open()
    {
        this.gameObject.SetActive(true);
        OnOpen();
    }

    virtual public void OnOpen()
    {
    }

    virtual public void Close()
    {
        this.gameObject.SetActive(false);
        OnClose();
    }

    virtual public void OnClose()
    {
        
    }
}

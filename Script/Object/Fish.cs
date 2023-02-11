using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody rg;
    int nRandSpeed, randRotY, randRepeat;
    bool bAggro;
    float ExitAggroTime, RotTime, nRandomRot;
    void Start()
    {
        bAggro = false;
        ExitAggroTime = 0;
        RotTime = 0;
        nRandomRot = 0;

        randRepeat = Random.Range(1, 10);
        rg = this.GetComponent<Rigidbody>();
        InvokeRepeating("InvokeMove", 3f, randRepeat);
        SetRotation();
        SetSpeed();
    }

    
    void Update()
    {
        if(!bAggro)
        {
            Quaternion targetRotation = Quaternion.Euler(0, nRandomRot, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1);

            this.transform.Translate(new Vector3(-0.005f * nRandSpeed,0,0));
        }
    }

    void SetSpeed(int speed = 1)
    {
        nRandSpeed = Random.Range(0, 2);
    }

    void SetRotation()
    {
        nRandomRot = Random.Range(0, 360);
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Debug.Log("Col!!" + this.gameObject.name);
        SetRotation();
        SetSpeed();
    }

    private void InvokeMove()
    {
        if (bAggro) return;

        SetRotation();
        SetSpeed();
    }

    public void SetAggro(GameObject Rod)
    {
        Transform tr = this.transform;
        bAggro = true;
        SetSpeed(0);

        Vector3 Direction = Rod.transform.position - tr.position;
        tr.rotation = Quaternion.Slerp(
            tr.rotation,
            Quaternion.LookRotation(Direction),
            1f
        );

        tr.rotation = Quaternion.Euler(
            new Vector3(tr.eulerAngles.x, tr.eulerAngles.y + 90, tr.eulerAngles.z));
    }

    public void ResetAggro()
    {
        bAggro = false;
    }
}

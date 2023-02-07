using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody rg;
    int randSpeed, randRotY, randRepeat;
    bool bAggro;
    float ExitAggroTime, RotTime;
    void Start()
    {
        bAggro = false;
        ExitAggroTime = 0;
        RotTime = 0;

        randRepeat = Random.Range(1, 10);
        rg = this.GetComponent<Rigidbody>();
        InvokeRepeating("InvokeMove", 3f, randRepeat);
        SetRotation();
        SetSpeed();
    }

    
    void Update()
    {
        // 서서히 회전을 구현해야하는데...이거말고 더 깨끗한 방식 있을건데..
        // RotTime += Time.deltaTime;
        // if(RotTime > )
    }

    void SetSpeed(int speed = 1)
    {
        randRepeat = Random.Range(1, 10);
        randSpeed = Random.Range(20, 70);

        rg.velocity = new Vector3(0,0,0);
        rg.AddForce(transform.rotation * Vector3.left * randSpeed * speed);
    }

    void SetRotation()
    {
        randRotY = Random.Range(0, 360);
        transform.Rotate(new Vector3(0, randRotY, 0));        
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

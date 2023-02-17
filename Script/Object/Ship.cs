using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject RodGroup, Rod;
    public float speed;
    private float x ,y;

    // 캐릭터로 옮겨야함
    private bool TEMPINVENTORY = false;

    private Rigidbody rg;

    void Start()
    {
        speed = 5.0f;
        rg = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // move ==================
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");

            Quaternion q = this.transform.rotation;

            if (y != 0) 
            {
                this.transform.Translate(new Vector3(0.1f * -y,0,0));
            }
            else
            {
                // this.transform.rotation = Quaternion.Euler(new Vector3(q.eulerAngles.x, 8, q.eulerAngles.y));
            }

            if (x != 0)
            {
                
                this.transform.rotation = Quaternion.Slerp(q, Quaternion.Euler(q.eulerAngles + new Vector3(0, 0, x*20 )), speed * Time.deltaTime);
            }

            if (rg.velocity.x > 8 ){
                rg.velocity = new Vector3(8, 0);
            }

            else if (rg.velocity.x < -8 ){
                rg.velocity = new Vector3(-8, 0);
            }
        }
        
        // rod ==================
        if (Input.GetKeyDown(KeyCode.Space)) { OnRodStart(); }

        // input
        // 캐릭터 인풋으로 옮겨야 함!
        if(Input.GetKeyDown(KeyCode.I))
        {
            GameObject Inven = GameObject.Find("Inventory");
            Inven.GetComponent<Canvas>().enabled = TEMPINVENTORY = !TEMPINVENTORY;
        }
    }

    private void OnRodStart()
    {
        if (Rod != null && RodGroup != null) 
        {
            if(Rod.GetComponent<Rod>().bAggro) { return; }

            if (RodGroup.activeSelf)
            {
                RodGroup.SetActive(false);
            }
            else
            {
                RodGroup.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float speed;
    private float x ,y;

    private Rigidbody rg;

    void Start()
    {
        speed = 5.0f;
        rg = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (y != 0)
        {
            this.transform.Translate(new Vector3(0.1f * -y,0,0));
        }

        if (x != 0)
        {
            Quaternion q = this.transform.rotation;
            this.transform.rotation = Quaternion.Slerp(q, Quaternion.Euler(q.eulerAngles + new Vector3(0, 0, x*20 )), speed * Time.deltaTime);
        }

        if (rg.velocity.x > 8 ){
            rg.velocity = new Vector3(8, 0);
        }

        else if (rg.velocity.x < -8 ){
            rg.velocity = new Vector3(-8, 0);
        }

        // transform.position += new Vector3(-x, 0, -y) * speed * 2 * Time.deltaTime;
    

        
        // if(x == 1) {
        //     this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(new Vector3(-90, 0, 0 )), speed * Time.deltaTime); 
        // }
            
        // else if(x == -1) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(new Vector3(-90, 0, 180 )), speed * Time.deltaTime);
        // if(y == 1) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(new Vector3(-90, 0, 270 )), speed * Time.deltaTime);
        // else if(y == -1) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(new Vector3(-90, 0, 90 )), speed * Time.deltaTime);

       
    }
}

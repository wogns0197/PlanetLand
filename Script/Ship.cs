using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Rigidbody rg;
    float maxSpeed;
    int speed;
    void Start()
    {
        rg = this.GetComponent<Rigidbody>();
        speed = 100;
        maxSpeed = 3f;
    }

    
    void Update()
    {
        Move();
    }

    private void Move()
    {    
        rg.AddForce(new Vector3(-Input.GetAxis("Horizontal"), 0f, 
        -Input.GetAxis("Vertical")) * speed * Time.deltaTime);
        
                // if (rg.velocity.x > maxSpeed)// right
        //     rg.velocity = new Vector2(maxSpeed, rg.velocity.y);
        // else if (rg.velocity.x < maxSpeed * (-1)) // Left Maxspeed
        //     rg.velocity = new Vector2(maxSpeed * (-1), rg.velocity.y);
    }
}

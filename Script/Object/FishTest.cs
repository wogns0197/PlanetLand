using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTest : MonoBehaviour
{
    public GameObject bait;
    public float speed = 1.0f;
    public float radius = 1.0f;

    private Vector3 center;
    private float angle;

    private void Start()
    {
        center = bait.transform.position;
    }

    private void Update()
    {
        angle += Time.deltaTime * speed;
        float x = center.x + Mathf.Cos(angle) * radius;
        float y = center.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            // Catch the fish
            // ...
        }
    }
}
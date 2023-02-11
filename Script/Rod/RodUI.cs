using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodUI : MonoBehaviour
{
    public Slider slider;
    public Rod Rod;
    public float interval = 0.01f;
    private float reduceValue = 0.01f;

    private float elapsedTime = 0.0f;

    void Start()
    {
        reduceValue = Random.Range(0.005f, 0.03f);
        Rod = GameObject.Find("Rod").GetComponent<Rod>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slider.value = slider.value + 5;
        }

        if (slider.value > 99.9f)
        {
            this.gameObject.SetActive(false);
            if (Rod != null)
            {
                Rod.OnFinishRodding();
            }
        }

        slider.value -= reduceValue;
    }
}

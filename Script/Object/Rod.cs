using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour
{
    Fish targetFish;
    public bool bAggro {get; set;}
    float AggroTime;
    Queue<Collider> QCol;
    public GameObject RodUI;
    void Start()
    {
        QCol = new Queue<Collider>();
        bAggro = false;
        AggroTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        WaitUntilAggro();
        StartRodding();
    }

    private void WaitUntilAggro()
    {
        if(QCol.Count > 0)
        {
            targetFish = QCol.Peek().gameObject.GetComponent<Fish>();
            if(targetFish != null)
            {
                AggroTime += Time.deltaTime;
            }

            if (bAggro == false & AggroTime > 2f && targetFish != null)
            {
                Debug.Log("Aggro get ---- " + targetFish.gameObject.name);        
                targetFish.SetAggro(this.gameObject);
                bAggro = true;
            }
        }
    }

    private void StartRodding()
    {
        if(!bAggro) { return; }

        if(AggroTime > 4f)
        {
            Debug.Log("Rod Finish ---- " + targetFish.gameObject.name);
            StartRodUI();
        }
    }

    private void StartRodUI()
    {
        if(RodUI != null && !RodUI.activeSelf)
        {
            RodUI.SetActive(true);
            RodUI.GetComponent<RodUI>().Init();
        }
    }
    public void OnFinishRodding()
    {
        Destroy(targetFish.gameObject);
        targetFish = null;
        AggroTime = 0;
        bAggro = false;
        QCol.Dequeue();
    }

    void OnTriggerEnter(Collider other)
    {
        // 이러면 하나만 Queue에 들어오게되니 수정해야함
        if (targetFish) { return;}

        Debug.Log("Trigger Enter!!!! ---- " + other.gameObject.name);        

        if(!QCol.Contains(other)) {
            QCol.Enqueue(other);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(QCol.Contains(other)) 
        {
            Debug.Log("Trigger Exit  " + other.gameObject.name);        
            QCol.Dequeue();

            if(targetFish != null) 
            {
                targetFish.ResetAggro();
                bAggro = false;
                targetFish = null;
            }
        }
    }
}

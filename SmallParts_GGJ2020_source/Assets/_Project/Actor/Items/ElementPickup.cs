// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ElementPickup : MonoBehaviour, IPickup
{
    [SerializeField]
    private Element element;
    private bool available = true;

    [SerializeField]
    private GameObject art;

    [SerializeField]
    private GameObject acquireEffect;

    
    [SerializeField]
    private float respawnDelay;
    private float respawnTimer;


    void IPickup.Pickup(ServitorController controller)
    {
        if (this.available)
        {
            controller.SetElement(this.element);
            if (this.art != null) this.art.SetActive(false);
            this.respawnTimer = 0f;
            this.available = false;
           
        }
    }

    void Update()
    {
        if (!this.available && this.respawnDelay > -0.1f)
        {
            this.respawnTimer += Time.deltaTime;
            if (this.respawnTimer >= this.respawnDelay)
            {
                if (this.art != null) this.art.SetActive(true);
                this.available = true;
               
            }
        }
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
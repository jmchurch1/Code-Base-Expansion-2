// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ServitorInteraction : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Interaction with: " + collision.gameObject.name);
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
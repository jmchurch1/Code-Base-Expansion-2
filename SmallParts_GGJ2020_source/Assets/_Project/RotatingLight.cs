// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class RotatingLight : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private void Update()
    {
        this.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Drift : MonoBehaviour
{
    [SerializeField]
    float speed=0.1f;
    [SerializeField]
    private float distance;
    float dir = 1f;
    float traveled;

    private void Update()
    {
        var offset = Time.deltaTime * speed;
        var movement = dir * offset;
        this.transform.Translate(Vector3.right * movement, Space.World);
        this.traveled += offset;
        if (this.traveled >= this.distance)
        {
            dir *= -1f;
            this.traveled = 0f;
        }
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
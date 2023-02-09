// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Stage_Start : Stage
{
    [SerializeField]
    private Transform camStart;

    [SerializeField]
    private Transform playerStart;

    public override void OnBeginInStage()
    {
        base.OnBeginInStage();
        Camera.main.transform.position = camStart.position;
        FindObjectOfType<ServitorController>().SetPosition(playerStart.position);
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
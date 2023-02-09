// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private Stage activeStage;
    private Stage[] stages;

    private void Start()
    {
        this.stages = GetComponentsInChildren<Stage>();
        for(int i =0; i < this.stages.Length; i++)
        {
            this.stages[i].BecomeActiveEvent += this.OnStageChange;
        }
        this.activeStage?.OnBeginInStage();
    }
    
    private void OnStageChange(object sender, EventArgs args)
    {
        
        activeStage = (Stage)sender;
        Debug.Log("Stage change discovered by manager, active stage now " + activeStage.name);
    }

    private void Update()
    {
        activeStage?.OnUpdateStage();
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Stage_Brush : Stage
{
    [SerializeField]
    private Transform cameraTarget;

    // this stage slowly zooms out the first time we arrive in it.
    private float zoomLambda = 0f;
    private float zoomStart = -4f;
    private float zoomEnd = -15f;

    private ServitorController servitor;
    private Vector3 targetPosition;
    private List<Drift> drifters;
    private List<Vector3> driftStarts = new List<Vector3>();

    private void Awake()
    {
        servitor = FindObjectOfType<ServitorController>();
        
    }
    protected override void OnStart()
    {
        base.OnStart();
   
        this.drifters = new List<Drift>(this.GetComponentsInChildren<Drift>());
        for (int i = 0; i < this.drifters.Count; i++)
        {
            this.driftStarts.Add(drifters[i].transform.position);
        }

    }

    public override void OnEnterStage(object sender, EventArgs args)
    {
        base.OnEnterStage(sender, args);
        for (int i = 0; i < this.drifters.Count; i++)
        {
            this.drifters[i].transform.position = this.driftStarts[i];
            this.drifters[i].enabled = true;
        }
    }
    public override void OnExitStage(object sender, EventArgs args)
    {
        base.OnExitStage(sender, args);
        for (int i = 0; i < this.drifters.Count; i++)
        {
            this.drifters[i].enabled = false;
        }
    }
    public override void OnUpdateStage()
    {
        
        base.OnUpdateStage();
        targetPosition = this.cameraTarget.position;

        if (this.zoomLambda < 1f)
        {

            servitor.SetLocked(true);
            this.zoomLambda += Time.deltaTime / 4f;
            targetPosition.z = Mathf.SmoothStep(this.zoomStart, this.zoomEnd, this.zoomLambda);
            this.cameraTarget.position = targetPosition;
            Camera.main.transform.position = targetPosition;
        }
        else 
        {
            servitor.SetLocked(false);
            this.enabled = false;
        }
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
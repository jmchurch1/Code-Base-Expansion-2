// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Stage_Yard : Stage
{
    [SerializeField]
    private Transform cameraTarget;

    // this stage slowly zooms out the first time we arrive in it.
    private float zoomLambda = 0f;
    private float zoomStart = -4f;
    private float zoomEnd = -15f;

    private ServitorController servitor;
    private Vector3 targetPosition;

    [SerializeField]
    private float followSpeed =  3f;

    private void Awake()
    {
        servitor = FindObjectOfType<ServitorController>();
        targetPosition = this.cameraTarget.position;
    }
    public override void OnUpdateStage()
    {

        base.OnUpdateStage();

        targetPosition.x = Mathf.Max(this.cameraTarget.position.x, servitor.transform.position.x);
        targetPosition.y = servitor.transform.position.y;
        
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, this.followSpeed);
        
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
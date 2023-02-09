// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Stage_Mountain : Stage
{
    [SerializeField]
    private Transform cameraTarget;

    // this stage follows the hero vertically between two height limits.
    [SerializeField]
    private Vector2 heightLimits;

    private ServitorController servitor;
    private Vector3 targetPosition;

    [SerializeField]
    private float followSpeed = 1f;
    private float heightOffset;
    private void Awake()
    {
        servitor = FindObjectOfType<ServitorController>();

    }
    protected override void OnStart()
    {
        
        this.heightLimits.x += this.transform.position.y;
        this.heightLimits.y += this.transform.position.y;
    }

    public override void OnEnterStage(object sender, EventArgs args)
    {
        base.OnEnterStage(sender, args);
        this.heightOffset = this.cameraTarget.position.y - this.servitor.transform.position.y;
        this.targetPosition.x = this.cameraTarget.position.x;
        targetPosition.z = this.cameraTarget.position.z;
    }
    public override void OnUpdateStage()
    {

        base.OnUpdateStage();
        //Debug.Log("Updating mountain.");
        //targetPosition.x = this.servitor.transform.position.x;
        targetPosition.y = this.servitor.transform.position.y + this.heightOffset;
       
        targetPosition.y = Mathf.Clamp(targetPosition.y, this.heightLimits.x, this.heightLimits.y);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, this.followSpeed);

    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
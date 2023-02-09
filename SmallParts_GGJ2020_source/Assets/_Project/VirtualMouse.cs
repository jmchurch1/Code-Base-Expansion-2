// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class VirtualMouse : MonoBehaviour
{
    [SerializeField]
    private Vector3 range;

    public Vector3 Position;

    Vector3 prevPos;
    Vector3 pos;

    private ServitorController servitor;
    private Transform anchorTransform;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    private void Start()
    {
        this.servitor = FindObjectOfType<ServitorController>();
        this.anchorTransform = this.servitor.transform;
    }
    private void Update()
    {
   
        var pos = Input.mousePosition;
        var delta = pos - this.prevPos;
        pos.x -= Screen.width / 2f;
        pos.y -= Screen.height / 2f;
        pos.x *= range.x;
        pos.y *= range.y;
        this.Position.x = pos.x + this.anchorTransform.position.x;
        this.Position.y = pos.y + this.anchorTransform.position.y;
        this.transform.position = this.Position;
    }

}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
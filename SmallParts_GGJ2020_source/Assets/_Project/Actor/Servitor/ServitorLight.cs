// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ServitorLight : MonoBehaviour
{
    Vector3 worldmouse;
    private Camera cam;
    private VirtualMouse vmouse;
    private void Start()
    {
        vmouse = FindObjectOfType<VirtualMouse>();
        cam = Camera.main;
        //var cams = FindObjectsOfType<Camera>();
        //foreach(var camera in cams)
        //{
        //    if (camera.CompareTag("3D Camera"))
        //    {
        //        cam = camera;
        //    }
        //}
    }
    private void Update()
    {
        if (cam)
        {
            //var mouse = Input.mousePosition;
            //mouse.z = -cam.transform.position.z;
            ////Debug.Log(mouse.ToString("0.00"));
            //var worldMouse = cam.ScreenToWorldPoint(mouse);
            ////Debug.Log(worldmouse.ToString("0.00"));
            Vector3 dir = vmouse.Position - this.transform.position;
            Debug.DrawLine(this.transform.position, vmouse.Position);
            this.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        }
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
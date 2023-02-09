// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TransitionIn : MonoBehaviour
{
    [SerializeField]
    private Transform
        _cameraStart,
        _playerStart;

    public Transform CamStart { get { return this._cameraStart; } }
    public Transform PlayerStart { get { return this._playerStart; } }

    public EventHandler TransitionEvent;
    public Stage PreviousStage { get; private set; }
    
    public void StartTransition(Stage previousStage)
    {
        this.PreviousStage = previousStage;
        var bumper = FindObjectOfType<Bumper>();
        bumper.StartTransition(this.EndTransition);
        
    }

    public void EndTransition()
    {
        Debug.Log("transition event ended");
        Camera.main.transform.position = this.CamStart.position;
        FindObjectOfType<ServitorController>().transform.position = this.PlayerStart.position;
        this.TransitionEvent?.Invoke(this, EventArgs.Empty);
        if (this.TransitionEvent == null)
        {
            Debug.Log("null transition event.");
        }
       
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
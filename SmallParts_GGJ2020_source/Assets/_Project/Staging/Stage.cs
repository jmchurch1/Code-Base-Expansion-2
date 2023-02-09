// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Stage : MonoBehaviour
{
    private List<TransitionIn> arrivals;
    private List<TransitionOut> departures;
    public EventHandler BecomeActiveEvent;
    private Stage lastStage;

    private void Start()
    {
        this.arrivals = new List<TransitionIn>(GetComponentsInChildren<TransitionIn>());
        Debug.Log(this.name + " found " + arrivals.Count);
        for (int i = 0; i < arrivals.Count; i++)
        {
            arrivals[i].TransitionEvent += this.OnEnterStage;
        }
        this.departures = new List<TransitionOut>(GetComponentsInChildren<TransitionOut>());
        for (int i = 0; i < departures.Count; i++)
        {
            departures[i].TransitionEvent += this.OnExitStage;
        }
        this.OnStart();
    }
    protected virtual void OnStart() { }
    public virtual void OnUpdateStage()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            FindObjectOfType<ServitorController>().SetPosition(this.arrivals[0].PlayerStart.position);
        }
    }
    public virtual void OnBeginInStage()
    {

    }
    public virtual void OnEnterStage(object sender, EventArgs args)
    {
        Debug.Log(this.name + " became active.");
        this.BecomeActiveEvent?.Invoke(this, EventArgs.Empty);
        
    }

    public virtual void OnExitStage(object sender, EventArgs args)
    {

    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TransitionOut : MonoBehaviour
{
    [SerializeField]
    private TransitionIn target;
    public Stage parentStage;
    public EventHandler TransitionEvent;

    private void Awake()
    {
        this.parentStage = GetComponentInParent<Stage>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.TransitionEvent?.Invoke(this, EventArgs.Empty);
        target.StartTransition(this.parentStage);
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
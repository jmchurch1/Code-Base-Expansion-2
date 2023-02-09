// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private float transitionSpeed;

    public bool IsFadingIn { get; private set; }
    public bool IsFading { get; private set; }
    private IEnumerator transition;
    private Action action;

    public void StartTransition(Action action)
    {
        Debug.Log("Starting bumper transition");
        if (!this.IsFadingIn)
        {
            Debug.Log("Fade in initialized.");
            this.action = action;
            this.transition = this.TransitionProcess();
            this.IsFading = true;
            this.IsFadingIn = true;
        }

    }
    
    private IEnumerator TransitionProcess()
    {
        Debug.Log("Starting transition process");
        Color c = this.sprite.color;
        bool complete = false;
        while (!complete)
        {
            c.a += Time.deltaTime * this.transitionSpeed ;
            if (c.a > 1f)
            {
                c.a = 1f;
                complete = true;
            }
            this.sprite.color = c;
            yield return null;
        }
        if (action != null) action();
        this.IsFadingIn = false;
        complete = false;
        while(!complete)
        {
            c.a -= Time.deltaTime * this.transitionSpeed;
            if (c.a <= 0f)
            {
                c.a = 0f;
                complete = true;
            }
            this.sprite.color = c;
            yield return null;
        }

        this.IsFading = false;
       
    }

    private void Update()
    {
        if (this.IsFading)
        {
            this.transition.MoveNext();
           
        }
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
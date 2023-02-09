// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BranchesTarget : ElementalTarget
{
    List<SpriteRenderer> FadeBurntRoots;
    private bool finalized = false;

    [SerializeField]
    private GameObject 
        burnedContainer,
        grownContainer,
        waitingContainer;

    public override void OnUseElement(Element element)
    {
        base.OnUseElement(element);
        if (element == Element.Water)
        {
            this.Grow();
           
        }
        else if (element == Element.Fire)
        {
            this.Die();
        }
        else
        {
            this.RequestElement();
        }
    }

    void Grow()
    {
        // we shift our colors, go ethereeal, add effects,
        // make wisps white.

        //for now its a quick swap between containers.
        this.waitingContainer.SetActive(false);
        this.grownContainer.SetActive(true);
        this.finalized = true;
    }

    void Die()
    {
        // we shift our colors, our size, and make wisps black.

        // for now it's a quick container swap
        this.waitingContainer.SetActive(false);
        this.burnedContainer.SetActive(true);
        this.finalized = true;
    
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
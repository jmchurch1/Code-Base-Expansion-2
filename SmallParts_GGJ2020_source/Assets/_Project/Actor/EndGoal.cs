// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class EndGoal : MonoBehaviour, IInteractable
{
    public GameObject openState;
    public GameObject closedState;
    public GameObject winscreen;

    public bool isOpen;

    void IInteractable.Interact(ServitorController controller)
    {
        Debug.Log("interacting with END");
        if (this.isOpen) this.Win();
        else controller.UseElement(this);
    }

    void IInteractable.OnUseElement(Element element)
    {
        if (!this.isOpen && element == Element.Water)
        {
            Debug.Log("Opening");
            this.isOpen = true;
            this.SwitchToOpen();
        }
    }

    bool IInteractable.VerifyElement(Element element)
    {
        Debug.Log("Verifying " + element.ToString());
        return element == Element.Water && !this.isOpen;
    }

    void SwitchToOpen()
    {
        Debug.Log("opening");
        this.closedState.SetActive(false);
        this.openState.SetActive(true);
    }

    void Win()
    {
        Debug.Log("WIN");
        this.winscreen.SetActive(true);
        GameObject.FindObjectOfType<ServitorController>().SetLocked(true);
    }

}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
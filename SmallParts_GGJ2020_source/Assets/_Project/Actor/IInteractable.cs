// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


/// <summary>
/// this requires an interaction key.
/// </summary>
public interface IInteractable
{
    void Interact(ServitorController controller);
    //void UseElement(ServitorController element);
    bool VerifyElement(Element element);
    void OnUseElement(Element element);
}

/// <summary>
/// This occurs on collision, even if we haven't interacted.
/// </summary>
public interface IPickup
{
    void Pickup(ServitorController controller);
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Invisiwall : MonoBehaviour, IMoveModifier
{
    [SerializeField]
    private Vector3 positiveModifier = Vector3.one;

    [SerializeField]
    private Vector3 negativeModifier = Vector3.one;

    Vector3 IMoveModifier.PositiveModifier { get { return this.positiveModifier; } }
    Vector3 IMoveModifier.NegativeModifier { get { return this.negativeModifier; } }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
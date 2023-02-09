// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public interface IMoveModifier
{
    Vector3 PositiveModifier { get; }
    Vector3 NegativeModifier { get; }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
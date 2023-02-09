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
/// Fetches child transforms and automatically parents them appropriately
/// to rig the servitor. Turns the body parts into prefabs that can be generated.
/// </summary>
public class ServitorRigger : MonoBehaviour
{
    private ServitorLimb leftLeg, rightLeg;
    private Transform leftArm, rightArm;

    /// <summary>
    /// The arms and legs use fake inverse-kinematics,
    /// where the feet and hands are placed and the rest points
    /// toward the body. We need to know how far they can get
    /// from the body before a change has to be made to prevent
    /// exposing the limits of the sprite.
    /// </summary>
    [SerializeField]
    private float maxLegDistance = 100f, maxArmDistance = 100f;

    [SerializeField]
    private ServitorLimb legPrefab;
    private List<ServitorLimb> legs = new List<ServitorLimb>();

    [SerializeField]
    private Transform bodyPrefab;
    private Transform body;

    private Transform rightLegNode, leftLegNode;
    private Transform face;

    private ServitorLimb testLeg;

    private List<Transform> antennaBones = new List<Transform>();

    private void Awake()
    {
        this.body = Instantiate(bodyPrefab, this.transform);
        this.body.localScale = Vector3.one * 0.13f;
        var bodyChildren = new List<Transform>(this.body.GetComponentsInChildren<Transform>());
        var mainBody = this.body.Find("_body main");
        List<Transform> bones = new List<Transform>();

        bodyChildren.Remove(mainBody);
        bodyChildren.Remove(this.body);
       
        this.rightLegNode = this.body.Find("_leg node right");
        this.leftLegNode = this.body.Find("_leg node left");
        for (int i = 0; i < bodyChildren.Count; i++)
        {
            var child = bodyChildren[i];
            if (child.name.Contains("body"))
            {
                child.SetParent(mainBody);
            }
 

        }
        
        this.SendMessage("SetBodyTransform", this.body);

        //this.testLeg = this.CreateLeg(1);
       
    }

    public ServitorLimb CreateLeg(int handedness)
    {
        var leg = Instantiate(this.legPrefab, this.transform);
        this.legs.Add(leg);
        var legNode = handedness > 0 ? this.rightLegNode : this.leftLegNode;
        leg.SetAnchor(legNode, handedness);
        return leg;

    }
    //public ServitorLimb CreateLeg(int handedness)
    //{
    //    var copy = this.legPrefab.GetCopy();
    //    copy.SetHandedness(handedness);
    //    return copy;
    //}
    
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
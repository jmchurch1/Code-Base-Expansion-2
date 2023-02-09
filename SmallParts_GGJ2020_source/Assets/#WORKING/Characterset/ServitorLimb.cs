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
/// A controller placed on the 'node' of the leg
/// where the thigh meets the body.
/// it controls how the feet, calf, and thigh rotate
/// and target new positions.
/// </summary>
public class ServitorLimb : MonoBehaviour
{
    public EventHandler OverreachEvent;

    public float MaxReach { get; private set; }
    public float MinReach { get; private set; }
    private Transform primaryBone;
    private Transform secondaryBone;
    private Transform endBone;
    private float extensionLength;
    private float primaryLength;

    private float bendAngle;
    private float extremeAngle;
    private float primaryAngle;
    private float fullLength;

    private float endDistance;

    [SerializeField]
    private float stepSpeed = 3f;

    public int handedness { get; private set; }
    private Vector3 endGoal;
    private Vector3 endAnchor;

    private float goalDistance;
    private float prevGoalDistance;
    
    private int direction;
    private float prevEndDistance;
    private Vector3 defaultGoal;
    private float calculatedDistance;


    /// <summary>
    /// This is the position in space we do the physics test.
    /// if we get too far away we test for one by our body,
    /// and if we get too close we test for one farther away.
    /// </summary>
    private Vector3 seekingPosition;
    private ContactFilter2D filter;

    public void SetHandedness(int dir)
    {
        //this.handedness = 1;
        this.handedness = dir;
        //  this.transform.localScale = new Vector3(handedness, 1f, 1f);

    }

    private void Awake()
    {
        this.primaryBone = this.transform.Find("_primary bone");
        this.secondaryBone = this.primaryBone.Find("_secondary bone");
        this.endBone = this.secondaryBone.Find("_end bone");
        this.endAnchor = this.endBone.position;
        this.defaultGoal = this.endBone.position;
        this.endGoal = this.defaultGoal;
        this.filter = new ContactFilter2D();
        this.filter.layerMask = LayerMask.GetMask("ground");
        this.CalculateLengths();
    }
   
    public void Update()
    {
        this.UpdateEndPosition();

        //this.goalDistance = Vector3.Distance(this.endGoal, this.transform.position);
        //this.direction = (int)Mathf.Sign(this.endGoal.x - this.transform.position.x);
        
        //if (this.direction == this.handedness && this.goalDistance < this.MinReach)
        //{
        //    this.OnUnderreach();
        //}
        //else if (this.direction != this.handedness && this.goalDistance > this.MaxReach)
        //{
        //    this.OnOverreach();
        //}

        this.UpdateAngles();
        this.primaryBone.eulerAngles = Vector3.forward * this.primaryAngle;

        this.secondaryBone.eulerAngles = Vector3.forward * this.bendAngle;


    }

    public void SetAnchor(Transform anchor, int handedness)
    {
        this.transform.position = anchor.position;
        this.handedness = handedness;
    }

    public void SetScale(float scale)
    {
        this.transform.localScale = Vector3.one * scale;
        this.CalculateLengths();
    }

    private void CalculateLengths()
    {
        this.extensionLength = Vector3.Distance(secondaryBone.position, endBone.position);
        this.primaryLength = Vector3.Distance(primaryBone.position, secondaryBone.position);
        this.endDistance = Vector3.Distance(primaryBone.position, endBone.position);
        this.calculatedDistance = this.endDistance;
        this.MaxReach = (this.extensionLength + this.primaryLength) * 0.8f;
        this.MinReach = this.extensionLength / 4f;
    }
    
    public void SetActive(bool active)
    {
       
        this.gameObject.SetActive(active);
        this.secondaryBone.gameObject.SetActive(active);
        this.primaryBone.gameObject.SetActive(active);
        this.endBone.gameObject.SetActive(active);
        this.enabled = active;

    }

    public void OnOverreach()
    {
        // when we overreach, we should look for a new spot just below our node
        Vector3 column = this.transform.position + (Vector3.right * this.handedness * this.MinReach * 1.1f);
        RaycastHit2D hit = Physics2D.Raycast(column + (Vector3.up * 2f), Vector2.down);
        if (hit.collider != null)
        {
            this.SetEndGoal(hit.point);
        }
    }

    public void OnUnderreach()
    {
        // when we underreach, we look at a 30 degree angle upward from our node,
        // then iterate the angle counter clockwise for 10 degree increments, until we have iterated
        // 10 times. We stop if we ever find a spot.
        //float angle = 30f;
        //RaycastHit2D hitData;
        //bool isHit = false;
        //int safety = 1000;
        //while (angle > -80f && !isHit && --safety > 0)
        //{
        //    float rads = angle * Mathf.Deg2Rad;
        //    Vector2 target = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads)) * this.MaxReach;
        //    hitData = Physics2D.Linecast(this.transform.position, target);
        //    if (hitData.collider != null)
        //    {
        //        isHit = true;
        //        this.SetEndGoal(hitData.point);
          
        //        break;
        //    }
        //    else
        //    {
        //        angle -= 10f;
        //    }
        //}
        //if (safety <= 0) Debug.LogError("Loop error");
        
    }
    public void SetEndGoal(Vector3 goal)
    {
        this.endGoal = goal;
    }
    private void UpdateEndPosition()
    {
        this.endAnchor = Vector3.Lerp(this.endBone.transform.position, this.endGoal, Time.deltaTime * this.stepSpeed);
        this.calculatedDistance = Vector3.Distance(this.primaryBone.position, this.endAnchor);
    }

    private void UpdateAngles()
    {
        float a = this.extensionLength;
        float a2 = a * a;
        float b = this.primaryLength;
        float b2 = b*b;
        float c = this.calculatedDistance;

        float c2 = c*c;

        this.extremeAngle = (float)Math.Acos((b2 + c2 - a2) /
                                           (2 * b * c));
        this.bendAngle = (float)Math.Acos((a2 + c2 - b2) /
                                           (2 * a * c));
        this.primaryAngle = (float)Math.Acos((a2 + b2 - c2) /
                                           (2 * a * b));

        this.extremeAngle *= Mathf.Rad2Deg;
        this.bendAngle *= Mathf.Rad2Deg;
        this.primaryAngle *= Mathf.Rad2Deg;

    }

}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
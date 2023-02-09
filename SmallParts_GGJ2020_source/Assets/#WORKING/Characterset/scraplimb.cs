//// q // cD // d // 

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.AI;

//using Object = UnityEngine.Object;
//using Random = UnityEngine.Random;

///// <summary>
///// A controller placed on the 'node' of the leg
///// where the thigh meets the body.
///// it controls how the feet, calf, and thigh rotate
///// and target new positions.
///// </summary>
//public class ServitorLimb : MonoBehaviour
//{
//    public EventHandler OverreachEvent;

//    public float MaxReach { get; private set; }
//    public float MinReach { get; private set; }
//    private Transform primary;
//    private Transform extension;
//    private Transform extremity;
//    private float extensionLength;
//    private float primaryLength;

//    private float bendAngle;
//    private float extremeAngle;
//    private float primaryAngle;
//    private float fullLength;

//    private float footDistance;

//    [SerializeField]
//    private float stepSpeed = 3f;

//    public int handedness { get; private set; }
//    private Vector3 extremityGoal;
//    private float goalDistance;
//    private float prevGoalDistance;

//    private int direction;
//    private float prevFootDistance;
//    private Vector3 defaultGoal;

//    public void SetHandedness(int dir)
//    {
//        //this.handedness = 1;
//        this.handedness = dir;
//        //  this.transform.localScale = new Vector3(handedness, 1f, 1f);

//    }

//    public ServitorLimb GetCopy()
//    {
//        var copy = Instantiate<ServitorLimb>(this, this.transform.parent);
//        copy.SetComponents(primary, extension, extremity);
//        return copy;
//    }
//    public void SetComponents(Transform primary, Transform extension, Transform extremity)
//    {
//        this.primary = primary;
//        this.extension = extension;
//        this.extremity = extremity;
//        this.extensionLength = Vector3.Distance(extension.position, extremity.position);
//        this.primaryLength = Vector3.Distance(primary.position, extension.position);
//        this.MaxReach = (this.extensionLength + this.primaryLength) * 0.8f;
//        this.MinReach = this.extensionLength / 4f;
//        this.defaultGoal = this.extremity.position;
//        this.extremityGoal = this.defaultGoal;

//    }

//    public void Update()
//    {
//        this.UpdateExtremityPosition();

//        this.footDistance = Vector3.Distance(this.transform.position, this.extremity.position);
//        this.goalDistance = Vector3.Distance(this.extremity.position, this.extremityGoal);

//        float stepDelta = this.goalDistance - this.prevGoalDistance;
//        float delta = this.footDistance - prevFootDistance;

//        this.UpdateAngles();
//        this.primary.eulerAngles = Vector3.forward * -this.primaryAngle;

//        this.extension.eulerAngles = Vector3.forward * -this.bendAngle;

//    }


//    public void SetActive(bool active)
//    {

//        this.gameObject.SetActive(active);
//        this.extension.gameObject.SetActive(active);
//        this.primary.gameObject.SetActive(active);
//        this.extremity.gameObject.SetActive(active);
//        this.enabled = active;

//    }

//    public void OnOverreach()
//    {
//        // when we overreach, we should look for a new spot just below our node
//        Vector3 column = this.transform.position + (Vector3.right * this.handedness * this.MinReach * 1.1f);
//        RaycastHit2D hit = Physics2D.Raycast(column + (Vector3.up * 2f), Vector2.down);
//        if (hit.collider != null)
//        {
//            this.SetExtremityGoal(hit.point);
//        }
//    }

//    public void OnUnderreach()
//    {
//        // when we underreach, we look at a 30 degree angle upward from our node,
//        // then iterate the angle counter clockwise for 10 degree increments, until we have iterated
//        // 10 times. We stop if we ever find a spot.
//        float angle = 30f;
//        RaycastHit2D hitData;
//        bool isHit = false;
//        int safety = 1000;
//        while (angle > -80f && !isHit && --safety > 0)
//        {
//            float rads = angle * Mathf.Deg2Rad;
//            Vector2 target = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads)) * this.MaxReach;
//            hitData = Physics2D.Linecast(this.transform.position, target);
//            if (hitData.collider != null)
//            {
//                isHit = true;
//                this.SetExtremityGoal(hitData.point);

//                break;
//            }
//            else
//            {
//                angle -= 10f;
//            }
//        }
//        if (safety <= 0) Debug.LogError("Loop error");

//    }
//    public void SetExtremityGoal(Vector3 goal)
//    {
//        this.extremityGoal = goal;
//    }
//    private void UpdateExtremityPosition()
//    {
//        this.extremity.transform.position = Vector3.Lerp(this.extremity.transform.position, this.extremityGoal, Time.deltaTime * this.stepSpeed);
//    }

//    private void UpdateAngles()
//    {
//        float a = this.extensionLength;
//        float a2 = a * a;
//        float b = this.primaryLength;
//        float b2 = b * b;
//        float c = this.footDistance;

//        float c2 = c * c;

//        this.extremeAngle = (float)Math.Acos((b2 + c2 - a2) /
//                                           (2 * b * c));
//        this.bendAngle = (float)Math.Acos((a2 + c2 - b2) /
//                                           (2 * a * c));
//        this.primaryAngle = (float)Math.Acos((a2 + b2 - c2) /
//                                           (2 * a * b));

//        this.extremeAngle *= Mathf.Rad2Deg;
//        this.bendAngle *= Mathf.Rad2Deg;
//        this.primaryAngle *= Mathf.Rad2Deg;

//    }

//}

//// q // cD // d // 
//// valkymaera Q1 2020
//// Unity 2019.3.0f5
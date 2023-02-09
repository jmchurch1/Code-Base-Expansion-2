// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ServitorFace : MonoBehaviour
{
   public enum Expression
    {
        None = 0,
        Small,
        Surprise,
        Happy,
        Sad,
        Angry,
        Dead
    }

    [SerializeField]
    private GameObject
        container,
        normal,
        small,
        surprise,
        happy,
        sad,
        angry,
        dead;

    [SerializeField]
    private Vector2 faceLookOffset;

    private List<GameObject> faces = new List<GameObject>();
    private float expressionTimer = -1f;
    private bool blink;
    private VirtualMouse mouse;
    private ServitorLight light;
    private Expression expression = Expression.None;
    private bool lockFace = false;


    private void Awake()
    {
        this.faces = new List<GameObject>()
        {
            normal, small, surprise, happy, sad, angry, dead
        };
        this.mouse = FindObjectOfType<VirtualMouse>();
        this.light = FindObjectOfType<ServitorLight>();
        this.faceLookOffset = new Vector2(0.3f, 0.6f);
    }
    private void Start()
    {
        this.Blink();
        this.SetExpression(Expression.None);
    }
    public void Blink()
    {
        if (this.blink) this.container.SetActive(false);
        this.Invoke("Unblink", 0.18f);
       
    }

    public void Unblink()
    {
        this.container.SetActive(true);
        this.Invoke("Blink", Random.Range(2f, 8f));
    }

    public void SetExpression(Expression expression, float timer = -1f, bool lockFace = false)
    {
        if (lockFace || !this.lockFace)
        {
            //Debug.Log("setting expression to " + expression.ToString());
            int index = (int)expression;
            for (int i = 0; i < this.faces.Count; i++)
            {
                this.faces[i].SetActive(i == index);
            }
            this.expressionTimer = timer;
            this.blink = index <= 2;
            if (!blink) this.transform.localPosition = Vector3.zero;
            this.expression = expression;
            this.lockFace = lockFace;
        }
    }

    private void Update()
    {
        if (this.expressionTimer > 0f)
        {
            this.expressionTimer -= Time.deltaTime;
            if (this.expressionTimer <= 0f)
            {
                this.lockFace = false;
                this.SetExpression(Expression.None);

            }
        }

        if (((int)this.expression) <= 2)
        {
            
            this.transform.localPosition = this.light.transform.right * this.faceLookOffset;
        }
        
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
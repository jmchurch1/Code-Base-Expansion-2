// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ElementalTarget : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<Element> elements;

    [SerializeField]
    private SpriteRenderer request;
    private float requestTimer;

    void IInteractable.Interact(ServitorController controller)
    {
        controller.UseElement(this);
    }

    bool IInteractable.VerifyElement(Element element)
    {
        bool valid = this.elements.Count < 1 || this.elements.Contains(element);
        return valid;
    }

    public virtual void OnUseElement(Element element)
    {

    }
    public void RequestElement()
    {
        if (request != null)
        {
            request.enabled = true;
            this.requestTimer = 4f;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {

        this.RequestElement();
    }

    private void Update()
    {
        if (this.requestTimer > 0f)
        {
            this.requestTimer -= Time.deltaTime;
            if (this.requestTimer <= 0f)
            {
                this.request.enabled = false;
            }
        }
    
    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
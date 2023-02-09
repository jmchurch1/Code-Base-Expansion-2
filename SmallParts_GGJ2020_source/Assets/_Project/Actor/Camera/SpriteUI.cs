// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SpriteUI : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer
        Water,
        Fire,
        Air;

    private ServitorController servitor;
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    private void Start()
    {
        this.renderers.Add(null);
        this.renderers.Add(Water);
        this.renderers.Add(Fire);
        this.renderers.Add(Air);

        this.servitor = FindObjectOfType<ServitorController>();
        this.servitor.ElementEvent += this.OnChangeElement;

    }

    private void OnChangeElement(object sender, EventArgs args)
    {
        this.SetVisibleElement(this.servitor.Element);
    }

    public void SetVisibleElement(Element element)
    {
        int index = (int)element;
        for (int i = 0; i < renderers.Count; i++)
        {
            if (i > 0)
            {
                this.renderers[i].enabled = i == index;
            }
        }
    }

    
    
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5
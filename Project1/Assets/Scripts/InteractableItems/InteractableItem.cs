using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    [SerializeField] private GameObject InteractionTips;

    void Start()
    {
        if (InteractionTips == null)
        {
            InteractionTips = GameObject.Find("InteractionTips");
            print("Please Give the 'InteractionTips' to this gameobject: " + gameObject.name);
        }

    }
    
    // virtual method for interactable item
    public virtual void InteractableItemOnTriggerEnter() {}
    public virtual void InteractableItemOnTriggerExit() {}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            InteractionTips.SetActive(true);
        }

        InteractableItemOnTriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            InteractionTips.SetActive(false);
        }
        
        InteractableItemOnTriggerExit();

    }
    
}

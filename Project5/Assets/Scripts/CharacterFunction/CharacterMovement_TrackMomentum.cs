using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement_TrackMomentum : Tools.Singleton<CharacterMovement_TrackMomentum>
{
    private Rigidbody characterRb;
    public Rigidbody connectRb;
    private Vector3 connectPreviousPostion,connectMovement,connectVelocity;
    

    private void Start()
    {
        characterRb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // if connectRb!=null, cancel the max velocity temporarily
        if (connectRb != null) 
        {
            //gameObject.GetComponent<CharacterMovement_TrackMomentum>().max
        }
        
    }

    private void FixedUpdate()
    {
        if (connectRb!=null)
        {
            // calculate the connectRb's velocity
            connectMovement = connectRb.position - connectPreviousPostion;
            connectVelocity = connectMovement / Time.deltaTime;
            connectPreviousPostion = connectRb.position;
            
            // fix the character velocity
            var relativeVelocity = characterRb.velocity + connectVelocity;
            characterRb.velocity.Set(relativeVelocity.x,relativeVelocity.y,relativeVelocity.z);

        }
    }
    
    
    
}
        

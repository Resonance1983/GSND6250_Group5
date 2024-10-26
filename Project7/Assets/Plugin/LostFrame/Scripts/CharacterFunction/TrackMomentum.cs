using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostFrame
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class TrackMomentum : Singleton<TrackMomentum>
    {
        private Rigidbody characterRb;

        [SerializeField] public bool isAutoConnect = false;

        [ConditionalHide("isAutoConnect", false)]
        public Rigidbody connectRb = null;

        [ConditionalHide("isAutoConnect", true)]
        public List<string> tagList = new();

        private Vector3 connectPreviousPostion, connectMovement, connectVelocity;

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

        private void OnTriggerEnter(Collider other)
        {
            if (tagList.Contains(other.tag)) other.TryGetComponent<Rigidbody>(out connectRb);
        }

        private void FixedUpdate()
        {
            if (connectRb != null)
            {
                // calculate the connectRb's velocity
                connectMovement = connectRb.position - connectPreviousPostion;
                connectVelocity = connectMovement / Time.deltaTime;
                connectPreviousPostion = connectRb.position;

                // fix the character velocity
                var relativeVelocity = characterRb.velocity + connectVelocity;
                characterRb.velocity.Set(relativeVelocity.x, relativeVelocity.y, relativeVelocity.z);
            }
        }
    }
}
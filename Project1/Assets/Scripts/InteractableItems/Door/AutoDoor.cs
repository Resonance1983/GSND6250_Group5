using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : InteractableItems
{
    [SerializeField] private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // 按F之后开门
        if (Input.GetKey(KeyCode.E))
        {
            _animator.SetBool("IsOpenning", true);
        }
    }

    public override void InteractableItemOnTriggerExit()
    {
        _animator.SetBool("IsOpenning", false);
    }

}

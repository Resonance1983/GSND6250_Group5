using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class CharacterCamera : Singleton<CharacterCamera>
{

    public float xsen = 1, ysen = 1;
    [Range(0,180)]
    public float maxPitch = 60;
    public Transform targetCamera;

    private float pitch = 0;

    // Update is called once per frame
    void Update()
    {
        var curEuler = targetCamera.localRotation.eulerAngles;
        curEuler = new Vector3(pitch - Input.GetAxis("Mouse Y"), curEuler.y + Input.GetAxis("Mouse X"), 0);
        curEuler.z = 0;
        curEuler.x = Mathf.Clamp(curEuler.x, -maxPitch, maxPitch);
        pitch = curEuler.x;
        targetCamera.localRotation = Quaternion.Euler(curEuler);
    }
}
using UnityEngine;

public class FanRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // 旋转速度

    void Update()
    {
        // 每帧旋转扇叶
        transform.Rotate(new Vector3(0,1,0), rotationSpeed * Time.deltaTime);
    }
}
using UnityEngine;

namespace LostFrame
{
    public class ControllableCamera : Singleton<ControllableCamera>
    {
        public Transform targetCamera;
        public float xsen = 1, ysen = 1;
        [Range(0, 180)] public float maxPitch = 60;
        private float pitch = 0;

        public Texture2D cursorTex;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (cursorTex)
                // para2: texture offset position
                Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
        }


        private void Update()
        {
            var curEuler = targetCamera.localRotation.eulerAngles;
            curEuler = new Vector3(pitch - Input.GetAxis("Mouse Y"), curEuler.y + Input.GetAxis("Mouse X"), 0);
            curEuler.z = 0;
            curEuler.x = Mathf.Clamp(curEuler.x, -maxPitch, maxPitch);
            pitch = curEuler.x;
            targetCamera.localRotation = Quaternion.Euler(curEuler);
        }
    }
}
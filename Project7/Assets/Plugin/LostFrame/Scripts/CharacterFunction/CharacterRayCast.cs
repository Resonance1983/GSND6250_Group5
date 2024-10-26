using UnityEngine;

namespace LostFrame
{
    public class CharacterRayCast : MonoBehaviour
    {
        [SerializeField] private Transform forwardTransform;
        [HideInInspector] public bool isEnabled = false;

        private Ray ray;
        private RaycastHit hitInfo;

        private void Update()
        {
            // draw ray hit line
            if (isEnabled)
            {
                ray = new Ray(forwardTransform.position, forwardTransform.forward);
                ItemRockFunction();
            }
        }


        private void ItemRockFunction()
        {
            // make glass disappear
            if (isEnabled && Physics.Raycast(ray, out hitInfo))
            {
                var target = hitInfo.collider.gameObject;

                // target can destroy and player click left mouse button
                if (target.tag.Equals("DestroyAvailable") && Input.GetMouseButtonDown(0))
                {
                    target.SetActive(false);
                    isEnabled = false;
                }

                Debug.Log(hitInfo.collider.gameObject.name);
            }
        }
    }
}
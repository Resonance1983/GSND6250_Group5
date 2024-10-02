using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRayCast : MonoBehaviour
{
    [SerializeField] private Transform forwardTransform;
    // 记得改回去
    [HideInInspector] public bool isEnabled = true;
    
    private Ray ray;
    private RaycastHit hitInfo;
    private LineRenderer lineRenderer;

    private void Start()
    {
        if (!gameObject.TryGetComponent<LineRenderer>(out LineRenderer lineRenderer))
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        lineRenderer.startWidth = 1f;   // 设置线宽度
        lineRenderer.positionCount = 2;  // 设置线渲染器端点数量
        lineRenderer.startColor = Color.red;   // 设置起点颜色
        lineRenderer.endColor = Color.red;     // 设置终点颜色
        var p = this.gameObject.transform.position;
        lineRenderer.SetPosition(0, p);  // 设置索引 0 位置的端点为 p
    }

    void Update()
    {
        // draw ray hit line
        if (isEnabled)
        {
            ray = new Ray(forwardTransform.position, forwardTransform.forward);
            ItemRockFunction();
        }
    }


    void ItemRockFunction()
    {
        Debug.Log("ItemRockFunction");
        // make glass disappear
        if (Physics.Raycast(ray, out hitInfo)) 
        {
            GameObject target = hitInfo.collider.gameObject;
            // 不知道为什么drawRayLine只运行一次但别的部分正常不想写了
            // DrawRayLine(this.gameObject.transform,target.transform);
            // target can destroy and player click left mouse button
            if (target.tag.Equals("DestroyAvailable") && Input.GetMouseButtonDown(0))
            {
                target.SetActive(false);
                isEnabled = false;
            }
            Debug.Log(hitInfo.collider.gameObject.name);
        }
    }


    void DrawRayLine(Transform startPoint,Transform endPoint)
    {
        Debug.Log("Drawing");
        if (lineRenderer != null)
        {   
            var sp = startPoint.position;
            lineRenderer.SetPosition(0, sp);
            
            var ep = endPoint.position;
            lineRenderer.SetPosition(1, ep);
        }
    }

}
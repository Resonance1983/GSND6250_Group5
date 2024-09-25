using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light targetLight;  // 需要控制的灯光
    public float interval = 3.0f;  // 每次灯光亮起的间隔时间，单位为秒

    private float timer;

    void Start()
    {
        if (targetLight == null)
        {
            // 尝试自动获取Light组件
            targetLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 如果计时器达到间隔时间
        if (timer >= interval)
        {
            // 切换灯光的启用状态
            targetLight.enabled = !targetLight.enabled;

            // 重置计时器
            timer = 0.0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTriger : MonoBehaviour
{

    public GameObject light1; // 特定灯光 1
    public GameObject light2; // 特定灯光 2
    public GameObject BossLight1; 
    public GameObject BossLight2;

    private void Start()
    {
        light1.SetActive(false);
        light2.SetActive(false);
        BossLight1.SetActive(false);
        BossLight2.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 检查是否是玩家
        {
            light1.SetActive(true); 
            light2.SetActive(true);
            BossLight1.SetActive(true);
            BossLight2.SetActive(true);

        }
    }
}

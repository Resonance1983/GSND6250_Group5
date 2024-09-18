using System.Collections.Generic;
using Tools;
using UnityEngine;

public class LightGenerate : MonoBehaviour
{

    [SerializeField] private Material lightMaterial;
    private List<Transform> pointsTransforms = new List<Transform>();
    [SerializeField] private GameObject LightPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        pointsTransforms = FindTools.FindChilds(gameObject.transform, "LightPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GenerateObject(pointsTransforms[0].position,pointsTransforms[1].position);
        }
    }
    
    // 创建胶囊体的方法，传入上顶点和下顶点的位置
    public void GenerateObject(Vector3 topVertex, Vector3 bottomVertex)
    {
        print(bottomVertex);
        // 计算中心点和高度
        Vector3 center = (topVertex + bottomVertex) / 2;
        float height = Vector3.Distance(topVertex, bottomVertex);
        print(center);
        
        // 实例化
        GameObject newObject = Instantiate(LightPrefab, center, Quaternion.identity);

        // 调整高度和角度
        newObject.transform.localScale = new Vector3(1, height / 2, 1);
        newObject.transform.rotation = Quaternion.LookRotation(topVertex-bottomVertex);
        newObject.transform.Rotate(90,0,0);
    }
}

using System.Collections.Generic;
using UnityEngine;


public class FindTools : MonoBehaviour
{
    /// <summary>
    /// transform类型递归查找子物体
    /// </summary>
    /// <returns>返回需要查找的子物体.</returns>
    /// <param name="parent">查找起点.</param>
    /// <param name="targetName">需要查找的子物体名字.</param>
    public static Transform FindChild(Transform parent, string targetName)
    {
        var target = parent.Find(targetName);
        //如果找到了直接返回
        if (target != null)
            return target;
        //如果没有没有找到，说明没有在该子层级，则先遍历该层级所有transform，然后通过递归继续查找----再次调用该方法
        for (var i = 0; i < parent.childCount; i++)
        {
            //通过再次调用该方法递归下一层级子物体
            target = FindChild(parent.GetChild(i), targetName);

            if (target != null)
                return target;
        }

        return target;
    }


    /// <summary>
    /// 泛型递归查找
    /// </summary>
    /// <returns>返回需要查找的类型的子物体的组件.</returns>
    /// <param name="parent">查找起点.</param>
    /// <param name="targetName">需要查找的子物体名字.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T FindChildComponent<T>(Transform parent, string targetName) where T : Component
    {
        var target = parent.Find(targetName);
        if (target != null) return target.GetComponent<T>();

        for (var i = 0; i < parent.childCount; i++)
        {
            target = FindChild(parent.GetChild(i), targetName);
            if (target != null) return target.GetComponent<T>();
        }

        return target.GetComponent<T>();
    }

    /// <summary>
    /// 递归查找所有对应名字的子物体
    /// </summary>
    /// <returns>返回需要查找的类型的子物体的组件.</returns>
    /// <param name="parent">查找起点.</param>
    /// <param name="targetName">需要查找的子物体名字.</param>
    public static List<Transform> FindChilds(Transform parent, string targetName)
    {
        var result = new List<Transform>();
        var allChilds = FindAllChilds(parent.gameObject);

        foreach (var child in allChilds)
            if (child.name.Equals(targetName))
                result.Add(child.transform);

        return result;
    }


    /// <summary>
    /// 递归获取所有子物体
    /// </summary>
    /// <returns>返回所有子物体的列表</returns>
    /// <param name="parent">查找起点.</param>
    /// <param name="targetName">需要查找的子物体名字.</param>
    /// <param name="depth">查找的深度,默认往下找1层</param>
    public static List<GameObject> FindAllChilds(GameObject parent)
    {
        var result = new List<GameObject>();

        //利用for循环 获取物体下的全部子物体
        for (var t = 0; t < parent.transform.childCount; t++)
        {
            //如果子物体下还有子物体 就将子物体传入进行回调查找 直到物体没有子物体为止
            if (parent.transform.GetChild(t).childCount > 0) FindAllChilds(parent.transform.GetChild(t).gameObject);

            result.Add(parent.transform.GetChild(t).gameObject);
        }

        return result;
    }
}
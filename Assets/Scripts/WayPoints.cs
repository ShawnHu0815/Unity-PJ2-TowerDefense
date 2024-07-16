using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static Transform[] positions;
    
    // 在脚本第一次加载到内存时被调用，并且只会被调用一次。这通常用于初始化脚本的变量和设置，而不会依赖于游戏对象的激活状态。
    private void Awake()
    {
        positions = new Transform[transform.childCount];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);
        }
    }
}

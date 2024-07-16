using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{   
    public float speed = 25.0f;
    public float mouseSpeed = 360.0f;
    // Update is called once per frame
    void Update()
    {   
        // 获取水平、垂直方向，鼠标滚轮的输入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        // 沿着水平、垂直方向移动，乘以速度和时间
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
        // 沿着垂直方向移动，乘以鼠标滚轮的值和速度和时间
        transform.Translate(new Vector3(0, -mouse, 0) * mouseSpeed * Time.deltaTime, Space.World);
    }
}

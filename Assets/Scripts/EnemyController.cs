using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{   
    public float speed = 10.0f;
    public float hp = 150;
    private float totalHp;
    public GameObject explosionEffectPrefab;
    private Slider hpSlider;
    private Transform[] positions;
    private int index = 0;
    
    void Start()
    {   
        // 获取所有路径点
        positions = WayPoints.positions;
        // 初始化血条组件
        totalHp = hp;
        hpSlider = GetComponentInChildren<Slider>();
    }
    
    void Update()
    {   
        // 调用Move方法，每帧移动
        Move();
    }

    void Move()
    {   
        // 如果index大于positions的长度-1，说明已经到达终点，直接返回
        if (index > positions.Length - 1) return;
        // 沿着positions的位置移动，先将目标位置-当前位置，然后归一化得到方向向量，最后乘以速度和时间
        transform.Translate((positions[index].position - transform.position).normalized * speed * Time.deltaTime);
        // 如果当前位置和目标位置的距离小于0.2f，说明已经到达目标位置，index+1
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        // 如果index大于positions的长度-1，说明已经到达终点，调用ReachDestination
        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }
    
    // 到达终点的处理
    void ReachDestination()
    {   
        GameManager.Instance.Fail();
        GameObject.Destroy(this.gameObject);
    }
    
    // 敌人受到伤害逻辑
    public void TakeDamage(float damage)
    {
        if(hp <= 0) return;
        hp -= damage;
        hpSlider.value = hp * 1.0f / totalHp;
        if(hp <= 0)
        {
            Die();
        }
    }
    
    // 敌人死亡逻辑
    void Die()
    {   
        // 生成爆炸特效
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
    
    // 当敌人被销毁时，敌人数量-1
    private void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{   
    private List<GameObject> enemies = new List<GameObject>(); // 保存进入攻击范围的敌人
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {   
            // 将进入攻击范围的敌人加入到enemies列表中
            enemies.Add(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            // 将离开攻击范围的敌人从enemies列表中移除
            enemies.Remove(col.gameObject);
        }
    }
    
    public float attackRateTime = 1; // 攻击间隔，每隔几秒攻击一次
    private float timer = 0; // 计时器

    public GameObject bulletPrefab; // 攻击的武器
    public Transform firePosition; // 发射位置
    public Transform head;
    
    public bool useLaser = false;  // 是否使用激光炮
    public float damageRate = 70;  // 激光炮每秒伤害
    public LineRenderer laserRenderer; // 激光炮的LineRenderer
    public GameObject laserEffect; // 激光炮的特效
    
    void Start()
    {
        timer = attackRateTime;
    } 
    
    void Update()
    {
        if (enemies.Count > 0 && enemies[0] != null)
        {
            Vector3 targetPosition = enemies[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        
        // 如果不使用激光炮
        if (useLaser == false)
        {
            timer += Time.deltaTime;
            if (enemies.Count > 0 && timer >= attackRateTime)
            {
                timer = 0;
                Attack();
            }
        }
        // 如果使用激光炮
        else if(enemies.Count > 0)
        {
            if (laserRenderer.enabled == false)
            {
                laserRenderer.enabled = true;
            }
            laserEffect.SetActive(true);
            if (enemies[0] == null)
            {
                UpdateEnemies();
            }
            // 如果enemies列表中有敌人，进行攻击
            if (enemies.Count > 0)
            {
                laserRenderer.SetPositions(new Vector3[]{firePosition.position, enemies[0].transform.position});
                enemies[0].GetComponent<EnemyController>().TakeDamage(damageRate * Time.deltaTime);
                laserEffect.transform.position = enemies[0].transform.position;
                Vector3 pos = transform.position;
                pos.y = enemies[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }
        // 如果enemies列表中没有敌人，不进行攻击
        else
        {   
            laserEffect.SetActive(false);
            laserRenderer.enabled = false;
        }

    }

    void Attack()
    {
        if (enemies[0] == null)
        {
            UpdateEnemies();
        }

        if (enemies.Count > 0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<BulletController>().SetTarget(enemies[0].transform);
        }
        else
        {
            timer = attackRateTime;
        }
    }
    
    // 更新enemies列表，移除空对象
    void UpdateEnemies()
    {   
        enemies.RemoveAll(item => item == null);
    }
}

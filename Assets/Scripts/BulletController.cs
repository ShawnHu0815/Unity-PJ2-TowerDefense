using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 50;
    public float speed = 40;
    public GameObject explosionEffectPrefab;
    private Transform target;
    
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Die();
            return;
        }
        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // 沿着目标方向移动
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {   
            // 子弹击中敌人，敌人受到伤害
            col.GetComponent<EnemyController>().TakeDamage(damage); 
            // 生成爆炸特效
            GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            // 销毁特效
            Destroy(effect, 1);
            Die();
            
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}

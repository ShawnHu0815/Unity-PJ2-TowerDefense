using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   
    public static int CountEnemyAlive = 0;
    public Wave[] waves;
    public Transform START;
    public float waveRate = 0.2f;
    private Coroutine coroutine;
    
    private void Start()
    {   
        // 启动协程
        coroutine = StartCoroutine(SpawnEnemy());
    }

    public void Stop()
    {
        StopCoroutine(coroutine);
    }
    
    // 协程，生成敌人
    IEnumerator SpawnEnemy()
    {   
        // 等待15秒后开始生成敌人
        yield return new WaitForSeconds(15);
        
        foreach (Wave wave in waves)
        {
             for(int i=0;i<wave.count;i++)
             {      
                 // 在START的位置生成一个敌人enemyPrefab，Quaternion.identity表示不旋转
                 GameObject.Instantiate(wave.enemyPrefab, START.position, Quaternion.identity);
                 CountEnemyAlive++;
                 // 每隔1秒生成一个敌人
                 if (i != wave.count - 1)
                 {
                     yield return new WaitForSeconds(wave.rate);
                 }
                 
             }
             while (CountEnemyAlive > 0)
             {
                 yield return 0;
             }
             yield return new WaitForSeconds(waveRate);
        }
        
        while(CountEnemyAlive > 0)
        {
            yield return 0;
        }
        GameManager.Instance.Win();
    }
}
